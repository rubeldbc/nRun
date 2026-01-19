using nRun.Models;

namespace nRun.Services;

/// <summary>
/// Service for scraping news articles from configured sites.
/// WebDriver is created per-operation and immediately disposed after each scrape
/// to prevent memory leaks and process accumulation.
/// </summary>
public class NewsScraperService
{
    public event EventHandler<NewsInfo>? ArticleScraped;
    public event EventHandler<string>? StatusChanged;
    public event EventHandler<(int current, int total)>? ProgressChanged;

    public async Task<ScrapeResult> ScrapeAllSitesAsync(CancellationToken cancellationToken = default)
    {
        var sites = ServiceContainer.Database.GetActiveSites();
        var allArticles = new List<NewsInfo>();
        int totalNew = 0, totalSkipped = 0;
        var startTime = DateTime.Now;

        for (int i = 0; i < sites.Count; i++)
        {
            if (cancellationToken.IsCancellationRequested) break;

            var site = sites[i];
            ProgressChanged?.Invoke(this, (i + 1, sites.Count));
            StatusChanged?.Invoke(this, $"Scraping: {site.SiteName}");

            try
            {
                var result = await ScrapeSiteAsync(site, cancellationToken);
                allArticles.AddRange(result.Articles);
                totalNew += result.NewArticlesCount;
                totalSkipped += result.SkippedCount;
            }
            catch (OperationCanceledException)
            {
                StatusChanged?.Invoke(this, "Scraping cancelled");
                break;
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke(this, $"Error scraping {site.SiteName}: {ex.Message}");
                ServiceContainer.Database.UpdateSiteStats(site.SiteId, false);
            }
        }

        StatusChanged?.Invoke(this, "Scraping completed");
        return ScrapeResult.Succeeded(allArticles, totalNew, totalSkipped, DateTime.Now - startTime);
    }

    public async Task<ScrapeResult> ScrapeSiteAsync(SiteInfo site, CancellationToken cancellationToken = default)
    {
        var articles = new List<NewsInfo>();
        int newCount = 0, skippedCount = 0;
        var startTime = DateTime.Now;

        // Check if cloudflare bypass is enabled for this site
        bool cloudflareBypassEnabled = ServiceContainer.CloudflareBypass.IsEnabled(site.SiteId);

        // Create headless WebDriver to find the article link (fast)
        WebDriverService? webDriver = null;
        try
        {
            // STEP 1: Use headless browser to find the article link (selector-based, fast)
            webDriver = WebDriverFactory.Create();
            StatusChanged?.Invoke(this, $"Loading: {site.SiteLink}");

            // Navigate to site
            await webDriver.NavigateToAsync(site.SiteLink, cancellationToken);

            // Wait for page to fully load
            StatusChanged?.Invoke(this, $"Waiting for page load: {site.SiteName}");
            try
            {
                await webDriver.WaitForPageLoadAsync(15, cancellationToken);
            }
            catch (OperationCanceledException) { throw; }
            catch { /* Page load timeout is acceptable */ }

            // Wait for JavaScript content to render
            await Task.Delay(2000, cancellationToken);

            // Find the first article link
            StatusChanged?.Invoke(this, $"Finding first article on: {site.SiteName}");
            var firstLink = webDriver.GetFirstLink(site.ArticleLinkSelector);

            // If not found, try with scroll
            if (string.IsNullOrEmpty(firstLink))
            {
                StatusChanged?.Invoke(this, $"Scrolling to find content: {site.SiteName}");
                await webDriver.ScrollAndWaitAsync(2, 300, cancellationToken);

                // Wait for element to appear
                var elementFound = await webDriver.TryWaitForElementAsync(site.ArticleLinkSelector, 10, cancellationToken);

                if (!elementFound)
                {
                    await webDriver.ScrollAndWaitAsync(2, 500, cancellationToken);
                    await Task.Delay(500, cancellationToken);
                }

                firstLink = webDriver.GetFirstLink(site.ArticleLinkSelector);
            }

            // If still no link found
            if (string.IsNullOrEmpty(firstLink))
            {
                StatusChanged?.Invoke(this, $"No article link found on: {site.SiteName}");
                ServiceContainer.Database.UpdateSiteStats(site.SiteId, true);
                return ScrapeResult.Succeeded(articles, newCount, skippedCount, DateTime.Now - startTime);
            }

            StatusChanged?.Invoke(this, $"Link found: {firstLink.Substring(0, Math.Min(60, firstLink.Length))}...");

            cancellationToken.ThrowIfCancellationRequested();

            // Normalize URL
            var articleUrl = NormalizeUrl(firstLink, site.SiteLink);
            if (string.IsNullOrEmpty(articleUrl))
            {
                StatusChanged?.Invoke(this, $"Invalid article URL on: {site.SiteName}");
                ServiceContainer.Database.UpdateSiteStats(site.SiteId, true);
                return ScrapeResult.Succeeded(articles, newCount, skippedCount, DateTime.Now - startTime);
            }

            // STEP 2: Close headless browser - we're done finding the link
            webDriver.ForceKillChrome();
            webDriver = null;

            // STEP 3: Fetch article title
            NewsInfo? article;
            if (cloudflareBypassEnabled)
            {
                // Use visible browser to get title quickly from browser tab
                article = await GetArticleTitleWithVisibleBrowserAsync(site, articleUrl, cancellationToken);
            }
            else
            {
                // Use headless browser with selector
                article = await GetArticleTitleWithHeadlessBrowserAsync(site, articleUrl, cancellationToken);
            }

            if (article == null)
            {
                StatusChanged?.Invoke(this, $"Failed to get article details on: {site.SiteName}");
                ServiceContainer.Database.UpdateSiteStats(site.SiteId, true);
                return ScrapeResult.Succeeded(articles, newCount, skippedCount, DateTime.Now - startTime);
            }

            // Log article info for debugging
            StatusChanged?.Invoke(this, $"Title: {article.NewsTitle}");

            // Check if article already exists
            if (ServiceContainer.Database.NewsExists(articleUrl))
            {
                // Check if existing article has a Cloudflare placeholder title
                var existingTitle = ServiceContainer.Database.GetNewsTitleByUrl(articleUrl);
                if (existingTitle != null && IsCloudflareTitle(existingTitle))
                {
                    // Update the existing article with the real title
                    StatusChanged?.Invoke(this, $"Updating article with real title (was: {existingTitle.Substring(0, Math.Min(30, existingTitle.Length))}...)");
                    ServiceContainer.Database.UpdateNewsByUrl(articleUrl, article.NewsTitle, article.NewsText ?? "");
                    StatusChanged?.Invoke(this, $"Updated: {article.ShortTitle}");

                    // Notify UI to refresh
                    ArticleScraped?.Invoke(this, article);
                    newCount++;
                }
                else
                {
                    skippedCount++;
                    StatusChanged?.Invoke(this, $"Already exists, skipping: {site.SiteName}");
                }

                ServiceContainer.Database.UpdateSiteStats(site.SiteId, true);
                return ScrapeResult.Succeeded(articles, newCount, skippedCount, DateTime.Now - startTime);
            }

            // New article - add to database and notify UI
            StatusChanged?.Invoke(this, $"New article! Adding to database...");
            article.Serial = ServiceContainer.Database.AddNews(article);
            articles.Add(article);
            newCount++;

            // ArticleScraped event - triggers UI article list update
            ArticleScraped?.Invoke(this, article);
            StatusChanged?.Invoke(this, $"Added: {article.ShortTitle}");

            ServiceContainer.Database.UpdateSiteStats(site.SiteId, true);
            return ScrapeResult.Succeeded(articles, newCount, skippedCount, DateTime.Now - startTime);
        }
        catch (OperationCanceledException)
        {
            StatusChanged?.Invoke(this, $"Scrape cancelled: {site.SiteName}");
            throw;
        }
        catch (Exception ex)
        {
            StatusChanged?.Invoke(this, $"Error: {ex.Message}");
            ServiceContainer.Database.UpdateSiteStats(site.SiteId, false);
            return ScrapeResult.Failed(ex.Message);
        }
        finally
        {
            // CRITICAL: Always dispose WebDriver after each site scrape
            if (webDriver != null)
            {
                StatusChanged?.Invoke(this, $"Closing browser for: {site.SiteName}");
                WebDriverFactory.SafeDispose(webDriver);
            }
        }
    }

    /// <summary>
    /// Gets article title using visible browser - fast method for bypass mode.
    /// Opens browser, navigates to URL, captures title from browser tab, closes browser.
    /// </summary>
    private async Task<NewsInfo?> GetArticleTitleWithVisibleBrowserAsync(
        SiteInfo site,
        string url,
        CancellationToken cancellationToken)
    {
        WebDriverService? browser = null;
        try
        {
            StatusChanged?.Invoke(this, $"[Bypass] Opening visible browser for article...");

            // Create visible browser
            browser = WebDriverFactory.CreateWithUserProfile(timeoutSeconds: 10);

            // Navigate to article
            await browser.NavigateToAsync(url, cancellationToken);

            // Quick capture - try to get title from browser tab as soon as possible
            string? title = null;
            for (int attempt = 0; attempt < 10; attempt++) // Max 5 seconds (10 * 500ms)
            {
                cancellationToken.ThrowIfCancellationRequested();

                title = browser.GetPageTitle();
                if (!string.IsNullOrEmpty(title) && !IsCloudflareTitle(title) && title.Length > 5)
                {
                    StatusChanged?.Invoke(this, $"[Bypass] Title captured: {title.Substring(0, Math.Min(50, title.Length))}...");
                    break;
                }

                await Task.Delay(500, cancellationToken);
            }

            if (string.IsNullOrEmpty(title) || IsCloudflareTitle(title))
            {
                StatusChanged?.Invoke(this, $"[Bypass] Could not capture title");
                return null;
            }

            // Get body if selector is provided (quick attempt)
            string body = "";
            if (!string.IsNullOrEmpty(site.BodySelector))
            {
                try
                {
                    body = browser.GetElementText(site.BodySelector) ?? "";
                }
                catch { }
            }

            if (string.IsNullOrEmpty(body))
            {
                body = title; // Use title as fallback
            }

            return new NewsInfo
            {
                SiteId = site.SiteId,
                SiteName = site.SiteName,
                SiteLogo = site.SiteLogo,
                NewsTitle = CleanText(title),
                NewsUrl = url,
                NewsText = CleanText(body),
                CreatedAt = DateTime.Now
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            StatusChanged?.Invoke(this, $"[Bypass] Error: {ex.Message}");
            return null;
        }
        finally
        {
            if (browser != null)
            {
                StatusChanged?.Invoke(this, $"[Bypass] Closing browser...");
                browser.ForceKillChrome();
            }
        }
    }

    /// <summary>
    /// Gets article title using headless browser with CSS selector.
    /// </summary>
    private async Task<NewsInfo?> GetArticleTitleWithHeadlessBrowserAsync(
        SiteInfo site,
        string url,
        CancellationToken cancellationToken)
    {
        WebDriverService? browser = null;
        try
        {
            StatusChanged?.Invoke(this, $"Loading article...");

            browser = WebDriverFactory.Create();

            // Navigate to article
            await browser.NavigateToAsync(url, cancellationToken);

            // Wait for page to load
            try
            {
                await browser.WaitForPageLoadAsync(15, cancellationToken);
            }
            catch (OperationCanceledException) { throw; }
            catch { }

            await Task.Delay(2000, cancellationToken);

            // Check for Cloudflare
            var pageTitle = browser.GetPageTitle();
            if (IsCloudflareTitle(pageTitle))
            {
                StatusChanged?.Invoke(this, $"Cloudflare detected on article page (bypass not enabled)");
                return null;
            }

            // Extract title
            var pageSource = browser.GetPageSource();
            string title = ExtractTitle(browser, site.TitleSelector, pageSource);

            // Fallback to browser tab title
            if (string.IsNullOrEmpty(title))
            {
                title = pageTitle ?? "";
            }

            if (string.IsNullOrEmpty(title) || IsCloudflareTitle(title))
            {
                StatusChanged?.Invoke(this, $"Could not extract title");
                return null;
            }

            // Extract body
            string body = "";
            if (!string.IsNullOrEmpty(site.BodySelector))
            {
                body = browser.GetElementText(site.BodySelector) ?? "";

                if (string.IsNullOrEmpty(body) && !string.IsNullOrEmpty(pageSource))
                {
                    var doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(pageSource);
                    var bodyNode = doc.DocumentNode.SelectSingleNode(ConvertCssToXPath(site.BodySelector));
                    body = bodyNode?.InnerText?.Trim() ?? "";
                }
            }

            if (string.IsNullOrEmpty(body))
            {
                body = title;
            }

            StatusChanged?.Invoke(this, $"Title: {title.Substring(0, Math.Min(50, title.Length))}...");

            return new NewsInfo
            {
                SiteId = site.SiteId,
                SiteName = site.SiteName,
                SiteLogo = site.SiteLogo,
                NewsTitle = CleanText(title),
                NewsUrl = url,
                NewsText = CleanText(body),
                CreatedAt = DateTime.Now
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            StatusChanged?.Invoke(this, $"Error: {ex.Message}");
            return null;
        }
        finally
        {
            if (browser != null)
            {
                StatusChanged?.Invoke(this, $"Closing browser...");
                WebDriverFactory.SafeDispose(browser);
            }
        }
    }

    /// <summary>
    /// Internal method to scrape article using an existing WebDriver instance
    /// </summary>
    private async Task<NewsInfo?> ScrapeArticleInternalAsync(
        WebDriverService webDriver,
        SiteInfo site,
        string url,
        CancellationToken cancellationToken,
        bool isUserBrowserRetry = false,
        bool cloudflareBypassEnabled = false)
    {
        try
        {
            bool isUsingVisibleBrowser = webDriver.UseUserProfile;

            // Navigate to article (skip if already loaded in user's browser)
            if (!isUserBrowserRetry)
            {
                StatusChanged?.Invoke(this, $"Navigating to article: {url}");
                await webDriver.NavigateToAsync(url, cancellationToken);

                // Wait for page to load (shorter timeout for visible browser)
                StatusChanged?.Invoke(this, $"Waiting for article page load...");
                try
                {
                    await webDriver.WaitForPageLoadAsync(isUsingVisibleBrowser ? 8 : 15, cancellationToken);
                }
                catch (OperationCanceledException) { throw; }
                catch { }

                // Shorter delay for visible browser (already bypassed Cloudflare)
                await Task.Delay(isUsingVisibleBrowser ? 500 : 2000, cancellationToken);

                // Check for Cloudflare only if NOT using visible browser
                if (!isUsingVisibleBrowser)
                {
                    var browserTitle = webDriver.GetPageTitle();
                    if (IsCloudflareTitle(browserTitle))
                    {
                        if (cloudflareBypassEnabled)
                        {
                            StatusChanged?.Invoke(this, $"Cloudflare detected! Opening your Chrome browser...");
                            return await RetryWithUserBrowserAsync(site, url, cancellationToken, webDriver);
                        }
                        else
                        {
                            StatusChanged?.Invoke(this, $"Cloudflare detected but bypass is disabled for: {site.SiteName}");
                            return null;
                        }
                    }
                }
            }

            string title = "";
            string body = "";

            // OPTIMIZATION: For visible browser, try browser tab title FIRST (fastest)
            if (isUsingVisibleBrowser || isUserBrowserRetry)
            {
                title = webDriver.GetPageTitle();
                if (!string.IsNullOrEmpty(title) && !IsCloudflareTitle(title))
                {
                    StatusChanged?.Invoke(this, $"Title from browser tab: {title.Substring(0, Math.Min(50, title.Length))}...");
                }
                else
                {
                    title = "";
                }
            }

            // If browser tab title didn't work, try selector-based extraction
            if (string.IsNullOrEmpty(title))
            {
                var pageSource = webDriver.GetPageSource();
                title = ExtractTitle(webDriver, site.TitleSelector, pageSource);

                // Extract body content if selector is provided
                if (!string.IsNullOrEmpty(site.BodySelector) && !string.IsNullOrEmpty(pageSource))
                {
                    body = webDriver.GetElementText(site.BodySelector) ?? "";

                    if (string.IsNullOrEmpty(body))
                    {
                        var doc = new HtmlAgilityPack.HtmlDocument();
                        doc.LoadHtml(pageSource);
                        var bodyNode = doc.DocumentNode.SelectSingleNode(ConvertCssToXPath(site.BodySelector));
                        body = bodyNode?.InnerText?.Trim() ?? "";
                    }
                }
            }

            // Still no title - try visible browser if bypass is enabled and not already using one
            if ((string.IsNullOrEmpty(title) || IsCloudflareTitle(title)) && !isUserBrowserRetry && !isUsingVisibleBrowser)
            {
                if (cloudflareBypassEnabled)
                {
                    StatusChanged?.Invoke(this, $"Cannot get title, trying visible browser...");
                    return await RetryWithUserBrowserAsync(site, url, cancellationToken, webDriver);
                }
                else
                {
                    StatusChanged?.Invoke(this, $"Cannot get title and bypass is disabled for: {site.SiteName}");
                    return null;
                }
            }

            // Extract body if not already extracted and selector is provided
            if (string.IsNullOrEmpty(body) && !string.IsNullOrEmpty(site.BodySelector))
            {
                body = webDriver.GetElementText(site.BodySelector) ?? "";

                if (string.IsNullOrEmpty(body))
                {
                    var pageSource = webDriver.GetPageSource();
                    if (!string.IsNullOrEmpty(pageSource))
                    {
                        var doc = new HtmlAgilityPack.HtmlDocument();
                        doc.LoadHtml(pageSource);
                        var bodyNode = doc.DocumentNode.SelectSingleNode(ConvertCssToXPath(site.BodySelector));
                        body = bodyNode?.InnerText?.Trim() ?? "";
                    }
                }
            }

            // FALLBACK: If body is still empty, use title as body text
            var createdAt = DateTime.Now;
            if (string.IsNullOrEmpty(body))
            {
                StatusChanged?.Invoke(this, $"Body empty, using title as fallback");
                body = title;
            }

            if (string.IsNullOrEmpty(title) || IsCloudflareTitle(title))
            {
                StatusChanged?.Invoke(this, $"Title is empty or blocked, article scrape failed");
                return null;
            }

            StatusChanged?.Invoke(this, $"Article scraped successfully");
            return new NewsInfo
            {
                SiteId = site.SiteId,
                SiteName = site.SiteName,
                SiteLogo = site.SiteLogo,
                NewsTitle = CleanText(title),
                NewsUrl = url,
                NewsText = CleanText(body),
                CreatedAt = createdAt
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            StatusChanged?.Invoke(this, $"ScrapeArticle error: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Extracts title using multiple methods: element text, aria-label, title attribute, HtmlAgilityPack
    /// </summary>
    private string ExtractTitle(WebDriverService webDriver, string titleSelector, string? pageSource)
    {
        string title = "";

        // 1. Try element text content
        title = webDriver.GetElementText(titleSelector) ?? "";
        if (!string.IsNullOrEmpty(title) && !IsCloudflareTitle(title))
        {
            StatusChanged?.Invoke(this, $"Title found (text): {title.Substring(0, Math.Min(50, title.Length))}...");
            return title;
        }

        // 2. Try aria-label attribute
        title = webDriver.GetElementAttribute(titleSelector, "aria-label") ?? "";
        if (!string.IsNullOrEmpty(title) && !IsCloudflareTitle(title))
        {
            StatusChanged?.Invoke(this, $"Title found (aria-label): {title.Substring(0, Math.Min(50, title.Length))}...");
            return title;
        }

        // 3. Try title attribute
        title = webDriver.GetElementAttribute(titleSelector, "title") ?? "";
        if (!string.IsNullOrEmpty(title) && !IsCloudflareTitle(title))
        {
            StatusChanged?.Invoke(this, $"Title found (title attr): {title.Substring(0, Math.Min(50, title.Length))}...");
            return title;
        }

        // 4. Try HtmlAgilityPack
        if (!string.IsNullOrEmpty(pageSource))
        {
            try
            {
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(pageSource);
                var xpath = ConvertCssToXPath(titleSelector);
                var titleNode = doc.DocumentNode.SelectSingleNode(xpath);

                if (titleNode != null)
                {
                    title = titleNode.InnerText?.Trim() ?? "";
                    if (string.IsNullOrEmpty(title) || IsCloudflareTitle(title))
                    {
                        title = titleNode.GetAttributeValue("aria-label", "");
                    }
                    if (string.IsNullOrEmpty(title) || IsCloudflareTitle(title))
                    {
                        title = titleNode.GetAttributeValue("title", "");
                    }

                    if (!string.IsNullOrEmpty(title) && !IsCloudflareTitle(title))
                    {
                        StatusChanged?.Invoke(this, $"Title found (HTML): {title.Substring(0, Math.Min(50, title.Length))}...");
                        return title;
                    }
                }
            }
            catch { }
        }

        return "";
    }

    /// <summary>
    /// Checks if the title indicates Cloudflare blocking
    /// </summary>
    private static bool IsCloudflareTitle(string? title)
    {
        if (string.IsNullOrEmpty(title)) return false;

        var lowerTitle = title.ToLowerInvariant();
        var cloudflareIndicators = new[]
        {
            "just a moment",
            "checking your browser",
            "please wait",
            "ddos protection",
            "attention required",
            "please stand by",
            "one more step"
        };

        foreach (var indicator in cloudflareIndicators)
        {
            if (lowerTitle.Contains(indicator))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Opens user's Chrome browser to bypass Cloudflare and get the article title.
    /// Uses user's real Chrome profile with cookies, history, etc.
    /// Disposes the existing headless browser first to ensure only one browser is open.
    /// </summary>
    private async Task<NewsInfo?> RetryWithUserBrowserAsync(
        SiteInfo site,
        string url,
        CancellationToken cancellationToken,
        WebDriverService? existingDriver = null)
    {
        // CRITICAL: Dispose the existing browser FIRST to avoid having two browsers open
        if (existingDriver != null)
        {
            StatusChanged?.Invoke(this, $"Closing browser before opening user browser...");

            // Force kill Chrome using taskkill - no graceful close as it can hang on Cloudflare
            existingDriver.ForceKillChrome();

            // Wait for Chrome to fully terminate before opening new browser
            await Task.Delay(2000, cancellationToken);
        }

        WebDriverService? userBrowser = null;
        try
        {
            StatusChanged?.Invoke(this, $"Opening visible browser...");

            // Create visible browser for Cloudflare bypass
            var settings = ServiceContainer.Settings.LoadSettings();
            userBrowser = WebDriverFactory.CreateWithUserProfile(timeoutSeconds: 10); // Short timeout

            // Navigate to the article
            StatusChanged?.Invoke(this, $"Loading: {url}");

            // Use a 5-second timeout for the entire title capture operation
            using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

            string title = "";
            var startTime = DateTime.Now;

            try
            {
                await userBrowser.NavigateToAsync(url, linkedCts.Token);

                // Try to capture title immediately - poll rapidly for up to 5 seconds
                StatusChanged?.Invoke(this, $"Capturing title...");

                for (int attempt = 0; attempt < 10; attempt++) // 10 attempts, 500ms each = 5 seconds max
                {
                    linkedCts.Token.ThrowIfCancellationRequested();

                    // Try browser tab title first (fastest)
                    title = userBrowser.GetPageTitle();
                    if (!string.IsNullOrEmpty(title) && !IsCloudflareTitle(title) && title.Length > 5)
                    {
                        StatusChanged?.Invoke(this, $"Title captured: {title.Substring(0, Math.Min(50, title.Length))}...");
                        break;
                    }

                    // Try selector-based extraction
                    var pageSource = userBrowser.GetPageSource();
                    if (!string.IsNullOrEmpty(pageSource) && pageSource.Length > 1000)
                    {
                        title = ExtractTitle(userBrowser, site.TitleSelector, pageSource);
                        if (!string.IsNullOrEmpty(title) && !IsCloudflareTitle(title))
                        {
                            StatusChanged?.Invoke(this, $"Title captured: {title.Substring(0, Math.Min(50, title.Length))}...");
                            break;
                        }
                    }

                    // Wait briefly before next attempt
                    await Task.Delay(500, linkedCts.Token);
                }
            }
            catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested)
            {
                // 5-second timeout reached
                StatusChanged?.Invoke(this, $"Timeout - could not capture title within 5 seconds");
                return null;
            }

            // If still no valid title, fail
            if (string.IsNullOrEmpty(title) || IsCloudflareTitle(title))
            {
                StatusChanged?.Invoke(this, $"Could not extract title from browser");
                return null;
            }

            // Extract body (quick attempt, don't wait long)
            string body = "";
            try
            {
                if (!string.IsNullOrEmpty(site.BodySelector))
                {
                    body = userBrowser.GetElementText(site.BodySelector) ?? "";
                }
            }
            catch { }

            var createdAt = DateTime.Now;
            if (string.IsNullOrEmpty(body))
            {
                body = title; // Use title as fallback for empty body
            }

            var elapsed = (DateTime.Now - startTime).TotalSeconds;
            StatusChanged?.Invoke(this, $"Article scraped in {elapsed:F1}s via visible browser");
            return new NewsInfo
            {
                SiteId = site.SiteId,
                SiteName = site.SiteName,
                SiteLogo = site.SiteLogo,
                NewsTitle = CleanText(title),
                NewsUrl = url,
                NewsText = CleanText(body),
                CreatedAt = createdAt
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            StatusChanged?.Invoke(this, $"Browser error: {ex.Message}");
            return null;
        }
        finally
        {
            // Always close browser immediately - use force kill for fast cleanup
            if (userBrowser != null)
            {
                StatusChanged?.Invoke(this, $"Closing browser...");
                userBrowser.ForceKillChrome();
            }
        }
    }

    /// <summary>
    /// Fallback method to find article links using a visible browser when headless browser fails.
    /// Opens visible browser, loads page, tries selector first, then broader search.
    /// Closes browser immediately when link is found.
    /// </summary>
    private async Task<string?> FindLinkWithVisibleBrowserAsync(SiteInfo site, CancellationToken cancellationToken)
    {
        WebDriverService? browser = null;
        try
        {
            StatusChanged?.Invoke(this, $"Opening visible browser to find links...");

            // Create visible browser
            browser = WebDriverFactory.CreateWithUserProfile(timeoutSeconds: 10);

            // Use 5-second timeout for finding links
            using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

            // Navigate to the main page
            StatusChanged?.Invoke(this, $"Loading: {site.SiteLink}");
            await browser.NavigateToAsync(site.SiteLink, linkedCts.Token);

            string? foundLink = null;

            // Poll for links - try every 500ms for up to 5 seconds
            for (int attempt = 0; attempt < 10; attempt++)
            {
                linkedCts.Token.ThrowIfCancellationRequested();

                // First try the configured selector
                foundLink = browser.GetFirstLink(site.ArticleLinkSelector);
                if (!string.IsNullOrEmpty(foundLink))
                {
                    StatusChanged?.Invoke(this, $"Link found with selector: {foundLink.Substring(0, Math.Min(50, foundLink.Length))}...");
                    return NormalizeUrl(foundLink, site.SiteLink);
                }

                // Broader search - look for common article link patterns in page source
                var pageSource = browser.GetPageSource();
                if (!string.IsNullOrEmpty(pageSource) && pageSource.Length > 1000)
                {
                    foundLink = ExtractFirstArticleLinkFromSource(pageSource, site.SiteLink);
                    if (!string.IsNullOrEmpty(foundLink))
                    {
                        StatusChanged?.Invoke(this, $"Link found with broader search: {foundLink.Substring(0, Math.Min(50, foundLink.Length))}...");
                        return foundLink;
                    }
                }

                await Task.Delay(500, linkedCts.Token);
            }

            StatusChanged?.Invoke(this, $"Could not find article links");
            return null;
        }
        catch (OperationCanceledException)
        {
            StatusChanged?.Invoke(this, $"Timeout finding links");
            return null;
        }
        catch (Exception ex)
        {
            StatusChanged?.Invoke(this, $"Error finding links: {ex.Message}");
            return null;
        }
        finally
        {
            // Close browser immediately
            if (browser != null)
            {
                StatusChanged?.Invoke(this, $"Closing browser...");
                browser.ForceKillChrome();
            }
        }
    }

    /// <summary>
    /// Extracts the first article-like link from HTML source using common patterns.
    /// Looks for links that appear to be news articles based on URL patterns.
    /// </summary>
    private static string? ExtractFirstArticleLinkFromSource(string pageSource, string baseUrl)
    {
        try
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(pageSource);

            // Get base domain for filtering
            var baseUri = new Uri(baseUrl);
            var baseDomain = baseUri.Host;

            // Find all anchor tags with href
            var links = doc.DocumentNode.SelectNodes("//a[@href]");
            if (links == null) return null;

            foreach (var link in links)
            {
                var href = link.GetAttributeValue("href", "");
                if (string.IsNullOrEmpty(href)) continue;

                // Normalize the URL
                var absoluteUrl = NormalizeUrl(href, baseUrl);
                if (string.IsNullOrEmpty(absoluteUrl)) continue;

                // Check if it looks like an article URL (common patterns)
                if (IsLikelyArticleUrl(absoluteUrl, baseDomain))
                {
                    return absoluteUrl;
                }
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Checks if a URL looks like a news article based on common patterns.
    /// </summary>
    private static bool IsLikelyArticleUrl(string url, string baseDomain)
    {
        try
        {
            var uri = new Uri(url);

            // Must be same domain
            if (!uri.Host.Contains(baseDomain) && !baseDomain.Contains(uri.Host))
                return false;

            var path = uri.AbsolutePath.ToLowerInvariant();

            // Skip common non-article paths
            var skipPatterns = new[] {
                "/about", "/contact", "/privacy", "/terms", "/login", "/register",
                "/search", "/category", "/tag", "/author", "/page/", "/wp-admin",
                "/feed", "/rss", ".xml", ".json", ".css", ".js", ".png", ".jpg",
                "/cdn-cgi/", "/static/", "/assets/", "/images/"
            };
            foreach (var skip in skipPatterns)
            {
                if (path.Contains(skip)) return false;
            }

            // Look for article-like patterns (numbers in URL often indicate article IDs)
            var articlePatterns = new[] {
                "/news/", "/article/", "/story/", "/post/", "/blog/",
                "/national/", "/international/", "/sports/", "/entertainment/",
                "/politics/", "/business/", "/technology/", "/health/",
                "/world/", "/local/", "/latest/"
            };

            // Check if URL contains article patterns
            foreach (var pattern in articlePatterns)
            {
                if (path.Contains(pattern)) return true;
            }

            // Check if URL has numeric ID (common for articles)
            if (System.Text.RegularExpressions.Regex.IsMatch(path, @"/\d{4,}"))
                return true;

            // Check if path has multiple segments (likely content, not homepage)
            var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length >= 2)
                return true;

            return false;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Tests CSS selectors on a target URL (creates its own WebDriver and disposes after)
    /// </summary>
    public async Task<(List<string> links, string? title, string? body)> TestSelectorsAsync(
        string url,
        string articleLinkSelector,
        string titleSelector,
        string bodySelector,
        bool cloudflareBypassEnabled = false,
        CancellationToken cancellationToken = default)
    {
        var links = new List<string>();
        string? title = null;
        string? body = null;

        WebDriverService? webDriver = null;
        try
        {
            webDriver = WebDriverFactory.Create();

            // Navigate to main page
            await webDriver.NavigateToAsync(url, cancellationToken);
            await Task.Delay(1000, cancellationToken);

            // Check for Cloudflare on main page
            var mainPageTitle = webDriver.GetPageTitle();
            if (IsCloudflareTitle(mainPageTitle))
            {
                if (cloudflareBypassEnabled)
                {
                    StatusChanged?.Invoke(this, "Cloudflare detected! Switching to visible browser...");

                    // Close headless browser and switch to visible browser
                    webDriver.ForceKillChrome();
                    await Task.Delay(2000, cancellationToken);

                    var settings = ServiceContainer.Settings.LoadSettings();
                    webDriver = WebDriverFactory.CreateWithUserProfile(timeoutSeconds: settings.BrowserTimeoutSeconds);

                    await webDriver.NavigateToAsync(url, cancellationToken);
                    await Task.Delay(2000, cancellationToken);
                }
                else
                {
                    StatusChanged?.Invoke(this, "Cloudflare detected but bypass is disabled");
                    return (links, null, null);
                }
            }

            // Test article link selector
            if (!string.IsNullOrEmpty(articleLinkSelector))
            {
                links = webDriver.GetAllLinks(articleLinkSelector);
            }

            // If no links found and bypass enabled, try visible browser
            if (links.Count == 0 && cloudflareBypassEnabled && !webDriver.UseUserProfile)
            {
                StatusChanged?.Invoke(this, "No links found, trying visible browser...");

                webDriver.ForceKillChrome();
                await Task.Delay(1000, cancellationToken);

                webDriver = WebDriverFactory.CreateWithUserProfile(timeoutSeconds: 10);
                await webDriver.NavigateToAsync(url, cancellationToken);
                await Task.Delay(2000, cancellationToken);

                if (!string.IsNullOrEmpty(articleLinkSelector))
                {
                    links = webDriver.GetAllLinks(articleLinkSelector);
                }
            }

            // If we found links, navigate to first one to test title/body selectors
            if (links.Count > 0 && (!string.IsNullOrEmpty(titleSelector) || !string.IsNullOrEmpty(bodySelector)))
            {
                var firstLink = NormalizeUrl(links[0], url);
                if (!string.IsNullOrEmpty(firstLink))
                {
                    await webDriver.NavigateToAsync(firstLink, cancellationToken);
                    await Task.Delay(500, cancellationToken);

                    // Check for Cloudflare on article page
                    var articlePageTitle = webDriver.GetPageTitle();
                    if (IsCloudflareTitle(articlePageTitle) && cloudflareBypassEnabled && !webDriver.UseUserProfile)
                    {
                        StatusChanged?.Invoke(this, "Cloudflare detected on article! Switching to visible browser...");

                        webDriver.ForceKillChrome();
                        await Task.Delay(2000, cancellationToken);

                        webDriver = WebDriverFactory.CreateWithUserProfile(timeoutSeconds: 10);
                        await webDriver.NavigateToAsync(firstLink, cancellationToken);
                        await Task.Delay(2000, cancellationToken);
                    }

                    var pageSource = webDriver.GetPageSource();

                    if (!string.IsNullOrEmpty(titleSelector))
                    {
                        title = webDriver.GetElementText(titleSelector);

                        // Fallback: Try HtmlAgilityPack with selector
                        if (string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(pageSource))
                        {
                            var doc = new HtmlAgilityPack.HtmlDocument();
                            doc.LoadHtml(pageSource);
                            var titleNode = doc.DocumentNode.SelectSingleNode(ConvertCssToXPath(titleSelector));
                            title = titleNode?.InnerText?.Trim();
                        }

                        // Fallback: Use browser tab title via Selenium
                        if (string.IsNullOrEmpty(title))
                        {
                            title = webDriver.GetPageTitle();
                        }
                    }

                    if (!string.IsNullOrEmpty(bodySelector))
                    {
                        body = webDriver.GetElementText(bodySelector);

                        // Fallback: Try HtmlAgilityPack with selector
                        if (string.IsNullOrEmpty(body) && !string.IsNullOrEmpty(pageSource))
                        {
                            var doc = new HtmlAgilityPack.HtmlDocument();
                            doc.LoadHtml(pageSource);
                            var bodyNode = doc.DocumentNode.SelectSingleNode(ConvertCssToXPath(bodySelector));
                            body = bodyNode?.InnerText?.Trim();
                        }

                        // Fallback: Use title as body
                        if (string.IsNullOrEmpty(body))
                        {
                            body = title;
                        }
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch
        {
            // Return whatever we got
        }
        finally
        {
            // Always dispose WebDriver after test
            if (webDriver != null)
            {
                WebDriverFactory.SafeDispose(webDriver);
            }
        }

        return (links, title, body);
    }

    private static string? NormalizeUrl(string url, string baseUrl)
    {
        if (string.IsNullOrEmpty(url)) return null;

        // Already absolute
        if (url.StartsWith("http://") || url.StartsWith("https://"))
        {
            return url;
        }

        // Relative URL
        try
        {
            var baseUri = new Uri(baseUrl);
            var absoluteUri = new Uri(baseUri, url);
            return absoluteUri.ToString();
        }
        catch
        {
            return null;
        }
    }

    private static string ConvertCssToXPath(string cssSelector)
    {
        // Basic CSS to XPath conversion for common selectors
        if (string.IsNullOrEmpty(cssSelector)) return "";

        cssSelector = cssSelector.Trim();

        // Class selector: .class
        if (cssSelector.StartsWith("."))
        {
            var className = cssSelector[1..];
            return $"//*[contains(@class, '{className}')]";
        }

        // ID selector: #id
        if (cssSelector.StartsWith("#"))
        {
            var id = cssSelector[1..];
            return $"//*[@id='{id}']";
        }

        // Tag selector: tag
        if (!cssSelector.Contains('.') && !cssSelector.Contains('#') && !cssSelector.Contains('['))
        {
            return $"//{cssSelector}";
        }

        // Tag with class: tag.class
        if (cssSelector.Contains('.') && !cssSelector.StartsWith("."))
        {
            var parts = cssSelector.Split('.');
            var tag = parts[0];
            var className = parts[1];
            return $"//{tag}[contains(@class, '{className}')]";
        }

        // Fallback - try as-is (may not work)
        return $"//{cssSelector}";
    }

    private static string CleanText(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";

        // Remove excessive whitespace
        text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ");
        return text.Trim();
    }
}

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

        // Create WebDriver for this scrape operation - will be disposed at the end
        WebDriverService? webDriver = null;
        try
        {
            webDriver = WebDriverFactory.Create();
            StatusChanged?.Invoke(this, $"Loading: {site.SiteLink}");

            // Navigate to site using async method
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

            // Check for Cloudflare IMMEDIATELY on main page - if detected, switch to user browser
            var mainPageTitle = webDriver.GetPageTitle();
            if (IsCloudflareTitle(mainPageTitle))
            {
                StatusChanged?.Invoke(this, $"Cloudflare detected on main page! Switching to user browser...");

                // Force close browser - ForceKillChrome uses taskkill to kill process tree
                webDriver.ForceKillChrome();

                // Wait for Chrome to fully terminate
                await Task.Delay(2000, cancellationToken);

                // Create user browser instead
                var settings = ServiceContainer.Settings.LoadSettings();
                webDriver = WebDriverFactory.CreateWithUserProfile(timeoutSeconds: settings.BrowserTimeoutSeconds);

                // Navigate to site with user browser
                StatusChanged?.Invoke(this, $"Loading with user browser: {site.SiteLink}");
                await webDriver.NavigateToAsync(site.SiteLink, cancellationToken);

                try
                {
                    await webDriver.WaitForPageLoadAsync(30, cancellationToken);
                }
                catch (OperationCanceledException) { throw; }
                catch { }

                await Task.Delay(2000, cancellationToken);
            }

            // Scroll to trigger dynamic content loading
            StatusChanged?.Invoke(this, $"Triggering dynamic content: {site.SiteName}");
            await webDriver.ScrollAndWaitAsync(2, 300, cancellationToken);

            // Wait for article element to appear
            StatusChanged?.Invoke(this, $"Waiting for element: {site.SiteName}");
            var elementFound = await webDriver.TryWaitForElementAsync(site.ArticleLinkSelector, 10, cancellationToken);

            if (!elementFound)
            {
                StatusChanged?.Invoke(this, $"Element not found, retrying: {site.SiteName}");
                // Retry with more scrolling
                await webDriver.ScrollAndWaitAsync(3, 500, cancellationToken);
                await Task.Delay(1000, cancellationToken);
            }

            // Find the first article link
            StatusChanged?.Invoke(this, $"Finding first article on: {site.SiteName}");
            var firstLink = webDriver.GetFirstLink(site.ArticleLinkSelector);

            if (string.IsNullOrEmpty(firstLink))
            {
                StatusChanged?.Invoke(this, $"No article found on: {site.SiteName}");
                ServiceContainer.Database.UpdateSiteStats(site.SiteId, true);
                return ScrapeResult.Succeeded(articles, newCount, skippedCount, DateTime.Now - startTime);
            }

            cancellationToken.ThrowIfCancellationRequested();

            // Normalize URL
            var articleUrl = NormalizeUrl(firstLink, site.SiteLink);
            if (string.IsNullOrEmpty(articleUrl))
            {
                StatusChanged?.Invoke(this, $"Invalid article URL on: {site.SiteName}");
                ServiceContainer.Database.UpdateSiteStats(site.SiteId, true);
                return ScrapeResult.Succeeded(articles, newCount, skippedCount, DateTime.Now - startTime);
            }

            // Fetch article content (using the same WebDriver instance)
            StatusChanged?.Invoke(this, $"Fetching article details...");
            var article = await ScrapeArticleInternalAsync(webDriver, site, articleUrl, cancellationToken);

            if (article == null)
            {
                StatusChanged?.Invoke(this, $"Failed to get article details on: {site.SiteName}");
                ServiceContainer.Database.UpdateSiteStats(site.SiteId, true);
                return ScrapeResult.Succeeded(articles, newCount, skippedCount, DateTime.Now - startTime);
            }

            // Log article info for debugging
            StatusChanged?.Invoke(this, $"Link: {articleUrl}");
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
    /// Internal method to scrape article using an existing WebDriver instance
    /// </summary>
    private async Task<NewsInfo?> ScrapeArticleInternalAsync(
        WebDriverService webDriver,
        SiteInfo site,
        string url,
        CancellationToken cancellationToken,
        bool isUserBrowserRetry = false)
    {
        try
        {
            // Navigate to article (skip if already loaded in user's browser)
            if (!isUserBrowserRetry)
            {
                StatusChanged?.Invoke(this, $"Navigating to article: {url}");
                await webDriver.NavigateToAsync(url, cancellationToken);

                // Wait for page to load
                StatusChanged?.Invoke(this, $"Waiting for article page load...");
                try
                {
                    await webDriver.WaitForPageLoadAsync(15, cancellationToken);
                }
                catch (OperationCanceledException) { throw; }
                catch { }

                await Task.Delay(2000, cancellationToken);

                // Check for Cloudflare IMMEDIATELY - if detected, switch to visible browser right away
                // Skip if already using user browser (from main page detection)
                var browserTitle = webDriver.GetPageTitle();
                if (IsCloudflareTitle(browserTitle) && !webDriver.UseUserProfile)
                {
                    StatusChanged?.Invoke(this, $"Cloudflare detected! Opening your Chrome browser...");
                    return await RetryWithUserBrowserAsync(site, url, cancellationToken, webDriver);
                }
            }

            var pageSource = webDriver.GetPageSource();
            StatusChanged?.Invoke(this, $"Page source length: {pageSource?.Length ?? 0} chars");

            string title = "";
            string body = "";

            // Extract title using multiple methods
            title = ExtractTitle(webDriver, site.TitleSelector, pageSource);

            // If title extraction failed completely and this is user's browser, try browser tab title
            if (string.IsNullOrEmpty(title) && (isUserBrowserRetry || webDriver.UseUserProfile))
            {
                // Try browser tab title as last resort
                title = webDriver.GetPageTitle();
                if (!string.IsNullOrEmpty(title) && !IsCloudflareTitle(title))
                {
                    StatusChanged?.Invoke(this, $"Title from browser tab: {title.Substring(0, Math.Min(50, title.Length))}...");
                }
            }

            // Still no title from headless browser - try visible browser
            // Skip if already using user browser (from main page detection or retry)
            if ((string.IsNullOrEmpty(title) || IsCloudflareTitle(title)) && !isUserBrowserRetry && !webDriver.UseUserProfile)
            {
                StatusChanged?.Invoke(this, $"Cannot get title, trying visible browser...");
                return await RetryWithUserBrowserAsync(site, url, cancellationToken, webDriver);
            }

            // Extract body content if selector is provided
            if (!string.IsNullOrEmpty(site.BodySelector))
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

            // FALLBACK: If body is still empty, use timestamp as body text
            var createdAt = DateTime.Now;
            if (string.IsNullOrEmpty(body))
            {
                StatusChanged?.Invoke(this, $"Body empty, using timestamp as fallback");
                body = createdAt.ToString("yyyy-MM-dd HH:mm:ss");
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
                body = createdAt.ToString("yyyy-MM-dd HH:mm:ss");
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
    /// Tests CSS selectors on a target URL (creates its own WebDriver and disposes after)
    /// </summary>
    public async Task<(List<string> links, string? title, string? body)> TestSelectorsAsync(
        string url,
        string articleLinkSelector,
        string titleSelector,
        string bodySelector,
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

            // Test article link selector
            if (!string.IsNullOrEmpty(articleLinkSelector))
            {
                links = webDriver.GetAllLinks(articleLinkSelector);
            }

            // If we found links, navigate to first one to test title/body selectors
            if (links.Count > 0 && (!string.IsNullOrEmpty(titleSelector) || !string.IsNullOrEmpty(bodySelector)))
            {
                var firstLink = NormalizeUrl(links[0], url);
                if (!string.IsNullOrEmpty(firstLink))
                {
                    await webDriver.NavigateToAsync(firstLink, cancellationToken);
                    await Task.Delay(500, cancellationToken);

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

                        // Fallback: Use timestamp as body
                        if (string.IsNullOrEmpty(body))
                        {
                            body = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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

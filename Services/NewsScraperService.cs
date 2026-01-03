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
                skippedCount++;
                StatusChanged?.Invoke(this, $"Already exists, skipping: {site.SiteName}");
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
        CancellationToken cancellationToken)
    {
        try
        {
            StatusChanged?.Invoke(this, $"Navigating to article: {url}");
            await webDriver.NavigateToAsync(url, cancellationToken);

            // Wait for article page to load
            StatusChanged?.Invoke(this, $"Waiting for article page load...");
            try
            {
                await webDriver.WaitForPageLoadAsync(15, cancellationToken);
            }
            catch (OperationCanceledException) { throw; }
            catch { /* Page load timeout is acceptable */ }

            // Wait for JavaScript to finish rendering
            await Task.Delay(2000, cancellationToken);

            var pageSource = webDriver.GetPageSource();
            StatusChanged?.Invoke(this, $"Page source length: {pageSource?.Length ?? 0} chars");

            string title = "";
            string body = "";

            // Try getting title with Selenium first (handles dynamic content)
            StatusChanged?.Invoke(this, $"Trying Selenium for title with selector: {site.TitleSelector}");
            title = webDriver.GetElementText(site.TitleSelector) ?? "";

            if (!string.IsNullOrEmpty(title))
            {
                StatusChanged?.Invoke(this, $"Title found via Selenium: {title.Substring(0, Math.Min(50, title.Length))}...");
            }
            else
            {
                StatusChanged?.Invoke(this, $"Selenium failed, trying HtmlAgilityPack...");

                // Fallback to HtmlAgilityPack for static HTML parsing
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(pageSource);

                var xpath = ConvertCssToXPath(site.TitleSelector);
                StatusChanged?.Invoke(this, $"XPath: {xpath}");

                var titleNode = doc.DocumentNode.SelectSingleNode(xpath);
                title = titleNode?.InnerText?.Trim() ?? "";

                if (!string.IsNullOrEmpty(title))
                {
                    StatusChanged?.Invoke(this, $"Title found via HtmlAgilityPack");
                }
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

            if (string.IsNullOrEmpty(title))
            {
                StatusChanged?.Invoke(this, $"Title is empty, article scrape failed");
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
                CreatedAt = DateTime.Now
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

                    if (!string.IsNullOrEmpty(titleSelector))
                    {
                        title = webDriver.GetElementText(titleSelector);
                    }

                    if (!string.IsNullOrEmpty(bodySelector))
                    {
                        body = webDriver.GetElementText(bodySelector);
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

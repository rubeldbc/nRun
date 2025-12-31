using nRun.Models;

namespace nRun.Services;

/// <summary>
/// Service for scraping news articles from configured sites
/// </summary>
public class NewsScraperService : IDisposable
{
    private readonly WebDriverService _webDriver;
    private bool _disposed;

    public event EventHandler<NewsInfo>? ArticleScraped;
    public event EventHandler<string>? StatusChanged;
    public event EventHandler<(int current, int total)>? ProgressChanged;

    public NewsScraperService()
    {
        var settings = ServiceContainer.Settings.LoadSettings();
        _webDriver = new WebDriverService
        {
            UseHeadless = settings.UseHeadlessBrowser,
            TimeoutSeconds = settings.BrowserTimeoutSeconds
        };
    }

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
        var settings = ServiceContainer.Settings.LoadSettings();

        try
        {
            StatusChanged?.Invoke(this, $"Loading: {site.SiteLink}");

            // Navigate to site
            await Task.Run(() => _webDriver.NavigateTo(site.SiteLink), cancellationToken);

            // Wait for page to fully load
            StatusChanged?.Invoke(this, $"Waiting for page load: {site.SiteName}");
            await Task.Run(() =>
            {
                try { _webDriver.WaitForPageLoad(15); } catch { }
            }, cancellationToken);

            // Wait for JavaScript content to render
            await Task.Delay(2000, cancellationToken);

            // Scroll to trigger dynamic content loading
            StatusChanged?.Invoke(this, $"Triggering dynamic content: {site.SiteName}");
            await Task.Run(() => _webDriver.ScrollAndWait(2, 300), cancellationToken);

            // Wait for article element to appear
            StatusChanged?.Invoke(this, $"Waiting for element: {site.SiteName}");
            var elementFound = await Task.Run(() => _webDriver.TryWaitForElement(site.ArticleLinkSelector, 10), cancellationToken);

            if (!elementFound)
            {
                StatusChanged?.Invoke(this, $"Element not found, retrying: {site.SiteName}");
                // Retry with more scrolling
                await Task.Run(() => _webDriver.ScrollAndWait(3, 500), cancellationToken);
                await Task.Delay(1000, cancellationToken);
            }

            // Find the first article link
            StatusChanged?.Invoke(this, $"Finding first article on: {site.SiteName}");
            var firstLink = _webDriver.GetFirstLink(site.ArticleLinkSelector);

            if (string.IsNullOrEmpty(firstLink))
            {
                StatusChanged?.Invoke(this, $"No article found on: {site.SiteName}");
                ServiceContainer.Database.UpdateSiteStats(site.SiteId, true);
                return ScrapeResult.Succeeded(articles, newCount, skippedCount, DateTime.Now - startTime);
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return ScrapeResult.Succeeded(articles, newCount, skippedCount, DateTime.Now - startTime);
            }

            // Normalize URL
            var articleUrl = NormalizeUrl(firstLink, site.SiteLink);
            if (string.IsNullOrEmpty(articleUrl))
            {
                StatusChanged?.Invoke(this, $"Invalid article URL on: {site.SiteName}");
                ServiceContainer.Database.UpdateSiteStats(site.SiteId, true);
                return ScrapeResult.Succeeded(articles, newCount, skippedCount, DateTime.Now - startTime);
            }

            // Fetch article content
            StatusChanged?.Invoke(this, $"Fetching article details...");
            var article = await ScrapeArticleAsync(site, articleUrl, cancellationToken);

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
        catch (Exception ex)
        {
            StatusChanged?.Invoke(this, $"Error: {ex.Message}");
            ServiceContainer.Database.UpdateSiteStats(site.SiteId, false);
            return ScrapeResult.Failed(ex.Message);
        }
    }

    private async Task<NewsInfo?> ScrapeArticleAsync(SiteInfo site, string url, CancellationToken cancellationToken)
    {
        try
        {
            StatusChanged?.Invoke(this, $"Navigating to article: {url}");
            await Task.Run(() => _webDriver.NavigateTo(url), cancellationToken);

            // Wait for article page to load
            StatusChanged?.Invoke(this, $"Waiting for article page load...");
            await Task.Run(() =>
            {
                try { _webDriver.WaitForPageLoad(15); } catch { }
            }, cancellationToken);

            // Wait for JavaScript to finish rendering
            await Task.Delay(2000, cancellationToken);

            var pageSource = _webDriver.GetPageSource();
            StatusChanged?.Invoke(this, $"Page source length: {pageSource?.Length ?? 0} chars");

            string title = "";
            string body = "";

            // Try getting title with Selenium first (handles dynamic content)
            StatusChanged?.Invoke(this, $"Trying Selenium for title with selector: {site.TitleSelector}");
            title = _webDriver.GetElementText(site.TitleSelector) ?? "";

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
                body = _webDriver.GetElementText(site.BodySelector) ?? "";

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
                NewsTitle = CleanText(title),
                NewsUrl = url,
                NewsText = CleanText(body),
                CreatedAt = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            StatusChanged?.Invoke(this, $"ScrapeArticle error: {ex.Message}");
            return null;
        }
    }

    public async Task<(List<string> links, string? title, string? body)> TestSelectorsAsync(
        string url,
        string articleLinkSelector,
        string titleSelector,
        string bodySelector)
    {
        var links = new List<string>();
        string? title = null;
        string? body = null;

        try
        {
            // Navigate to main page
            await Task.Run(() => _webDriver.NavigateTo(url));
            await Task.Delay(1000);

            // Test article link selector
            if (!string.IsNullOrEmpty(articleLinkSelector))
            {
                links = _webDriver.GetAllLinks(articleLinkSelector);
            }

            // If we found links, navigate to first one to test title/body selectors
            if (links.Count > 0 && (!string.IsNullOrEmpty(titleSelector) || !string.IsNullOrEmpty(bodySelector)))
            {
                var firstLink = NormalizeUrl(links[0], url);
                if (!string.IsNullOrEmpty(firstLink))
                {
                    await Task.Run(() => _webDriver.NavigateTo(firstLink));
                    await Task.Delay(500);

                    if (!string.IsNullOrEmpty(titleSelector))
                    {
                        title = _webDriver.GetElementText(titleSelector);
                    }

                    if (!string.IsNullOrEmpty(bodySelector))
                    {
                        body = _webDriver.GetElementText(bodySelector);
                    }
                }
            }
        }
        catch
        {
            // Return whatever we got
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

    public void ResetBrowser()
    {
        _webDriver.ResetDriver();
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            try
            {
                _webDriver.Dispose();
            }
            catch { }
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }

    ~NewsScraperService()
    {
        Dispose();
    }
}

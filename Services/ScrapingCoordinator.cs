using nRun.Models;

namespace nRun.Services;

/// <summary>
/// Coordinates news scraping workflow with proper resource management.
/// Provides a high-level API for scraping operations that handles
/// WebDriver lifecycle, progress tracking, and cleanup internally.
/// </summary>
public class ScrapingCoordinator : IDisposable
{
    private readonly BackgroundScraperService _backgroundScraper;
    private readonly NewsScraperService _oneTimeScraper;
    private bool _disposed;

    // Store event handler references for cleanup
    private readonly EventHandler<NewsInfo> _bgArticleScrapedHandler;
    private readonly EventHandler<string> _bgStatusChangedHandler;
    private readonly EventHandler<(int current, int total)> _bgProgressChangedHandler;
    private readonly EventHandler<bool> _bgRunningStateChangedHandler;
    private readonly EventHandler<NewsInfo> _otArticleScrapedHandler;
    private readonly EventHandler<string> _otStatusChangedHandler;
    private readonly EventHandler<(int current, int total)> _otProgressChangedHandler;

    public event EventHandler<NewsInfo>? ArticleScraped;
    public event EventHandler<string>? StatusChanged;
    public event EventHandler<(int current, int total)>? ProgressChanged;
    public event EventHandler<bool>? RunningStateChanged;
    public event EventHandler<Exception>? ErrorOccurred;

    /// <summary>
    /// Gets whether background scraping is currently running
    /// </summary>
    public bool IsBackgroundRunning => _backgroundScraper.IsRunning;

    public ScrapingCoordinator()
    {
        _backgroundScraper = new BackgroundScraperService();
        _oneTimeScraper = new NewsScraperService();

        // Create and store handler references for proper cleanup
        _bgArticleScrapedHandler = (s, e) => ArticleScraped?.Invoke(this, e);
        _bgStatusChangedHandler = (s, e) => StatusChanged?.Invoke(this, e);
        _bgProgressChangedHandler = (s, e) => ProgressChanged?.Invoke(this, e);
        _bgRunningStateChangedHandler = (s, e) => RunningStateChanged?.Invoke(this, e);
        _otArticleScrapedHandler = (s, e) => ArticleScraped?.Invoke(this, e);
        _otStatusChangedHandler = (s, e) => StatusChanged?.Invoke(this, e);
        _otProgressChangedHandler = (s, e) => ProgressChanged?.Invoke(this, e);

        // Wire up events
        _backgroundScraper.ArticleScraped += _bgArticleScrapedHandler;
        _backgroundScraper.StatusChanged += _bgStatusChangedHandler;
        _backgroundScraper.ProgressChanged += _bgProgressChangedHandler;
        _backgroundScraper.RunningStateChanged += _bgRunningStateChangedHandler;

        _oneTimeScraper.ArticleScraped += _otArticleScrapedHandler;
        _oneTimeScraper.StatusChanged += _otStatusChangedHandler;
        _oneTimeScraper.ProgressChanged += _otProgressChangedHandler;
    }

    /// <summary>
    /// Starts background scraping with configured interval
    /// </summary>
    public void StartBackgroundScraping()
    {
        if (!ServiceContainer.Database.IsConnected)
        {
            StatusChanged?.Invoke(this, "Cannot start: Database not connected");
            return;
        }

        _backgroundScraper.Start();
    }

    /// <summary>
    /// Stops background scraping
    /// </summary>
    public void StopBackgroundScraping()
    {
        _backgroundScraper.Stop();
    }

    /// <summary>
    /// Updates the background scraping interval from settings
    /// </summary>
    public void UpdateInterval()
    {
        _backgroundScraper.UpdateInterval();
    }

    /// <summary>
    /// Scrapes all active sites once (not in background)
    /// </summary>
    public async Task<ScrapeResult> ScrapeAllSitesOnceAsync(CancellationToken cancellationToken = default)
    {
        if (!ServiceContainer.Database.IsConnected)
        {
            StatusChanged?.Invoke(this, "Cannot scrape: Database not connected");
            return ScrapeResult.Failed("Database not connected");
        }

        try
        {
            return await _oneTimeScraper.ScrapeAllSitesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            ErrorOccurred?.Invoke(this, ex);
            return ScrapeResult.Failed(ex.Message);
        }
        finally
        {
            // Ensure cleanup after one-time scrape
            WebDriverFactory.DisposeAll();
        }
    }

    /// <summary>
    /// Scrapes a single site once
    /// </summary>
    public async Task<ScrapeResult> ScrapeSiteOnceAsync(SiteInfo site, CancellationToken cancellationToken = default)
    {
        if (!ServiceContainer.Database.IsConnected)
        {
            StatusChanged?.Invoke(this, "Cannot scrape: Database not connected");
            return ScrapeResult.Failed("Database not connected");
        }

        try
        {
            ProgressChanged?.Invoke(this, (1, 1));
            return await _oneTimeScraper.ScrapeSiteAsync(site, cancellationToken);
        }
        catch (Exception ex)
        {
            ErrorOccurred?.Invoke(this, ex);
            return ScrapeResult.Failed(ex.Message);
        }
        finally
        {
            // Ensure cleanup after one-time scrape
            WebDriverFactory.DisposeAll();
        }
    }

    /// <summary>
    /// Tests CSS selectors on a target URL
    /// </summary>
    public async Task<(List<string> links, string? title, string? body)> TestSelectorsAsync(
        string url,
        string articleLinkSelector,
        string titleSelector,
        string bodySelector,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _oneTimeScraper.TestSelectorsAsync(
                url, articleLinkSelector, titleSelector, bodySelector, cancellationToken);
        }
        catch (Exception ex)
        {
            ErrorOccurred?.Invoke(this, ex);
            return (new List<string>(), null, null);
        }
        finally
        {
            // Ensure cleanup after test
            WebDriverFactory.DisposeAll();
        }
    }

    /// <summary>
    /// Runs a single background scrape cycle
    /// </summary>
    public async Task RunBackgroundCycleAsync()
    {
        if (!ServiceContainer.Database.IsConnected)
        {
            StatusChanged?.Invoke(this, "Cannot scrape: Database not connected");
            return;
        }

        try
        {
            await _backgroundScraper.RunOnceAsync();
        }
        catch (Exception ex)
        {
            ErrorOccurred?.Invoke(this, ex);
        }
    }

    /// <summary>
    /// Gets the current status of active WebDriver instances
    /// </summary>
    public (int activeDrivers, int trackedProcesses) GetResourceStatus()
    {
        return (
            WebDriverFactory.ActiveDriverCount,
            ProcessCleanupService.TrackedProcessCount
        );
    }

    /// <summary>
    /// Forces cleanup of all WebDriver resources
    /// </summary>
    public void ForceCleanup()
    {
        WebDriverFactory.DisposeAll();
        ProcessCleanupService.ForceCleanupAll();
        StatusChanged?.Invoke(this, "Resource cleanup completed");
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            // Unsubscribe from events to prevent memory leaks
            _backgroundScraper.ArticleScraped -= _bgArticleScrapedHandler;
            _backgroundScraper.StatusChanged -= _bgStatusChangedHandler;
            _backgroundScraper.ProgressChanged -= _bgProgressChangedHandler;
            _backgroundScraper.RunningStateChanged -= _bgRunningStateChangedHandler;

            _oneTimeScraper.ArticleScraped -= _otArticleScrapedHandler;
            _oneTimeScraper.StatusChanged -= _otStatusChangedHandler;
            _oneTimeScraper.ProgressChanged -= _otProgressChangedHandler;

            StopBackgroundScraping();
            _backgroundScraper.Dispose();

            // Final cleanup
            WebDriverFactory.DisposeAll();
            ProcessCleanupService.ForceCleanupAll();

            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }

    ~ScrapingCoordinator()
    {
        Dispose();
    }
}

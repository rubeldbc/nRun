using nRun.Models;

namespace nRun.Services;

/// <summary>
/// Background service that periodically scrapes news from all active sites.
/// Creates a fresh NewsScraperService instance for each scraping cycle to ensure
/// WebDriver resources are properly cleaned up between runs.
/// </summary>
public class BackgroundScraperService : IDisposable
{
    private System.Threading.Timer? _timer;
    private CancellationTokenSource? _cts;
    private bool _isRunning;
    private bool _disposed;
    private readonly object _lock = new();
    private bool _isExecuting; // Prevent concurrent executions

    public bool IsRunning => _isRunning;

    public event EventHandler<NewsInfo>? ArticleScraped;
    public event EventHandler<string>? StatusChanged;
    public event EventHandler<(int current, int total)>? ProgressChanged;
    public event EventHandler<bool>? RunningStateChanged;

    public void Start()
    {
        lock (_lock)
        {
            if (_isRunning) return;

            var settings = ServiceContainer.Settings.LoadSettings();
            var intervalMs = settings.CheckIntervalMinutes * 60 * 1000;

            _cts = new CancellationTokenSource();

            _timer = new System.Threading.Timer(
                async _ => await ExecuteScrapeAsync(),
                null,
                0, // Start immediately
                intervalMs
            );

            _isRunning = true;
            RunningStateChanged?.Invoke(this, true);
            StatusChanged?.Invoke(this, "Background scraping started");
        }
    }

    public void Stop()
    {
        // Don't lock during the entire stop operation - it can cause deadlocks
        if (!_isRunning) return;

        _isRunning = false;
        StatusChanged?.Invoke(this, "Stopping scraper...");

        // Cancel any ongoing operations immediately
        try
        {
            _cts?.Cancel();
        }
        catch { }

        // Stop the timer immediately (prevents new scrapes from starting)
        try
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
        }
        catch { }

        // CRITICAL: Force kill all WebDriver processes immediately on a background thread
        // This prevents freezing - don't wait for graceful shutdown
        _ = Task.Run(() =>
        {
            try
            {
                // Force dispose all WebDriver instances
                WebDriverFactory.DisposeAll();
                // Force kill any remaining Chrome/ChromeDriver processes
                ProcessCleanupService.ForceCleanupAll();
            }
            catch { }
        });

        // Dispose timer on background thread to avoid blocking UI
        var timerToDispose = _timer;
        _timer = null;
        if (timerToDispose != null)
        {
            _ = Task.Run(() =>
            {
                try { timerToDispose.Dispose(); } catch { }
            });
        }

        // Dispose CTS on background thread
        var ctsToDispose = _cts;
        _cts = null;
        if (ctsToDispose != null)
        {
            _ = Task.Run(() =>
            {
                try { ctsToDispose.Dispose(); } catch { }
            });
        }

        _isExecuting = false;
        RunningStateChanged?.Invoke(this, false);
        StatusChanged?.Invoke(this, "Background scraping stopped");
    }

    public void UpdateInterval()
    {
        lock (_lock)
        {
            if (!_isRunning || _timer == null) return;

            var settings = ServiceContainer.Settings.LoadSettings();
            var intervalMs = settings.CheckIntervalMinutes * 60 * 1000;
            _timer.Change(intervalMs, intervalMs);

            StatusChanged?.Invoke(this, $"Interval updated to {settings.CheckIntervalMinutes} minutes");
        }
    }

    public async Task RunOnceAsync()
    {
        var scraper = new NewsScraperService();
        scraper.ArticleScraped += (s, e) => ArticleScraped?.Invoke(this, e);
        scraper.StatusChanged += (s, e) => StatusChanged?.Invoke(this, e);
        scraper.ProgressChanged += (s, e) => ProgressChanged?.Invoke(this, e);

        try
        {
            await scraper.ScrapeAllSitesAsync(_cts?.Token ?? CancellationToken.None);
        }
        finally
        {
            // Ensure all WebDrivers are cleaned up
            WebDriverFactory.DisposeAll();
        }
    }

    /// <summary>
    /// Run scraper for a single site once
    /// </summary>
    public async Task RunSiteOnceAsync(SiteInfo site)
    {
        var scraper = new NewsScraperService();
        scraper.ArticleScraped += (s, e) => ArticleScraped?.Invoke(this, e);
        scraper.StatusChanged += (s, e) => StatusChanged?.Invoke(this, e);
        scraper.ProgressChanged += (s, e) => ProgressChanged?.Invoke(this, e);

        try
        {
            ProgressChanged?.Invoke(this, (1, 1));
            await scraper.ScrapeSiteAsync(site, _cts?.Token ?? CancellationToken.None);
        }
        finally
        {
            // Ensure WebDriver is cleaned up
            // Note: ScrapeSiteAsync already disposes its WebDriver in finally block,
            // but we call DisposeAll() as an extra safety measure
            WebDriverFactory.DisposeAll();
        }
    }

    private async Task ExecuteScrapeAsync()
    {
        if (_cts == null) return;

        // Prevent concurrent executions
        if (_isExecuting) return;

        _isExecuting = true;
        try
        {
            StatusChanged?.Invoke(this, "Starting scheduled scrape...");

            var scraper = new NewsScraperService();
            scraper.ArticleScraped += (s, e) => ArticleScraped?.Invoke(this, e);
            scraper.StatusChanged += (s, e) => StatusChanged?.Invoke(this, e);
            scraper.ProgressChanged += (s, e) => ProgressChanged?.Invoke(this, e);

            await scraper.ScrapeAllSitesAsync(_cts.Token);
        }
        catch (OperationCanceledException)
        {
            StatusChanged?.Invoke(this, "Scrape cancelled");
        }
        catch (Exception ex)
        {
            StatusChanged?.Invoke(this, $"Scrape error: {ex.Message}");
        }
        finally
        {
            _isExecuting = false;
            // Cleanup any remaining WebDriver instances after each scheduled run
            WebDriverFactory.DisposeAll();
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Stop();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }

    ~BackgroundScraperService()
    {
        Dispose();
    }
}

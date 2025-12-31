using nRun.Models;

namespace nRun.Services;

/// <summary>
/// Background service that periodically scrapes news from all active sites
/// </summary>
public class BackgroundScraperService : IDisposable
{
    private System.Threading.Timer? _timer;
    private NewsScraperService? _scraper;
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
            _scraper = new NewsScraperService();
            _scraper.ArticleScraped += (s, e) => ArticleScraped?.Invoke(this, e);
            _scraper.StatusChanged += (s, e) => StatusChanged?.Invoke(this, e);
            _scraper.ProgressChanged += (s, e) => ProgressChanged?.Invoke(this, e);

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
        lock (_lock)
        {
            if (!_isRunning) return;

            // Cancel any ongoing operations
            try
            {
                _cts?.Cancel();
            }
            catch { }

            // Stop and dispose the timer
            try
            {
                _timer?.Change(Timeout.Infinite, Timeout.Infinite);
                _timer?.Dispose();
            }
            catch { }
            finally
            {
                _timer = null;
            }

            // Wait a bit for any executing operation to notice cancellation
            // but don't wait too long
            var waitCount = 0;
            while (_isExecuting && waitCount < 10)
            {
                Thread.Sleep(100);
                waitCount++;
            }

            // Dispose the scraper (this will close Chrome)
            try
            {
                _scraper?.Dispose();
            }
            catch { }
            finally
            {
                _scraper = null;
            }

            // Dispose the cancellation token source
            try
            {
                _cts?.Dispose();
            }
            catch { }
            finally
            {
                _cts = null;
            }

            _isRunning = false;
            RunningStateChanged?.Invoke(this, false);
            StatusChanged?.Invoke(this, "Background scraping stopped");
        }
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
        if (_scraper != null && _cts != null)
        {
            await _scraper.ScrapeAllSitesAsync(_cts.Token);
        }
        else
        {
            // Run without background service
            using var scraper = new NewsScraperService();
            scraper.ArticleScraped += (s, e) => ArticleScraped?.Invoke(this, e);
            scraper.StatusChanged += (s, e) => StatusChanged?.Invoke(this, e);
            scraper.ProgressChanged += (s, e) => ProgressChanged?.Invoke(this, e);

            await scraper.ScrapeAllSitesAsync();
        }
    }

    /// <summary>
    /// Run scraper for a single site once
    /// </summary>
    public async Task RunSiteOnceAsync(SiteInfo site)
    {
        if (_scraper != null && _cts != null)
        {
            ProgressChanged?.Invoke(this, (1, 1));
            await _scraper.ScrapeSiteAsync(site, _cts.Token);
        }
        else
        {
            // Run without background service
            using var scraper = new NewsScraperService();
            scraper.ArticleScraped += (s, e) => ArticleScraped?.Invoke(this, e);
            scraper.StatusChanged += (s, e) => StatusChanged?.Invoke(this, e);
            scraper.ProgressChanged += (s, e) => ProgressChanged?.Invoke(this, e);

            ProgressChanged?.Invoke(this, (1, 1));
            await scraper.ScrapeSiteAsync(site);
        }
    }

    private async Task ExecuteScrapeAsync()
    {
        if (_scraper == null || _cts == null) return;

        // Prevent concurrent executions
        if (_isExecuting) return;

        _isExecuting = true;
        try
        {
            StatusChanged?.Invoke(this, "Starting scheduled scrape...");
            await _scraper.ScrapeAllSitesAsync(_cts.Token);
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

using nRun.Data;
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

            var settings = SettingsManager.LoadSettings();
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

            _cts?.Cancel();
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            _timer?.Dispose();
            _timer = null;

            _scraper?.Dispose();
            _scraper = null;

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

            var settings = SettingsManager.LoadSettings();
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
    /// ????????? ???? ????????? ???? ??????? ???
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
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Stop();
            _cts?.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}

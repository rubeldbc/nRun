using nRun.Models;

namespace nRun.Services;

/// <summary>
/// Background service for collecting TikTok data on schedule
/// </summary>
public class TikTokDataCollectionService : IDisposable
{
    private TikTokScraperService? _scraper;
    private CancellationTokenSource? _cts;
    private Task? _runningTask;
    private bool _disposed;
    private readonly object _lock = new();
    private readonly RateLimiter _rateLimiter;

    private List<TkSchedule> _schedules = new();

    public bool IsRunning { get; private set; }
    public bool SkipInitialCollection { get; set; }

    public event EventHandler<string>? StatusChanged;
    public event EventHandler<(int current, int total)>? ProgressChanged;
    public event EventHandler<bool>? RunningStateChanged;
    public event EventHandler<TkData>? DataCollected;

    public TikTokDataCollectionService()
    {
        _rateLimiter = new RateLimiter(10, 5); // 10s base + 0-5s jitter
    }

    public void UpdateSchedules(List<TkSchedule> schedules)
    {
        lock (_lock)
        {
            _schedules = schedules.Where(s => s.IsActive).ToList();
        }
    }

    public void UpdateDelaySeconds(int seconds)
    {
        _rateLimiter.BaseDelaySeconds = Math.Max(1, seconds);
        _rateLimiter.JitterMaxSeconds = Math.Max(1, seconds / 2); // Jitter is half of base
    }

    public void Start()
    {
        if (IsRunning) return;

        _cts = new CancellationTokenSource();
        IsRunning = true;
        OnRunningStateChanged(true);
        OnStatusChanged("TikTok data collection started");

        _runningTask = Task.Run(() => RunCollectionLoop(_cts.Token));
    }

    public void Stop()
    {
        if (!IsRunning) return;

        IsRunning = false;
        OnStatusChanged("Stopping TikTok data collection...");

        // Cancel ongoing operations
        try
        {
            _cts?.Cancel();
        }
        catch { }

        // Force cleanup on background thread - don't block UI
        _ = Task.Run(() =>
        {
            try
            {
                // Force dispose all WebDriver instances immediately
                WebDriverFactory.DisposeAll();
                ProcessCleanupService.ForceCleanupAll();
            }
            catch { }
        });

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

        OnRunningStateChanged(false);
        OnStatusChanged("TikTok data collection stopped");
    }

    private async Task RunCollectionLoop(CancellationToken ct)
    {
        // Fetch data immediately on start unless SkipInitialCollection is set
        if (!SkipInitialCollection)
        {
            try
            {
                OnStatusChanged("Running initial data collection...");
                await RunDataCollection(ct).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                OnStatusChanged($"Error in initial collection: {ex.Message}");
            }
        }
        else
        {
            OnStatusChanged("Waiting for scheduled time...");
        }

        // Reset the flag for future starts
        SkipInitialCollection = false;

        // Then wait for scheduled times
        while (!ct.IsCancellationRequested)
        {
            try
            {
                // Check if current time matches any schedule
                var now = DateTime.Now;
                var currentTime = now.TimeOfDay;

                List<TkSchedule> activeSchedules;
                lock (_lock)
                {
                    activeSchedules = _schedules.ToList();
                }

                bool shouldRun = false;
                foreach (var schedule in activeSchedules)
                {
                    // Check if within 1 minute of scheduled time
                    var diff = Math.Abs((currentTime - schedule.Timing).TotalMinutes);
                    if (diff < 1)
                    {
                        shouldRun = true;
                        OnStatusChanged($"Schedule triggered: {schedule.TimingDisplay}");
                        break;
                    }
                }

                if (shouldRun)
                {
                    await RunDataCollection(ct).ConfigureAwait(false);
                    // Wait at least 2 minutes after collection to avoid re-triggering
                    await Task.Delay(TimeSpan.FromMinutes(2), ct).ConfigureAwait(false);
                }
                else
                {
                    // Check every 30 seconds for schedule match
                    await Task.Delay(TimeSpan.FromSeconds(30), ct).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                OnStatusChanged($"Error in collection loop: {ex.Message}");
                await Task.Delay(TimeSpan.FromMinutes(1), ct).ConfigureAwait(false);
            }
        }
    }

    public async Task RunDataCollectionNow(CancellationToken ct = default)
    {
        await RunDataCollection(ct).ConfigureAwait(false);
    }

    private async Task RunDataCollection(CancellationToken ct)
    {
        try
        {
            var profiles = ServiceContainer.Database.GetActiveTkProfiles();
            if (profiles.Count == 0)
            {
                OnStatusChanged("No active TikTok profiles to collect data for");
                return;
            }

            OnStatusChanged($"Starting data collection for {profiles.Count} profiles...");
            OnProgressChanged(0, profiles.Count);

            _scraper ??= new TikTokScraperService();
            int savedCount = 0;

            for (int i = 0; i < profiles.Count; i++)
            {
                if (ct.IsCancellationRequested) break;

                var profile = profiles[i];
                OnStatusChanged($"Fetching data for @{profile.Username} ({i + 1}/{profiles.Count})...");

                try
                {
                    var data = await _scraper.FetchStatsAsync(profile).ConfigureAwait(false);
                    if (data != null)
                    {
                        // Save immediately to get the DataId from database
                        var dataId = ServiceContainer.Database.AddTkData(data);
                        data.DataId = dataId;
                        savedCount++;

                        // Now notify with the correct DataId
                        OnDataCollected(data);
                        OnStatusChanged($"Collected data for @{profile.Username}: {data.FollowerCountDisplay} followers (ID: {dataId})");
                    }
                    else
                    {
                        OnStatusChanged($"Failed to fetch data for @{profile.Username}");
                    }
                }
                catch (Exception ex)
                {
                    OnStatusChanged($"Error fetching @{profile.Username}: {ex.Message}");
                }

                OnProgressChanged(i + 1, profiles.Count);

                // Wait between profiles with jitter (except for the last one)
                if (i < profiles.Count - 1 && !ct.IsCancellationRequested)
                {
                    var delayMs = _rateLimiter.GetNextDelayMs();
                    OnStatusChanged($"Waiting ({_rateLimiter.GetDelayDescription()}) before next profile...");
                    await Task.Delay(delayMs, ct).ConfigureAwait(false);
                }
            }

            if (savedCount > 0)
            {
                OnStatusChanged($"Data collection complete. Saved {savedCount} records.");
            }
            else
            {
                OnStatusChanged("Data collection complete. No data collected.");
            }

            OnProgressChanged(profiles.Count, profiles.Count);
        }
        catch (OperationCanceledException)
        {
            OnStatusChanged("Data collection cancelled");
        }
        catch (Exception ex)
        {
            OnStatusChanged($"Error during data collection: {ex.Message}");
        }
    }

    private void OnStatusChanged(string message)
    {
        StatusChanged?.Invoke(this, message);
    }

    private void OnProgressChanged(int current, int total)
    {
        ProgressChanged?.Invoke(this, (current, total));
    }

    private void OnRunningStateChanged(bool isRunning)
    {
        RunningStateChanged?.Invoke(this, isRunning);
    }

    private void OnDataCollected(TkData data)
    {
        DataCollected?.Invoke(this, data);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Stop();

            // Clear schedules to release memory
            lock (_lock)
            {
                _schedules.Clear();
            }

            _scraper?.Dispose();
            _cts?.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }

    ~TikTokDataCollectionService()
    {
        Dispose();
    }
}

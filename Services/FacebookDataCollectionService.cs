using nRun.Models;

namespace nRun.Services;

/// <summary>
/// Background service for collecting Facebook page data on schedule
/// </summary>
public class FacebookDataCollectionService : IDisposable
{
    private FacebookScraperService? _scraper;
    private CancellationTokenSource? _cts;
    private Task? _runningTask;
    private bool _disposed;
    private readonly object _lock = new();
    private readonly RateLimiter _rateLimiter;
    private readonly Random _random = new();

    private List<FbSchedule> _schedules = new();
    private int _chunkSize = 10;
    private int _chunkDelayMinutes = 5;

    public bool IsRunning { get; private set; }
    public bool SkipInitialCollection { get; set; }

    public event EventHandler<string>? StatusChanged;
    public event EventHandler<(int current, int total)>? ProgressChanged;
    public event EventHandler<bool>? RunningStateChanged;
    public event EventHandler<FbData>? DataCollected;

    public FacebookDataCollectionService()
    {
        _rateLimiter = new RateLimiter(10, 5); // 10s base + 0-5s jitter
    }

    public void UpdateSchedules(List<FbSchedule> schedules)
    {
        lock (_lock)
        {
            _schedules = schedules.Where(s => s.IsActive).ToList();
        }
    }

    public void UpdateDelaySeconds(int seconds)
    {
        _rateLimiter.BaseDelaySeconds = Math.Max(1, seconds);
        // Jitter is 2x base for range base to base*3 (e.g., 15 → 15-45 seconds)
        _rateLimiter.JitterMaxSeconds = Math.Max(1, seconds * 2);
    }

    public void UpdateChunkSettings(int chunkSize, int chunkDelayMinutes)
    {
        _chunkSize = Math.Max(1, chunkSize);
        _chunkDelayMinutes = Math.Max(1, chunkDelayMinutes);
    }

    public void Start()
    {
        if (IsRunning) return;

        _cts = new CancellationTokenSource();
        IsRunning = true;
        OnRunningStateChanged(true);
        OnStatusChanged("Facebook data collection started");

        _runningTask = Task.Run(() => RunCollectionLoop(_cts.Token));
    }

    public void Stop()
    {
        if (!IsRunning) return;

        IsRunning = false;
        OnStatusChanged("Stopping Facebook data collection...");

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
        OnStatusChanged("Facebook data collection stopped");
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

                List<FbSchedule> activeSchedules;
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
            var profiles = ServiceContainer.Database.GetActiveFbProfiles();
            if (profiles.Count == 0)
            {
                OnStatusChanged("No active Facebook profiles to collect data for");
                return;
            }

            // Calculate chunks
            int totalChunks = (int)Math.Ceiling((double)profiles.Count / _chunkSize);
            OnStatusChanged($"Starting data collection for {profiles.Count} profiles in {totalChunks} chunk(s)...");
            OnProgressChanged(0, profiles.Count);

            _scraper ??= new FacebookScraperService();
            int savedCount = 0;
            int processedCount = 0;

            for (int chunkIndex = 0; chunkIndex < totalChunks; chunkIndex++)
            {
                if (ct.IsCancellationRequested) break;

                int startIdx = chunkIndex * _chunkSize;
                int endIdx = Math.Min(startIdx + _chunkSize, profiles.Count);
                int chunkNum = chunkIndex + 1;

                OnStatusChanged($"Processing chunk {chunkNum}/{totalChunks} ({startIdx + 1}-{endIdx} of {profiles.Count})...");

                // Process profiles in this chunk
                for (int i = startIdx; i < endIdx; i++)
                {
                    if (ct.IsCancellationRequested) break;

                    var profile = profiles[i];
                    OnStatusChanged($"[Chunk {chunkNum}/{totalChunks}] Fetching {profile.Username} ({i + 1}/{profiles.Count})...");

                    try
                    {
                        var data = await _scraper.FetchStatsAsync(profile, ct).ConfigureAwait(false);
                        if (data != null)
                        {
                            // Save immediately to get the DataId from database
                            var dataId = ServiceContainer.Database.AddFbData(data);
                            data.DataId = dataId;
                            savedCount++;

                            // Now notify with the correct DataId
                            OnDataCollected(data);
                            OnStatusChanged($"[Chunk {chunkNum}/{totalChunks}] Collected {profile.Username}: {data.FollowersCountDisplay} followers");
                        }
                        else
                        {
                            OnStatusChanged($"[Chunk {chunkNum}/{totalChunks}] Failed to fetch {profile.Username}");
                        }
                    }
                    catch (Exception ex)
                    {
                        OnStatusChanged($"[Chunk {chunkNum}/{totalChunks}] Error fetching {profile.Username}: {ex.Message}");
                    }

                    processedCount++;
                    OnProgressChanged(processedCount, profiles.Count);

                    // Wait between profiles with jitter (except for the last one in the entire collection)
                    if (i < profiles.Count - 1 && !ct.IsCancellationRequested)
                    {
                        var delayMs = _rateLimiter.GetNextDelayMs();
                        OnStatusChanged($"[Chunk {chunkNum}/{totalChunks}] Waiting ({_rateLimiter.GetDelayDescription()})...");
                        await Task.Delay(delayMs, ct).ConfigureAwait(false);
                    }
                }

                // Wait between chunks (except for the last chunk)
                if (chunkIndex < totalChunks - 1 && !ct.IsCancellationRequested)
                {
                    // Randomize chunk delay: base ± 2 minutes (e.g., 5 min → 3-7 min)
                    int minDelay = Math.Max(1, _chunkDelayMinutes - 2);
                    int maxDelay = _chunkDelayMinutes + 2;
                    int randomDelayMinutes = _random.Next(minDelay, maxDelay + 1);
                    OnStatusChanged($"Chunk {chunkNum} complete. Waiting {randomDelayMinutes} minute(s) before next chunk...");
                    await Task.Delay(TimeSpan.FromMinutes(randomDelayMinutes), ct).ConfigureAwait(false);
                }
            }

            if (savedCount > 0)
            {
                OnStatusChanged($"Data collection complete. Saved {savedCount}/{profiles.Count} records.");
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

    private void OnDataCollected(FbData data)
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

    ~FacebookDataCollectionService()
    {
        Dispose();
    }
}

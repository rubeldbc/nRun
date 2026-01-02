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

    private List<TkSchedule> _schedules = new();
    private int _delaySeconds = 10;

    public bool IsRunning { get; private set; }

    public event EventHandler<string>? StatusChanged;
    public event EventHandler<(int current, int total)>? ProgressChanged;
    public event EventHandler<bool>? RunningStateChanged;
    public event EventHandler<TkData>? DataCollected;

    public void UpdateSchedules(List<TkSchedule> schedules)
    {
        lock (_lock)
        {
            _schedules = schedules.Where(s => s.IsActive).ToList();
        }
    }

    public void UpdateDelaySeconds(int seconds)
    {
        _delaySeconds = Math.Max(1, seconds);
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

        _cts?.Cancel();
        IsRunning = false;
        OnRunningStateChanged(false);
        OnStatusChanged("TikTok data collection stopped");

        try
        {
            _runningTask?.Wait(TimeSpan.FromSeconds(5));
        }
        catch { }

        _scraper?.CloseDriver();
    }

    private async Task RunCollectionLoop(CancellationToken ct)
    {
        // Fetch data immediately on start
        try
        {
            OnStatusChanged("Running initial data collection...");
            await RunDataCollection(ct);
        }
        catch (Exception ex)
        {
            OnStatusChanged($"Error in initial collection: {ex.Message}");
        }

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
                    await RunDataCollection(ct);
                    // Wait at least 2 minutes after collection to avoid re-triggering
                    await Task.Delay(TimeSpan.FromMinutes(2), ct);
                }
                else
                {
                    // Check every 30 seconds for schedule match
                    await Task.Delay(TimeSpan.FromSeconds(30), ct);
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                OnStatusChanged($"Error in collection loop: {ex.Message}");
                await Task.Delay(TimeSpan.FromMinutes(1), ct);
            }
        }
    }

    public async Task RunDataCollectionNow(CancellationToken ct = default)
    {
        await RunDataCollection(ct);
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
            var collectedData = new List<TkData>();

            for (int i = 0; i < profiles.Count; i++)
            {
                if (ct.IsCancellationRequested) break;

                var profile = profiles[i];
                OnStatusChanged($"Fetching data for @{profile.Username} ({i + 1}/{profiles.Count})...");

                try
                {
                    var data = await _scraper.FetchStatsAsync(profile);
                    if (data != null)
                    {
                        collectedData.Add(data);
                        OnDataCollected(data);
                        OnStatusChanged($"Collected data for @{profile.Username}: {data.FollowerCountDisplay} followers");
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

                // Wait between profiles (except for the last one)
                if (i < profiles.Count - 1 && !ct.IsCancellationRequested)
                {
                    OnStatusChanged($"Waiting {_delaySeconds} second(s) before next profile...");
                    await Task.Delay(TimeSpan.FromSeconds(_delaySeconds), ct);
                }
            }

            // Save all collected data to database
            if (collectedData.Count > 0)
            {
                OnStatusChanged($"Saving {collectedData.Count} records to database...");
                ServiceContainer.Database.AddTkDataBatch(collectedData);
                OnStatusChanged($"Data collection complete. Saved {collectedData.Count} records.");
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

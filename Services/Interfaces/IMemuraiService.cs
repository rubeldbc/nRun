using nRun.Models;

namespace nRun.Services.Interfaces;

/// <summary>
/// Service for syncing news data to Memurai/Redis server
/// </summary>
public interface IMemuraiService : IDisposable
{
    /// <summary>
    /// Whether the sync service is currently running
    /// </summary>
    bool IsRunning { get; }

    /// <summary>
    /// Whether connected to Memurai server
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Start the background sync service
    /// </summary>
    void Start();

    /// <summary>
    /// Stop the background sync service
    /// </summary>
    void Stop();

    /// <summary>
    /// Sync news data immediately
    /// </summary>
    Task SyncNowAsync(List<NewsInfo> articles);

    /// <summary>
    /// Test connection to Memurai server
    /// </summary>
    Task<bool> TestConnectionAsync();

    /// <summary>
    /// Get stored data from Memurai server
    /// </summary>
    Task<string?> GetStoredDataAsync();

    /// <summary>
    /// Update connection settings
    /// </summary>
    void UpdateSettings();

    /// <summary>
    /// Event raised when sync status changes
    /// </summary>
    event EventHandler<string>? StatusChanged;

    /// <summary>
    /// Event raised when running state changes
    /// </summary>
    event EventHandler<bool>? RunningStateChanged;
}

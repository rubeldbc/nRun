namespace nRun.Services.Interfaces;

/// <summary>
/// Interface for managing No-Scrape time window logic
/// </summary>
public interface INoScrapWindowService
{
    /// <summary>
    /// Check if the current time is within the no-scrape window
    /// </summary>
    bool IsInNoScrapWindow();

    /// <summary>
    /// Get the remaining time until the no-scrape window ends
    /// </summary>
    TimeSpan GetRemainingTime();

    /// <summary>
    /// Get formatted status message for the no-scrape window
    /// </summary>
    string GetStatusMessage();

    /// <summary>
    /// Get the configured no-scrape window times
    /// </summary>
    (TimeSpan start, TimeSpan end) GetWindowTimes();

    /// <summary>
    /// Check if no-scrape feature is enabled
    /// </summary>
    bool IsEnabled { get; }
}

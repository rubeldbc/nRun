using nRun.Services.Interfaces;

namespace nRun.Services;

/// <summary>
/// Service to manage No-Scrape time window logic
/// Handles checking if scraping is currently blocked and calculating remaining time
/// </summary>
public class NoScrapWindowService : INoScrapWindowService
{
    private readonly ISettingsManager _settings;

    public NoScrapWindowService(ISettingsManager settings)
    {
        _settings = settings;
    }

    /// <summary>
    /// Check if the current time is within the no-scrape window
    /// </summary>
    public bool IsInNoScrapWindow()
    {
        var settings = _settings.LoadSettings();
        if (!settings.NoScrapEnabled)
            return false;

        var now = DateTime.Now.TimeOfDay;
        var start = new TimeSpan(settings.NoScrapStartHour, settings.NoScrapStartMinute, 0);
        var end = new TimeSpan(settings.NoScrapEndHour, settings.NoScrapEndMinute, 0);

        // Handle overnight window (e.g., 22:00 to 06:00)
        if (start <= end)
        {
            // Same day window (e.g., 01:00 to 06:00)
            return now >= start && now < end;
        }
        else
        {
            // Overnight window (e.g., 22:00 to 06:00)
            return now >= start || now < end;
        }
    }

    /// <summary>
    /// Get the remaining time until the no-scrape window ends
    /// </summary>
    public TimeSpan GetRemainingTime()
    {
        var settings = _settings.LoadSettings();
        var now = DateTime.Now.TimeOfDay;
        var start = new TimeSpan(settings.NoScrapStartHour, settings.NoScrapStartMinute, 0);
        var end = new TimeSpan(settings.NoScrapEndHour, settings.NoScrapEndMinute, 0);

        TimeSpan remaining;

        if (start <= end)
        {
            // Same day window
            remaining = end - now;
        }
        else
        {
            // Overnight window
            if (now >= start)
            {
                // We're in the evening part (e.g., after 22:00)
                remaining = TimeSpan.FromDays(1) - now + end;
            }
            else
            {
                // We're in the morning part (e.g., before 06:00)
                remaining = end - now;
            }
        }

        return remaining < TimeSpan.Zero ? TimeSpan.Zero : remaining;
    }

    /// <summary>
    /// Get formatted status message for the no-scrape window
    /// </summary>
    public string GetStatusMessage()
    {
        if (!IsInNoScrapWindow())
            return string.Empty;

        var remaining = GetRemainingTime();
        return $"No-Scrape: Resumes in {remaining:hh\\:mm\\:ss}";
    }

    /// <summary>
    /// Get the configured no-scrape window times
    /// </summary>
    public (TimeSpan start, TimeSpan end) GetWindowTimes()
    {
        var settings = _settings.LoadSettings();
        return (
            new TimeSpan(settings.NoScrapStartHour, settings.NoScrapStartMinute, 0),
            new TimeSpan(settings.NoScrapEndHour, settings.NoScrapEndMinute, 0)
        );
    }

    /// <summary>
    /// Check if no-scrape feature is enabled
    /// </summary>
    public bool IsEnabled
    {
        get
        {
            var settings = _settings.LoadSettings();
            return settings.NoScrapEnabled;
        }
    }
}

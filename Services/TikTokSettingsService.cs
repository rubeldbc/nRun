using nRun.Models;

namespace nRun.Services;

/// <summary>
/// Service for managing TikTok-related settings and schedules.
/// Extracts settings management logic from MainForm.
/// </summary>
public class TikTokSettingsService
{
    private List<TkSchedule> _schedules = new();
    private int _nextScheduleId = 1;

    public event EventHandler? SettingsChanged;

    /// <summary>
    /// Gets the current delay between TikTok requests in seconds
    /// </summary>
    public int DelaySeconds
    {
        get => ServiceContainer.Settings.LoadSettings().TikTokDelaySeconds;
        set
        {
            var settings = ServiceContainer.Settings.LoadSettings();
            settings.TikTokDelaySeconds = value;
            ServiceContainer.Settings.SaveSettings(settings);
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Gets the list of schedules
    /// </summary>
    public IReadOnlyList<TkSchedule> Schedules => _schedules.AsReadOnly();

    /// <summary>
    /// Gets the count of active schedules
    /// </summary>
    public int ActiveScheduleCount => _schedules.Count(s => s.IsActive);

    /// <summary>
    /// Loads TikTok settings from app settings
    /// </summary>
    public void LoadSettings()
    {
        var settings = ServiceContainer.Settings.LoadSettings();

        _schedules.Clear();
        _nextScheduleId = 1;

        foreach (var schedSetting in settings.TikTokSchedules)
        {
            var schedule = new TkSchedule
            {
                Id = _nextScheduleId++,
                Timing = new TimeSpan(schedSetting.Hour, schedSetting.Minute, 0),
                IsActive = schedSetting.IsEnabled
            };
            _schedules.Add(schedule);
        }

        UpdateSerialNumbers();
    }

    /// <summary>
    /// Saves TikTok settings to app settings
    /// </summary>
    public void SaveSettings()
    {
        var settings = ServiceContainer.Settings.LoadSettings();

        settings.TikTokSchedules.Clear();
        foreach (var schedule in _schedules)
        {
            settings.TikTokSchedules.Add(new TikTokScheduleSettings
            {
                Hour = schedule.Timing.Hours,
                Minute = schedule.Timing.Minutes,
                IsEnabled = schedule.IsActive
            });
        }

        ServiceContainer.Settings.SaveSettings(settings);
        SettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Adds a new schedule
    /// </summary>
    public TkSchedule AddSchedule(TimeSpan timing, bool isActive = true)
    {
        var schedule = new TkSchedule
        {
            Id = _nextScheduleId++,
            Timing = timing,
            IsActive = isActive
        };
        _schedules.Add(schedule);
        UpdateSerialNumbers();
        SaveSettings();
        return schedule;
    }

    /// <summary>
    /// Removes a schedule
    /// </summary>
    public bool RemoveSchedule(int scheduleId)
    {
        var schedule = _schedules.FirstOrDefault(s => s.Id == scheduleId);
        if (schedule != null)
        {
            _schedules.Remove(schedule);
            UpdateSerialNumbers();
            SaveSettings();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Removes a schedule by reference
    /// </summary>
    public bool RemoveSchedule(TkSchedule schedule)
    {
        if (_schedules.Remove(schedule))
        {
            UpdateSerialNumbers();
            SaveSettings();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Updates a schedule's active state
    /// </summary>
    public void SetScheduleActive(int scheduleId, bool isActive)
    {
        var schedule = _schedules.FirstOrDefault(s => s.Id == scheduleId);
        if (schedule != null)
        {
            schedule.IsActive = isActive;
            SaveSettings();
        }
    }

    /// <summary>
    /// Updates a schedule's timing
    /// </summary>
    public void UpdateScheduleTiming(int scheduleId, TimeSpan timing)
    {
        var schedule = _schedules.FirstOrDefault(s => s.Id == scheduleId);
        if (schedule != null)
        {
            schedule.Timing = timing;
            SaveSettings();
        }
    }

    /// <summary>
    /// Gets the next scheduled time from now
    /// </summary>
    public (TimeSpan? nextTime, TimeSpan timeUntil)? GetNextScheduledTime()
    {
        var activeSchedules = _schedules.Where(s => s.IsActive).ToList();
        if (activeSchedules.Count == 0)
        {
            return null;
        }

        var now = DateTime.Now;
        var currentTime = now.TimeOfDay;
        TimeSpan? nextScheduleTime = null;
        TimeSpan minDiff = TimeSpan.MaxValue;

        foreach (var schedule in activeSchedules)
        {
            var scheduleTime = schedule.Timing;
            TimeSpan diff;

            if (scheduleTime > currentTime)
            {
                // Schedule is later today
                diff = scheduleTime - currentTime;
            }
            else
            {
                // Schedule is tomorrow (already passed today)
                diff = TimeSpan.FromDays(1) - currentTime + scheduleTime;
            }

            if (diff < minDiff)
            {
                minDiff = diff;
                nextScheduleTime = scheduleTime;
            }
        }

        return nextScheduleTime.HasValue ? (nextScheduleTime, minDiff) : null;
    }

    /// <summary>
    /// Formats a countdown timespan for display
    /// </summary>
    public static string FormatCountdown(TimeSpan diff)
    {
        if (diff.TotalHours >= 1)
        {
            return $"{(int)diff.TotalHours}h {diff.Minutes:D2}m";
        }
        else if (diff.TotalMinutes >= 1)
        {
            return $"{diff.Minutes}m {diff.Seconds:D2}s";
        }
        else
        {
            return $"{diff.Seconds}s";
        }
    }

    private void UpdateSerialNumbers()
    {
        for (int i = 0; i < _schedules.Count; i++)
        {
            _schedules[i].SerialNumber = i + 1;
        }
    }
}

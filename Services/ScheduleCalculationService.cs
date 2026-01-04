using nRun.Services.Interfaces;

namespace nRun.Services;

/// <summary>
/// Service for calculating next schedule times and formatting countdowns
/// Used by TikTok and Facebook data collection features
/// </summary>
public class ScheduleCalculationService : IScheduleCalculationService
{
    /// <summary>
    /// Calculate the next active schedule from a list of schedules
    /// </summary>
    public IScheduleCalculationService.ScheduleResult CalculateNextSchedule(IEnumerable<TimeSpan> schedules)
    {
        var scheduleList = schedules.ToList();
        if (scheduleList.Count == 0)
        {
            return new IScheduleCalculationService.ScheduleResult(
                null,
                TimeSpan.MaxValue,
                "No schedules",
                "No schedules"
            );
        }

        var currentTime = DateTime.Now.TimeOfDay;
        TimeSpan? nextScheduleTime = null;
        TimeSpan minDiff = TimeSpan.MaxValue;

        foreach (var scheduleTime in scheduleList)
        {
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

        if (nextScheduleTime.HasValue)
        {
            return new IScheduleCalculationService.ScheduleResult(
                nextScheduleTime,
                minDiff,
                FormatTime(nextScheduleTime.Value),
                FormatCountdown(minDiff)
            );
        }

        return new IScheduleCalculationService.ScheduleResult(
            null,
            TimeSpan.MaxValue,
            "--:--",
            "N/A"
        );
    }

    /// <summary>
    /// Format a TimeSpan as a countdown string
    /// </summary>
    public string FormatCountdown(TimeSpan remaining)
    {
        if (remaining.TotalHours >= 1)
        {
            return $"{(int)remaining.TotalHours}h {remaining.Minutes:D2}m";
        }
        else if (remaining.TotalMinutes >= 1)
        {
            return $"{remaining.Minutes}m {remaining.Seconds:D2}s";
        }
        else
        {
            return $"{remaining.Seconds}s";
        }
    }

    /// <summary>
    /// Format a TimeSpan as a time string (HH:mm)
    /// </summary>
    public string FormatTime(TimeSpan time)
    {
        return DateTime.Today.Add(time).ToString("HH:mm");
    }
}

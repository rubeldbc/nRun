namespace nRun.Services.Interfaces;

/// <summary>
/// Interface for calculating next schedule times and formatting countdowns
/// </summary>
public interface IScheduleCalculationService
{
    /// <summary>
    /// Result of a schedule calculation
    /// </summary>
    public record ScheduleResult(
        TimeSpan? NextScheduleTime,
        TimeSpan TimeUntilNext,
        string FormattedTime,
        string FormattedCountdown
    );

    /// <summary>
    /// Calculate the next active schedule from a list of schedules
    /// </summary>
    /// <param name="schedules">List of schedule times (TimeSpan from midnight)</param>
    /// <param name="activeFilter">Predicate to filter active schedules (optional)</param>
    /// <returns>ScheduleResult with next time and formatted strings</returns>
    ScheduleResult CalculateNextSchedule(IEnumerable<TimeSpan> schedules);

    /// <summary>
    /// Format a TimeSpan as a countdown string
    /// </summary>
    /// <param name="remaining">Time remaining</param>
    /// <returns>Formatted string like "2h 30m", "15m 30s", or "45s"</returns>
    string FormatCountdown(TimeSpan remaining);

    /// <summary>
    /// Format a TimeSpan as a time string (HH:mm)
    /// </summary>
    /// <param name="time">Time of day</param>
    /// <returns>Formatted string like "14:30"</returns>
    string FormatTime(TimeSpan time);
}

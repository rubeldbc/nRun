namespace nRun.Models;

/// <summary>
/// Represents a scheduled time for Facebook data collection
/// </summary>
public class FbSchedule
{
    public int Id { get; set; }
    public TimeSpan Timing { get; set; }
    public bool IsActive { get; set; } = true;

    // For display purposes
    public int SerialNumber { get; set; }
    public string TimingDisplay => Timing.ToString(@"hh\:mm");
    public string StatusDisplay => IsActive ? "Active" : "Inactive";
}

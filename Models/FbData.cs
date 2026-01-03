namespace nRun.Models;

/// <summary>
/// Represents Facebook page statistics data (fb_data table)
/// </summary>
public class FbData
{
    public int DataId { get; set; }
    public long UserId { get; set; }
    public long FollowersCount { get; set; }
    public long TalkingAbout { get; set; }
    public DateTime RecordedAt { get; set; } = DateTime.Now;

    // Change values (difference from previous record)
    public long FollowersChange { get; set; }
    public long TalkingAboutChange { get; set; }

    // For display purposes (joined from fb_profile)
    public string Username { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;

    // Display as plain numbers
    public string FollowersCountDisplay => FollowersCount.ToString("N0");
    public string TalkingAboutDisplay => TalkingAbout.ToString("N0");
    public string RecordedAtDisplay => RecordedAt.ToString("yyyy-MM-dd HH:mm:ss");

    // Display change values with +/- prefix
    public string FollowersChangeDisplay => FormatChange(FollowersChange);
    public string TalkingAboutChangeDisplay => FormatChange(TalkingAboutChange);

    private static string FormatChange(long change)
    {
        if (change > 0) return $"+{change:N0}";
        if (change < 0) return change.ToString("N0");
        return "0";
    }
}

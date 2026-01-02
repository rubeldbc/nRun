namespace nRun.Models;

/// <summary>
/// Represents TikTok statistics data (tk_data table)
/// </summary>
public class TkData
{
    public int DataId { get; set; }
    public long UserId { get; set; }
    public long FollowerCount { get; set; }
    public long HeartCount { get; set; }
    public int VideoCount { get; set; }
    public DateTime RecordedAt { get; set; } = DateTime.Now;

    // Change values (difference from previous record)
    public long FollowersChange { get; set; }
    public long HeartsChange { get; set; }
    public int VideosChange { get; set; }

    // For display purposes (joined from tk_profile)
    public string Username { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;

    // Display as plain numbers
    public string FollowerCountDisplay => FollowerCount.ToString("N0");
    public string HeartCountDisplay => HeartCount.ToString("N0");
    public string RecordedAtDisplay => RecordedAt.ToString("yyyy-MM-dd HH:mm:ss");

    // Display change values with +/- prefix
    public string FollowersChangeDisplay => FormatChange(FollowersChange);
    public string HeartsChangeDisplay => FormatChange(HeartsChange);
    public string VideosChangeDisplay => FormatChange(VideosChange);

    private static string FormatChange(long change)
    {
        if (change > 0) return $"+{change:N0}";
        if (change < 0) return change.ToString("N0");
        return "0";
    }
}

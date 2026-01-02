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

    // For display purposes (joined from tk_profile)
    public string Username { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;

    // Display as plain numbers
    public string FollowerCountDisplay => FollowerCount.ToString();
    public string HeartCountDisplay => HeartCount.ToString();
    public string RecordedAtDisplay => RecordedAt.ToString("yyyy-MM-dd HH:mm:ss");
}

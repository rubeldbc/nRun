namespace nRun.Models;

/// <summary>
/// Represents a TikTok profile (tk_profile table)
/// </summary>
public class TkProfile
{
    public bool Status { get; set; } = true;
    public long UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public DateTime? CreatedAtTs { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Avatar (not stored in DB, just local path)
    public string? AvatarUrl { get; set; }
    public string? AvatarLocalPath { get; set; }

    // For display purposes in olvTiktokID
    public string DisplayName => $"{Username} ({Nickname})";
}

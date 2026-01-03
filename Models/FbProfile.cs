namespace nRun.Models;

/// <summary>
/// Represents a Facebook page profile (fb_profile table)
/// </summary>
public class FbProfile
{
    public bool Status { get; set; } = true;
    public long UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyType { get; set; } = string.Empty;
    public string PageType { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Avatar (local path only)
    public string? AvatarUrl { get; set; }
    public string? AvatarLocalPath { get; set; }

    // For display purposes
    public string DisplayName => string.IsNullOrEmpty(Nickname) ? Username : $"{Username} ({Nickname})";
}

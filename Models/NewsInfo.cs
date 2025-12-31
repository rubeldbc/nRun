namespace nRun.Models;

/// <summary>
/// Represents a scraped news article (news_info table)
/// </summary>
public class NewsInfo
{
    public long Serial { get; set; }
    public string SiteId { get; set; } = string.Empty;
    public string SiteName { get; set; } = string.Empty; // For display (joined from site_info)
    public string? SiteLogo { get; set; } // For display (joined from site_info)
    public string NewsTitle { get; set; } = string.Empty;
    public string NewsText { get; set; } = string.Empty;
    public string NewsUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsRead { get; set; }

    // For display purposes
    public string CreatedAtDisplay => CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
    public string ShortTitle => NewsTitle.Length > 80 ? NewsTitle[..77] + "..." : NewsTitle;
}

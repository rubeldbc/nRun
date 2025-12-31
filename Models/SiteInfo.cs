namespace nRun.Models;

/// <summary>
/// Represents a news source site (site_info table)
/// </summary>
public class SiteInfo
{
    public string SiteId { get; set; } = Guid.NewGuid().ToString("N")[..12].ToUpper();
    public string SiteName { get; set; } = string.Empty;
    public string SiteLink { get; set; } = string.Empty;
    public string? SiteLogo { get; set; }
    public string? SiteCategory { get; set; }
    public string? SiteCountry { get; set; }
    public bool IsActive { get; set; } = true;

    // CSS Selectors for scraping
    public string ArticleLinkSelector { get; set; } = string.Empty;
    public string TitleSelector { get; set; } = string.Empty;
    public string BodySelector { get; set; } = string.Empty;

    // Statistics
    public int SuccessCount { get; set; }
    public int FailureCount { get; set; }
    public DateTime? LastChecked { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public override string ToString() => SiteName;
}

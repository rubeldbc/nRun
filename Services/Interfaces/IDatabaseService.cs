using nRun.Models;

namespace nRun.Services.Interfaces;

/// <summary>
/// Interface for database operations
/// </summary>
public interface IDatabaseService
{
    bool IsConnected { get; }

    void Initialize();
    void UpdateConnectionString(string connectionString);

    // Site operations
    List<SiteInfo> GetAllSites();
    List<SiteInfo> GetActiveSites();
    SiteInfo? GetSiteById(string siteId);
    string AddSite(SiteInfo site);
    void UpdateSite(SiteInfo site);
    void UpdateSiteStats(string siteId, bool success);
    void DeleteSite(string siteId);

    // News operations
    bool NewsExists(string newsUrl);
    long AddNews(NewsInfo news);
    List<NewsInfo> GetRecentNews(int limit = 100);
    List<NewsInfo> GetNewsBySite(string siteId, int limit = 50);
    NewsInfo? GetNewsById(long serial);
    void MarkNewsAsRead(long serial);
    void DeleteNews(long serial);
    long GetNewsCount();
}

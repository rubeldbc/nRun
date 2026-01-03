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

    // TikTok Profile operations
    List<TkProfile> GetAllTkProfiles();
    List<TkProfile> GetActiveTkProfiles();
    TkProfile? GetTkProfileById(long userId);
    TkProfile? GetTkProfileByUsername(string username);
    bool TkProfileExists(long userId);
    void AddTkProfile(TkProfile profile);
    void UpdateTkProfile(TkProfile profile);
    void UpdateTkProfileStatus(long userId, bool status);
    void DeleteTkProfile(long userId);

    // TikTok Data operations
    int AddTkData(TkData data);
    void AddTkDataBatch(List<TkData> dataList);
    List<TkData> GetRecentTkData(int limit = 100);
    List<TkData> GetFilteredTkData(string? username = null, DateTime? fromDate = null, DateTime? toDate = null, int limit = 500);
    List<TkData> GetTkDataByUserId(long userId, int limit = 50);
    TkData? GetLatestTkDataByUserId(long userId);
    long GetTkDataCount();

    // Facebook Profile operations
    List<FbProfile> GetAllFbProfiles();
    List<FbProfile> GetActiveFbProfiles();
    FbProfile? GetFbProfileById(long userId);
    FbProfile? GetFbProfileByUsername(string username);
    bool FbProfileExists(long userId);
    void AddFbProfile(FbProfile profile);
    void UpdateFbProfile(FbProfile profile);
    void UpdateFbProfileStatus(long userId, bool status);
    void DeleteFbProfile(long userId);

    // Facebook Data operations
    int AddFbData(FbData data);
    List<FbData> GetRecentFbData(int limit = 100);
    List<FbData> GetFilteredFbData(string? username = null, DateTime? fromDate = null, DateTime? toDate = null, int limit = 500);
    List<FbData> GetFbDataByUserId(long userId, int limit = 50);
    FbData? GetLatestFbDataByUserId(long userId);
    long GetFbDataCount();

    // Facebook Table Management
    void CreateFacebookTables();
    void DeleteFacebookTables();
    bool FacebookTablesExist();
}

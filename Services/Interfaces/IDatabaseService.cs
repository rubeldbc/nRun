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
    Task<List<SiteInfo>> GetAllSitesAsync(CancellationToken cancellationToken = default);
    List<SiteInfo> GetActiveSites();
    SiteInfo? GetSiteById(string siteId);
    string AddSite(SiteInfo site);
    void UpdateSite(SiteInfo site);
    void UpdateSiteStats(string siteId, bool success);
    void DeleteSite(string siteId);

    // News operations
    bool NewsExists(string newsUrl);
    string? GetNewsTitleByUrl(string newsUrl);
    bool UpdateNewsByUrl(string newsUrl, string newTitle, string newText);
    long AddNews(NewsInfo news);
    List<NewsInfo> GetRecentNews(int limit = 100);
    Task<List<NewsInfo>> GetRecentNewsAsync(int limit = 100, CancellationToken cancellationToken = default);
    List<NewsInfo> GetNewsBySite(string siteId, int limit = 50);
    NewsInfo? GetNewsById(long serial);
    void MarkNewsAsRead(long serial);
    void DeleteNews(long serial);
    long GetNewsCount();
    Task<long> GetNewsCountAsync(CancellationToken cancellationToken = default);

    // TikTok Profile operations
    List<TkProfile> GetAllTkProfiles();
    Task<List<TkProfile>> GetAllTkProfilesAsync(CancellationToken cancellationToken = default);
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
    Task<List<TkData>> GetRecentTkDataAsync(int limit = 100, CancellationToken cancellationToken = default);
    List<TkData> GetFilteredTkData(string? username = null, DateTime? fromDate = null, DateTime? toDate = null, int limit = 500);
    Task<List<TkData>> GetFilteredTkDataAsync(string? username = null, DateTime? fromDate = null, DateTime? toDate = null, int limit = 500, CancellationToken cancellationToken = default);
    List<TkData> GetTkDataByUserId(long userId, int limit = 50);
    TkData? GetLatestTkDataByUserId(long userId);
    long GetTkDataCount();

    // Facebook Profile operations
    List<FbProfile> GetAllFbProfiles();
    Task<List<FbProfile>> GetAllFbProfilesAsync(CancellationToken cancellationToken = default);
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
    Task<List<FbData>> GetRecentFbDataAsync(int limit = 100, CancellationToken cancellationToken = default);
    List<FbData> GetFilteredFbData(string? username = null, DateTime? fromDate = null, DateTime? toDate = null, int limit = 500);
    Task<List<FbData>> GetFilteredFbDataAsync(string? username = null, DateTime? fromDate = null, DateTime? toDate = null, int limit = 500, CancellationToken cancellationToken = default);
    List<FbData> GetFbDataByUserId(long userId, int limit = 50);
    FbData? GetLatestFbDataByUserId(long userId);
    long GetFbDataCount();

    // Facebook Table Management
    void CreateFacebookTables();
    void DeleteFacebookTables();
    bool FacebookTablesExist();
}

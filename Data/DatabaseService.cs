using Npgsql;
using nRun.Models;
using nRun.Services.Interfaces;

namespace nRun.Data;

/// <summary>
/// PostgreSQL database service for storing sites and news articles
/// </summary>
public class DatabaseService : IDatabaseService
{
    private readonly ISettingsManager _settingsManager;
    private string _connectionString = string.Empty;
    private readonly object _lock = new();
    private bool _isInitialized = false;

    public bool IsConnected => _isInitialized && !string.IsNullOrEmpty(_connectionString);

    public DatabaseService(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
    }

    public void Initialize()
    {
        var settings = _settingsManager.LoadSettings();
        _connectionString = settings.GetConnectionString();

        if (!string.IsNullOrEmpty(settings.DbPassword))
        {
            try
            {
                using var conn = GetConnection();
                EnsureTablesExist(conn);
                _isInitialized = true;
            }
            catch
            {
                _isInitialized = false;
            }
        }
    }

    public void UpdateConnectionString(string connectionString)
    {
        _connectionString = connectionString;
        try
        {
            using var conn = GetConnection();
            EnsureTablesExist(conn);
            _isInitialized = true;
        }
        catch
        {
            _isInitialized = false;
        }
    }

    private void EnsureTablesExist(NpgsqlConnection conn)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS site_info (
                site_id VARCHAR(20) PRIMARY KEY,
                site_name VARCHAR(255) NOT NULL,
                site_link VARCHAR(500) NOT NULL,
                site_logo VARCHAR(500),
                site_category VARCHAR(100),
                site_country VARCHAR(100),
                is_active BOOLEAN DEFAULT TRUE,
                article_link_selector TEXT,
                title_selector TEXT,
                body_selector TEXT,
                success_count INTEGER DEFAULT 0,
                failure_count INTEGER DEFAULT 0,
                last_checked TIMESTAMP,
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            );

            CREATE TABLE IF NOT EXISTS news_info (
                serial BIGSERIAL PRIMARY KEY,
                site_id VARCHAR(20) NOT NULL REFERENCES site_info(site_id) ON DELETE CASCADE,
                news_title TEXT NOT NULL,
                news_text TEXT,
                news_url VARCHAR(1000) NOT NULL UNIQUE,
                is_read BOOLEAN DEFAULT FALSE,
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            );

            CREATE TABLE IF NOT EXISTS app_settings (
                id INTEGER PRIMARY KEY DEFAULT 1 CHECK (id = 1),
                check_interval_minutes INTEGER DEFAULT 5,
                delay_between_links_seconds INTEGER DEFAULT 2,
                max_articles_per_site INTEGER DEFAULT 10,
                use_headless_browser BOOLEAN DEFAULT TRUE,
                browser_timeout_seconds INTEGER DEFAULT 30,
                auto_start_scraping BOOLEAN DEFAULT FALSE,
                max_displayed_articles INTEGER DEFAULT 100,
                last_modified TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            );

            CREATE INDEX IF NOT EXISTS idx_site_info_link ON site_info(site_link);
            CREATE INDEX IF NOT EXISTS idx_news_info_site ON news_info(site_id);
            CREATE INDEX IF NOT EXISTS idx_news_info_created ON news_info(created_at DESC);
            CREATE INDEX IF NOT EXISTS idx_news_info_url ON news_info(news_url);
        ";

        cmd.ExecuteNonQuery();

        using var insertCmd = conn.CreateCommand();
        insertCmd.CommandText = "INSERT INTO app_settings (id) VALUES (1) ON CONFLICT DO NOTHING";
        insertCmd.ExecuteNonQuery();

        // Add column if it doesn't exist (for existing databases)
        using var alterCmd = conn.CreateCommand();
        alterCmd.CommandText = @"
            DO $$
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns
                               WHERE table_name = 'app_settings' AND column_name = 'max_displayed_articles') THEN
                    ALTER TABLE app_settings ADD COLUMN max_displayed_articles INTEGER DEFAULT 100;
                END IF;
            END $$;";
        alterCmd.ExecuteNonQuery();
    }

    private NpgsqlConnection GetConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;
    }

    #region Site Info Operations

    public List<SiteInfo> GetAllSites()
    {
        if (!IsConnected) return new List<SiteInfo>();

        lock (_lock)
        {
            var sites = new List<SiteInfo>();
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM site_info ORDER BY site_name";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                sites.Add(MapSiteInfo(reader));
            }
            return sites;
        }
    }

    public List<SiteInfo> GetActiveSites()
    {
        if (!IsConnected) return new List<SiteInfo>();

        lock (_lock)
        {
            var sites = new List<SiteInfo>();
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM site_info WHERE is_active = TRUE ORDER BY site_name";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                sites.Add(MapSiteInfo(reader));
            }
            return sites;
        }
    }

    public SiteInfo? GetSiteById(string siteId)
    {
        if (!IsConnected) return null;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM site_info WHERE site_id = @siteId";
            cmd.Parameters.AddWithValue("siteId", siteId);

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapSiteInfo(reader) : null;
        }
    }

    public string AddSite(SiteInfo site)
    {
        if (!IsConnected) throw new InvalidOperationException("Database not connected");

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO site_info (site_id, site_name, site_link, site_logo, site_category, site_country,
                    is_active, article_link_selector, title_selector, body_selector)
                VALUES (@siteId, @siteName, @siteLink, @siteLogo, @siteCategory, @siteCountry,
                    @isActive, @articleSelector, @titleSelector, @bodySelector)
                RETURNING site_id";

            cmd.Parameters.AddWithValue("siteId", site.SiteId);
            cmd.Parameters.AddWithValue("siteName", site.SiteName);
            cmd.Parameters.AddWithValue("siteLink", site.SiteLink);
            cmd.Parameters.AddWithValue("siteLogo", (object?)site.SiteLogo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("siteCategory", (object?)site.SiteCategory ?? DBNull.Value);
            cmd.Parameters.AddWithValue("siteCountry", (object?)site.SiteCountry ?? DBNull.Value);
            cmd.Parameters.AddWithValue("isActive", site.IsActive);
            cmd.Parameters.AddWithValue("articleSelector", site.ArticleLinkSelector ?? "");
            cmd.Parameters.AddWithValue("titleSelector", site.TitleSelector ?? "");
            cmd.Parameters.AddWithValue("bodySelector", site.BodySelector ?? "");

            return cmd.ExecuteScalar()?.ToString() ?? site.SiteId;
        }
    }

    public void UpdateSite(SiteInfo site)
    {
        if (!IsConnected) return;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE site_info SET
                    site_name = @siteName,
                    site_link = @siteLink,
                    site_logo = @siteLogo,
                    site_category = @siteCategory,
                    site_country = @siteCountry,
                    is_active = @isActive,
                    article_link_selector = @articleSelector,
                    title_selector = @titleSelector,
                    body_selector = @bodySelector
                WHERE site_id = @siteId";

            cmd.Parameters.AddWithValue("siteId", site.SiteId);
            cmd.Parameters.AddWithValue("siteName", site.SiteName);
            cmd.Parameters.AddWithValue("siteLink", site.SiteLink);
            cmd.Parameters.AddWithValue("siteLogo", (object?)site.SiteLogo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("siteCategory", (object?)site.SiteCategory ?? DBNull.Value);
            cmd.Parameters.AddWithValue("siteCountry", (object?)site.SiteCountry ?? DBNull.Value);
            cmd.Parameters.AddWithValue("isActive", site.IsActive);
            cmd.Parameters.AddWithValue("articleSelector", site.ArticleLinkSelector ?? "");
            cmd.Parameters.AddWithValue("titleSelector", site.TitleSelector ?? "");
            cmd.Parameters.AddWithValue("bodySelector", site.BodySelector ?? "");

            cmd.ExecuteNonQuery();
        }
    }

    public void UpdateSiteStats(string siteId, bool success)
    {
        if (!IsConnected) return;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = success
                ? "UPDATE site_info SET success_count = success_count + 1, last_checked = @lastChecked WHERE site_id = @siteId"
                : "UPDATE site_info SET failure_count = failure_count + 1, last_checked = @lastChecked WHERE site_id = @siteId";

            cmd.Parameters.AddWithValue("siteId", siteId);
            cmd.Parameters.AddWithValue("lastChecked", DateTime.Now);
            cmd.ExecuteNonQuery();
        }
    }

    public void DeleteSite(string siteId)
    {
        if (!IsConnected) return;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM site_info WHERE site_id = @siteId";
            cmd.Parameters.AddWithValue("siteId", siteId);
            cmd.ExecuteNonQuery();
        }
    }

    private static SiteInfo MapSiteInfo(NpgsqlDataReader reader) => new()
    {
        SiteId = reader.GetString(reader.GetOrdinal("site_id")),
        SiteName = reader.GetString(reader.GetOrdinal("site_name")),
        SiteLink = reader.GetString(reader.GetOrdinal("site_link")),
        SiteLogo = reader.IsDBNull(reader.GetOrdinal("site_logo")) ? null : reader.GetString(reader.GetOrdinal("site_logo")),
        SiteCategory = reader.IsDBNull(reader.GetOrdinal("site_category")) ? null : reader.GetString(reader.GetOrdinal("site_category")),
        SiteCountry = reader.IsDBNull(reader.GetOrdinal("site_country")) ? null : reader.GetString(reader.GetOrdinal("site_country")),
        IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
        ArticleLinkSelector = reader.IsDBNull(reader.GetOrdinal("article_link_selector")) ? "" : reader.GetString(reader.GetOrdinal("article_link_selector")),
        TitleSelector = reader.IsDBNull(reader.GetOrdinal("title_selector")) ? "" : reader.GetString(reader.GetOrdinal("title_selector")),
        BodySelector = reader.IsDBNull(reader.GetOrdinal("body_selector")) ? "" : reader.GetString(reader.GetOrdinal("body_selector")),
        SuccessCount = reader.GetInt32(reader.GetOrdinal("success_count")),
        FailureCount = reader.GetInt32(reader.GetOrdinal("failure_count")),
        LastChecked = reader.IsDBNull(reader.GetOrdinal("last_checked")) ? null : reader.GetDateTime(reader.GetOrdinal("last_checked")),
        CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
    };

    #endregion

    #region News Info Operations

    public bool NewsExists(string newsUrl)
    {
        if (!IsConnected) return false;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM news_info WHERE news_url = @url";
            cmd.Parameters.AddWithValue("url", newsUrl);
            return Convert.ToInt64(cmd.ExecuteScalar()) > 0;
        }
    }

    public long AddNews(NewsInfo news)
    {
        if (!IsConnected) throw new InvalidOperationException("Database not connected");

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO news_info (site_id, news_title, news_text, news_url, created_at)
                VALUES (@siteId, @title, @text, @url, @createdAt)
                RETURNING serial";

            cmd.Parameters.AddWithValue("siteId", news.SiteId);
            cmd.Parameters.AddWithValue("title", news.NewsTitle);
            cmd.Parameters.AddWithValue("text", news.NewsText ?? "");
            cmd.Parameters.AddWithValue("url", news.NewsUrl);
            cmd.Parameters.AddWithValue("createdAt", news.CreatedAt);

            return Convert.ToInt64(cmd.ExecuteScalar());
        }
    }

    public List<NewsInfo> GetRecentNews(int limit = 100)
    {
        if (!IsConnected) return new List<NewsInfo>();

        lock (_lock)
        {
            var news = new List<NewsInfo>();
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $@"
                SELECT n.*, s.site_name, s.site_logo
                FROM news_info n
                JOIN site_info s ON n.site_id = s.site_id
                ORDER BY n.created_at DESC
                LIMIT {limit}";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                news.Add(MapNewsInfo(reader));
            }
            return news;
        }
    }

    public List<NewsInfo> GetNewsBySite(string siteId, int limit = 50)
    {
        if (!IsConnected) return new List<NewsInfo>();

        lock (_lock)
        {
            var news = new List<NewsInfo>();
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT n.*, s.site_name, s.site_logo
                FROM news_info n
                JOIN site_info s ON n.site_id = s.site_id
                WHERE n.site_id = @siteId
                ORDER BY n.created_at DESC
                LIMIT @limit";
            cmd.Parameters.AddWithValue("siteId", siteId);
            cmd.Parameters.AddWithValue("limit", limit);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                news.Add(MapNewsInfo(reader));
            }
            return news;
        }
    }

    public NewsInfo? GetNewsById(long serial)
    {
        if (!IsConnected) return null;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT n.*, s.site_name, s.site_logo
                FROM news_info n
                JOIN site_info s ON n.site_id = s.site_id
                WHERE n.serial = @serial";
            cmd.Parameters.AddWithValue("serial", serial);

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapNewsInfo(reader) : null;
        }
    }

    public void MarkNewsAsRead(long serial)
    {
        if (!IsConnected) return;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE news_info SET is_read = TRUE WHERE serial = @serial";
            cmd.Parameters.AddWithValue("serial", serial);
            cmd.ExecuteNonQuery();
        }
    }

    public void DeleteNews(long serial)
    {
        if (!IsConnected) return;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM news_info WHERE serial = @serial";
            cmd.Parameters.AddWithValue("serial", serial);
            cmd.ExecuteNonQuery();
        }
    }

    public long GetNewsCount()
    {
        if (!IsConnected) return 0;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM news_info";
            return Convert.ToInt64(cmd.ExecuteScalar());
        }
    }

    private static NewsInfo MapNewsInfo(NpgsqlDataReader reader) => new()
    {
        Serial = reader.GetInt64(reader.GetOrdinal("serial")),
        SiteId = reader.GetString(reader.GetOrdinal("site_id")),
        SiteName = reader.GetString(reader.GetOrdinal("site_name")),
        SiteLogo = reader.IsDBNull(reader.GetOrdinal("site_logo")) ? null : reader.GetString(reader.GetOrdinal("site_logo")),
        NewsTitle = reader.GetString(reader.GetOrdinal("news_title")),
        NewsText = reader.IsDBNull(reader.GetOrdinal("news_text")) ? "" : reader.GetString(reader.GetOrdinal("news_text")),
        NewsUrl = reader.GetString(reader.GetOrdinal("news_url")),
        IsRead = reader.GetBoolean(reader.GetOrdinal("is_read")),
        CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
    };

    #endregion

    #region TikTok Profile Operations

    public List<TkProfile> GetAllTkProfiles()
    {
        if (!IsConnected) return new List<TkProfile>();

        lock (_lock)
        {
            var profiles = new List<TkProfile>();
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM tk_profile ORDER BY username";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                profiles.Add(MapTkProfile(reader));
            }
            return profiles;
        }
    }

    public List<TkProfile> GetActiveTkProfiles()
    {
        if (!IsConnected) return new List<TkProfile>();

        lock (_lock)
        {
            var profiles = new List<TkProfile>();
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM tk_profile WHERE status = TRUE ORDER BY username";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                profiles.Add(MapTkProfile(reader));
            }
            return profiles;
        }
    }

    public TkProfile? GetTkProfileById(long userId)
    {
        if (!IsConnected) return null;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM tk_profile WHERE user_id = @userId";
            cmd.Parameters.AddWithValue("userId", userId);

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapTkProfile(reader) : null;
        }
    }

    public TkProfile? GetTkProfileByUsername(string username)
    {
        if (!IsConnected) return null;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM tk_profile WHERE username = @username";
            cmd.Parameters.AddWithValue("username", username);

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapTkProfile(reader) : null;
        }
    }

    public bool TkProfileExists(long userId)
    {
        if (!IsConnected) return false;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM tk_profile WHERE user_id = @userId";
            cmd.Parameters.AddWithValue("userId", userId);
            return Convert.ToInt64(cmd.ExecuteScalar()) > 0;
        }
    }

    public void AddTkProfile(TkProfile profile)
    {
        if (!IsConnected) throw new InvalidOperationException("Database not connected");

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO tk_profile (status, user_id, username, nickname, region, created_at_ts, created_at)
                VALUES (@status, @userId, @username, @nickname, @region, @createdAtTs, @createdAt)
                ON CONFLICT (user_id) DO NOTHING";

            cmd.Parameters.AddWithValue("status", profile.Status);
            cmd.Parameters.AddWithValue("userId", profile.UserId);
            cmd.Parameters.AddWithValue("username", profile.Username);
            cmd.Parameters.AddWithValue("nickname", profile.Nickname);
            cmd.Parameters.AddWithValue("region", profile.Region);
            cmd.Parameters.AddWithValue("createdAtTs", profile.CreatedAtTs.HasValue ? profile.CreatedAtTs.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("createdAt", profile.CreatedAt);

            cmd.ExecuteNonQuery();
        }
    }

    public void UpdateTkProfile(TkProfile profile)
    {
        if (!IsConnected) return;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE tk_profile SET
                    status = @status,
                    username = @username,
                    nickname = @nickname,
                    region = @region,
                    created_at_ts = @createdAtTs
                WHERE user_id = @userId";

            cmd.Parameters.AddWithValue("status", profile.Status);
            cmd.Parameters.AddWithValue("userId", profile.UserId);
            cmd.Parameters.AddWithValue("username", profile.Username);
            cmd.Parameters.AddWithValue("nickname", profile.Nickname);
            cmd.Parameters.AddWithValue("region", profile.Region);
            cmd.Parameters.AddWithValue("createdAtTs", profile.CreatedAtTs.HasValue ? profile.CreatedAtTs.Value : DBNull.Value);

            cmd.ExecuteNonQuery();
        }
    }

    public void UpdateTkProfileStatus(long userId, bool status)
    {
        if (!IsConnected) return;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE tk_profile SET status = @status WHERE user_id = @userId";
            cmd.Parameters.AddWithValue("status", status);
            cmd.Parameters.AddWithValue("userId", userId);
            cmd.ExecuteNonQuery();
        }
    }

    public void DeleteTkProfile(long userId)
    {
        if (!IsConnected) return;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM tk_profile WHERE user_id = @userId";
            cmd.Parameters.AddWithValue("userId", userId);
            cmd.ExecuteNonQuery();
        }
    }

    private static TkProfile MapTkProfile(NpgsqlDataReader reader) => new()
    {
        Status = reader.GetBoolean(reader.GetOrdinal("status")),
        UserId = reader.GetInt64(reader.GetOrdinal("user_id")),
        Username = reader.IsDBNull(reader.GetOrdinal("username")) ? "" : reader.GetString(reader.GetOrdinal("username")),
        Nickname = reader.IsDBNull(reader.GetOrdinal("nickname")) ? "" : reader.GetString(reader.GetOrdinal("nickname")),
        Region = reader.IsDBNull(reader.GetOrdinal("region")) ? "" : reader.GetString(reader.GetOrdinal("region")),
        CreatedAtTs = reader.IsDBNull(reader.GetOrdinal("created_at_ts")) ? null : reader.GetDateTime(reader.GetOrdinal("created_at_ts")),
        CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
    };

    #endregion

    #region TikTok Data Operations

    public void AddTkData(TkData data)
    {
        if (!IsConnected) throw new InvalidOperationException("Database not connected");

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO tk_data (user_id, follower_count, heart_count, video_count, recorded_at)
                VALUES (@userId, @followerCount, @heartCount, @videoCount, @recordedAt)";

            cmd.Parameters.AddWithValue("userId", data.UserId);
            cmd.Parameters.AddWithValue("followerCount", data.FollowerCount);
            cmd.Parameters.AddWithValue("heartCount", data.HeartCount);
            cmd.Parameters.AddWithValue("videoCount", data.VideoCount);
            cmd.Parameters.AddWithValue("recordedAt", data.RecordedAt);

            cmd.ExecuteNonQuery();
        }
    }

    public void AddTkDataBatch(List<TkData> dataList)
    {
        if (!IsConnected) throw new InvalidOperationException("Database not connected");
        if (dataList.Count == 0) return;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var transaction = conn.BeginTransaction();

            try
            {
                foreach (var data in dataList)
                {
                    using var cmd = conn.CreateCommand();
                    cmd.Transaction = transaction;
                    cmd.CommandText = @"
                        INSERT INTO tk_data (user_id, follower_count, heart_count, video_count, recorded_at)
                        VALUES (@userId, @followerCount, @heartCount, @videoCount, @recordedAt)";

                    cmd.Parameters.AddWithValue("userId", data.UserId);
                    cmd.Parameters.AddWithValue("followerCount", data.FollowerCount);
                    cmd.Parameters.AddWithValue("heartCount", data.HeartCount);
                    cmd.Parameters.AddWithValue("videoCount", data.VideoCount);
                    cmd.Parameters.AddWithValue("recordedAt", data.RecordedAt);

                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public List<TkData> GetRecentTkData(int limit = 100)
    {
        if (!IsConnected) return new List<TkData>();

        lock (_lock)
        {
            var dataList = new List<TkData>();
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $@"
                SELECT d.*, p.username, p.nickname
                FROM tk_data d
                JOIN tk_profile p ON d.user_id = p.user_id
                ORDER BY d.recorded_at DESC
                LIMIT {limit}";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dataList.Add(MapTkData(reader));
            }
            return dataList;
        }
    }

    public List<TkData> GetTkDataByUserId(long userId, int limit = 50)
    {
        if (!IsConnected) return new List<TkData>();

        lock (_lock)
        {
            var dataList = new List<TkData>();
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT d.*, p.username, p.nickname
                FROM tk_data d
                JOIN tk_profile p ON d.user_id = p.user_id
                WHERE d.user_id = @userId
                ORDER BY d.recorded_at DESC
                LIMIT @limit";
            cmd.Parameters.AddWithValue("userId", userId);
            cmd.Parameters.AddWithValue("limit", limit);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dataList.Add(MapTkData(reader));
            }
            return dataList;
        }
    }

    public TkData? GetLatestTkDataByUserId(long userId)
    {
        if (!IsConnected) return null;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT d.*, p.username, p.nickname
                FROM tk_data d
                JOIN tk_profile p ON d.user_id = p.user_id
                WHERE d.user_id = @userId
                ORDER BY d.recorded_at DESC
                LIMIT 1";
            cmd.Parameters.AddWithValue("userId", userId);

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapTkData(reader) : null;
        }
    }

    public long GetTkDataCount()
    {
        if (!IsConnected) return 0;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM tk_data";
            return Convert.ToInt64(cmd.ExecuteScalar());
        }
    }

    private static TkData MapTkData(NpgsqlDataReader reader) => new()
    {
        DataId = reader.GetInt32(reader.GetOrdinal("data_id")),
        UserId = reader.GetInt64(reader.GetOrdinal("user_id")),
        FollowerCount = reader.IsDBNull(reader.GetOrdinal("follower_count")) ? 0 : reader.GetInt64(reader.GetOrdinal("follower_count")),
        HeartCount = reader.IsDBNull(reader.GetOrdinal("heart_count")) ? 0 : reader.GetInt64(reader.GetOrdinal("heart_count")),
        VideoCount = reader.IsDBNull(reader.GetOrdinal("video_count")) ? 0 : reader.GetInt32(reader.GetOrdinal("video_count")),
        RecordedAt = reader.GetDateTime(reader.GetOrdinal("recorded_at")),
        Username = reader.IsDBNull(reader.GetOrdinal("username")) ? "" : reader.GetString(reader.GetOrdinal("username")),
        Nickname = reader.IsDBNull(reader.GetOrdinal("nickname")) ? "" : reader.GetString(reader.GetOrdinal("nickname"))
    };

    #endregion
}

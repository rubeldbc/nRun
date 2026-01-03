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

                // Ensure Facebook tables exist (for databases created before FB feature was added)
                EnsureFacebookTablesExist(conn);

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

            // Ensure Facebook tables exist (for databases created before FB feature was added)
            EnsureFacebookTablesExist(conn);

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
                site_name VARCHAR(255) NOT NULL UNIQUE,
                site_link VARCHAR(500) NOT NULL,
                site_logo VARCHAR(500),
                site_category VARCHAR(100) NOT NULL,
                site_country VARCHAR(100) NOT NULL,
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

            CREATE TABLE IF NOT EXISTS tk_profile (
                user_id BIGINT PRIMARY KEY,
                status BOOLEAN DEFAULT TRUE,
                username VARCHAR(255),
                nickname VARCHAR(255),
                region VARCHAR(100),
                created_at_ts TIMESTAMP,
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            );

            CREATE TABLE IF NOT EXISTS tk_data (
                data_id SERIAL PRIMARY KEY,
                user_id BIGINT NOT NULL REFERENCES tk_profile(user_id) ON DELETE CASCADE,
                follower_count BIGINT DEFAULT 0,
                heart_count BIGINT DEFAULT 0,
                video_count INTEGER DEFAULT 0,
                followers_change BIGINT DEFAULT 0,
                hearts_change BIGINT DEFAULT 0,
                videos_change INTEGER DEFAULT 0,
                recorded_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            );

            CREATE TABLE IF NOT EXISTS fb_profile (
                user_id BIGINT PRIMARY KEY,
                status BOOLEAN DEFAULT TRUE,
                username VARCHAR(255),
                nickname VARCHAR(255),
                company_name VARCHAR(255),
                company_type VARCHAR(100),
                page_type VARCHAR(100),
                region VARCHAR(100),
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            );

            CREATE TABLE IF NOT EXISTS fb_data (
                data_id SERIAL PRIMARY KEY,
                user_id BIGINT NOT NULL REFERENCES fb_profile(user_id) ON DELETE CASCADE,
                followers_count BIGINT DEFAULT 0,
                talking_about BIGINT DEFAULT 0,
                followers_change BIGINT DEFAULT 0,
                talking_about_change BIGINT DEFAULT 0,
                recorded_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            );

            CREATE INDEX IF NOT EXISTS idx_site_info_link ON site_info(site_link);
            CREATE INDEX IF NOT EXISTS idx_site_info_name ON site_info(site_name);
            CREATE INDEX IF NOT EXISTS idx_news_info_site ON news_info(site_id);
            CREATE INDEX IF NOT EXISTS idx_news_info_created ON news_info(created_at DESC);
            CREATE INDEX IF NOT EXISTS idx_news_info_url ON news_info(news_url);
            CREATE INDEX IF NOT EXISTS idx_tk_profile_username ON tk_profile(username);
            CREATE INDEX IF NOT EXISTS idx_tk_data_user ON tk_data(user_id);
            CREATE INDEX IF NOT EXISTS idx_tk_data_recorded ON tk_data(recorded_at DESC);
            CREATE INDEX IF NOT EXISTS idx_fb_profile_username ON fb_profile(username);
            CREATE INDEX IF NOT EXISTS idx_fb_data_user ON fb_data(user_id);
            CREATE INDEX IF NOT EXISTS idx_fb_data_recorded ON fb_data(recorded_at DESC);
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

    private void EnsureFacebookTablesExist(NpgsqlConnection conn)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS fb_profile (
                user_id BIGINT PRIMARY KEY,
                status BOOLEAN DEFAULT TRUE,
                username VARCHAR(255),
                nickname VARCHAR(255),
                company_name VARCHAR(255),
                company_type VARCHAR(100),
                page_type VARCHAR(100),
                region VARCHAR(100),
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            );

            CREATE TABLE IF NOT EXISTS fb_data (
                data_id SERIAL PRIMARY KEY,
                user_id BIGINT NOT NULL REFERENCES fb_profile(user_id) ON DELETE CASCADE,
                followers_count BIGINT DEFAULT 0,
                talking_about BIGINT DEFAULT 0,
                followers_change BIGINT DEFAULT 0,
                talking_about_change BIGINT DEFAULT 0,
                recorded_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            );

            CREATE INDEX IF NOT EXISTS idx_fb_profile_username ON fb_profile(username);
            CREATE INDEX IF NOT EXISTS idx_fb_data_user ON fb_data(user_id);
            CREATE INDEX IF NOT EXISTS idx_fb_data_recorded ON fb_data(recorded_at DESC);
     ";
        cmd.ExecuteNonQuery();
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

    public int AddTkData(TkData data)
    {
        if (!IsConnected) throw new InvalidOperationException("Database not connected");

        lock (_lock)
        {
            using var conn = GetConnection();

            // Get the latest previous record for this user to calculate changes
            var previousData = GetLatestTkDataByUserIdInternal(conn, data.UserId);
            if (previousData != null)
            {
                data.FollowersChange = data.FollowerCount - previousData.FollowerCount;
                data.HeartsChange = data.HeartCount - previousData.HeartCount;
                data.VideosChange = data.VideoCount - previousData.VideoCount;
            }
            else
            {
                // First record for this user, no changes
                data.FollowersChange = 0;
                data.HeartsChange = 0;
                data.VideosChange = 0;
            }

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO tk_data (user_id, follower_count, heart_count, video_count,
                    followers_change, hearts_change, videos_change, recorded_at)
                VALUES (@userId, @followerCount, @heartCount, @videoCount,
                    @followersChange, @heartsChange, @videosChange, @recordedAt)
                RETURNING data_id";

            cmd.Parameters.AddWithValue("userId", data.UserId);
            cmd.Parameters.AddWithValue("followerCount", data.FollowerCount);
            cmd.Parameters.AddWithValue("heartCount", data.HeartCount);
            cmd.Parameters.AddWithValue("videoCount", data.VideoCount);
            cmd.Parameters.AddWithValue("followersChange", data.FollowersChange);
            cmd.Parameters.AddWithValue("heartsChange", data.HeartsChange);
            cmd.Parameters.AddWithValue("videosChange", data.VideosChange);
            cmd.Parameters.AddWithValue("recordedAt", data.RecordedAt);

            var result = cmd.ExecuteScalar();
            var dataId = Convert.ToInt32(result);
            data.DataId = dataId;
            return dataId;
        }
    }

    private TkData? GetLatestTkDataByUserIdInternal(NpgsqlConnection conn, long userId)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT data_id, user_id, follower_count, heart_count, video_count,
                   followers_change, hearts_change, videos_change, recorded_at
            FROM tk_data
            WHERE user_id = @userId
            ORDER BY recorded_at DESC
            LIMIT 1";
        cmd.Parameters.AddWithValue("userId", userId);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new TkData
            {
                DataId = reader.GetInt32(reader.GetOrdinal("data_id")),
                UserId = reader.GetInt64(reader.GetOrdinal("user_id")),
                FollowerCount = reader.IsDBNull(reader.GetOrdinal("follower_count")) ? 0 : reader.GetInt64(reader.GetOrdinal("follower_count")),
                HeartCount = reader.IsDBNull(reader.GetOrdinal("heart_count")) ? 0 : reader.GetInt64(reader.GetOrdinal("heart_count")),
                VideoCount = reader.IsDBNull(reader.GetOrdinal("video_count")) ? 0 : reader.GetInt32(reader.GetOrdinal("video_count")),
                FollowersChange = reader.IsDBNull(reader.GetOrdinal("followers_change")) ? 0 : reader.GetInt64(reader.GetOrdinal("followers_change")),
                HeartsChange = reader.IsDBNull(reader.GetOrdinal("hearts_change")) ? 0 : reader.GetInt64(reader.GetOrdinal("hearts_change")),
                VideosChange = reader.IsDBNull(reader.GetOrdinal("videos_change")) ? 0 : reader.GetInt32(reader.GetOrdinal("videos_change")),
                RecordedAt = reader.GetDateTime(reader.GetOrdinal("recorded_at"))
            };
        }
        return null;
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

    public List<TkData> GetFilteredTkData(string? username = null, DateTime? fromDate = null, DateTime? toDate = null, int limit = 500)
    {
        if (!IsConnected) return new List<TkData>();

        lock (_lock)
        {
            var dataList = new List<TkData>();
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();

            var whereClause = new List<string>();
            if (!string.IsNullOrEmpty(username))
            {
                whereClause.Add("p.username = @username");
                cmd.Parameters.AddWithValue("username", username);
            }
            if (fromDate.HasValue)
            {
                whereClause.Add("d.recorded_at >= @fromDate");
                cmd.Parameters.AddWithValue("fromDate", fromDate.Value);
            }
            if (toDate.HasValue)
            {
                whereClause.Add("d.recorded_at <= @toDate");
                cmd.Parameters.AddWithValue("toDate", toDate.Value);
            }

            var whereStr = whereClause.Count > 0 ? "WHERE " + string.Join(" AND ", whereClause) : "";

            cmd.CommandText = $@"
                SELECT d.*, p.username, p.nickname
                FROM tk_data d
                JOIN tk_profile p ON d.user_id = p.user_id
                {whereStr}
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
        FollowersChange = GetColumnOrDefault(reader, "followers_change", 0L),
        HeartsChange = GetColumnOrDefault(reader, "hearts_change", 0L),
        VideosChange = GetColumnOrDefault(reader, "videos_change", 0),
        RecordedAt = reader.GetDateTime(reader.GetOrdinal("recorded_at")),
        Username = reader.IsDBNull(reader.GetOrdinal("username")) ? "" : reader.GetString(reader.GetOrdinal("username")),
        Nickname = reader.IsDBNull(reader.GetOrdinal("nickname")) ? "" : reader.GetString(reader.GetOrdinal("nickname"))
    };

    private static T GetColumnOrDefault<T>(NpgsqlDataReader reader, string columnName, T defaultValue)
    {
        try
        {
            var ordinal = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(ordinal)) return defaultValue;
            return (T)Convert.ChangeType(reader.GetValue(ordinal), typeof(T));
        }
        catch
        {
            // Column doesn't exist (old database schema)
            return defaultValue;
        }
    }

    #endregion

    #region Facebook Profile Operations

    public List<FbProfile> GetAllFbProfiles()
    {
        if (!IsConnected) return new List<FbProfile>();

        lock (_lock)
        {
            var profiles = new List<FbProfile>();
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM fb_profile ORDER BY username";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                profiles.Add(MapFbProfile(reader));
            }
            return profiles;
        }
    }

    public List<FbProfile> GetActiveFbProfiles()
    {
        if (!IsConnected) return new List<FbProfile>();

        lock (_lock)
        {
            var profiles = new List<FbProfile>();
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM fb_profile WHERE status = TRUE ORDER BY username";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                profiles.Add(MapFbProfile(reader));
            }
            return profiles;
        }
    }

    public FbProfile? GetFbProfileById(long userId)
    {
        if (!IsConnected) return null;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM fb_profile WHERE user_id = @userId";
            cmd.Parameters.AddWithValue("userId", userId);

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapFbProfile(reader) : null;
        }
    }

    public FbProfile? GetFbProfileByUsername(string username)
    {
        if (!IsConnected) return null;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM fb_profile WHERE username = @username";
            cmd.Parameters.AddWithValue("username", username);

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapFbProfile(reader) : null;
        }
    }

    public bool FbProfileExists(long userId)
    {
        if (!IsConnected) return false;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM fb_profile WHERE user_id = @userId";
            cmd.Parameters.AddWithValue("userId", userId);
            return Convert.ToInt64(cmd.ExecuteScalar()) > 0;
        }
    }

    public void AddFbProfile(FbProfile profile)
    {
        if (!IsConnected) throw new InvalidOperationException("Database not connected");

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO fb_profile (status, user_id, username, nickname, company_name, company_type, page_type, region, created_at)
                VALUES (@status, @userId, @username, @nickname, @companyName, @companyType, @pageType, @region, @createdAt)
                ON CONFLICT (user_id) DO NOTHING";

            cmd.Parameters.AddWithValue("status", profile.Status);
            cmd.Parameters.AddWithValue("userId", profile.UserId);
            cmd.Parameters.AddWithValue("username", profile.Username);
            cmd.Parameters.AddWithValue("nickname", profile.Nickname);
            cmd.Parameters.AddWithValue("companyName", profile.CompanyName);
            cmd.Parameters.AddWithValue("companyType", profile.CompanyType);
            cmd.Parameters.AddWithValue("pageType", profile.PageType);
            cmd.Parameters.AddWithValue("region", profile.Region);
            cmd.Parameters.AddWithValue("createdAt", profile.CreatedAt);

            cmd.ExecuteNonQuery();
        }
    }

    public void UpdateFbProfile(FbProfile profile)
    {
        if (!IsConnected) return;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE fb_profile SET
                    status = @status,
                    username = @username,
                    nickname = @nickname,
                    company_name = @companyName,
                    company_type = @companyType,
                    page_type = @pageType,
                    region = @region
                WHERE user_id = @userId";

            cmd.Parameters.AddWithValue("status", profile.Status);
            cmd.Parameters.AddWithValue("userId", profile.UserId);
            cmd.Parameters.AddWithValue("username", profile.Username);
            cmd.Parameters.AddWithValue("nickname", profile.Nickname);
            cmd.Parameters.AddWithValue("companyName", profile.CompanyName);
            cmd.Parameters.AddWithValue("companyType", profile.CompanyType);
            cmd.Parameters.AddWithValue("pageType", profile.PageType);
            cmd.Parameters.AddWithValue("region", profile.Region);

            cmd.ExecuteNonQuery();
        }
    }

    public void UpdateFbProfileStatus(long userId, bool status)
    {
        if (!IsConnected) return;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE fb_profile SET status = @status WHERE user_id = @userId";
            cmd.Parameters.AddWithValue("status", status);
            cmd.Parameters.AddWithValue("userId", userId);
            cmd.ExecuteNonQuery();
        }
    }

    public void DeleteFbProfile(long userId)
    {
        if (!IsConnected) return;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM fb_profile WHERE user_id = @userId";
            cmd.Parameters.AddWithValue("userId", userId);
            cmd.ExecuteNonQuery();
        }
    }

    private static FbProfile MapFbProfile(NpgsqlDataReader reader) => new()
    {
        Status = reader.GetBoolean(reader.GetOrdinal("status")),
        UserId = reader.GetInt64(reader.GetOrdinal("user_id")),
        Username = reader.IsDBNull(reader.GetOrdinal("username")) ? "" : reader.GetString(reader.GetOrdinal("username")),
        Nickname = reader.IsDBNull(reader.GetOrdinal("nickname")) ? "" : reader.GetString(reader.GetOrdinal("nickname")),
        CompanyName = reader.IsDBNull(reader.GetOrdinal("company_name")) ? "" : reader.GetString(reader.GetOrdinal("company_name")),
        CompanyType = reader.IsDBNull(reader.GetOrdinal("company_type")) ? "" : reader.GetString(reader.GetOrdinal("company_type")),
        PageType = reader.IsDBNull(reader.GetOrdinal("page_type")) ? "" : reader.GetString(reader.GetOrdinal("page_type")),
        Region = reader.IsDBNull(reader.GetOrdinal("region")) ? "" : reader.GetString(reader.GetOrdinal("region")),
        CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
    };

    #endregion

    #region Facebook Data Operations

    public int AddFbData(FbData data)
    {
        if (!IsConnected) throw new InvalidOperationException("Database not connected");

        lock (_lock)
        {
            using var conn = GetConnection();

            // Get the latest previous record for this user to calculate changes
            var previousData = GetLatestFbDataByUserIdInternal(conn, data.UserId);
            if (previousData != null)
            {
                data.FollowersChange = data.FollowersCount - previousData.FollowersCount;
                data.TalkingAboutChange = data.TalkingAbout - previousData.TalkingAbout;
            }
            else
            {
                // First record for this user, no changes
                data.FollowersChange = 0;
                data.TalkingAboutChange = 0;
            }

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO fb_data (user_id, followers_count, talking_about,
                    followers_change, talking_about_change, recorded_at)
                VALUES (@userId, @followersCount, @talkingAbout,
                    @followersChange, @talkingAboutChange, @recordedAt)
                RETURNING data_id";

            cmd.Parameters.AddWithValue("userId", data.UserId);
            cmd.Parameters.AddWithValue("followersCount", data.FollowersCount);
            cmd.Parameters.AddWithValue("talkingAbout", data.TalkingAbout);
            cmd.Parameters.AddWithValue("followersChange", data.FollowersChange);
            cmd.Parameters.AddWithValue("talkingAboutChange", data.TalkingAboutChange);
            cmd.Parameters.AddWithValue("recordedAt", data.RecordedAt);

            var result = cmd.ExecuteScalar();
            var dataId = Convert.ToInt32(result);
            data.DataId = dataId;
            return dataId;
        }
    }

    private FbData? GetLatestFbDataByUserIdInternal(NpgsqlConnection conn, long userId)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT data_id, user_id, followers_count, talking_about,
                   followers_change, talking_about_change, recorded_at
            FROM fb_data
            WHERE user_id = @userId
            ORDER BY recorded_at DESC
            LIMIT 1";
        cmd.Parameters.AddWithValue("userId", userId);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new FbData
            {
                DataId = reader.GetInt32(reader.GetOrdinal("data_id")),
                UserId = reader.GetInt64(reader.GetOrdinal("user_id")),
                FollowersCount = reader.IsDBNull(reader.GetOrdinal("followers_count")) ? 0 : reader.GetInt64(reader.GetOrdinal("followers_count")),
                TalkingAbout = reader.IsDBNull(reader.GetOrdinal("talking_about")) ? 0 : reader.GetInt64(reader.GetOrdinal("talking_about")),
                FollowersChange = reader.IsDBNull(reader.GetOrdinal("followers_change")) ? 0 : reader.GetInt64(reader.GetOrdinal("followers_change")),
                TalkingAboutChange = reader.IsDBNull(reader.GetOrdinal("talking_about_change")) ? 0 : reader.GetInt64(reader.GetOrdinal("talking_about_change")),
                RecordedAt = reader.GetDateTime(reader.GetOrdinal("recorded_at"))
            };
        }
        return null;
    }

    public List<FbData> GetRecentFbData(int limit = 100)
    {
        if (!IsConnected) return new List<FbData>();

        lock (_lock)
        {
            var dataList = new List<FbData>();
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $@"
                SELECT d.*, p.username, p.nickname
                FROM fb_data d
                JOIN fb_profile p ON d.user_id = p.user_id
                ORDER BY d.recorded_at DESC
                LIMIT {limit}";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dataList.Add(MapFbData(reader));
            }
            return dataList;
        }
    }

    public List<FbData> GetFilteredFbData(string? username = null, DateTime? fromDate = null, DateTime? toDate = null, int limit = 500)
    {
        if (!IsConnected) return new List<FbData>();

        lock (_lock)
        {
            var dataList = new List<FbData>();
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();

            var whereClause = new List<string>();
            if (!string.IsNullOrEmpty(username))
            {
                whereClause.Add("p.username = @username");
                cmd.Parameters.AddWithValue("username", username);
            }
            if (fromDate.HasValue)
            {
                whereClause.Add("d.recorded_at >= @fromDate");
                cmd.Parameters.AddWithValue("fromDate", fromDate.Value);
            }
            if (toDate.HasValue)
            {
                whereClause.Add("d.recorded_at <= @toDate");
                cmd.Parameters.AddWithValue("toDate", toDate.Value);
            }

            var whereStr = whereClause.Count > 0 ? "WHERE " + string.Join(" AND ", whereClause) : "";

            cmd.CommandText = $@"
                SELECT d.*, p.username, p.nickname
                FROM fb_data d
                JOIN fb_profile p ON d.user_id = p.user_id
                {whereStr}
                ORDER BY d.recorded_at DESC
                LIMIT {limit}";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dataList.Add(MapFbData(reader));
            }
            return dataList;
        }
    }

    public List<FbData> GetFbDataByUserId(long userId, int limit = 50)
    {
        if (!IsConnected) return new List<FbData>();

        lock (_lock)
        {
            var dataList = new List<FbData>();
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT d.*, p.username, p.nickname
                FROM fb_data d
                JOIN fb_profile p ON d.user_id = p.user_id
                WHERE d.user_id = @userId
                ORDER BY d.recorded_at DESC
                LIMIT @limit";
            cmd.Parameters.AddWithValue("userId", userId);
            cmd.Parameters.AddWithValue("limit", limit);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dataList.Add(MapFbData(reader));
            }
            return dataList;
        }
    }

    public FbData? GetLatestFbDataByUserId(long userId)
    {
        if (!IsConnected) return null;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT d.*, p.username, p.nickname
                FROM fb_data d
                JOIN fb_profile p ON d.user_id = p.user_id
                WHERE d.user_id = @userId
                ORDER BY d.recorded_at DESC
                LIMIT 1";
            cmd.Parameters.AddWithValue("userId", userId);

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapFbData(reader) : null;
        }
    }

    public long GetFbDataCount()
    {
        if (!IsConnected) return 0;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM fb_data";
            return Convert.ToInt64(cmd.ExecuteScalar());
        }
    }

    private static FbData MapFbData(NpgsqlDataReader reader) => new()
    {
        DataId = reader.GetInt32(reader.GetOrdinal("data_id")),
        UserId = reader.GetInt64(reader.GetOrdinal("user_id")),
        FollowersCount = reader.IsDBNull(reader.GetOrdinal("followers_count")) ? 0 : reader.GetInt64(reader.GetOrdinal("followers_count")),
        TalkingAbout = reader.IsDBNull(reader.GetOrdinal("talking_about")) ? 0 : reader.GetInt64(reader.GetOrdinal("talking_about")),
        FollowersChange = GetColumnOrDefault(reader, "followers_change", 0L),
        TalkingAboutChange = GetColumnOrDefault(reader, "talking_about_change", 0L),
        RecordedAt = reader.GetDateTime(reader.GetOrdinal("recorded_at")),
        Username = reader.IsDBNull(reader.GetOrdinal("username")) ? "" : reader.GetString(reader.GetOrdinal("username")),
        Nickname = reader.IsDBNull(reader.GetOrdinal("nickname")) ? "" : reader.GetString(reader.GetOrdinal("nickname"))
    };

    #endregion

    #region Facebook Table Management

    public void CreateFacebookTables()
    {
        if (!IsConnected) throw new InvalidOperationException("Database not connected");

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS fb_profile (
                    user_id BIGINT PRIMARY KEY,
                    status BOOLEAN DEFAULT TRUE,
                    username VARCHAR(255),
                    nickname VARCHAR(255),
                    company_name VARCHAR(255),
                    company_type VARCHAR(100),
                    page_type VARCHAR(100),
                    region VARCHAR(100),
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                );

                CREATE TABLE IF NOT EXISTS fb_data (
                    data_id SERIAL PRIMARY KEY,
                    user_id BIGINT NOT NULL REFERENCES fb_profile(user_id) ON DELETE CASCADE,
                    followers_count BIGINT DEFAULT 0,
                    talking_about BIGINT DEFAULT 0,
                    followers_change BIGINT DEFAULT 0,
                    talking_about_change BIGINT DEFAULT 0,
                    recorded_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                );

                CREATE INDEX IF NOT EXISTS idx_fb_profile_username ON fb_profile(username);
                CREATE INDEX IF NOT EXISTS idx_fb_data_user ON fb_data(user_id);
                CREATE INDEX IF NOT EXISTS idx_fb_data_recorded ON fb_data(recorded_at DESC);
            ";
            cmd.ExecuteNonQuery();
        }
    }

    public void DeleteFacebookTables()
    {
        if (!IsConnected) throw new InvalidOperationException("Database not connected");

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                DROP TABLE IF EXISTS fb_data CASCADE;
                DROP TABLE IF EXISTS fb_profile CASCADE;
            ";
            cmd.ExecuteNonQuery();
        }
    }

    public bool FacebookTablesExist()
    {
        if (!IsConnected) return false;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT COUNT(*) FROM information_schema.tables
                WHERE table_name IN ('fb_profile', 'fb_data')";
            return Convert.ToInt64(cmd.ExecuteScalar()) == 2;
        }
    }

    #endregion
}

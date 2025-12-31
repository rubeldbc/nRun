using Npgsql;
using nRun.Models;

namespace nRun.Data;

/// <summary>
/// PostgreSQL database service for storing sites and news articles
/// </summary>
public static class DatabaseService
{
    private static string _connectionString = string.Empty;
    private static readonly object _lock = new();
    private static bool _isInitialized = false;

    public static bool IsConnected => _isInitialized && !string.IsNullOrEmpty(_connectionString);

    public static void Initialize()
    {
        var settings = SettingsManager.LoadSettings();
        _connectionString = settings.GetConnectionString();

        if (!string.IsNullOrEmpty(settings.DbPassword))
        {
            try
            {
                using var conn = GetConnection();
                // Ensure required tables exist so further queries do not fail
                EnsureTablesExist(conn);
                _isInitialized = true;
            }
            catch
            {
                _isInitialized = false;
            }
        }
    }

    public static void UpdateConnectionString(string connectionString)
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

    private static void EnsureTablesExist(NpgsqlConnection conn)
    {
        // Create tables if they do not exist. Using IF NOT EXISTS avoids errors when they already exist.
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
                last_modified TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            );

            -- Create indexes if they don't exist
            CREATE INDEX IF NOT EXISTS idx_site_info_link ON site_info(site_link);
            CREATE INDEX IF NOT EXISTS idx_news_info_site ON news_info(site_id);
            CREATE INDEX IF NOT EXISTS idx_news_info_created ON news_info(created_at DESC);
            CREATE INDEX IF NOT EXISTS idx_news_info_url ON news_info(news_url);
        ";

        cmd.ExecuteNonQuery();

        // Ensure default app_settings row exists
        using var insertCmd = conn.CreateCommand();
        insertCmd.CommandText = "INSERT INTO app_settings (id) VALUES (1) ON CONFLICT DO NOTHING";
        insertCmd.ExecuteNonQuery();
    }

    private static NpgsqlConnection GetConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;
    }

    #region Site Info Operations

    public static List<SiteInfo> GetAllSites()
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

    public static List<SiteInfo> GetActiveSites()
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

    public static SiteInfo? GetSiteById(string siteId)
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

    public static string AddSite(SiteInfo site)
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

    public static void UpdateSite(SiteInfo site)
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

    public static void UpdateSiteStats(string siteId, bool success)
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

    public static void DeleteSite(string siteId)
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

    public static bool NewsExists(string newsUrl)
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

    public static long AddNews(NewsInfo news)
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

    public static List<NewsInfo> GetRecentNews(int limit = 100)
    {
        if (!IsConnected) return new List<NewsInfo>();

        lock (_lock)
        {
            var news = new List<NewsInfo>();
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT n.*, s.site_name
                FROM news_info n
                JOIN site_info s ON n.site_id = s.site_id
                ORDER BY n.created_at DESC
                LIMIT @limit";
            cmd.Parameters.AddWithValue("limit", limit);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                news.Add(MapNewsInfo(reader));
            }
            return news;
        }
    }

    public static List<NewsInfo> GetNewsBySite(string siteId, int limit = 50)
    {
        if (!IsConnected) return new List<NewsInfo>();

        lock (_lock)
        {
            var news = new List<NewsInfo>();
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT n.*, s.site_name
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

    public static NewsInfo? GetNewsById(long serial)
    {
        if (!IsConnected) return null;

        lock (_lock)
        {
            using var conn = GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT n.*, s.site_name
                FROM news_info n
                JOIN site_info s ON n.site_id = s.site_id
                WHERE n.serial = @serial";
            cmd.Parameters.AddWithValue("serial", serial);

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapNewsInfo(reader) : null;
        }
    }

    public static void MarkNewsAsRead(long serial)
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

    public static void DeleteNews(long serial)
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

    public static long GetNewsCount()
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
        NewsTitle = reader.GetString(reader.GetOrdinal("news_title")),
        NewsText = reader.IsDBNull(reader.GetOrdinal("news_text")) ? "" : reader.GetString(reader.GetOrdinal("news_text")),
        NewsUrl = reader.GetString(reader.GetOrdinal("news_url")),
        IsRead = reader.GetBoolean(reader.GetOrdinal("is_read")),
        CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
    };

    #endregion
}

using Npgsql;

namespace nRun.Models;

/// <summary>
/// Application settings stored in local JSON file
/// </summary>
public class AppSettings
{
    // Scraping Settings
    public int CheckIntervalMinutes { get; set; } = 5;
    public int DelayBetweenLinksSeconds { get; set; } = 2; // 0-10000 seconds
    public int MaxArticlesPerSite { get; set; } = 10;
    public bool UseHeadlessBrowser { get; set; } = true;
    public int BrowserTimeoutSeconds { get; set; } = 30;
    public bool AutoStartScraping { get; set; } = false;

    // No Scrape Time Window Settings
    public bool NoScrapEnabled { get; set; } = false;
    public int NoScrapStartHour { get; set; } = 0;
    public int NoScrapStartMinute { get; set; } = 0;
    public int NoScrapEndHour { get; set; } = 6;
    public int NoScrapEndMinute { get; set; } = 0;

    // Display Settings
    public int MaxDisplayedArticles { get; set; } = 100;

    // Memurai/Redis Settings
    public string MemuraiHost { get; set; } = "localhost";
    public int MemuraiPort { get; set; } = 6379;
    public string MemuraiPassword { get; set; } = string.Empty;
    public int MemuraiSyncIntervalSeconds { get; set; } = 30;

    // TikTok Settings
    public int TikTokDelaySeconds { get; set; } = 10;
    public List<TikTokScheduleSettings> TikTokSchedules { get; set; } = new();

    // Facebook Settings
    public int FacebookDelaySeconds { get; set; } = 10;
    public int FacebookChunkSize { get; set; } = 10;
    public int FacebookChunkDelayMinutes { get; set; } = 5;
    public int FacebookBulkDelaySeconds { get; set; } = 10;
    public List<FacebookScheduleSettings> FacebookSchedules { get; set; } = new();

    // Database Connection Settings
    public string DbHost { get; set; } = "localhost";
    public int DbPort { get; set; } = 5432;
    public string DbName { get; set; } = "nrun_db";
    public string DbUser { get; set; } = "postgres";
    public string DbPassword { get; set; } = string.Empty;

    public string GetConnectionString()
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = DbHost,
            Port = DbPort,
            Database = DbName,
            Username = DbUser,
            Password = DbPassword
        };
        return builder.ConnectionString;
    }
}

/// <summary>
/// Database connection settings for PostgreSQL
/// </summary>
public class DbConnectionSettings
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5432;
    public string Database { get; set; } = "nrun_db";
    public string Username { get; set; } = "postgres";
    public string Password { get; set; } = string.Empty;

    public string GetConnectionString()
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = Host,
            Port = Port,
            Database = Database,
            Username = Username,
            Password = Password
        };
        return builder.ConnectionString;
    }

    public string GetServerConnectionString()
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = Host,
            Port = Port,
            Database = "postgres",
            Username = Username,
            Password = Password
        };
        return builder.ConnectionString;
    }
}

/// <summary>
/// TikTok schedule settings for persistence
/// </summary>
public class TikTokScheduleSettings
{
    public int Hour { get; set; }
    public int Minute { get; set; }
    public bool IsEnabled { get; set; } = true;
}

/// <summary>
/// Facebook schedule settings for persistence
/// </summary>
public class FacebookScheduleSettings
{
    public int Hour { get; set; }
    public int Minute { get; set; }
    public bool IsEnabled { get; set; } = true;
}

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

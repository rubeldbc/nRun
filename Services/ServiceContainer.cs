using nRun.Data;
using nRun.Services.Interfaces;

namespace nRun.Services;

/// <summary>
/// Simple service container for managing service instances
/// </summary>
public static class ServiceContainer
{
    private static IDatabaseService? _database;
    private static ISettingsManager? _settings;
    private static ILogoDownloadService? _logoDownload;

    public static IDatabaseService Database => _database ?? throw new InvalidOperationException("ServiceContainer not initialized. Call Initialize() first.");
    public static ISettingsManager Settings => _settings ?? throw new InvalidOperationException("ServiceContainer not initialized. Call Initialize() first.");
    public static ILogoDownloadService LogoDownload => _logoDownload ?? throw new InvalidOperationException("ServiceContainer not initialized. Call Initialize() first.");

    /// <summary>
    /// Initialize all services. Call this at application startup.
    /// </summary>
    public static void Initialize()
    {
        _settings = new SettingsManager();
        _database = new DatabaseService(_settings);
        _logoDownload = new LogoDownloadService();
    }

    /// <summary>
    /// Initialize with custom service implementations (for testing)
    /// </summary>
    public static void Initialize(IDatabaseService database, ISettingsManager settings, ILogoDownloadService logoDownload)
    {
        _database = database;
        _settings = settings;
        _logoDownload = logoDownload;
    }
}

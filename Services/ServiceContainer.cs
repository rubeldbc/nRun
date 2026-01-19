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
    private static IMemuraiService? _memurai;
    private static INoScrapWindowService? _noScrapWindow;
    private static IScheduleCalculationService? _scheduleCalculation;
    private static IBulkImportService? _bulkImport;
    private static CacheService? _cache;
    private static CloudflareBypassSettingsManager? _cloudflareBypass;

    public static IDatabaseService Database => _database ?? throw new InvalidOperationException("ServiceContainer not initialized. Call Initialize() first.");
    public static ISettingsManager Settings => _settings ?? throw new InvalidOperationException("ServiceContainer not initialized. Call Initialize() first.");
    public static ILogoDownloadService LogoDownload => _logoDownload ?? throw new InvalidOperationException("ServiceContainer not initialized. Call Initialize() first.");
    public static IMemuraiService Memurai => _memurai ?? throw new InvalidOperationException("ServiceContainer not initialized. Call Initialize() first.");
    public static INoScrapWindowService NoScrapWindow => _noScrapWindow ?? throw new InvalidOperationException("ServiceContainer not initialized. Call Initialize() first.");
    public static IScheduleCalculationService ScheduleCalculation => _scheduleCalculation ?? throw new InvalidOperationException("ServiceContainer not initialized. Call Initialize() first.");
    public static IBulkImportService BulkImport => _bulkImport ?? throw new InvalidOperationException("ServiceContainer not initialized. Call Initialize() first.");
    public static CacheService Cache => _cache ?? throw new InvalidOperationException("ServiceContainer not initialized. Call Initialize() first.");
    public static CloudflareBypassSettingsManager CloudflareBypass => _cloudflareBypass ?? throw new InvalidOperationException("ServiceContainer not initialized. Call Initialize() first.");

    /// <summary>
    /// Initialize all services. Call this at application startup.
    /// </summary>
    public static void Initialize()
    {
        _cache = new CacheService();
        _settings = new SettingsManager();
        _database = new DatabaseService(_settings);
        _logoDownload = new LogoDownloadService();
        _memurai = new MemuraiService(_settings);
        _noScrapWindow = new NoScrapWindowService(_settings);
        _scheduleCalculation = new ScheduleCalculationService();
        _bulkImport = new BulkImportService();
        _cloudflareBypass = new CloudflareBypassSettingsManager();
    }

    /// <summary>
    /// Initialize with custom service implementations (for testing)
    /// </summary>
    public static void Initialize(IDatabaseService database, ISettingsManager settings, ILogoDownloadService logoDownload, IMemuraiService? memurai = null)
    {
        _cache = new CacheService();
        _database = database;
        _settings = settings;
        _logoDownload = logoDownload;
        _memurai = memurai ?? new MemuraiService(settings);
        _noScrapWindow = new NoScrapWindowService(settings);
        _scheduleCalculation = new ScheduleCalculationService();
        _bulkImport = new BulkImportService();
        _cloudflareBypass = new CloudflareBypassSettingsManager();
    }
}

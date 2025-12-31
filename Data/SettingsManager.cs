using System.Text.Json;
using nRun.Models;

namespace nRun.Data;

/// <summary>
/// Manages application settings stored in local JSON file
/// </summary>
public static class SettingsManager
{
    private static readonly string SettingsFolder = Path.Combine(Application.StartupPath, "Settings");
    private static readonly string SettingsFile = Path.Combine(SettingsFolder, "app_settings.json");
    private static readonly object _lock = new();

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static void Initialize()
    {
        if (!Directory.Exists(SettingsFolder))
        {
            Directory.CreateDirectory(SettingsFolder);
        }

        // Create default settings if not exists
        if (!File.Exists(SettingsFile))
        {
            SaveSettings(new AppSettings());
        }
    }

    public static AppSettings LoadSettings()
    {
        lock (_lock)
        {
            try
            {
                if (File.Exists(SettingsFile))
                {
                    var json = File.ReadAllText(SettingsFile);
                    return JsonSerializer.Deserialize<AppSettings>(json, _jsonOptions) ?? new AppSettings();
                }
            }
            catch
            {
                // Return default settings on error
            }
            return new AppSettings();
        }
    }

    public static void SaveSettings(AppSettings settings)
    {
        lock (_lock)
        {
            if (!Directory.Exists(SettingsFolder))
            {
                Directory.CreateDirectory(SettingsFolder);
            }

            var json = JsonSerializer.Serialize(settings, _jsonOptions);
            File.WriteAllText(SettingsFile, json);
        }
    }

    public static void UpdateScrapingSettings(int checkIntervalMinutes, int delayBetweenLinksSeconds,
        int maxArticlesPerSite, int browserTimeoutSeconds, bool useHeadless, bool autoStart)
    {
        var settings = LoadSettings();
        settings.CheckIntervalMinutes = checkIntervalMinutes;
        settings.DelayBetweenLinksSeconds = delayBetweenLinksSeconds;
        settings.MaxArticlesPerSite = maxArticlesPerSite;
        settings.BrowserTimeoutSeconds = browserTimeoutSeconds;
        settings.UseHeadlessBrowser = useHeadless;
        settings.AutoStartScraping = autoStart;
        SaveSettings(settings);
    }
}

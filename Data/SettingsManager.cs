using System.Text.Json;
using nRun.Models;
using nRun.Services.Interfaces;

namespace nRun.Data;

/// <summary>
/// Manages application settings stored in local JSON file
/// </summary>
public class SettingsManager : ISettingsManager
{
    private readonly string _settingsFolder;
    private readonly string _settingsFile;
    private readonly object _lock = new();

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public SettingsManager()
    {
        _settingsFolder = Path.Combine(Application.StartupPath, "Settings");
        _settingsFile = Path.Combine(_settingsFolder, "app_settings.json");
    }

    public void Initialize()
    {
        if (!Directory.Exists(_settingsFolder))
        {
            Directory.CreateDirectory(_settingsFolder);
        }

        // Create default settings if not exists
        if (!File.Exists(_settingsFile))
        {
            SaveSettings(new AppSettings());
        }
    }

    public AppSettings LoadSettings()
    {
        lock (_lock)
        {
            try
            {
                if (File.Exists(_settingsFile))
                {
                    var json = File.ReadAllText(_settingsFile);
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

    public void SaveSettings(AppSettings settings)
    {
        lock (_lock)
        {
            if (!Directory.Exists(_settingsFolder))
            {
                Directory.CreateDirectory(_settingsFolder);
            }

            var json = JsonSerializer.Serialize(settings, _jsonOptions);
            File.WriteAllText(_settingsFile, json);
        }
    }

    public void UpdateScrapingSettings(int checkIntervalMinutes, int delayBetweenLinksSeconds,
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

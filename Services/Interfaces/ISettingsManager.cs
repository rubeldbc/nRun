using nRun.Models;

namespace nRun.Services.Interfaces;

/// <summary>
/// Interface for application settings management
/// </summary>
public interface ISettingsManager
{
    void Initialize();
    AppSettings LoadSettings();
    void SaveSettings(AppSettings settings);
    void UpdateScrapingSettings(int checkIntervalMinutes, int delayBetweenLinksSeconds,
        int maxArticlesPerSite, int browserTimeoutSeconds, bool useHeadless, bool autoStart);
}

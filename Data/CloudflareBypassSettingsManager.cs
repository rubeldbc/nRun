using System.Text.Json;

namespace nRun.Data;

/// <summary>
/// Manages per-site cloudflare bypass settings stored in local JSON file
/// </summary>
public class CloudflareBypassSettingsManager
{
    private readonly string _settingsFile;
    private readonly object _lock = new();
    private HashSet<string> _enabledSiteIds = new();

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public CloudflareBypassSettingsManager()
    {
        var settingsFolder = Path.Combine(Application.StartupPath, "Settings");
        _settingsFile = Path.Combine(settingsFolder, "settings.json");

        // Ensure settings folder exists
        if (!Directory.Exists(settingsFolder))
        {
            Directory.CreateDirectory(settingsFolder);
        }

        Load();
    }

    /// <summary>
    /// Loads settings from file
    /// </summary>
    public void Load()
    {
        lock (_lock)
        {
            try
            {
                if (File.Exists(_settingsFile))
                {
                    var json = File.ReadAllText(_settingsFile);
                    var settings = JsonSerializer.Deserialize<CloudflareBypassSettings>(json, _jsonOptions);
                    _enabledSiteIds = settings?.CloudflareBypassEnabledSiteIds?.ToHashSet() ?? new HashSet<string>();
                }
            }
            catch
            {
                _enabledSiteIds = new HashSet<string>();
            }
        }
    }

    /// <summary>
    /// Saves settings to file
    /// </summary>
    public void Save()
    {
        lock (_lock)
        {
            try
            {
                var settingsFolder = Path.GetDirectoryName(_settingsFile);
                if (!string.IsNullOrEmpty(settingsFolder) && !Directory.Exists(settingsFolder))
                {
                    Directory.CreateDirectory(settingsFolder);
                }

                var settings = new CloudflareBypassSettings
                {
                    CloudflareBypassEnabledSiteIds = _enabledSiteIds.ToList()
                };

                var json = JsonSerializer.Serialize(settings, _jsonOptions);
                File.WriteAllText(_settingsFile, json);
            }
            catch
            {
                // Ignore save errors
            }
        }
    }

    /// <summary>
    /// Checks if cloudflare bypass is enabled for a site
    /// </summary>
    public bool IsEnabled(string siteId)
    {
        lock (_lock)
        {
            return _enabledSiteIds.Contains(siteId);
        }
    }

    /// <summary>
    /// Sets cloudflare bypass enabled/disabled for a site
    /// </summary>
    public void SetEnabled(string siteId, bool enabled)
    {
        lock (_lock)
        {
            if (enabled)
            {
                _enabledSiteIds.Add(siteId);
            }
            else
            {
                _enabledSiteIds.Remove(siteId);
            }
            Save();
        }
    }

    /// <summary>
    /// Toggles cloudflare bypass for a site
    /// </summary>
    public bool Toggle(string siteId)
    {
        lock (_lock)
        {
            if (_enabledSiteIds.Contains(siteId))
            {
                _enabledSiteIds.Remove(siteId);
                Save();
                return false;
            }
            else
            {
                _enabledSiteIds.Add(siteId);
                Save();
                return true;
            }
        }
    }

    /// <summary>
    /// Gets all enabled site IDs
    /// </summary>
    public HashSet<string> GetEnabledSiteIds()
    {
        lock (_lock)
        {
            return new HashSet<string>(_enabledSiteIds);
        }
    }
}

/// <summary>
/// Settings model for JSON serialization
/// </summary>
public class CloudflareBypassSettings
{
    public List<string> CloudflareBypassEnabledSiteIds { get; set; } = new();
}

using nRun.Models;

namespace nRun.Services;

/// <summary>
/// Factory for creating WebDriver instances on-demand with automatic tracking and cleanup.
/// All drivers created through this factory are tracked and can be disposed collectively.
/// </summary>
public static class WebDriverFactory
{
    private static readonly object _lock = new();
    private static readonly Dictionary<Guid, WeakReference<WebDriverService>> _activeDrivers = new();
    private static readonly List<Guid> _disposedDriverIds = new();

    /// <summary>
    /// Creates a new WebDriver instance with settings from AppSettings
    /// </summary>
    public static WebDriverService Create(AppSettings? settings = null)
    {
        settings ??= ServiceContainer.Settings.LoadSettings();

        var driver = new WebDriverService
        {
            UseHeadless = settings.UseHeadlessBrowser,
            TimeoutSeconds = settings.BrowserTimeoutSeconds
        };

        lock (_lock)
        {
            _activeDrivers[driver.InstanceId] = new WeakReference<WebDriverService>(driver);
            CleanupDeadReferences();
        }

        return driver;
    }

    /// <summary>
    /// Creates a new WebDriver instance with custom settings
    /// </summary>
    public static WebDriverService Create(bool useHeadless, int timeoutSeconds)
    {
        var driver = new WebDriverService
        {
            UseHeadless = useHeadless,
            TimeoutSeconds = timeoutSeconds
        };

        lock (_lock)
        {
            _activeDrivers[driver.InstanceId] = new WeakReference<WebDriverService>(driver);
            CleanupDeadReferences();
        }

        return driver;
    }

    /// <summary>
    /// Creates a visible (non-headless) WebDriver for bypassing Cloudflare detection.
    /// Does NOT use the user's Chrome profile to avoid conflicts with existing Chrome windows.
    /// Instead uses anti-detection measures to appear as a real browser.
    /// </summary>
    public static WebDriverService CreateWithUserProfile(int timeoutSeconds)
    {
        var driver = new WebDriverService
        {
            UseHeadless = false,
            TimeoutSeconds = timeoutSeconds,
            UseUserProfile = false  // Don't use user profile - avoids conflict with existing Chrome
        };

        lock (_lock)
        {
            _activeDrivers[driver.InstanceId] = new WeakReference<WebDriverService>(driver);
            CleanupDeadReferences();
        }

        return driver;
    }

    /// <summary>
    /// Safely disposes a driver and removes it from tracking
    /// </summary>
    public static void SafeDispose(WebDriverService? driver)
    {
        if (driver == null) return;

        try
        {
            lock (_lock)
            {
                _activeDrivers.Remove(driver.InstanceId);
                _disposedDriverIds.Add(driver.InstanceId);
            }

            driver.CloseDriver();
            driver.Dispose();
        }
        catch
        {
            // Ensure cleanup doesn't throw
        }
    }

    /// <summary>
    /// Disposes all tracked drivers that are still alive
    /// </summary>
    public static void DisposeAll()
    {
        lock (_lock)
        {
            var driversToDispose = new List<WebDriverService>();

            foreach (var kvp in _activeDrivers)
            {
                if (kvp.Value.TryGetTarget(out var driver))
                {
                    driversToDispose.Add(driver);
                }
            }

            _activeDrivers.Clear();

            foreach (var driver in driversToDispose)
            {
                try
                {
                    driver.CloseDriver();
                    driver.Dispose();
                }
                catch
                {
                    // Continue disposing other drivers
                }
            }
        }

        // NOTE: Removed call to ProcessCleanupService.ForceCleanupAll() here
        // to avoid mutual recursion (ForceCleanupAll calls DisposeAll, which was calling ForceCleanupAll)
        // Process cleanup should be initiated from ProcessCleanupService.ForceCleanupAll() which already calls this method
    }

    /// <summary>
    /// Gets the count of currently tracked (potentially active) drivers
    /// </summary>
    public static int ActiveDriverCount
    {
        get
        {
            lock (_lock)
            {
                CleanupDeadReferences();
                return _activeDrivers.Count;
            }
        }
    }

    /// <summary>
    /// Checks if a specific driver instance is still being tracked
    /// </summary>
    public static bool IsTracked(WebDriverService driver)
    {
        lock (_lock)
        {
            return _activeDrivers.ContainsKey(driver.InstanceId);
        }
    }

    /// <summary>
    /// Removes references to drivers that have been garbage collected
    /// </summary>
    private static void CleanupDeadReferences()
    {
        var deadIds = new List<Guid>();

        foreach (var kvp in _activeDrivers)
        {
            if (!kvp.Value.TryGetTarget(out _))
            {
                deadIds.Add(kvp.Key);
            }
        }

        foreach (var id in deadIds)
        {
            _activeDrivers.Remove(id);
        }

        // Keep disposed IDs list from growing too large
        while (_disposedDriverIds.Count > 100)
        {
            _disposedDriverIds.RemoveAt(0);
        }
    }
}

using System.Diagnostics;

namespace nRun.Services;

/// <summary>
/// Service for cleaning up Chrome and ChromeDriver processes on application exit
/// </summary>
public static class ProcessCleanupService
{
    private static readonly HashSet<int> _trackedProcessIds = new();
    private static readonly object _lock = new();

    /// <summary>
    /// Tracks a process ID for cleanup on application exit
    /// </summary>
    public static void TrackProcess(int processId)
    {
        lock (_lock)
        {
            _trackedProcessIds.Add(processId);
        }
    }

    /// <summary>
    /// Removes a process ID from tracking (e.g., when it exits normally)
    /// </summary>
    public static void UntrackProcess(int processId)
    {
        lock (_lock)
        {
            _trackedProcessIds.Remove(processId);
        }
    }

    /// <summary>
    /// Kills all tracked Chrome/ChromeDriver processes
    /// </summary>
    public static void CleanupTrackedProcesses()
    {
        lock (_lock)
        {
            foreach (var pid in _trackedProcessIds)
            {
                try
                {
                    var process = Process.GetProcessById(pid);
                    if (!process.HasExited)
                    {
                        process.Kill(true); // Kill entire process tree
                    }
                }
                catch
                {
                    // Process already exited or access denied
                }
            }
            _trackedProcessIds.Clear();
        }
    }

    /// <summary>
    /// Kills all Chrome and ChromeDriver processes started by this application
    /// This is a more aggressive cleanup for when normal disposal fails
    /// </summary>
    public static void KillAllChromeProcesses()
    {
        try
        {
            // Get current process to identify our child processes
            var currentProcess = Process.GetCurrentProcess();
            var currentProcessId = currentProcess.Id;

            // Kill chromedriver processes
            foreach (var process in Process.GetProcessesByName("chromedriver"))
            {
                try
                {
                    // Check if this is a child process of our application
                    if (IsChildProcess(process, currentProcessId))
                    {
                        if (!process.HasExited)
                        {
                            process.Kill(true);
                        }
                    }
                }
                catch
                {
                    // Ignore errors for individual processes
                }
            }

            // Kill Chrome processes started by ChromeDriver (they typically have specific command line args)
            foreach (var process in Process.GetProcessesByName("chrome"))
            {
                try
                {
                    // ChromeDriver-controlled Chrome instances typically have these characteristics
                    if (IsChromeDriverControlledChrome(process))
                    {
                        if (!process.HasExited)
                        {
                            process.Kill(true);
                        }
                    }
                }
                catch
                {
                    // Ignore errors for individual processes
                }
            }
        }
        catch
        {
            // Ignore overall errors
        }
    }

    private static bool IsChildProcess(Process process, int parentProcessId)
    {
        try
        {
            // On Windows, we can't easily get parent process ID without WMI
            // So we'll rely on the tracked process IDs instead
            lock (_lock)
            {
                return _trackedProcessIds.Contains(process.Id);
            }
        }
        catch
        {
            return false;
        }
    }

    private static bool IsChromeDriverControlledChrome(Process process)
    {
        try
        {
            // ChromeDriver-controlled Chrome instances typically:
            // 1. Have "--enable-automation" in command line
            // 2. Have a specific user-data-dir pattern
            // 3. Have "--remote-debugging-port"

            // Since we can't easily get command line on all systems,
            // we check if the process is recent (started around the same time as our app)
            var startTime = process.StartTime;
            var appStartTime = Process.GetCurrentProcess().StartTime;

            // If Chrome started within 1 minute of our app and after it, it's likely ours
            var timeDiff = startTime - appStartTime;
            if (timeDiff.TotalSeconds >= 0 && timeDiff.TotalMinutes < 60)
            {
                // Additional check: ChromeDriver-controlled Chrome typically has
                // a specific window title pattern or no main window
                var mainWindowTitle = process.MainWindowTitle;
                if (string.IsNullOrEmpty(mainWindowTitle) ||
                    mainWindowTitle.Contains("data:") ||
                    mainWindowTitle.Contains("about:blank"))
                {
                    return true;
                }
            }
        }
        catch
        {
            // Can't determine, assume not ours
        }

        return false;
    }

    /// <summary>
    /// Force cleanup all potential orphaned processes
    /// Call this on application exit
    /// </summary>
    public static void ForceCleanupAll()
    {
        CleanupTrackedProcesses();
        KillAllChromeProcesses();

        // Force garbage collection to release any remaining handles
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }
}

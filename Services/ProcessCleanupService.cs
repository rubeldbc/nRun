using System.Diagnostics;

namespace nRun.Services;

/// <summary>
/// Service for cleaning up Chrome and ChromeDriver processes on application exit.
/// Tracks all WebDriver-related processes and provides methods for cleanup.
/// </summary>
public static class ProcessCleanupService
{
    private static readonly HashSet<int> _trackedProcessIds = new();
    private static readonly Dictionary<int, DateTime> _processStartTimes = new();
    private static readonly object _lock = new();
    
    // Cache app start time to avoid repeated Process.GetCurrentProcess() calls
    private static readonly DateTime _appStartTime = GetAppStartTimeSafe();

    private static DateTime GetAppStartTimeSafe()
    {
        try
     {
            return Process.GetCurrentProcess().StartTime;
        }
        catch
   {
            return DateTime.Now;
     }
    }

    /// <summary>
    /// Maximum age (in minutes) for orphaned process detection
    /// </summary>
    public static int MaxProcessAgeMinutes { get; set; } = 30;

    /// <summary>
    /// Tracks a process ID for cleanup on application exit
    /// </summary>
    public static void TrackProcess(int processId)
    {
        lock (_lock)
        {
            _trackedProcessIds.Add(processId);
            _processStartTimes[processId] = DateTime.Now;
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
            _processStartTimes.Remove(processId);
        }
    }

    /// <summary>
    /// Gets the count of currently tracked processes
    /// </summary>
    public static int TrackedProcessCount
    {
        get
        {
            lock (_lock)
            {
                return _trackedProcessIds.Count;
            }
        }
    }

    /// <summary>
    /// Gets list of currently tracked process IDs (for diagnostics)
    /// </summary>
    public static List<int> GetTrackedProcessIds()
    {
        lock (_lock)
        {
            return new List<int>(_trackedProcessIds);
        }
    }

    /// <summary>
    /// Kills all tracked Chrome/ChromeDriver processes
    /// </summary>
    public static void CleanupTrackedProcesses()
    {
        lock (_lock)
        {
            var killedCount = 0;
            foreach (var pid in _trackedProcessIds.ToList())
            {
                try
                {
                    var process = Process.GetProcessById(pid);
                    if (!process.HasExited)
                    {
                        process.Kill(true); // Kill entire process tree
                        killedCount++;
                    }
                }
                catch
                {
                    // Process already exited or access denied
                }
            }
            _trackedProcessIds.Clear();
            _processStartTimes.Clear();

            if (killedCount > 0)
            {
                Debug.WriteLine($"ProcessCleanupService: Killed {killedCount} tracked processes");
            }
        }
    }

    /// <summary>
    /// Cleans up processes that have been running longer than MaxProcessAgeMinutes.
    /// Useful for detecting and cleaning orphaned processes.
    /// </summary>
    public static int CleanupOrphanedProcesses()
    {
        lock (_lock)
        {
            var now = DateTime.Now;
            var orphanedPids = new List<int>();

            foreach (var kvp in _processStartTimes)
            {
                var age = now - kvp.Value;
                if (age.TotalMinutes > MaxProcessAgeMinutes)
                {
                    orphanedPids.Add(kvp.Key);
                }
            }

            var killedCount = 0;
            foreach (var pid in orphanedPids)
            {
                try
                {
                    var process = Process.GetProcessById(pid);
                    if (!process.HasExited)
                    {
                        process.Kill(true);
                        killedCount++;
                    }
                }
                catch
                {
                    // Process already exited
                }

                _trackedProcessIds.Remove(pid);
                _processStartTimes.Remove(pid);
            }

            if (killedCount > 0)
            {
                Debug.WriteLine($"ProcessCleanupService: Killed {killedCount} orphaned processes (older than {MaxProcessAgeMinutes} minutes)");
            }

            return killedCount;
        }
    }

    /// <summary>
    /// Kills all Chrome and ChromeDriver processes started by this application.
    /// This is a more aggressive cleanup for when normal disposal fails.
    /// </summary>
    public static void KillAllChromeProcesses()
    {
        try
        {
            // Get current process to identify our child processes
            var currentProcess = Process.GetCurrentProcess();
            var currentProcessId = currentProcess.Id;
            var killedCount = 0;

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
                            killedCount++;
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
                            killedCount++;
                        }
                    }
                }
                catch
                {
                    // Ignore errors for individual processes
                }
            }

            if (killedCount > 0)
            {
                Debug.WriteLine($"ProcessCleanupService: Killed {killedCount} Chrome/ChromeDriver processes");
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

       DateTime startTime;
            try
   {
   startTime = process.StartTime;
            }
            catch
       {
         // Can't access StartTime (access denied or process exited)
                return false;
 }

       // If Chrome started within 1 minute of our app and after it, it's likely ours
            var timeDiff = startTime - _appStartTime;
            if (timeDiff.TotalSeconds >= 0 && timeDiff.TotalMinutes < 60)
            {
       // Additional check: ChromeDriver-controlled Chrome typically has
           // a specific window title pattern or no main window
      string mainWindowTitle;
     try
         {
     mainWindowTitle = process.MainWindowTitle;
                }
        catch
         {
   // Can't access MainWindowTitle
            return false;
         }
      
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
    /// Force cleanup all potential orphaned processes.
    /// Call this on application exit or after critical operations.
    /// </summary>
    public static void ForceCleanupAll()
    {
        // First cleanup tracked processes
        CleanupTrackedProcesses();

        // Then kill any remaining Chrome/ChromeDriver processes that might be orphaned
        KillAllChromeProcesses();

        // Cleanup WebDriver factory tracked instances
        try
        {
            WebDriverFactory.DisposeAll();
        }
        catch
        {
            // Ignore errors
        }

        // Force garbage collection to release any remaining handles
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        Debug.WriteLine("ProcessCleanupService: ForceCleanupAll completed");
    }

    /// <summary>
    /// Performs a light cleanup suitable for calling after each operation.
    /// Cleans up orphaned processes and removes dead tracked references.
    /// </summary>
    public static void PerformOperationCleanup()
    {
        // Remove dead process references
        lock (_lock)
        {
            var deadPids = new List<int>();
            foreach (var pid in _trackedProcessIds)
            {
                try
                {
                    var process = Process.GetProcessById(pid);
                    if (process.HasExited)
                    {
                        deadPids.Add(pid);
                    }
                }
                catch
                {
                    // Process doesn't exist anymore
                    deadPids.Add(pid);
                }
            }

            foreach (var pid in deadPids)
            {
                _trackedProcessIds.Remove(pid);
                _processStartTimes.Remove(pid);
            }
        }

        // Cleanup orphaned processes (older than MaxProcessAgeMinutes)
        CleanupOrphanedProcesses();
    }
}

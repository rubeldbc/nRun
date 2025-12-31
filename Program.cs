using nRun.Data;
using nRun.Services;
using nRun.UI.Forms;

namespace nRun;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        // Set up application exit handlers for cleanup
        Application.ApplicationExit += Application_ApplicationExit;
        AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        ApplicationConfiguration.Initialize();

        // Initialize database
        DatabaseService.Initialize();

        try
        {
            Application.Run(new MainForm());
        }
        finally
        {
            // Ensure cleanup on normal exit
            CleanupOnExit();
        }
    }

    private static void Application_ApplicationExit(object? sender, EventArgs e)
    {
        CleanupOnExit();
    }

    private static void CurrentDomain_ProcessExit(object? sender, EventArgs e)
    {
        CleanupOnExit();
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        // Try to cleanup even on unhandled exceptions
        CleanupOnExit();
    }

    private static bool _cleanupDone = false;
    private static readonly object _cleanupLock = new();

    private static void CleanupOnExit()
    {
        lock (_cleanupLock)
        {
            if (_cleanupDone) return;
            _cleanupDone = true;

            try
            {
                // Force cleanup all Chrome/ChromeDriver processes
                ProcessCleanupService.ForceCleanupAll();
            }
            catch
            {
                // Ignore cleanup errors during exit
            }
        }
    }
}

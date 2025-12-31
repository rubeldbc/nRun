using nRun.Data;
using nRun.UI.Forms;

namespace nRun;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Initialize database
        DatabaseService.Initialize();

        Application.Run(new MainForm());
    }
}

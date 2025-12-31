using nRun.Services;

namespace nRun.Helpers;

/// <summary>
/// Helper class for common UI operations
/// </summary>
public static class UIHelper
{
    /// <summary>
    /// Executes an action on the UI thread, invoking if required
    /// </summary>
    public static void InvokeIfRequired(Control control, Action action)
    {
        if (control.InvokeRequired)
        {
            control.Invoke(action);
        }
        else
        {
            action();
        }
    }

    /// <summary>
    /// Checks if database is connected and shows error message if not
    /// </summary>
    /// <returns>True if connected, false otherwise</returns>
    public static bool CheckDatabaseConnection(string errorMessage = "Please configure database connection first.")
    {
        if (!ServiceContainer.Database.IsConnected)
        {
            MessageBox.Show(errorMessage, "Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        return true;
    }

    /// <summary>
    /// Shows a confirmation dialog for discarding unsaved changes
    /// </summary>
    /// <returns>True if user wants to continue (discard changes), false to cancel</returns>
    public static bool ConfirmDiscardChanges(string action = "continue")
    {
        var result = MessageBox.Show(
            $"You have unsaved changes. {action}?\n\nContinue?",
            "Unsaved Changes",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        return result == DialogResult.Yes;
    }

    /// <summary>
    /// Shows a confirmation dialog with custom message
    /// </summary>
    public static bool Confirm(string message, string title = "Confirm")
    {
        var result = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        return result == DialogResult.Yes;
    }

    /// <summary>
    /// Shows an error message box
    /// </summary>
    public static void ShowError(string message, string title = "Error")
    {
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    /// <summary>
    /// Shows a warning message box
    /// </summary>
    public static void ShowWarning(string message, string title = "Warning")
    {
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    /// <summary>
    /// Shows an info message box
    /// </summary>
    public static void ShowInfo(string message, string title = "Information")
    {
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}

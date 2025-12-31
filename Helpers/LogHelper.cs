namespace nRun.Helpers;

/// <summary>
/// Helper class for logging to TextBox controls with consistent formatting
/// </summary>
public static class LogHelper
{
    private const int DefaultMaxLines = 500;

    /// <summary>
    /// Appends a log message to a TextBox with timestamp and level formatting
    /// </summary>
    public static void AppendLog(TextBox textBox, string message, string level = "INFO", int maxLines = DefaultMaxLines)
    {
        if (textBox.InvokeRequired)
        {
            textBox.Invoke(() => AppendLog(textBox, message, level, maxLines));
            return;
        }

        var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        var logLine = $"[{timestamp}] [{level}] {message}{Environment.NewLine}";

        textBox.AppendText(logLine);

        // Limit log lines to prevent memory issues
        if (maxLines > 0)
        {
            var lines = textBox.Lines;
            if (lines.Length > maxLines)
            {
                textBox.Lines = lines.Skip(lines.Length - maxLines).ToArray();
            }
        }

        textBox.SelectionStart = textBox.Text.Length;
        textBox.ScrollToCaret();
    }

    /// <summary>
    /// Appends a log message to main log and also to error log if level is ERROR or WARN
    /// </summary>
    public static void AppendLogWithErrorPanel(TextBox mainLog, TextBox errorLog, string message, string level = "INFO", int maxLines = DefaultMaxLines)
    {
        if (mainLog.InvokeRequired)
        {
            mainLog.Invoke(() => AppendLogWithErrorPanel(mainLog, errorLog, message, level, maxLines));
            return;
        }

        var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        var logLine = $"[{timestamp}] [{level}] {message}{Environment.NewLine}";

        // Always append to main log
        mainLog.AppendText(logLine);

        // Limit main log lines
        if (maxLines > 0)
        {
            var lines = mainLog.Lines;
            if (lines.Length > maxLines)
            {
                mainLog.Lines = lines.Skip(lines.Length - maxLines).ToArray();
            }
        }

        mainLog.SelectionStart = mainLog.Text.Length;
        mainLog.ScrollToCaret();

        // Also log errors and warnings to the error log
        if (level == "ERROR" || level == "WARN")
        {
            errorLog.AppendText(logLine);

            // Limit error log lines
            if (maxLines > 0)
            {
                var errorLines = errorLog.Lines;
                if (errorLines.Length > maxLines)
                {
                    errorLog.Lines = errorLines.Skip(errorLines.Length - maxLines).ToArray();
                }
            }

            errorLog.SelectionStart = errorLog.Text.Length;
            errorLog.ScrollToCaret();
        }
    }

    /// <summary>
    /// Simple log append without level (for test results/simple logging)
    /// </summary>
    public static void AppendSimple(TextBox textBox, string message)
    {
        if (textBox.InvokeRequired)
        {
            textBox.Invoke(() => AppendSimple(textBox, message));
            return;
        }

        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        textBox.AppendText($"[{timestamp}] {message}{Environment.NewLine}");
        textBox.SelectionStart = textBox.Text.Length;
        textBox.ScrollToCaret();
    }
}

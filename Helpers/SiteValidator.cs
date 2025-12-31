using nRun.Models;

namespace nRun.Helpers;

/// <summary>
/// Centralized validation for SiteInfo objects
/// </summary>
public static class SiteValidator
{
    /// <summary>
    /// Validates a SiteInfo object and returns a list of error messages
    /// </summary>
    public static List<string> Validate(SiteInfo site, string rowPrefix = "")
    {
        var errors = new List<string>();
        var prefix = string.IsNullOrEmpty(rowPrefix) ? "" : $"{rowPrefix}: ";

        if (string.IsNullOrWhiteSpace(site.SiteName))
        {
            errors.Add($"{prefix}Site name is required");
        }

        if (string.IsNullOrWhiteSpace(site.SiteLink))
        {
            errors.Add($"{prefix}Site URL is required");
        }
        else if (!Uri.TryCreate(site.SiteLink, UriKind.Absolute, out _))
        {
            errors.Add($"{prefix}Invalid URL format");
        }

        if (string.IsNullOrWhiteSpace(site.ArticleLinkSelector))
        {
            errors.Add($"{prefix}Article link selector is required");
        }

        return errors;
    }

    /// <summary>
    /// Validates a SiteInfo and logs errors using provided action
    /// </summary>
    /// <returns>True if valid, false otherwise</returns>
    public static bool ValidateWithLog(SiteInfo site, Action<string> logError, Control? focusControl = null)
    {
        if (string.IsNullOrWhiteSpace(site.SiteName))
        {
            logError("Please enter a site name.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(site.SiteLink))
        {
            logError("Please enter a site URL.");
            return false;
        }

        if (!Uri.TryCreate(site.SiteLink, UriKind.Absolute, out _))
        {
            logError("Please enter a valid URL.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(site.ArticleLinkSelector))
        {
            logError("Please enter an article link selector.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Validates multiple sites and returns all errors
    /// </summary>
    public static List<string> ValidateAll(IEnumerable<SiteInfo> sites)
    {
        var errors = new List<string>();
        int rowNum = 1;

        foreach (var site in sites)
        {
            var siteErrors = Validate(site, $"Row {rowNum}");
            errors.AddRange(siteErrors);
            rowNum++;
        }

        return errors;
    }

    /// <summary>
    /// Shows validation errors in a message box (for batch validation)
    /// </summary>
    /// <returns>True if no errors, false if there were errors</returns>
    public static bool ShowValidationErrors(List<string> errors, int maxDisplay = 10)
    {
        if (errors.Count == 0)
            return true;

        var message = "Please fix the following errors:\n\n" +
            string.Join("\n", errors.Take(maxDisplay));

        if (errors.Count > maxDisplay)
        {
            message += $"\n...and {errors.Count - maxDisplay} more";
        }

        MessageBox.Show(message, "Validation Errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
    }
}

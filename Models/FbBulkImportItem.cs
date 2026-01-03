namespace nRun.Models;

/// <summary>
/// Represents an item in the Facebook bulk import list (CSV import)
/// </summary>
public class FbBulkImportItem
{
    public string Username { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyType { get; set; } = string.Empty;
    public string PageType { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public FbProfile? Profile { get; set; }

    public FbBulkImportItem() { }

    /// <summary>
    /// Creates a bulk import item from CSV line
    /// CSV format: page_link, company_type, page_type, region
    /// Note: company_name is extracted from page source during scraping
    /// </summary>
    public FbBulkImportItem(string csvLine)
    {
        var parts = csvLine.Split(',');
        if (parts.Length > 0)
        {
            Username = ExtractUsernameFromUrl(parts[0].Trim());
        }
        if (parts.Length > 1)
        {
            CompanyType = parts[1].Trim();
        }
        if (parts.Length > 2)
        {
            PageType = parts[2].Trim();
        }
        if (parts.Length > 3)
        {
            Region = parts[3].Trim();
        }
    }

    /// <summary>
    /// Extracts the page username from a Facebook URL (always returns lowercase)
    /// </summary>
    public static string ExtractUsernameFromUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return string.Empty;

        // Remove trailing slash
        url = url.TrimEnd('/');

        // Handle various Facebook URL formats
        // https://www.facebook.com/pagename
        // https://facebook.com/pagename
        // facebook.com/pagename
        // www.facebook.com/pagename

        var uri = url;
        if (!uri.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            uri = "https://" + uri;
        }

        string username = string.Empty;

        try
        {
            var parsedUri = new Uri(uri);
            var path = parsedUri.AbsolutePath.Trim('/');

            // Remove any query string or additional path segments
            var segments = path.Split('/');
            if (segments.Length > 0)
            {
                username = segments[0];
            }
        }
        catch
        {
            // Fallback: try to extract manually
            var lastSlash = url.LastIndexOf('/');
            if (lastSlash >= 0 && lastSlash < url.Length - 1)
            {
                username = url.Substring(lastSlash + 1);
            }
        }

        // Always return lowercase username
        return string.IsNullOrEmpty(username) ? url.ToLowerInvariant() : username.ToLowerInvariant();
    }

    public void UpdateFromProfile(FbProfile profile)
    {
        Profile = profile;
        Username = profile.Username;
        Nickname = profile.Nickname;
        // CompanyName is always taken from scraped profile (og:title)
        CompanyName = profile.CompanyName;
        // These fields are from CSV but can be overridden if profile has values
        if (!string.IsNullOrEmpty(profile.CompanyType))
            CompanyType = profile.CompanyType;
        if (!string.IsNullOrEmpty(profile.PageType))
            PageType = profile.PageType;
        if (!string.IsNullOrEmpty(profile.Region))
            Region = profile.Region;
    }
}

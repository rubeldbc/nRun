using System.Text.RegularExpressions;
using nRun.Services.Interfaces;

namespace nRun.Services;

/// <summary>
/// Service for bulk import file parsing and processing
/// Used by TikTok and Facebook ID manager forms
/// </summary>
public class BulkImportService : IBulkImportService
{
    private static readonly Random _random = new();

    /// <summary>
    /// Parse a file for TikTok usernames
    /// </summary>
    public List<string> ParseTikTokFile(string filePath)
    {
        var usernames = new List<string>();
        var extension = Path.GetExtension(filePath).ToLower();

        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string username;
            if (extension == ".csv")
            {
                // For CSV, try to extract from the first column or URL column
                var parts = line.Split(',');
                var firstPart = parts[0].Trim().Trim('"');
                username = ExtractTikTokUsername(firstPart);
            }
            else
            {
                // For TXT, each line is a username or URL
                username = ExtractTikTokUsername(line.Trim());
            }

            if (!string.IsNullOrEmpty(username) && !usernames.Contains(username))
            {
                usernames.Add(username);
            }
        }

        return usernames;
    }

    /// <summary>
    /// Parse a file for Facebook page URLs/usernames
    /// </summary>
    public List<IBulkImportService.ImportItem> ParseFacebookFile(string filePath)
    {
        var items = new List<IBulkImportService.ImportItem>();
        var extension = Path.GetExtension(filePath).ToLower();

        var lines = File.ReadAllLines(filePath);

        // Skip header if CSV
        int startIndex = 0;
        if (extension == ".csv" && lines.Length > 0)
        {
            var firstLine = lines[0].ToLower();
            if (firstLine.Contains("page_link") || firstLine.Contains("url") || firstLine.Contains("username"))
            {
                startIndex = 1;
            }
        }

        for (int i = startIndex; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            if (extension == ".csv")
            {
                var parts = ParseCsvLine(line);
                if (parts.Length > 0)
                {
                    var pageId = ExtractFacebookPageId(parts[0]);
                    if (!string.IsNullOrEmpty(pageId))
                    {
                        items.Add(new IBulkImportService.ImportItem(
                            Username: pageId,
                            CompanyType: parts.Length > 1 ? parts[1].Trim().Trim('"') : null,
                            PageType: parts.Length > 2 ? parts[2].Trim().Trim('"') : null,
                            Region: parts.Length > 3 ? parts[3].Trim().Trim('"') : null,
                            OriginalUrl: parts[0].Trim().Trim('"')
                        ));
                    }
                }
            }
            else
            {
                // For TXT, each line is a URL or page name
                var pageId = ExtractFacebookPageId(line.Trim());
                if (!string.IsNullOrEmpty(pageId))
                {
                    items.Add(new IBulkImportService.ImportItem(
                        Username: pageId,
                        OriginalUrl: line.Trim()
                    ));
                }
            }
        }

        return items;
    }

    /// <summary>
    /// Extract username from a TikTok URL or @username format
    /// </summary>
    public string ExtractTikTokUsername(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // Remove @ prefix if present
        input = input.TrimStart('@');

        // Check if it's a URL
        if (input.Contains("tiktok.com"))
        {
            // Extract username from URL like https://www.tiktok.com/@username
            var match = Regex.Match(input, @"tiktok\.com/@?([^/?]+)");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
        }

        // Return cleaned input (might be just a username)
        return input.Trim();
    }

    /// <summary>
    /// Extract page identifier from a Facebook URL
    /// </summary>
    public string ExtractFacebookPageId(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        input = input.Trim().Trim('"');

        // Check if it's a URL
        if (input.Contains("facebook.com"))
        {
            // Extract page ID from URL like https://www.facebook.com/pagename
            var match = Regex.Match(input, @"facebook\.com/(?:pages/[^/]+/)?([^/?]+)");
            if (match.Success)
            {
                var pageId = match.Groups[1].Value;
                // Skip common non-page paths
                if (pageId != "profile.php" && pageId != "groups" && pageId != "events")
                {
                    return pageId;
                }
            }
        }

        // Return cleaned input (might be just a page name)
        return input.Trim();
    }

    /// <summary>
    /// Calculate delay with jitter for rate limiting
    /// </summary>
    public TimeSpan CalculateDelayWithJitter(int baseDelaySeconds, double jitterFactor = 0.5)
    {
        var jitter = (int)(baseDelaySeconds * jitterFactor);
        var actualDelay = baseDelaySeconds + _random.Next(-jitter, jitter + 1);
        return TimeSpan.FromSeconds(Math.Max(1, actualDelay));
    }

    /// <summary>
    /// Parse a CSV line handling quoted values
    /// </summary>
    private static string[] ParseCsvLine(string line)
    {
        var result = new List<string>();
        var current = new System.Text.StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    // Escaped quote
                    current.Append('"');
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        result.Add(current.ToString());
        return result.ToArray();
    }
}

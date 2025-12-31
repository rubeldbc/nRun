using System.Net.Http;
using System.Text.RegularExpressions;
using SkiaSharp;

namespace nRun.Services;

/// <summary>
/// Service for downloading and processing website logos/favicons
/// </summary>
public static class LogoDownloadService
{
    private static readonly HttpClient _httpClient;
    private static readonly string LogosFolder;

    static LogoDownloadService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
        _httpClient.Timeout = TimeSpan.FromSeconds(30);

        LogosFolder = Path.Combine(Application.StartupPath, "logos");
        if (!Directory.Exists(LogosFolder))
        {
            Directory.CreateDirectory(LogosFolder);
        }
    }

    /// <summary>
    /// Downloads and processes a website logo
    /// </summary>
    /// <param name="siteUrl">The website URL</param>
    /// <param name="siteName">The site name (not used for filename, kept for compatibility)</param>
    /// <param name="existingLogoName">Optional: use this logo name instead of extracting from URL (from database)</param>
    /// <returns>Tuple of (path to saved logo file or null, logo name used for saving)</returns>
    public static async Task<(string? path, string logoName)> DownloadLogoAsync(string siteUrl, string siteName, string? existingLogoName = null)
    {
        // Use existing logo name from database if provided, otherwise extract from URL
        var logoName = !string.IsNullOrEmpty(existingLogoName)
            ? existingLogoName
            : ExtractLogoNameFromUrl(siteUrl);

        try
        {
            // Get base URL
            var uri = new Uri(siteUrl);
            var baseUrl = $"{uri.Scheme}://{uri.Host}";

            // Try multiple favicon sources - prefer higher resolution first
            var faviconUrls = new List<string>
            {
                // High resolution icons first
                $"{baseUrl}/apple-touch-icon-180x180.png",
                $"{baseUrl}/apple-touch-icon-152x152.png",
                $"{baseUrl}/apple-touch-icon-144x144.png",
                $"{baseUrl}/apple-touch-icon-120x120.png",
                $"{baseUrl}/apple-touch-icon.png",
                $"{baseUrl}/apple-touch-icon-precomposed.png",
                $"{baseUrl}/icon-192x192.png",
                $"{baseUrl}/icon-512x512.png",
                $"{baseUrl}/logo.png",
                $"{baseUrl}/favicon-32x32.png",
                $"{baseUrl}/favicon.png",
                $"{baseUrl}/favicon.ico",
                // Fallback to external services with high resolution
                $"https://www.google.com/s2/favicons?domain={uri.Host}&sz=256",
                $"https://icons.duckduckgo.com/ip3/{uri.Host}.ico"
            };

            // Try to find favicon from HTML
            var htmlFavicons = await TryGetFaviconsFromHtml(siteUrl, baseUrl);
            faviconUrls.InsertRange(0, htmlFavicons);

            byte[]? imageData = null;

            foreach (var faviconUrl in faviconUrls)
            {
                try
                {
                    imageData = await DownloadImageAsync(faviconUrl);
                    if (imageData != null && imageData.Length > 100) // Valid image
                    {
                        break;
                    }
                }
                catch
                {
                    // Try next URL
                }
            }

            if (imageData == null || imageData.Length < 100)
            {
                return (null, logoName);
            }

            // Process and save the image using URL-based logo name
            var fileName = logoName + ".webp";
            var filePath = Path.Combine(LogosFolder, fileName);

            var success = ProcessAndSaveImage(imageData, filePath);
            return (success ? filePath : null, logoName);
        }
        catch
        {
            return (null, logoName);
        }
    }

    private static async Task<List<string>> TryGetFaviconsFromHtml(string siteUrl, string baseUrl)
    {
        var favicons = new List<(string url, int size)>();

        try
        {
            var html = await _httpClient.GetStringAsync(siteUrl);

            // Look for link tags with icons - capture the whole tag to extract size
            var linkPattern = @"<link[^>]*(?:rel=[""'](?:(?:shortcut\s+)?icon|apple-touch-icon(?:-precomposed)?)[""'][^>]*href=[""']([^""']+)[""']|href=[""']([^""']+)[""'][^>]*rel=[""'](?:(?:shortcut\s+)?icon|apple-touch-icon(?:-precomposed)?)[""'])[^>]*>";

            var matches = Regex.Matches(html, linkPattern, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                var href = !string.IsNullOrEmpty(match.Groups[1].Value) ? match.Groups[1].Value : match.Groups[2].Value;
                var absoluteUrl = MakeAbsoluteUrl(href, baseUrl);

                if (string.IsNullOrEmpty(absoluteUrl))
                    continue;

                // Try to extract size from the tag
                int size = 0;
                var sizeMatch = Regex.Match(match.Value, @"sizes=[""'](\d+)x\d+[""']", RegexOptions.IgnoreCase);
                if (sizeMatch.Success)
                {
                    int.TryParse(sizeMatch.Groups[1].Value, out size);
                }
                else
                {
                    // Try to extract size from filename
                    var filenameSizeMatch = Regex.Match(href, @"(\d{2,3})x\d{2,3}", RegexOptions.IgnoreCase);
                    if (filenameSizeMatch.Success)
                    {
                        int.TryParse(filenameSizeMatch.Groups[1].Value, out size);
                    }
                }

                // Prioritize apple-touch-icon
                if (match.Value.Contains("apple-touch-icon", StringComparison.OrdinalIgnoreCase))
                {
                    size = Math.Max(size, 100); // Give apple-touch-icon higher priority
                }

                if (!favicons.Any(f => f.url == absoluteUrl))
                {
                    favicons.Add((absoluteUrl, size));
                }
            }

            // Also look for og:image which is often a high-quality logo
            var ogImagePattern = @"<meta[^>]*property=[""']og:image[""'][^>]*content=[""']([^""']+)[""']";
            var ogMatch = Regex.Match(html, ogImagePattern, RegexOptions.IgnoreCase);
            if (ogMatch.Success)
            {
                var ogUrl = MakeAbsoluteUrl(ogMatch.Groups[1].Value, baseUrl);
                if (!string.IsNullOrEmpty(ogUrl) && !favicons.Any(f => f.url == ogUrl))
                {
                    favicons.Add((ogUrl, 500)); // High priority for og:image
                }
            }
        }
        catch
        {
            // Ignore HTML parsing errors
        }

        // Sort by size descending (larger icons first)
        return favicons.OrderByDescending(f => f.size).Select(f => f.url).ToList();
    }

    private static string? MakeAbsoluteUrl(string url, string baseUrl)
    {
        if (string.IsNullOrEmpty(url)) return null;

        if (url.StartsWith("http://") || url.StartsWith("https://"))
        {
            return url;
        }

        if (url.StartsWith("//"))
        {
            return "https:" + url;
        }

        if (url.StartsWith("/"))
        {
            return baseUrl + url;
        }

        return baseUrl + "/" + url;
    }

    private static async Task<byte[]?> DownloadImageAsync(string url)
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
        }
        catch
        {
            // Ignore download errors
        }

        return null;
    }

    private static bool ProcessAndSaveImage(byte[] imageData, string outputPath)
    {
        try
        {
            using var inputStream = new MemoryStream(imageData);
            using var originalBitmap = SKBitmap.Decode(inputStream);

            if (originalBitmap == null)
            {
                return false;
            }

            // Save as WebP with original size and high quality
            using var image = SKImage.FromBitmap(originalBitmap);
            using var data = image.Encode(SKEncodedImageFormat.Webp, 100); // Maximum quality
            using var fileStream = File.OpenWrite(outputPath);
            data.SaveTo(fileStream);

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string SanitizeFileName(string siteName)
    {
        if (string.IsNullOrWhiteSpace(siteName))
        {
            return "unknown_site";
        }

        // Replace spaces with underscores
        var fileName = siteName.Replace(" ", "_");

        // Remove invalid filename characters
        var invalidChars = Path.GetInvalidFileNameChars();
        foreach (var c in invalidChars)
        {
            fileName = fileName.Replace(c.ToString(), "");
        }

        // Ensure filename is not too long
        if (fileName.Length > 50)
        {
            fileName = fileName.Substring(0, 50);
        }

        return fileName.ToLowerInvariant();
    }

    /// <summary>
    /// Extracts the logo name from a site URL by getting the largest domain part
    /// e.g., https://jagonews24.com/archive -> jagonews24
    /// e.g., https://bn.bdcrictime.com/ -> bdcrictime
    /// </summary>
    public static string ExtractLogoNameFromUrl(string siteUrl)
    {
        try
        {
            var uri = new Uri(siteUrl);
            var host = uri.Host.ToLowerInvariant();

            // Remove www. prefix if present
            if (host.StartsWith("www."))
            {
                host = host[4..];
            }

            // Split by dots and find the largest part (excluding common TLDs)
            var parts = host.Split('.');

            // Common TLDs and subdomains to exclude
            var excludeParts = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "com", "org", "net", "edu", "gov", "io", "co", "uk", "us", "bd", "in", "au", "ca",
                "www", "m", "mobile", "bn", "en", "es", "fr", "de", "jp", "cn", "kr", "ru",
                "news", "blog", "app", "api", "cdn", "static", "img", "images", "media"
            };

            // Find the largest non-excluded part
            string largestPart = "";
            foreach (var part in parts)
            {
                if (!excludeParts.Contains(part) && part.Length > largestPart.Length)
                {
                    largestPart = part;
                }
            }

            // If no valid part found, use the first non-TLD part
            if (string.IsNullOrEmpty(largestPart) && parts.Length > 0)
            {
                largestPart = parts[0];
            }

            return string.IsNullOrEmpty(largestPart) ? "unknown_site" : largestPart;
        }
        catch
        {
            return "unknown_site";
        }
    }

    /// <summary>
    /// Gets the logo path for a logo name (stored in site_logo column)
    /// </summary>
    public static string? GetLogoPath(string? logoName)
    {
        if (string.IsNullOrEmpty(logoName))
            return null;

        var fileName = logoName + ".webp";
        var filePath = Path.Combine(LogosFolder, fileName);
        return File.Exists(filePath) ? filePath : null;
    }

    /// <summary>
    /// Gets the logos folder path
    /// </summary>
    public static string GetLogosFolder() => LogosFolder;

    /// <summary>
    /// Gets logo from online without saving to disk (for preview purposes)
    /// </summary>
    public static async Task<byte[]?> GetLogoFromOnlineAsync(string siteUrl)
    {
        try
        {
            var uri = new Uri(siteUrl);
            var baseUrl = $"{uri.Scheme}://{uri.Host}";

            // Try multiple favicon sources - prefer higher resolution first
            var faviconUrls = new List<string>
            {
                // High resolution icons first
                $"{baseUrl}/apple-touch-icon-180x180.png",
                $"{baseUrl}/apple-touch-icon-152x152.png",
                $"{baseUrl}/apple-touch-icon-144x144.png",
                $"{baseUrl}/apple-touch-icon-120x120.png",
                $"{baseUrl}/apple-touch-icon.png",
                $"{baseUrl}/apple-touch-icon-precomposed.png",
                $"{baseUrl}/icon-192x192.png",
                $"{baseUrl}/icon-512x512.png",
                $"{baseUrl}/logo.png",
                $"{baseUrl}/favicon-32x32.png",
                $"{baseUrl}/favicon.png",
                $"{baseUrl}/favicon.ico",
                // Fallback to external services with high resolution
                $"https://www.google.com/s2/favicons?domain={uri.Host}&sz=256",
                $"https://icons.duckduckgo.com/ip3/{uri.Host}.ico"
            };

            // Try to find favicon from HTML
            var htmlFavicons = await TryGetFaviconsFromHtml(siteUrl, baseUrl);
            faviconUrls.InsertRange(0, htmlFavicons);

            foreach (var faviconUrl in faviconUrls)
            {
                try
                {
                    var imageData = await DownloadImageAsync(faviconUrl);
                    if (imageData != null && imageData.Length > 100)
                    {
                        return imageData;
                    }
                }
                catch
                {
                    // Try next URL
                }
            }
        }
        catch
        {
            // Ignore errors
        }

        return null;
    }
}

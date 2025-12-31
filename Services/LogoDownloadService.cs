using System.Net.Http;
using System.Text.RegularExpressions;
using nRun.Services.Interfaces;
using SkiaSharp;

namespace nRun.Services;

/// <summary>
/// Service for downloading and processing website logos/favicons
/// </summary>
public class LogoDownloadService : ILogoDownloadService
{
    private readonly HttpClient _httpClient;
    private readonly string _logosFolder;

    // Common TLDs and subdomains to exclude when extracting logo name
    private static readonly HashSet<string> ExcludeParts = new(StringComparer.OrdinalIgnoreCase)
    {
        "com", "org", "net", "edu", "gov", "io", "co", "uk", "us", "bd", "in", "au", "ca",
        "www", "m", "mobile", "bn", "en", "es", "fr", "de", "jp", "cn", "kr", "ru",
        "news", "blog", "app", "api", "cdn", "static", "img", "images", "media"
    };

    public LogoDownloadService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
        _httpClient.Timeout = TimeSpan.FromSeconds(30);

        _logosFolder = Path.Combine(Application.StartupPath, "logos");
        if (!Directory.Exists(_logosFolder))
        {
            Directory.CreateDirectory(_logosFolder);
        }
    }

    public async Task<(string? path, string logoName)> DownloadLogoAsync(string siteUrl, string siteName, string? existingLogoName = null)
    {
        var logoName = !string.IsNullOrEmpty(existingLogoName)
            ? existingLogoName
            : ExtractLogoNameFromUrl(siteUrl);

        try
        {
            var imageData = await TryDownloadLogoAsync(siteUrl);

            if (imageData == null || imageData.Length < 100)
            {
                return (null, logoName);
            }

            var fileName = logoName + ".webp";
            var filePath = Path.Combine(_logosFolder, fileName);

            var success = ProcessAndSaveImage(imageData, filePath);
            return (success ? filePath : null, logoName);
        }
        catch
        {
            return (null, logoName);
        }
    }

    public async Task<byte[]?> GetLogoFromOnlineAsync(string siteUrl)
    {
        try
        {
            return await TryDownloadLogoAsync(siteUrl);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Shared method to try downloading logo from various sources
    /// </summary>
    private async Task<byte[]?> TryDownloadLogoAsync(string siteUrl)
    {
        var uri = new Uri(siteUrl);
        var baseUrl = $"{uri.Scheme}://{uri.Host}";

        // Get all favicon URLs to try
        var faviconUrls = GetFaviconUrls(uri, baseUrl);

        // Try to find favicon from HTML first
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

        return null;
    }

    /// <summary>
    /// Gets the list of favicon URLs to try for a given site
    /// </summary>
    private static List<string> GetFaviconUrls(Uri uri, string baseUrl)
    {
        return new List<string>
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
    }

    private async Task<List<string>> TryGetFaviconsFromHtml(string siteUrl, string baseUrl)
    {
        var favicons = new List<(string url, int size)>();

        try
        {
            var html = await _httpClient.GetStringAsync(siteUrl);

            // Look for link tags with icons
            var linkPattern = @"<link[^>]*(?:rel=[""'](?:(?:shortcut\s+)?icon|apple-touch-icon(?:-precomposed)?)[""'][^>]*href=[""']([^""']+)[""']|href=[""']([^""']+)[""'][^>]*rel=[""'](?:(?:shortcut\s+)?icon|apple-touch-icon(?:-precomposed)?)[""'])[^>]*>";

            var matches = Regex.Matches(html, linkPattern, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                var href = !string.IsNullOrEmpty(match.Groups[1].Value) ? match.Groups[1].Value : match.Groups[2].Value;
                var absoluteUrl = MakeAbsoluteUrl(href, baseUrl);

                if (string.IsNullOrEmpty(absoluteUrl))
                    continue;

                int size = 0;
                var sizeMatch = Regex.Match(match.Value, @"sizes=[""'](\d+)x\d+[""']", RegexOptions.IgnoreCase);
                if (sizeMatch.Success)
                {
                    int.TryParse(sizeMatch.Groups[1].Value, out size);
                }
                else
                {
                    var filenameSizeMatch = Regex.Match(href, @"(\d{2,3})x\d{2,3}", RegexOptions.IgnoreCase);
                    if (filenameSizeMatch.Success)
                    {
                        int.TryParse(filenameSizeMatch.Groups[1].Value, out size);
                    }
                }

                if (match.Value.Contains("apple-touch-icon", StringComparison.OrdinalIgnoreCase))
                {
                    size = Math.Max(size, 100);
                }

                if (!favicons.Any(f => f.url == absoluteUrl))
                {
                    favicons.Add((absoluteUrl, size));
                }
            }

            // Also look for og:image
            var ogImagePattern = @"<meta[^>]*property=[""']og:image[""'][^>]*content=[""']([^""']+)[""']";
            var ogMatch = Regex.Match(html, ogImagePattern, RegexOptions.IgnoreCase);
            if (ogMatch.Success)
            {
                var ogUrl = MakeAbsoluteUrl(ogMatch.Groups[1].Value, baseUrl);
                if (!string.IsNullOrEmpty(ogUrl) && !favicons.Any(f => f.url == ogUrl))
                {
                    favicons.Add((ogUrl, 500));
                }
            }
        }
        catch
        {
            // Ignore HTML parsing errors
        }

        return favicons.OrderByDescending(f => f.size).Select(f => f.url).ToList();
    }

    private static string? MakeAbsoluteUrl(string url, string baseUrl)
    {
        if (string.IsNullOrEmpty(url)) return null;

        if (url.StartsWith("http://") || url.StartsWith("https://"))
            return url;

        if (url.StartsWith("//"))
            return "https:" + url;

        if (url.StartsWith("/"))
            return baseUrl + url;

        return baseUrl + "/" + url;
    }

    private async Task<byte[]?> DownloadImageAsync(string url)
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
                return false;

            using var image = SKImage.FromBitmap(originalBitmap);
            using var data = image.Encode(SKEncodedImageFormat.Webp, 100);
            using var fileStream = File.OpenWrite(outputPath);
            data.SaveTo(fileStream);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public string ExtractLogoNameFromUrl(string siteUrl)
    {
        try
        {
            var uri = new Uri(siteUrl);
            var host = uri.Host.ToLowerInvariant();

            if (host.StartsWith("www."))
                host = host[4..];

            var parts = host.Split('.');

            string largestPart = "";
            foreach (var part in parts)
            {
                if (!ExcludeParts.Contains(part) && part.Length > largestPart.Length)
                {
                    largestPart = part;
                }
            }

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

    public string? GetLogoPath(string? logoName)
    {
        if (string.IsNullOrEmpty(logoName))
            return null;

        var fileName = logoName + ".webp";
        var filePath = Path.Combine(_logosFolder, fileName);
        return File.Exists(filePath) ? filePath : null;
    }

    public string GetLogosFolder() => _logosFolder;
}

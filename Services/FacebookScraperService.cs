using System.Net.Http;
using System.Text.RegularExpressions;
using nRun.Models;
using SkiaSharp;

namespace nRun.Services;

/// <summary>
/// Service for scraping Facebook page information using Selenium WebDriver.
/// WebDriver is created per-operation and immediately disposed after each fetch
/// to prevent memory leaks and process accumulation.
/// Extracts data from HTML meta tags (og:description, og:title, og:image, etc.)
/// </summary>
public class FacebookScraperService : IDisposable
{
    private HttpClient? _httpClient;
    private bool _disposed;
    private readonly RateLimiter _rateLimiter;
    private readonly ResilientRequestHandler _requestHandler;

    // Event handler references for cleanup
    private readonly EventHandler<string>? _requestStatusHandler;
    private readonly EventHandler<string>? _requestErrorHandler;

    private static readonly string FacebookLogosFolder = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "fb-logos");

    public event EventHandler<string>? StatusChanged;
    public event EventHandler<string>? ErrorOccurred;

    public int TimeoutSeconds { get; set; } = 30;
    public bool UseHeadless { get; set; } = true;

    /// <summary>
    /// Gets the rate limiter for external configuration
    /// </summary>
    public RateLimiter RateLimiter => _rateLimiter;

    public FacebookScraperService(int baseDelaySeconds = 10, int jitterMaxSeconds = 5)
    {
        _rateLimiter = new RateLimiter(baseDelaySeconds, jitterMaxSeconds);
        _requestHandler = new ResilientRequestHandler(_rateLimiter);

        // Store handler references for later cleanup
        _requestStatusHandler = (s, msg) => OnStatusChanged(msg);
        _requestErrorHandler = (s, msg) => OnError(msg);
        _requestHandler.StatusChanged += _requestStatusHandler;
        _requestHandler.ErrorOccurred += _requestErrorHandler;

        // Ensure fb-logos folder exists
        if (!Directory.Exists(FacebookLogosFolder))
        {
            Directory.CreateDirectory(FacebookLogosFolder);
        }
    }

    /// <summary>
    /// Creates a new WebDriver instance for Facebook scraping
    /// </summary>
    private WebDriverService CreateWebDriver()
    {
        return WebDriverFactory.Create(UseHeadless, TimeoutSeconds);
    }

    private HttpClient GetHttpClient()
    {
        if (_httpClient == null)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", WebDriverConfig.GetDefaultUserAgent());
        }
        return _httpClient;
    }

    /// <summary>
    /// Waits before the next request using rate limiter with jitter
    /// </summary>
    public async Task WaitBeforeNextRequestAsync(CancellationToken cancellationToken = default)
    {
        await _requestHandler.WaitBeforeNextRequestAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the current delay description for display
    /// </summary>
    public string GetDelayInfo() => _rateLimiter.GetDelayDescription();

    /// <summary>
    /// Downloads avatar image, resizes to 88x88, converts to WebP, and saves to fb-logos folder
    /// </summary>
    public async Task<string?> DownloadAndConvertAvatarAsync(string avatarUrl, string username)
    {
        try
        {
            if (string.IsNullOrEmpty(avatarUrl)) return null;

            OnStatusChanged($"Downloading avatar for {username}...");

            var client = GetHttpClient();
            var imageBytes = await client.GetByteArrayAsync(avatarUrl).ConfigureAwait(false);

            // Decode image using SkiaSharp
            using var originalBitmap = SKBitmap.Decode(imageBytes);
            if (originalBitmap == null)
            {
                OnError("Failed to decode avatar image");
                return null;
            }

            // Resize to 88x88 with high quality
            var resizeInfo = new SKImageInfo(88, 88);
            using var resizedBitmap = originalBitmap.Resize(resizeInfo, SKFilterQuality.High);
            if (resizedBitmap == null)
            {
                OnError("Failed to resize avatar image");
                return null;
            }

            // Encode as WebP
            using var image = SKImage.FromBitmap(resizedBitmap);
            using var webpData = image.Encode(SKEncodedImageFormat.Webp, 90);

            // Save to file
            var filename = $"{username}.webp";
            var filepath = Path.Combine(FacebookLogosFolder, filename);

            using var fileStream = File.OpenWrite(filepath);
            webpData.SaveTo(fileStream);

            OnStatusChanged($"Avatar saved for {username}");
            return filepath;
        }
        catch (Exception ex)
        {
            OnError($"Failed to download avatar: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Extracts username from Facebook page URL (always returns lowercase)
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
            // Fallback: try to extract manually using regex
            var match = Regex.Match(url, @"facebook\.com/([^/?]+)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                username = match.Groups[1].Value;
            }
        }

        // Always return lowercase username
        return string.IsNullOrEmpty(username) ? url.ToLowerInvariant() : username.ToLowerInvariant();
    }

    /// <summary>
    /// Fetches Facebook page profile information from a page URL with bulk import item data
    /// </summary>
    public async Task<FbProfile?> FetchProfileInfoAsync(string pageUrl, FbBulkImportItem? importItem = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var username = ExtractUsernameFromUrl(pageUrl);
        if (string.IsNullOrEmpty(username))
        {
            OnError("Invalid Facebook URL. Expected format: https://www.facebook.com/pagename");
            return null;
        }

        OnStatusChanged($"Fetching profile info for {username}...");

        WebDriverService? webDriver = null;
        FbProfile? profile = null;

        try
        {
            webDriver = CreateWebDriver();
            var url = $"https://www.facebook.com/{username}";

            await webDriver.NavigateToAsync(url, cancellationToken);

            // Wait for page to load
            await Task.Delay(3000, cancellationToken);

            // Extract data from meta tags
            profile = ExtractProfileFromMetaTags(webDriver, username);

            if (profile != null)
            {
                // Apply import item data if provided (CompanyName comes from scraping, not CSV)
                if (importItem != null)
                {
                    if (!string.IsNullOrEmpty(importItem.CompanyType))
                        profile.CompanyType = importItem.CompanyType;
                    if (!string.IsNullOrEmpty(importItem.PageType))
                        profile.PageType = importItem.PageType;
                    if (!string.IsNullOrEmpty(importItem.Region))
                        profile.Region = importItem.Region;
                }

                OnStatusChanged($"Successfully fetched profile: {username}");
            }
            else
            {
                OnError($"Could not extract profile data for {username}");
            }
        }
        catch (OperationCanceledException)
        {
            OnStatusChanged($"Profile fetch cancelled for {username}");
            throw;
        }
        catch (Exception ex)
        {
            OnError($"Error fetching profile: {ex.Message}");
            profile = null;
        }
        finally
        {
            // CRITICAL: Always dispose WebDriver after each profile fetch
            if (webDriver != null)
            {
                OnStatusChanged($"Closing browser for {username}");
                WebDriverFactory.SafeDispose(webDriver);
            }
        }

        // Download and convert avatar if URL is available (after WebDriver is closed)
        if (profile != null && !string.IsNullOrEmpty(profile.AvatarUrl))
        {
            cancellationToken.ThrowIfCancellationRequested();
            profile.AvatarLocalPath = await DownloadAndConvertAvatarAsync(profile.AvatarUrl, profile.Username);
        }

        return profile;
    }

    /// <summary>
    /// Fetches Facebook page statistics for a profile.
    /// Creates a new WebDriver for this operation and disposes it after completion.
    /// </summary>
    public async Task<FbData?> FetchStatsAsync(FbProfile profile, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        OnStatusChanged($"Fetching stats for {profile.Username}...");

        WebDriverService? webDriver = null;
        FbData? stats = null;

        try
        {
            webDriver = CreateWebDriver();
            var url = $"https://www.facebook.com/{profile.Username}";

            await webDriver.NavigateToAsync(url, cancellationToken);

            // Wait for page to load
            await Task.Delay(3000, cancellationToken);

            // Extract stats from meta tags
            stats = ExtractStatsFromMetaTags(webDriver, profile.UserId);

            if (stats != null)
            {
                stats.Username = profile.Username;
                stats.Nickname = profile.Nickname;
                OnStatusChanged($"Successfully fetched stats for {profile.Username}");
            }
            else
            {
                OnError($"Could not extract stats for {profile.Username}");
            }
        }
        catch (OperationCanceledException)
        {
            OnStatusChanged($"Stats fetch cancelled for {profile.Username}");
            throw;
        }
        catch (Exception ex)
        {
            OnError($"Error fetching stats: {ex.Message}");
            stats = null;
        }
        finally
        {
            // CRITICAL: Always dispose WebDriver after each stats fetch
            if (webDriver != null)
            {
                OnStatusChanged($"Closing browser for {profile.Username}");
                WebDriverFactory.SafeDispose(webDriver);
            }
        }

        return stats;
    }

    /// <summary>
    /// Extracts profile information from Facebook page meta tags
    /// </summary>
    private FbProfile? ExtractProfileFromMetaTags(WebDriverService driver, string username)
    {
        try
        {
            var pageSource = driver.GetPageSource();
            if (string.IsNullOrEmpty(pageSource))
            {
                return null;
            }

            var profile = new FbProfile
            {
                Username = username,
                Status = true,
                CreatedAt = DateTime.Now
            };

            // Extract user_id from al:ios:url meta tag
            // <meta property="al:ios:url" content="fb://profile/100065050291573" />
            var userIdMatch = Regex.Match(pageSource,
                @"<meta\s+property=""al:ios:url""\s+content=""fb://profile/(\d+)""",
                RegexOptions.IgnoreCase);
            if (userIdMatch.Success && long.TryParse(userIdMatch.Groups[1].Value, out var userId))
            {
                profile.UserId = userId;
            }
            else
            {
                // Fallback: try alternate patterns
                userIdMatch = Regex.Match(pageSource, @"fb://profile/(\d+)", RegexOptions.IgnoreCase);
                if (userIdMatch.Success && long.TryParse(userIdMatch.Groups[1].Value, out userId))
                {
                    profile.UserId = userId;
                }
                else
                {
                    // Generate a hash-based user ID as last resort
                    profile.UserId = Math.Abs(username.GetHashCode());
                }
            }

            // Extract username from og:url if needed (always lowercase)
            // <meta property="og:url" content="https://www.facebook.com/dbcnews.tv" />
            var ogUrlMatch = Regex.Match(pageSource,
                @"<meta\s+property=""og:url""\s+content=""https?://(?:www\.)?facebook\.com/([^""/?]+)""",
                RegexOptions.IgnoreCase);
            if (ogUrlMatch.Success)
            {
                profile.Username = ogUrlMatch.Groups[1].Value.ToLowerInvariant();
            }

            // Extract Nickname and CompanyName from og:description (before first dot or comma)
            // Both Nickname and CompanyName will be the same
            // Example 1: "Ajker Patrika, Dhaka. 1,112,489 likes..." -> "Ajker Patrika"
            // Example 2: "Kaler Kantho. 8,936,411 likes..." -> "Kaler Kantho"
            var descMatch = Regex.Match(pageSource,
                @"<meta\s+(?:property|name)=""(?:og:description|twitter:description)""\s+content=""([^""]+)""",
                RegexOptions.IgnoreCase);
            if (descMatch.Success)
            {
                var description = descMatch.Groups[1].Value;
                // Find whichever comes first: dot (.) or comma (,)
                var dotIndex = description.IndexOf('.');
                var commaIndex = description.IndexOf(',');

                int endIndex = -1;
                if (dotIndex > 0 && commaIndex > 0)
                {
                    // Both exist, take whichever comes first
                    endIndex = Math.Min(dotIndex, commaIndex);
                }
                else if (dotIndex > 0)
                {
                    endIndex = dotIndex;
                }
                else if (commaIndex > 0)
                {
                    endIndex = commaIndex;
                }

                string name;
                if (endIndex > 0)
                {
                    name = description.Substring(0, endIndex).Trim();
                }
                else
                {
                    // No dot or comma found, use full description (trimmed)
                    name = description.Trim();
                }

                // Nickname and CompanyName are the same
                profile.Nickname = name;
                profile.CompanyName = name;
            }

            // Extract avatar from og:image
            // <meta property="og:image" content="https://scontent.../.jpg?..." />
            var imageMatch = Regex.Match(pageSource,
                @"<meta\s+property=""og:image""\s+content=""([^""]+)""",
                RegexOptions.IgnoreCase);
            if (imageMatch.Success)
            {
                var avatarUrl = imageMatch.Groups[1].Value;
                // Clean the URL: decode HTML entities and remove unwanted spaces
                avatarUrl = CleanAvatarUrl(avatarUrl);
                profile.AvatarUrl = avatarUrl;
            }

            return profile.UserId > 0 ? profile : null;
        }
        catch (Exception ex)
        {
            OnError($"Error extracting profile from meta tags: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Extracts stats from Facebook page meta tags
    /// </summary>
    private FbData? ExtractStatsFromMetaTags(WebDriverService driver, long userId)
    {
        try
        {
            var pageSource = driver.GetPageSource();
            if (string.IsNullOrEmpty(pageSource))
            {
                return null;
            }

            var data = new FbData
            {
                UserId = userId,
                RecordedAt = DateTime.Now
            };

            // Extract followers and talking_about from og:description or twitter:description
            // Example: "Kaler Kantho. 8,936,411 likes &#xb7; 4,780,976 talking about this..."
            var descMatch = Regex.Match(pageSource,
                @"<meta\s+(?:property|name)=""(?:og:description|twitter:description)""\s+content=""([^""]+)""",
                RegexOptions.IgnoreCase);

            if (descMatch.Success)
            {
                var description = descMatch.Groups[1].Value;

                // Parse likes/followers count
                // Example: "Kaler Kantho. 8,936,411 likes &#xb7; 4,780,976 talking about this."
                // The number is just before the word "likes"
                // Pattern: capture digits and commas before "likes"
                var likesMatch = Regex.Match(description,
                    @"([\d,]+)\s+likes",
                    RegexOptions.IgnoreCase);
                if (likesMatch.Success)
                {
                    data.FollowersCount = ParseCount(likesMatch.Groups[1].Value);
                }

                // Parse talking about count
                // Example: "4,780,976 talking about this"
                // The number is just before "talking about"
                var talkingMatch = Regex.Match(description,
                    @"([\d,]+)\s+talking\s+about",
                    RegexOptions.IgnoreCase);
                if (talkingMatch.Success)
                {
                    data.TalkingAbout = ParseCount(talkingMatch.Groups[1].Value);
                }
            }

            return data;
        }
        catch (Exception ex)
        {
            OnError($"Error extracting stats from meta tags: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Cleans the avatar URL by decoding HTML entities and removing unwanted spaces
    /// </summary>
    private static string CleanAvatarUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return url;

        // Step 1: Decode HTML entities (e.g., &amp; -> &)
        url = System.Net.WebUtility.HtmlDecode(url);

        // Step 2: Remove any spaces from the URL
        // Facebook sometimes has broken URLs with spaces like "stp=d st-jpg" or "kNvw EQbvJL"
        url = url.Replace(" ", "");

        return url;
    }

    /// <summary>
    /// Parses count strings like "14,624,776" or "14.624.776" or "1.2M"
    /// </summary>
    private static long ParseCount(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return 0;

        text = text.Trim().ToUpperInvariant();

        // Remove spaces
        text = text.Replace(" ", "");

        // Handle suffixes first
        double multiplier = 1;
        if (text.EndsWith("B"))
        {
            multiplier = 1_000_000_000;
            text = text[..^1];
        }
        else if (text.EndsWith("M"))
        {
            multiplier = 1_000_000;
            text = text[..^1];
        }
        else if (text.EndsWith("K"))
        {
            multiplier = 1_000;
            text = text[..^1];
        }

        // Remove thousand separators (both comma and dot can be used as thousand separators)
        // We need to determine which is the decimal separator
        // If there's a dot followed by exactly 3 digits at the end, it's a thousand separator
        // Otherwise, dot is decimal separator

        var hasComma = text.Contains(',');
        var hasDot = text.Contains('.');

        if (hasComma && hasDot)
        {
            // Both present - comma is likely thousand separator, dot is decimal
            text = text.Replace(",", "");
        }
        else if (hasComma)
        {
            // Only comma - could be thousand separator (14,624,776) or decimal separator (14,5)
            var parts = text.Split(',');
            if (parts.Length > 1 && parts[^1].Length == 3)
            {
                // Thousand separator
                text = text.Replace(",", "");
            }
            else
            {
                // Decimal separator
                text = text.Replace(",", ".");
            }
        }
        else if (hasDot)
        {
            // Only dot - could be thousand separator (14.624.776) or decimal separator (14.5)
            var parts = text.Split('.');
            if (parts.Length > 2 || (parts.Length == 2 && parts[^1].Length == 3))
            {
                // Thousand separator
                text = text.Replace(".", "");
            }
            // else it's a decimal separator, keep as is
        }

        if (double.TryParse(text, System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var value))
        {
            return (long)(value * multiplier);
        }

        return 0;
    }

    private void OnStatusChanged(string message)
    {
        StatusChanged?.Invoke(this, message);
    }

    private void OnError(string message)
    {
        ErrorOccurred?.Invoke(this, message);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            // Unsubscribe event handlers to prevent memory leaks
            if (_requestHandler != null)
            {
                if (_requestStatusHandler != null)
                    _requestHandler.StatusChanged -= _requestStatusHandler;
                if (_requestErrorHandler != null)
                    _requestHandler.ErrorOccurred -= _requestErrorHandler;
            }

            _httpClient?.Dispose();
            _httpClient = null;
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }

    ~FacebookScraperService()
    {
        Dispose();
    }
}

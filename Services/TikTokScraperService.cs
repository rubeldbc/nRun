using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using nRun.Models;
using OpenQA.Selenium;
using SkiaSharp;

namespace nRun.Services;

/// <summary>
/// Service for scraping TikTok profile information using Selenium WebDriver
/// </summary>
public class TikTokScraperService : IDisposable
{
    private WebDriverService? _webDriver;
    private HttpClient? _httpClient;
    private bool _disposed;
    private readonly RateLimiter _rateLimiter;
    private readonly ResilientRequestHandler _requestHandler;

    private static readonly string TikTokLogosFolder = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "tiktok_logos");

    public event EventHandler<string>? StatusChanged;
    public event EventHandler<string>? ErrorOccurred;

    public int TimeoutSeconds { get; set; } = 30;
    public bool UseHeadless { get; set; } = true;

    /// <summary>
    /// Gets the rate limiter for external configuration
    /// </summary>
    public RateLimiter RateLimiter => _rateLimiter;

    public TikTokScraperService(int baseDelaySeconds = 10, int jitterMaxSeconds = 5)
    {
        _rateLimiter = new RateLimiter(baseDelaySeconds, jitterMaxSeconds);
        _requestHandler = new ResilientRequestHandler(_rateLimiter);
        _requestHandler.StatusChanged += (s, msg) => OnStatusChanged(msg);
        _requestHandler.ErrorOccurred += (s, msg) => OnError(msg);

        // Ensure tiktok_logos folder exists
        if (!Directory.Exists(TikTokLogosFolder))
        {
            Directory.CreateDirectory(TikTokLogosFolder);
        }
    }

    private WebDriverService GetWebDriver()
    {
        if (_webDriver == null)
        {
            _webDriver = new WebDriverService
            {
                TimeoutSeconds = TimeoutSeconds,
                UseHeadless = UseHeadless
            };
        }
        return _webDriver;
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
        await _requestHandler.WaitBeforeNextRequestAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the current delay description for display
    /// </summary>
    public string GetDelayInfo() => _rateLimiter.GetDelayDescription();

    /// <summary>
    /// Downloads avatar image, resizes to 88x88, converts to WebP, and saves to tiktok_logos folder
    /// </summary>
    public async Task<string?> DownloadAndConvertAvatarAsync(string avatarUrl, string username)
    {
        try
        {
            if (string.IsNullOrEmpty(avatarUrl)) return null;

            OnStatusChanged($"Downloading avatar for @{username}...");

            var client = GetHttpClient();
            var imageBytes = await client.GetByteArrayAsync(avatarUrl);

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
            var filepath = Path.Combine(TikTokLogosFolder, filename);

            using var fileStream = File.OpenWrite(filepath);
            webpData.SaveTo(fileStream);

            OnStatusChanged($"Avatar saved for @{username}");
            return filepath;
        }
        catch (Exception ex)
        {
            OnError($"Failed to download avatar: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Extracts username from TikTok profile URL
    /// </summary>
    public static string? ExtractUsernameFromUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return null;

        // Match patterns like: https://www.tiktok.com/@username or tiktok.com/@username
        var match = Regex.Match(url, @"tiktok\.com/@([^/?]+)", RegexOptions.IgnoreCase);
        if (match.Success)
        {
            return match.Groups[1].Value;
        }

        return null;
    }

    /// <summary>
    /// Fetches TikTok profile information from a profile URL
    /// </summary>
    public async Task<TkProfile?> FetchProfileInfoAsync(string profileUrl)
    {
        return await FetchProfileInfoAsync(profileUrl, CancellationToken.None);
    }

    /// <summary>
    /// Fetches TikTok profile information from a profile URL with cancellation support
    /// </summary>
    public async Task<TkProfile?> FetchProfileInfoAsync(string profileUrl, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var profile = await Task.Run(() => FetchProfileInfo(profileUrl), cancellationToken);

        // Download and convert avatar if URL is available
        if (profile != null && !string.IsNullOrEmpty(profile.AvatarUrl))
        {
            cancellationToken.ThrowIfCancellationRequested();
            profile.AvatarLocalPath = await DownloadAndConvertAvatarAsync(profile.AvatarUrl, profile.Username);
        }

        return profile;
    }

    /// <summary>
    /// Fetches TikTok profile information from a profile URL (synchronous)
    /// </summary>
    public TkProfile? FetchProfileInfo(string profileUrl)
    {
        try
        {
            var username = ExtractUsernameFromUrl(profileUrl);
            if (string.IsNullOrEmpty(username))
            {
                OnError("Invalid TikTok URL. Expected format: https://www.tiktok.com/@username");
                return null;
            }

            OnStatusChanged($"Fetching profile info for @{username}...");

            var driver = GetWebDriver();
            var url = $"https://www.tiktok.com/@{username}";
            driver.NavigateTo(url);

            // Wait for profile to load
            Thread.Sleep(3000);

            // Try to extract data from the page's embedded JSON (SIGI_STATE)
            var profile = TryExtractFromSigiState(driver, username);
            if (profile != null)
            {
                OnStatusChanged($"Successfully fetched profile: @{username}");
                return profile;
            }

            // Fallback: Try to extract from page elements
            profile = TryExtractFromPageElements(driver, username);
            if (profile != null)
            {
                OnStatusChanged($"Successfully fetched profile: @{username}");
                return profile;
            }

            OnError($"Could not extract profile data for @{username}");
            return null;
        }
        catch (Exception ex)
        {
            OnError($"Error fetching profile: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Fetches TikTok statistics for a profile
    /// </summary>
    public async Task<TkData?> FetchStatsAsync(TkProfile profile)
    {
        return await Task.Run(() => FetchStats(profile));
    }

    /// <summary>
    /// Fetches TikTok statistics for a profile (synchronous)
    /// </summary>
    public TkData? FetchStats(TkProfile profile)
    {
        try
        {
            OnStatusChanged($"Fetching stats for @{profile.Username}...");

            var driver = GetWebDriver();
            var url = $"https://www.tiktok.com/@{profile.Username}";
            driver.NavigateTo(url);

            // Wait for profile to load
            Thread.Sleep(3000);

            // Try to extract stats from SIGI_STATE
            var stats = TryExtractStatsFromSigiState(driver, profile.UserId);
            if (stats != null)
            {
                stats.Username = profile.Username;
                stats.Nickname = profile.Nickname;
                OnStatusChanged($"Successfully fetched stats for @{profile.Username}");
                return stats;
            }

            // Fallback: Try to extract from page elements
            stats = TryExtractStatsFromPageElements(driver, profile.UserId);
            if (stats != null)
            {
                stats.Username = profile.Username;
                stats.Nickname = profile.Nickname;
                OnStatusChanged($"Successfully fetched stats for @{profile.Username}");
                return stats;
            }

            OnError($"Could not extract stats for @{profile.Username}");
            return null;
        }
        catch (Exception ex)
        {
            OnError($"Error fetching stats: {ex.Message}");
            return null;
        }
    }

    private TkProfile? TryExtractFromSigiState(WebDriverService driver, string username)
    {
        try
        {
            // TikTok embeds data in a script tag with id="SIGI_STATE" or "__UNIVERSAL_DATA_FOR_REHYDRATION__"
            var script = driver.ExecuteScript(@"
                var sigiState = document.getElementById('SIGI_STATE');
                if (sigiState) return sigiState.textContent;
                var univData = document.getElementById('__UNIVERSAL_DATA_FOR_REHYDRATION__');
                if (univData) return univData.textContent;
                return null;
            ")?.ToString();

            if (string.IsNullOrEmpty(script)) return null;

            using var doc = JsonDocument.Parse(script);
            var root = doc.RootElement;

            // Try different JSON structures TikTok uses
            JsonElement userInfo;
            string? regionFromAppContext = null;

            // Structure 1: SIGI_STATE format
            if (root.TryGetProperty("UserModule", out var userModule) &&
                userModule.TryGetProperty("users", out var users) &&
                users.TryGetProperty(username, out userInfo))
            {
                return ParseUserInfo(userInfo, username, null);
            }

            // Structure 2: __UNIVERSAL_DATA_FOR_REHYDRATION__ format
            if (root.TryGetProperty("__DEFAULT_SCOPE__", out var defaultScope))
            {
                // Extract region from webapp.app-context
                if (defaultScope.TryGetProperty("webapp.app-context", out var appContext))
                {
                    if (appContext.TryGetProperty("region", out var regionProp))
                        regionFromAppContext = regionProp.GetString();
                }

                // Extract user info from webapp.user-detail
                if (defaultScope.TryGetProperty("webapp.user-detail", out var userDetail) &&
                    userDetail.TryGetProperty("userInfo", out var userInfoWrapper) &&
                    userInfoWrapper.TryGetProperty("user", out userInfo))
                {
                    return ParseUserInfo(userInfo, username, regionFromAppContext);
                }
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    private TkProfile? ParseUserInfo(JsonElement userInfo, string username, string? regionFromAppContext)
    {
        try
        {
            var profile = new TkProfile
            {
                Username = username,
                Status = true,
                CreatedAt = DateTime.Now
            };

            if (userInfo.TryGetProperty("id", out var idProp))
            {
                if (idProp.ValueKind == JsonValueKind.String && long.TryParse(idProp.GetString(), out var id))
                    profile.UserId = id;
                else if (idProp.ValueKind == JsonValueKind.Number)
                    profile.UserId = idProp.GetInt64();
            }

            if (userInfo.TryGetProperty("uniqueId", out var uniqueId))
                profile.Username = uniqueId.GetString() ?? username;

            if (userInfo.TryGetProperty("nickname", out var nickname))
                profile.Nickname = nickname.GetString() ?? "";

            // Use region from webapp.app-context (passed as parameter)
            if (!string.IsNullOrEmpty(regionFromAppContext))
            {
                profile.Region = regionFromAppContext;
            }
            // Fallback: Try to get region from user object
            else if (userInfo.TryGetProperty("region", out var region))
                profile.Region = region.GetString() ?? "";
            else if (userInfo.TryGetProperty("regionCode", out region))
                profile.Region = region.GetString() ?? "";

            if (userInfo.TryGetProperty("createTime", out var createTime))
            {
                if (createTime.ValueKind == JsonValueKind.Number)
                {
                    var timestamp = createTime.GetInt64();
                    profile.CreatedAtTs = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
                }
            }

            // Extract avatarLarger URL from user object
            if (userInfo.TryGetProperty("avatarLarger", out var avatar))
                profile.AvatarUrl = avatar.GetString();
            else if (userInfo.TryGetProperty("avatarMedium", out avatar))
                profile.AvatarUrl = avatar.GetString();
            else if (userInfo.TryGetProperty("avatarThumb", out avatar))
                profile.AvatarUrl = avatar.GetString();

            return profile.UserId > 0 ? profile : null;
        }
        catch
        {
            return null;
        }
    }

    private TkProfile? TryExtractFromPageElements(WebDriverService driver, string username)
    {
        try
        {
            var profile = new TkProfile
            {
                Username = username,
                Status = true,
                CreatedAt = DateTime.Now
            };

            // Try to get nickname from page
            var nicknameElement = driver.FindElement("h1[data-e2e='user-subtitle']") ??
                                  driver.FindElement("h2[data-e2e='user-subtitle']") ??
                                  driver.FindElement("[class*='ShareTitle']");

            if (nicknameElement != null)
            {
                profile.Nickname = nicknameElement.Text;
            }

            // Generate a hash-based user ID if we can't get the real one
            // This is a fallback - real ID extraction is preferred
            profile.UserId = Math.Abs(username.GetHashCode());

            return profile;
        }
        catch
        {
            return null;
        }
    }

    private TkData? TryExtractStatsFromSigiState(WebDriverService driver, long userId)
    {
        try
        {
            var script = driver.ExecuteScript(@"
                var sigiState = document.getElementById('SIGI_STATE');
                if (sigiState) return sigiState.textContent;
                var univData = document.getElementById('__UNIVERSAL_DATA_FOR_REHYDRATION__');
                if (univData) return univData.textContent;
                return null;
            ")?.ToString();

            if (string.IsNullOrEmpty(script)) return null;

            using var doc = JsonDocument.Parse(script);
            var root = doc.RootElement;

            JsonElement stats;

            // Structure 1: SIGI_STATE with UserModule.stats
            if (root.TryGetProperty("UserModule", out var userModule) &&
                userModule.TryGetProperty("stats", out var statsModule))
            {
                // Find stats by iterating
                foreach (var prop in statsModule.EnumerateObject())
                {
                    stats = prop.Value;
                    return ParseStats(stats, userId);
                }
            }

            // Structure 2: __UNIVERSAL_DATA_FOR_REHYDRATION__
            if (root.TryGetProperty("__DEFAULT_SCOPE__", out var defaultScope) &&
                defaultScope.TryGetProperty("webapp.user-detail", out var userDetail) &&
                userDetail.TryGetProperty("userInfo", out var userInfoWrapper))
            {
                // Try statsV2 first (newer format), then stats
                if (userInfoWrapper.TryGetProperty("statsV2", out stats))
                {
                    return ParseStats(stats, userId);
                }
                if (userInfoWrapper.TryGetProperty("stats", out stats))
                {
                    return ParseStats(stats, userId);
                }
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    private TkData? ParseStats(JsonElement stats, long userId)
    {
        try
        {
            var data = new TkData
            {
                UserId = userId,
                RecordedAt = DateTime.Now
            };

            if (stats.TryGetProperty("followerCount", out var followers))
                data.FollowerCount = GetJsonInt64(followers);

            if (stats.TryGetProperty("heartCount", out var hearts))
                data.HeartCount = GetJsonInt64(hearts);
            else if (stats.TryGetProperty("heart", out hearts))
                data.HeartCount = GetJsonInt64(hearts);

            if (stats.TryGetProperty("videoCount", out var videos))
                data.VideoCount = (int)GetJsonInt64(videos);

            return data;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Safely extracts Int64 from JSON element (handles both number and string types)
    /// </summary>
    private static long GetJsonInt64(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Number)
            return element.GetInt64();
        if (element.ValueKind == JsonValueKind.String)
        {
            var str = element.GetString();
            if (long.TryParse(str, out var val))
                return val;
        }
        return 0;
    }

    private TkData? TryExtractStatsFromPageElements(WebDriverService driver, long userId)
    {
        try
        {
            var data = new TkData
            {
                UserId = userId,
                RecordedAt = DateTime.Now
            };

            // Try various selectors for follower count
            var followerText = driver.GetElementText("[data-e2e='followers-count']") ??
                               driver.GetElementText("[title='Followers']") ??
                               driver.GetElementText("[class*='FollowerCount']");

            if (!string.IsNullOrEmpty(followerText))
            {
                data.FollowerCount = ParseCount(followerText);
            }

            // Try various selectors for likes/hearts count
            var likesText = driver.GetElementText("[data-e2e='likes-count']") ??
                            driver.GetElementText("[title='Likes']") ??
                            driver.GetElementText("[class*='LikeCount']");

            if (!string.IsNullOrEmpty(likesText))
            {
                data.HeartCount = ParseCount(likesText);
            }

            // Try to get video count
            var videoText = driver.GetElementText("[data-e2e='video-count']") ??
                            driver.GetElementText("[class*='VideoCount']");

            if (!string.IsNullOrEmpty(videoText))
            {
                data.VideoCount = (int)ParseCount(videoText);
            }

            return data;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Parses count strings like "1.2M", "500K", "1,234"
    /// </summary>
    private static long ParseCount(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return 0;

        text = text.Trim().ToUpperInvariant();

        // Remove commas
        text = text.Replace(",", "");

        // Handle suffixes
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

        if (double.TryParse(text, out var value))
        {
            return (long)(value * multiplier);
        }

        return 0;
    }

    public void ResetDriver()
    {
        _webDriver?.ResetDriver();
    }

    public void CloseDriver()
    {
        _webDriver?.CloseDriver();
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
            _webDriver?.Dispose();
            _webDriver = null;
            _httpClient?.Dispose();
            _httpClient = null;
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }

    ~TikTokScraperService()
    {
        Dispose();
    }
}

using System.Text.Json;
using nRun.Models;
using nRun.Services.Interfaces;
using StackExchange.Redis;

namespace nRun.Services;

/// <summary>
/// Service for syncing news data to Memurai/Redis server
/// </summary>
public class MemuraiService : IMemuraiService
{
    private readonly ISettingsManager _settingsManager;
    private ConnectionMultiplexer? _redis;
    private IDatabase? _db;
    private System.Threading.Timer? _syncTimer;
    private bool _isRunning;
    private bool _disposed;
    private List<NewsInfo> _currentArticles = new();
    private readonly object _lock = new();

    private const string TopNewsKey = "top_news";

    public bool IsRunning => _isRunning;
    public bool IsConnected => _redis?.IsConnected ?? false;

    public event EventHandler<string>? StatusChanged;
    public event EventHandler<bool>? RunningStateChanged;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public MemuraiService(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
    }

    public void Start()
    {
        if (_isRunning) return;

        try
        {
            var settings = _settingsManager.LoadSettings();
            Connect(settings);

            if (!IsConnected)
            {
                OnStatusChanged("Failed to connect to Memurai");
                return;
            }

            // Start periodic sync timer
            var interval = TimeSpan.FromSeconds(settings.MemuraiSyncIntervalSeconds);
            _syncTimer = new System.Threading.Timer(
                async _ => await SyncTimerCallback(),
                null,
                TimeSpan.Zero,
                interval);

            _isRunning = true;
            OnRunningStateChanged(true);
            OnStatusChanged("Memurai sync started");
        }
        catch (Exception ex)
        {
            OnStatusChanged($"Memurai start error: {ex.Message}");
        }
    }

    public void Stop()
    {
        if (!_isRunning) return;

        _syncTimer?.Dispose();
        _syncTimer = null;

        _isRunning = false;
        OnRunningStateChanged(false);
        OnStatusChanged("Memurai sync stopped");
    }

    public async Task SyncNowAsync(List<NewsInfo> articles)
    {
        if (!IsConnected)
        {
            OnStatusChanged("Not connected to Memurai");
            return;
        }

        lock (_lock)
        {
            _currentArticles = articles.ToList();
        }

        await SyncToRedisAsync();
    }

    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            var settings = _settingsManager.LoadSettings();
            var configString = GetConnectionString(settings);

            var options = ConfigurationOptions.Parse(configString);
            options.ConnectTimeout = 3000;
            options.SyncTimeout = 3000;
            options.AbortOnConnectFail = true;

            await using var testConnection = await ConnectionMultiplexer.ConnectAsync(options);
            var db = testConnection.GetDatabase();
            await db.PingAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string?> GetStoredDataAsync()
    {
        try
        {
            var settings = _settingsManager.LoadSettings();
            var configString = GetConnectionString(settings);

            var options = ConfigurationOptions.Parse(configString);
            options.ConnectTimeout = 3000;
            options.AbortOnConnectFail = true;

            await using var connection = await ConnectionMultiplexer.ConnectAsync(options);
            var db = connection.GetDatabase();

            var data = await db.StringGetAsync(TopNewsKey);
            var count = await db.StringGetAsync($"{TopNewsKey}:count");
            var updated = await db.StringGetAsync($"{TopNewsKey}:updated");

            if (data.IsNullOrEmpty)
            {
                return "No data found in 'top_news' key.";
            }

            var result = new System.Text.StringBuilder();
            result.AppendLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            result.AppendLine("║                            MEMURAI DATA VIEWER                               ║");
            result.AppendLine("╠══════════════════════════════════════════════════════════════════════════════╣");
            result.AppendLine($"║  Key:          {TopNewsKey,-60} ║");
            result.AppendLine($"║  Count:        {count,-60} ║");
            result.AppendLine($"║  Last Updated: {updated,-60} ║");
            result.AppendLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            result.AppendLine();

            // Parse and format the JSON data
            try
            {
                var articles = JsonSerializer.Deserialize<List<MemuraiNewsItem>>(data.ToString(), _jsonOptions);
                if (articles != null && articles.Count > 0)
                {
                    int index = 1;
                    foreach (var article in articles)
                    {
                        result.AppendLine($"┌─────────────────────────────────────────────────────────────────────────────┐");
                        result.AppendLine($"│ #{index,-3} {TruncateString(article.Title ?? "No Title", 70),-71} │");
                        result.AppendLine($"├─────────────────────────────────────────────────────────────────────────────┤");
                        result.AppendLine($"│ ID:      {article.Id,-66} │");
                        result.AppendLine($"│ Site:    {TruncateString(article.SiteName ?? "Unknown", 66),-66} │");
                        result.AppendLine($"│ Logo:    {TruncateString(article.SiteLogo ?? "", 66),-66} │");
                        result.AppendLine($"│ URL:     {TruncateString(article.Url ?? "", 66),-66} │");
                        result.AppendLine($"│ Date:    {article.CreatedAt,-66} │");
                        result.AppendLine($"│ Read:    {(article.IsRead ? "Yes" : "No"),-66} │");
                        if (!string.IsNullOrEmpty(article.Text))
                        {
                            result.AppendLine($"├─────────────────────────────────────────────────────────────────────────────┤");
                            var textLines = WrapText(article.Text, 73);
                            foreach (var line in textLines.Take(5)) // Show first 5 lines of text
                            {
                                result.AppendLine($"│ {line,-75} │");
                            }
                            if (textLines.Count > 5)
                            {
                                result.AppendLine($"│ {"... (truncated)",-75} │");
                            }
                        }
                        result.AppendLine($"└─────────────────────────────────────────────────────────────────────────────┘");
                        result.AppendLine();
                        index++;
                    }
                }
                else
                {
                    result.AppendLine("No articles found in the data.");
                }
            }
            catch
            {
                // If JSON parsing fails, show raw data with pretty print
                result.AppendLine("Raw JSON Data:");
                result.AppendLine("─────────────────────────────────────────────────────────────────────────────");
                try
                {
                    var jsonDoc = JsonDocument.Parse(data.ToString());
                    var prettyJson = JsonSerializer.Serialize(jsonDoc, new JsonSerializerOptions { WriteIndented = true });
                    result.AppendLine(prettyJson);
                }
                catch
                {
                    result.AppendLine(data.ToString());
                }
            }

            return result.ToString();
        }
        catch (Exception ex)
        {
            return $"Error retrieving data: {ex.Message}";
        }
    }

    private static string TruncateString(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text)) return "";
        return text.Length <= maxLength ? text : text.Substring(0, maxLength - 3) + "...";
    }

    private static List<string> WrapText(string text, int maxWidth)
    {
        var lines = new List<string>();
        if (string.IsNullOrEmpty(text)) return lines;

        // Replace newlines with spaces and clean up
        text = text.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
        text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ").Trim();

        while (text.Length > 0)
        {
            if (text.Length <= maxWidth)
            {
                lines.Add(text);
                break;
            }

            int splitAt = text.LastIndexOf(' ', maxWidth);
            if (splitAt <= 0) splitAt = maxWidth;

            lines.Add(text.Substring(0, splitAt));
            text = text.Substring(splitAt).TrimStart();
        }

        return lines;
    }

    private class MemuraiNewsItem
    {
        public long Id { get; set; }
        public string? SiteId { get; set; }
        public string? SiteName { get; set; }
        public string? SiteLogo { get; set; }
        public string? Title { get; set; }
        public string? Url { get; set; }
        public string? Text { get; set; }
        public bool IsRead { get; set; }
        public string? CreatedAt { get; set; }
    }

    public void UpdateSettings()
    {
        if (_isRunning)
        {
            Stop();
            Start();
        }
    }

    public void UpdateArticles(List<NewsInfo> articles)
    {
        lock (_lock)
        {
            _currentArticles = articles.ToList();
        }
    }

    private void Connect(AppSettings settings)
    {
        try
        {
            Disconnect();

            var configString = GetConnectionString(settings);
            _redis = ConnectionMultiplexer.Connect(configString);
            _db = _redis.GetDatabase();

            OnStatusChanged("Connected to Memurai");
        }
        catch (Exception ex)
        {
            OnStatusChanged($"Memurai connection error: {ex.Message}");
        }
    }

    private void Disconnect()
    {
        _redis?.Dispose();
        _redis = null;
        _db = null;
    }

    private string GetConnectionString(AppSettings settings)
    {
        var config = $"{settings.MemuraiHost}:{settings.MemuraiPort}";
        if (!string.IsNullOrEmpty(settings.MemuraiPassword))
        {
            config += $",password={settings.MemuraiPassword}";
        }
        config += ",abortConnect=false,connectTimeout=5000";
        return config;
    }

    private async Task SyncTimerCallback()
    {
        try
        {
            await SyncToRedisAsync();
        }
        catch (Exception ex)
        {
            OnStatusChanged($"Memurai sync error: {ex.Message}");
        }
    }

    private async Task SyncToRedisAsync()
    {
        if (_db == null || !IsConnected) return;

        List<NewsInfo> articles;
        lock (_lock)
        {
            articles = _currentArticles.ToList();
        }

        if (articles.Count == 0)
        {
            OnStatusChanged("No articles to sync");
            return;
        }

        try
        {
            // Create JSON-serializable news items for Next.js
            var newsItems = articles.Select(a => new
            {
                id = a.Serial,
                siteId = a.SiteId,
                siteName = a.SiteName,
                siteLogo = a.SiteLogo,
                title = a.NewsTitle,
                url = a.NewsUrl,
                text = a.NewsText,
                isRead = a.IsRead,
                createdAt = a.CreatedAt.ToString("o")
            }).ToList();

            var json = JsonSerializer.Serialize(newsItems, _jsonOptions);

            // Store as a single JSON string under "top_news" key
            await _db.StringSetAsync(TopNewsKey, json);

            // Also store metadata
            await _db.StringSetAsync($"{TopNewsKey}:count", articles.Count);
            await _db.StringSetAsync($"{TopNewsKey}:updated", DateTime.UtcNow.ToString("o"));

            OnStatusChanged($"Synced {articles.Count} articles to Memurai");
        }
        catch (Exception ex)
        {
            OnStatusChanged($"Memurai sync error: {ex.Message}");
        }
    }

    private void OnStatusChanged(string status)
    {
        StatusChanged?.Invoke(this, status);
    }

    private void OnRunningStateChanged(bool isRunning)
    {
        RunningStateChanged?.Invoke(this, isRunning);
    }

    public void Dispose()
    {
        if (_disposed) return;

        Stop();
        Disconnect();

        _disposed = true;
        GC.SuppressFinalize(this);
    }
}

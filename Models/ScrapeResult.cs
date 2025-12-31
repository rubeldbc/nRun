namespace nRun.Models;

/// <summary>
/// Result of a scraping operation
/// </summary>
public class ScrapeResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public List<NewsInfo> Articles { get; set; } = new();
    public int NewArticlesCount { get; set; }
    public int SkippedCount { get; set; }
    public TimeSpan Duration { get; set; }

    public static ScrapeResult Failed(string error) => new()
    {
        Success = false,
        ErrorMessage = error
    };

    public static ScrapeResult Succeeded(List<NewsInfo> articles, int newCount, int skipped, TimeSpan duration) => new()
    {
        Success = true,
        Articles = articles,
        NewArticlesCount = newCount,
        SkippedCount = skipped,
        Duration = duration
    };
}

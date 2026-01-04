namespace nRun.Services.Interfaces;

/// <summary>
/// Interface for bulk import file parsing and processing
/// </summary>
public interface IBulkImportService
{
    /// <summary>
    /// Represents a parsed import item
    /// </summary>
    public record ImportItem(
        string Username,
        string? CompanyType = null,
        string? PageType = null,
        string? Region = null,
        string? OriginalUrl = null
    );

    /// <summary>
    /// Parse a file for TikTok usernames
    /// </summary>
    /// <param name="filePath">Path to the file (.txt or .csv)</param>
    /// <returns>List of usernames without @ prefix</returns>
    List<string> ParseTikTokFile(string filePath);

    /// <summary>
    /// Parse a file for Facebook page URLs/usernames
    /// </summary>
    /// <param name="filePath">Path to the file (.txt or .csv)</param>
    /// <returns>List of import items with metadata</returns>
    List<ImportItem> ParseFacebookFile(string filePath);

    /// <summary>
    /// Extract username from a TikTok URL or @username format
    /// </summary>
    /// <param name="input">URL or @username</param>
    /// <returns>Clean username without @ prefix</returns>
    string ExtractTikTokUsername(string input);

    /// <summary>
    /// Extract page identifier from a Facebook URL
    /// </summary>
    /// <param name="input">URL or page name</param>
    /// <returns>Clean page identifier</returns>
    string ExtractFacebookPageId(string input);

    /// <summary>
    /// Calculate delay with jitter for rate limiting
    /// </summary>
    /// <param name="baseDelaySeconds">Base delay in seconds</param>
    /// <param name="jitterFactor">Jitter factor (0.5 = 50% jitter)</param>
    /// <returns>Delay with random jitter applied</returns>
    TimeSpan CalculateDelayWithJitter(int baseDelaySeconds, double jitterFactor = 0.5);
}

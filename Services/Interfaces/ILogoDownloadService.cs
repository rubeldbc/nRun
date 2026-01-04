namespace nRun.Services.Interfaces;

/// <summary>
/// Interface for downloading and managing website logos
/// </summary>
public interface ILogoDownloadService : IDisposable
{
    /// <summary>
    /// Downloads and processes a website logo
    /// </summary>
    /// <param name="siteUrl">The website URL</param>
    /// <param name="siteName">The site name</param>
    /// <param name="existingLogoName">Optional: use this logo name instead of extracting from URL</param>
    /// <returns>Tuple of (path to saved logo file or null, logo name used for saving)</returns>
    Task<(string? path, string logoName)> DownloadLogoAsync(string siteUrl, string siteName, string? existingLogoName = null);

    /// <summary>
    /// Gets logo from online without saving to disk (for preview purposes)
    /// </summary>
    Task<byte[]?> GetLogoFromOnlineAsync(string siteUrl);

    /// <summary>
    /// Extracts the logo name from a site URL by getting the largest domain part
    /// </summary>
    string ExtractLogoNameFromUrl(string siteUrl);

    /// <summary>
    /// Gets the logo path for a logo name
    /// </summary>
    string? GetLogoPath(string? logoName);

    /// <summary>
    /// Gets the logos folder path
    /// </summary>
    string GetLogosFolder();
}

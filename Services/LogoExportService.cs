using SkiaSharp;

namespace nRun.Services;

public enum LogoImageFormat
{
    Png,
    WebP,
    Jpg
}

public enum LogoExportPlatform
{
    TikTok,
    Facebook
}

/// <summary>
/// Service for exporting profile logos/avatars to various formats and sizes.
/// </summary>
public class LogoExportService : IDisposable
{
    private HttpClient? _httpClient;
    private bool _disposed;

    public event EventHandler<string>? StatusChanged;
    public event EventHandler<string>? ErrorOccurred;

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
    /// Gets the default output folder for the specified platform.
    /// </summary>
    public static string GetDefaultOutputFolder(LogoExportPlatform platform)
    {
        var folderName = platform == LogoExportPlatform.TikTok ? "tk-logos" : "fb-logos";
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName);
    }

    /// <summary>
    /// Gets the file extension for the specified format.
    /// </summary>
    public static string GetFileExtension(LogoImageFormat format)
    {
        return format switch
        {
            LogoImageFormat.Png => "png",
            LogoImageFormat.WebP => "webp",
            LogoImageFormat.Jpg => "jpg",
            _ => "png"
        };
    }

    /// <summary>
    /// Gets the SkiaSharp encoded image format.
    /// </summary>
    private static SKEncodedImageFormat GetSkiaFormat(LogoImageFormat format)
    {
        return format switch
        {
            LogoImageFormat.Png => SKEncodedImageFormat.Png,
            LogoImageFormat.WebP => SKEncodedImageFormat.Webp,
            LogoImageFormat.Jpg => SKEncodedImageFormat.Jpeg,
            _ => SKEncodedImageFormat.Png
        };
    }

    /// <summary>
    /// Exports a logo from URL to the specified folder with the given format and size.
    /// </summary>
    /// <param name="avatarUrl">The URL of the avatar/logo to download</param>
    /// <param name="username">The username (used for filename)</param>
    /// <param name="outputFolder">The folder to save the logo</param>
    /// <param name="format">The image format to save as</param>
    /// <param name="useOriginalSize">If true, keeps original size; if false, uses customWidth/customHeight</param>
    /// <param name="customWidth">Custom width (used if useOriginalSize is false)</param>
    /// <param name="customHeight">Custom height (used if useOriginalSize is false)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if export was successful, false otherwise</returns>
    public async Task<bool> ExportLogoAsync(
        string avatarUrl,
        string username,
        string outputFolder,
        LogoImageFormat format,
        bool useOriginalSize,
        int customWidth = 88,
        int customHeight = 88,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrEmpty(avatarUrl))
            {
                OnError($"No avatar URL for {username}");
                return false;
            }

            OnStatusChanged($"Downloading logo for {username}...");

            // Ensure output folder exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Download image
            var client = GetHttpClient();
            var imageBytes = await client.GetByteArrayAsync(avatarUrl, cancellationToken).ConfigureAwait(false);

            if (imageBytes == null || imageBytes.Length < 100)
            {
                OnError($"Failed to download logo for {username}");
                return false;
            }

            // Decode image
            using var originalBitmap = SKBitmap.Decode(imageBytes);
            if (originalBitmap == null)
            {
                OnError($"Failed to decode logo for {username}");
                return false;
            }

            SKBitmap bitmapToSave;

            if (useOriginalSize)
            {
                bitmapToSave = originalBitmap;
            }
            else
            {
                // Resize to custom dimensions
                var resizeInfo = new SKImageInfo(customWidth, customHeight);
                var resizedBitmap = originalBitmap.Resize(resizeInfo, SKFilterQuality.High);
                if (resizedBitmap == null)
                {
                    OnError($"Failed to resize logo for {username}");
                    return false;
                }
                bitmapToSave = resizedBitmap;
            }

            try
            {
                // Encode to target format
                using var image = SKImage.FromBitmap(bitmapToSave);
                var skFormat = GetSkiaFormat(format);
                var quality = format == LogoImageFormat.Jpg ? 90 : 100;
                using var encodedData = image.Encode(skFormat, quality);

                if (encodedData == null)
                {
                    OnError($"Failed to encode logo for {username}");
                    return false;
                }

                // Save to file
                var extension = GetFileExtension(format);
                var filename = $"{username}.{extension}";
                var filepath = Path.Combine(outputFolder, filename);

                using var fileStream = File.Create(filepath);
                encodedData.SaveTo(fileStream);

                OnStatusChanged($"Exported logo for {username}");
                return true;
            }
            finally
            {
                // Dispose resized bitmap if we created one
                if (!useOriginalSize && bitmapToSave != originalBitmap)
                {
                    bitmapToSave.Dispose();
                }
            }
        }
        catch (OperationCanceledException)
        {
            OnStatusChanged($"Export cancelled for {username}");
            throw;
        }
        catch (Exception ex)
        {
            OnError($"Error exporting logo for {username}: {ex.Message}");
            return false;
        }
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
            _httpClient?.Dispose();
            _httpClient = null;
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }

    ~LogoExportService()
    {
        Dispose();
    }
}

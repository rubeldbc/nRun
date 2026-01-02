using OpenQA.Selenium.Chrome;

namespace nRun.Services;

/// <summary>
/// Configures Selenium WebDriver with standard browser settings
/// </summary>
public static class WebDriverConfig
{
    // Standard desktop User-Agent (Chrome on Windows)
    private const string DefaultUserAgent =
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";

    // Standard desktop viewport
    private const int DefaultWindowWidth = 1920;
    private const int DefaultWindowHeight = 1080;

    /// <summary>
    /// Creates ChromeOptions configured for standard browser behavior
    /// </summary>
    public static ChromeOptions CreateStandardOptions(bool headless = true, string? userAgent = null)
    {
        var options = new ChromeOptions();

        // Set User-Agent
        var agent = userAgent ?? DefaultUserAgent;
        options.AddArgument($"--user-agent={agent}");

        // Standard window size
        options.AddArgument($"--window-size={DefaultWindowWidth},{DefaultWindowHeight}");

        // Headless mode if requested
        if (headless)
        {
            options.AddArgument("--headless=new");
        }

        // Standard browser arguments for stability
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--disable-extensions");

        // Set language
        options.AddArgument("--lang=en-US");

        // Disable automation info bar
        options.AddExcludedArgument("enable-automation");

        // Performance optimizations
        options.AddArgument("--disable-background-timer-throttling");
        options.AddArgument("--disable-backgrounding-occluded-windows");

        // Page load strategy
        options.PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.Normal;

        return options;
    }

    /// <summary>
    /// Gets the default User-Agent string
    /// </summary>
    public static string GetDefaultUserAgent() => DefaultUserAgent;

    /// <summary>
    /// Gets the default window dimensions
    /// </summary>
    public static (int Width, int Height) GetDefaultWindowSize() => (DefaultWindowWidth, DefaultWindowHeight);
}

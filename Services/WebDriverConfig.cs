using OpenQA.Selenium.Chrome;

namespace nRun.Services;

/// <summary>
/// Configures Selenium WebDriver with standard browser settings and anti-detection measures
/// </summary>
public static class WebDriverConfig
{
    // Latest Chrome User-Agent (Windows 10, Chrome 131 - Jan 2025)
    // IMPORTANT: Must NOT contain "HeadlessChrome"
    private const string DefaultUserAgent =
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36";

    // Standard desktop viewport (modern resolution, not 800x600)
    private const int DefaultWindowWidth = 1920;
    private const int DefaultWindowHeight = 1080;

    /// <summary>
    /// Creates ChromeOptions configured for undetected browser behavior
    /// </summary>
    public static ChromeOptions CreateStandardOptions(bool headless = true, string? userAgent = null)
    {
        var options = new ChromeOptions();

        // Set User-Agent (must not contain HeadlessChrome)
        var agent = userAgent ?? DefaultUserAgent;
        options.AddArgument($"--user-agent={agent}");

        // Standard window size (1920x1080, not default 800x600)
        options.AddArgument($"--window-size={DefaultWindowWidth},{DefaultWindowHeight}");

        // New Headless Mode (--headless=new, not old --headless)
        if (headless)
        {
            options.AddArgument("--headless=new");
        }

        // Anti-detection: Disable automation flags
        options.AddArgument("--disable-blink-features=AutomationControlled");
        options.AddExcludedArgument("enable-automation");
        options.AddAdditionalOption("useAutomationExtension", false);

        // Standard browser arguments for stability
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--disable-extensions");
        options.AddArgument("--disable-infobars");

        // Set language
        options.AddArgument("--lang=en-US");

        // Performance optimizations
        options.AddArgument("--disable-background-timer-throttling");
        options.AddArgument("--disable-backgrounding-occluded-windows");
        options.AddArgument("--disable-renderer-backgrounding");
        options.AddArgument("--disable-software-rasterizer");

        // Page load strategy
        options.PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.Normal;

        return options;
    }

    /// <summary>
    /// Gets the default User-Agent string (latest Chrome on Windows 10)
    /// </summary>
    public static string GetDefaultUserAgent() => DefaultUserAgent;

    /// <summary>
    /// Gets the default window dimensions (1920x1080)
    /// </summary>
    public static (int Width, int Height) GetDefaultWindowSize() => (DefaultWindowWidth, DefaultWindowHeight);
}

using System.Diagnostics;
using System.Net.Http;
using System.Net.Sockets;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace nRun.Services;

/// <summary>
/// Manages Chrome WebDriver lifecycle for headless browser operations.
/// Designed to be created per-operation and disposed immediately after use.
/// </summary>
public class WebDriverService : IDisposable
{
    private IWebDriver? _driver;
    private ChromeDriverService? _driverService;
    private readonly object _lock = new();
    private bool _disposed;
    private int _chromeDriverProcessId;
    private readonly Guid _instanceId = Guid.NewGuid();

    public int TimeoutSeconds { get; set; } = 60;
    public bool UseHeadless { get; set; } = true;
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Unique identifier for this driver instance (useful for tracking/debugging)
    /// </summary>
    public Guid InstanceId => _instanceId;

    public IWebDriver GetDriver()
    {
        lock (_lock)
        {
        if (_driver == null)
  {
      _driver = CreateDriver();
  }
        return _driver;
     }
    }

    // Latest Chrome User-Agent (Windows 10, Chrome 131 - Jan 2025)
    // IMPORTANT: Must NOT contain "HeadlessChrome"
    private const string LatestUserAgent =
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36";

    private IWebDriver CreateDriver()
    {
        var options = new ChromeOptions();

        // New Headless Mode (--headless=new, NOT old --headless flag)
        if (UseHeadless)
        {
            options.AddArgument("--headless=new");
        }

        // Standard window size: 1920x1080 (NOT default 800x600)
        options.AddArgument("--window-size=1920,1080");

        // User-Agent: Latest Chrome on Windows 10 (must NOT contain "HeadlessChrome")
        options.AddArgument($"--user-agent={LatestUserAgent}");

        // Anti-detection: Disable automation flags
        options.AddArgument("--disable-blink-features=AutomationControlled");
        options.AddExcludedArgument("enable-automation");
        options.AddAdditionalOption("useAutomationExtension", false);

        // Standard browser arguments
        options.AddArgument("--disable-infobars");
        options.AddArgument("--disable-extensions");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--lang=en-US");

        // Additional stability options
        options.AddArgument("--disable-software-rasterizer");
        options.AddArgument("--disable-background-timer-throttling");
        options.AddArgument("--disable-backgrounding-occluded-windows");
        options.AddArgument("--disable-renderer-backgrounding");
        options.PageLoadStrategy = PageLoadStrategy.Normal;

        // Check for local ChromeDriver path
        var localDriverPath = ChromeVersionService.GetLocalChromeDriverPath();

        if (File.Exists(localDriverPath))
        {
            // Use local ChromeDriver
            _driverService = ChromeDriverService.CreateDefaultService(
                ChromeVersionService.GetLocalChromeDriverDirectory());
        }
        else
        {
            // Use ChromeDriver from PATH
            _driverService = ChromeDriverService.CreateDefaultService();
        }

        _driverService.HideCommandPromptWindow = true;
        _driverService.SuppressInitialDiagnosticInformation = true;

        // Use a slightly larger command timeout than PageLoad to avoid default 30s remote timeouts
        var commandTimeout = TimeSpan.FromSeconds(Math.Max(TimeoutSeconds * 2, 60));
        var driver = new ChromeDriver(_driverService, options, commandTimeout);

        // Track the ChromeDriver process for cleanup
        try
        {
            _chromeDriverProcessId = _driverService.ProcessId;
            ProcessCleanupService.TrackProcess(_chromeDriverProcessId);
        }
        catch { }

        // Set timeouts
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(TimeoutSeconds);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(30);

        // Hide navigator.webdriver flag using JavaScript injection
        HideWebDriverFlag(driver);

        return driver;
    }

    /// <summary>
    /// Hides the navigator.webdriver flag to avoid detection
    /// </summary>
    private static void HideWebDriverFlag(IWebDriver driver)
    {
        try
        {
            var js = (IJavaScriptExecutor)driver;

            // Remove webdriver property
            js.ExecuteScript(@"
                Object.defineProperty(navigator, 'webdriver', {
                    get: () => undefined
                });
            ");

            // Override navigator.plugins to appear more like a real browser
            js.ExecuteScript(@"
                Object.defineProperty(navigator, 'plugins', {
                    get: () => [1, 2, 3, 4, 5]
                });
            ");

            // Override navigator.languages
            js.ExecuteScript(@"
                Object.defineProperty(navigator, 'languages', {
                    get: () => ['en-US', 'en']
                });
            ");

            // Hide automation-related chrome properties
            js.ExecuteScript(@"
                window.chrome = {
                    runtime: {}
                };
            ");

            // Override permissions query to avoid detection
            js.ExecuteScript(@"
                const originalQuery = window.navigator.permissions.query;
                window.navigator.permissions.query = (parameters) => (
                    parameters.name === 'notifications' ?
                        Promise.resolve({ state: Notification.permission }) :
                        originalQuery(parameters)
                );
            ");
        }
        catch
        {
            // Ignore errors - page might not be loaded yet
        }
    }

    /// <summary>
    /// Navigates to a URL asynchronously with retry logic
    /// </summary>
    public async Task NavigateToAsync(string url, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("URL cannot be null or empty", nameof(url));
        }

        // Ensure URL has a valid scheme
        if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            url = "https://" + url;
        }

        // Validate URL format
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            throw new ArgumentException($"Invalid URL format: {url}", nameof(url));
        }

        // Navigation with retry logic - catch on every attempt and only rethrow after all attempts
        Exception? lastException = null;
        for (int attempt = 1; attempt <= MaxRetryAttempts; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await Task.Run(() => GetDriver().Navigate().GoToUrl(uri), cancellationToken);
                // Re-apply anti-detection measures after navigation
                ReapplyAntiDetection();
                return; // Success
            }
            catch (Exception ex) when (ex is WebDriverException || IsNetworkException(ex))
            {
                lastException = ex;

                // Detect renderer timeout / page load timeout and try to stop the page load and proceed
                var isRendererTimeout = ex is WebDriverTimeoutException
                    || (ex.Message != null && ex.Message.Contains("Timed out receiving message from renderer", StringComparison.OrdinalIgnoreCase));

                // Detect connection/network errors that require driver reset
                var isConnectionError = IsNetworkException(ex)
                    || (ex.Message != null && (
                        ex.Message.Contains("connection was forcibly closed", StringComparison.OrdinalIgnoreCase) ||
                        ex.Message.Contains("An error occurred while sending the request", StringComparison.OrdinalIgnoreCase) ||
                        ex.Message.Contains("The HTTP request to the remote WebDriver", StringComparison.OrdinalIgnoreCase)));

                if (isRendererTimeout && !isConnectionError)
                {
                    try
                    {
                        var js = (IJavaScriptExecutor)GetDriver();
                        // Stop further loading and give the page a moment
                        js.ExecuteScript("window.stop();");
                        await Task.Delay(500, cancellationToken);

                        // If we have any page source content, assume partial load is acceptable and continue
                        var src = GetDriver().PageSource;
                        if (!string.IsNullOrEmpty(src))
                        {
                            return;
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch
                    {
                        // ignore and fall back to retry logic below
                    }
                }

                if (attempt >= MaxRetryAttempts)
                {
                    // No more retries
                    break;
                }

                // Reset driver and retry after a progressive delay
                try { ResetDriver(); } catch { }
                await Task.Delay(1000 * attempt, cancellationToken);
            }
        }

        // All attempts failed - throw a wrapped exception with last captured exception
        throw new WebDriverException($"Failed to navigate to {url} after {MaxRetryAttempts} attempts", lastException);
    }

    /// <summary>
    /// Navigates to a URL synchronously (legacy - prefer NavigateToAsync)
    /// </summary>
    public void NavigateTo(string url)
    {
        NavigateToAsync(url, CancellationToken.None).GetAwaiter().GetResult();
    }

    public string GetPageSource()
    {
        return GetDriver().PageSource;
    }

    public List<IWebElement> FindElements(string cssSelector)
    {
        try
        {
       return GetDriver().FindElements(By.CssSelector(cssSelector)).ToList();
  }
        catch
    {
return new List<IWebElement>();
   }
    }

    public IWebElement? FindElement(string cssSelector)
    {
        try
        {
            return GetDriver().FindElement(By.CssSelector(cssSelector));
        }
        catch
        {
     return null;
        }
    }

    public string? GetElementText(string cssSelector)
    {
        var element = FindElement(cssSelector);
        return element?.Text;
    }

    public string? GetElementAttribute(string cssSelector, string attribute)
    {
        var element = FindElement(cssSelector);
        return element?.GetAttribute(attribute);
 }

    public List<string> GetAllLinks(string cssSelector)
    {
    var links = new List<string>();
        var elements = FindElements(cssSelector);

        foreach (var element in elements)
        {
      var href = element.GetAttribute("href");
 if (!string.IsNullOrEmpty(href))
            {
        links.Add(href);
    }
    }

        return links;
    }

    /// <summary>
    /// Gets the first link matching the CSS selector - optimized for performance
    /// </summary>
    public string? GetFirstLink(string cssSelector)
    {
        try
     {
            var element = GetDriver().FindElement(By.CssSelector(cssSelector));
 return element?.GetAttribute("href");
        }
    catch
        {
    return null;
        }
    }

    public void WaitForElement(string cssSelector, int timeoutSeconds = 10)
  {
        var wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(timeoutSeconds));
        wait.Until(d => d.FindElements(By.CssSelector(cssSelector)).Count > 0);
    }

    /// <summary>
    /// Waits for element to appear asynchronously, returns false on timeout instead of throwing
    /// </summary>
    public async Task<bool> TryWaitForElementAsync(string cssSelector, int timeoutSeconds = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            return await Task.Run(() =>
            {
                var wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(timeoutSeconds));
                wait.Until(d => d.FindElements(By.CssSelector(cssSelector)).Count > 0);
                return true;
            }, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Waits for element to appear, returns false on timeout instead of throwing (legacy)
    /// </summary>
    public bool TryWaitForElement(string cssSelector, int timeoutSeconds = 10)
    {
        return TryWaitForElementAsync(cssSelector, timeoutSeconds, CancellationToken.None).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Waits for page to fully load asynchronously
    /// </summary>
    public async Task WaitForPageLoadAsync(int timeoutSeconds = 30, CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
        {
            var wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(timeoutSeconds));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
        }, cancellationToken);
    }

    /// <summary>
    /// Waits for page to fully load (legacy - prefer WaitForPageLoadAsync)
    /// </summary>
    public void WaitForPageLoad(int timeoutSeconds = 30)
    {
        var wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(timeoutSeconds));
        wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
    }

    /// <summary>
    /// Executes JavaScript code
/// </summary>
    public object? ExecuteScript(string script)
  {
        try
        {
            var js = (IJavaScriptExecutor)GetDriver();
            return js.ExecuteScript(script);
        }
   catch
        {
     return null;
   }
    }

    /// <summary>
    /// Scrolls the page and waits for content to load asynchronously
    /// </summary>
    public async Task ScrollAndWaitAsync(int scrollCount = 2, int waitMs = 500, CancellationToken cancellationToken = default)
    {
        var js = (IJavaScriptExecutor)GetDriver();

        for (int i = 0; i < scrollCount; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            js.ExecuteScript("window.scrollBy(0, 300);");
            await Task.Delay(waitMs, cancellationToken);
        }

        // Return to top
        js.ExecuteScript("window.scrollTo(0, 0);");
        await Task.Delay(waitMs, cancellationToken);
    }

    /// <summary>
    /// Scrolls the page and waits for content to load (legacy - prefer ScrollAndWaitAsync)
    /// </summary>
    public void ScrollAndWait(int scrollCount = 2, int waitMs = 500)
    {
        ScrollAndWaitAsync(scrollCount, waitMs, CancellationToken.None).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Scrolls to page bottom asynchronously
    /// </summary>
    public async Task ScrollToBottomAsync(int waitMs = 500, CancellationToken cancellationToken = default)
    {
        var js = (IJavaScriptExecutor)GetDriver();
        js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
        await Task.Delay(waitMs, cancellationToken);
    }

    /// <summary>
    /// Scrolls to page bottom (legacy - prefer ScrollToBottomAsync)
    /// </summary>
    public void ScrollToBottom()
    {
        ScrollToBottomAsync(500, CancellationToken.None).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Simulates human-like mouse movement to an element before clicking.
    /// This helps avoid detection by moving the cursor gradually instead of direct clicks.
    /// </summary>
    public async Task MoveToElementAndClickAsync(IWebElement element, CancellationToken cancellationToken = default)
    {
        try
        {
            var driver = GetDriver();
            var actions = new Actions(driver);
            var random = new Random();

            // Get element location
            var location = element.Location;
            var size = element.Size;

            // Calculate center of element with small random offset
            var targetX = location.X + size.Width / 2 + random.Next(-5, 5);
            var targetY = location.Y + size.Height / 2 + random.Next(-5, 5);

            // Simulate gradual mouse movement with multiple steps
            var steps = random.Next(3, 6);
            for (int i = 0; i < steps; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Move towards element with some randomness
                actions.MoveToElement(element, random.Next(-10, 10), random.Next(-10, 10));

                // Small random delay between movements (50-150ms)
                await Task.Delay(random.Next(50, 150), cancellationToken);
            }

            // Final move to element center
            actions.MoveToElement(element);

            // Small delay before click (100-300ms) to simulate human behavior
            await Task.Delay(random.Next(100, 300), cancellationToken);

            // Perform click
            actions.Click().Perform();
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch
        {
            // Fallback to direct click if mouse simulation fails
            element.Click();
        }
    }

    /// <summary>
    /// Simulates human-like mouse movement to an element (without clicking)
    /// </summary>
    public async Task MoveToElementAsync(IWebElement element, CancellationToken cancellationToken = default)
    {
        try
        {
            var driver = GetDriver();
            var actions = new Actions(driver);
            var random = new Random();

            // Simulate gradual mouse movement
            var steps = random.Next(2, 4);
            for (int i = 0; i < steps; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                actions.MoveToElement(element, random.Next(-5, 5), random.Next(-5, 5));
                await Task.Delay(random.Next(30, 100), cancellationToken);
            }

            // Final move to element
            actions.MoveToElement(element).Perform();
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch
        {
            // Ignore if movement fails
        }
    }

    /// <summary>
    /// Simulates random mouse movements on the page to appear more human-like
    /// </summary>
    public async Task SimulateRandomMouseMovementAsync(int movements = 3, CancellationToken cancellationToken = default)
    {
        try
        {
            var driver = GetDriver();
            var actions = new Actions(driver);
            var random = new Random();
            var js = (IJavaScriptExecutor)driver;

            // Get viewport size
            var viewportWidth = Convert.ToInt32(js.ExecuteScript("return window.innerWidth;"));
            var viewportHeight = Convert.ToInt32(js.ExecuteScript("return window.innerHeight;"));

            for (int i = 0; i < movements; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Move to random position within viewport
                var x = random.Next(100, viewportWidth - 100);
                var y = random.Next(100, viewportHeight - 100);

                actions.MoveByOffset(x, y).Perform();
                actions.MoveByOffset(-x, -y).Perform(); // Reset position

                // Random delay between movements (200-500ms)
                await Task.Delay(random.Next(200, 500), cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch
        {
            // Ignore if simulation fails
        }
    }

    /// <summary>
    /// Checks if the exception is a network-related exception (SocketException, HttpRequestException, etc.)
    /// </summary>
    private static bool IsNetworkException(Exception ex)
    {
        if (ex is SocketException || ex is HttpRequestException)
            return true;

        // Check inner exceptions recursively
        var inner = ex.InnerException;
        while (inner != null)
        {
            if (inner is SocketException || inner is HttpRequestException)
                return true;
            inner = inner.InnerException;
        }

        return false;
    }

    /// <summary>
    /// Re-applies webdriver flag hiding after navigation (recommended after each page load)
    /// </summary>
    public void ReapplyAntiDetection()
    {
        try
        {
            HideWebDriverFlag(GetDriver());
        }
        catch
        {
            // Ignore errors
        }
    }

    public void ResetDriver()
    {
        lock (_lock)
        {
     CloseDriver();
    _driver = CreateDriver();
        }
    }

    public void CloseDriver()
    {
        lock (_lock)
        {
            var processIdToKill = _chromeDriverProcessId;

            if (_driver != null)
            {
                // Try graceful quit with timeout - don't let it hang
                var quitTask = Task.Run(() =>
                {
                    try
                    {
                        _driver.Quit();
                    }
                    catch { }
                });

                // Wait max 3 seconds for graceful quit
                if (!quitTask.Wait(3000))
                {
                    // Quit is hanging - force kill the process
                    ForceKillProcess(processIdToKill);
                }

                try
                {
                    _driver.Dispose();
                }
                catch { }
                finally
                {
                    _driver = null;
                }
            }

            // Dispose the driver service
            if (_driverService != null)
            {
                try
                {
                    // Untrack the process
                    if (_chromeDriverProcessId > 0)
                    {
                        ProcessCleanupService.UntrackProcess(_chromeDriverProcessId);
                    }

                    _driverService.Dispose();
                }
                catch { }
                finally
                {
                    _driverService = null;
                    _chromeDriverProcessId = 0;
                }
            }

            // Ensure process is killed if it's still running
            ForceKillProcess(processIdToKill);
        }
    }

    private static void ForceKillProcess(int processId)
    {
        if (processId <= 0) return;

        try
        {
            var process = Process.GetProcessById(processId);
            if (!process.HasExited)
            {
                process.Kill(true); // Kill entire process tree
            }
        }
        catch { }
    }

    public void Dispose()
    {
     if (!_disposed)
      {
     CloseDriver();

            // Force kill any remaining Chrome processes that might be orphaned
            try
            {
                if (_chromeDriverProcessId > 0)
                {
                    var process = Process.GetProcessById(_chromeDriverProcessId);
                    if (!process.HasExited)
                    {
                        process.Kill(true);
                    }
                }
            }
            catch { }

     _disposed = true;
  }
        GC.SuppressFinalize(this);
    }

    ~WebDriverService()
    {
        Dispose();
    }
}

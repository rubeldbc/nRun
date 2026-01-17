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
    /// When true, uses the user's default Chrome profile with cookies, history, etc.
    /// This makes the browser appear as a real human-used browser.
    /// </summary>
    public bool UseUserProfile { get; set; } = false;

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

        // ===== USE USER'S DEFAULT CHROME PROFILE =====
        // This makes the browser appear exactly like the user's real Chrome
        if (UseUserProfile)
        {
            var userDataDir = GetChromeUserDataDirectory();
            if (!string.IsNullOrEmpty(userDataDir) && Directory.Exists(userDataDir))
            {
                options.AddArgument($"--user-data-dir={userDataDir}");
                options.AddArgument("--profile-directory=Default");
            }

            // Don't use headless when using user profile
            // Don't disable extensions - keep user's extensions
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-blink-features=AutomationControlled");
            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalOption("useAutomationExtension", false);
            options.PageLoadStrategy = PageLoadStrategy.Normal;
        }
        else
        {
            // ===== STANDARD MODE (headless or automation) =====

            // New Headless Mode (--headless=new, NOT old --headless flag)
            if (UseHeadless)
            {
                options.AddArgument("--headless=new");
            }

            // Standard window size: 1920x1080 (NOT default 800x600)
            options.AddArgument("--window-size=1920,1080");

            // User-Agent: Latest Chrome on Windows 10 (must NOT contain "HeadlessChrome")
            options.AddArgument($"--user-agent={LatestUserAgent}");

            // ===== ANTI-DETECTION: Make browser appear as real human browser =====

            // Disable automation flags (critical for Cloudflare bypass)
            options.AddArgument("--disable-blink-features=AutomationControlled");
            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalOption("useAutomationExtension", false);

            // Disable webdriver mode completely
            options.AddArgument("--disable-web-security");
            options.AddArgument("--allow-running-insecure-content");

            // Make browser fingerprint look real
            options.AddArgument("--disable-features=IsolateOrigins,site-per-process");
            options.AddArgument("--flag-switches-begin");
            options.AddArgument("--flag-switches-end");

            // Realistic browser settings
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-popup-blocking");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument("--disable-default-apps");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-hang-monitor");
            options.AddArgument("--disable-prompt-on-repost");
            options.AddArgument("--disable-sync");
            options.AddArgument("--disable-translate");
            options.AddArgument("--metrics-recording-only");
            options.AddArgument("--no-first-run");
            options.AddArgument("--safebrowsing-disable-auto-update");

            // Prevent detection via navigator properties
            options.AddArgument("--disable-plugins-discovery");
            options.AddArgument("--disable-bundled-ppapi-flash");

            // Standard browser arguments
            options.AddArgument("--disable-infobars");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--lang=en-US,en;q=0.9");

            // Additional stability options
            options.AddArgument("--disable-software-rasterizer");
            options.AddArgument("--disable-background-timer-throttling");
            options.AddArgument("--disable-backgrounding-occluded-windows");
            options.AddArgument("--disable-renderer-backgrounding");
            options.AddArgument("--disable-background-networking");
            options.AddArgument("--disable-client-side-phishing-detection");
            options.AddArgument("--disable-component-update");
            options.AddArgument("--disable-domain-reliability");
            options.AddArgument("--disable-features=TranslateUI");

            options.PageLoadStrategy = PageLoadStrategy.Normal;
        }

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
        
        ChromeDriver driver;
        try
        {
            driver = new ChromeDriver(_driverService, options, commandTimeout);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("session not created") || ex.Message.Contains("Chrome instance exited"))
        {
            // If using user profile and Chrome is already running, retry without user profile
            if (UseUserProfile)
            {
                _driverService?.Dispose();
                _driverService = null;
                
                // Recreate without user profile - fall back to standard mode
                UseUserProfile = false;
                return CreateDriver();
            }
            
            throw new InvalidOperationException(
                "Failed to create Chrome session. This may occur if:\n" +
                "1. Chrome browser is not installed\n" +
                "2. ChromeDriver version doesn't match Chrome version\n" +
                "3. Chrome is already running with the same profile\n" +
                "Original error: " + ex.Message, ex);
        }

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

        // CRITICAL: Use CDP to inject stealth script BEFORE any page loads
        // This runs before Cloudflare can detect webdriver
        InjectStealthViaCDP(driver);

        // Hide navigator.webdriver flag using JavaScript injection (backup)
        HideWebDriverFlag(driver);

        return driver;
    }

    /// <summary>
    /// Comprehensive stealth script to make browser appear as real human browser.
    /// This bypasses Cloudflare and other bot detection systems.
    /// </summary>
    private static void HideWebDriverFlag(IWebDriver driver)
    {
        try
        {
            var js = (IJavaScriptExecutor)driver;

            // Comprehensive stealth script - mimics puppeteer-extra-plugin-stealth
            js.ExecuteScript(@"
                // 1. Remove webdriver property completely
                Object.defineProperty(navigator, 'webdriver', {
                    get: () => undefined,
                    configurable: true
                });
                delete navigator.__proto__.webdriver;

                // 2. Fix navigator.plugins to look like real Chrome
                Object.defineProperty(navigator, 'plugins', {
                    get: () => {
                        const plugins = [
                            { name: 'Chrome PDF Plugin', filename: 'internal-pdf-viewer', description: 'Portable Document Format' },
                            { name: 'Chrome PDF Viewer', filename: 'mhjfbmdgcfjbbpaeojofohoefgiehjai', description: '' },
                            { name: 'Native Client', filename: 'internal-nacl-plugin', description: '' }
                        ];
                        plugins.length = 3;
                        plugins.item = (i) => plugins[i];
                        plugins.namedItem = (name) => plugins.find(p => p.name === name);
                        plugins.refresh = () => {};
                        return plugins;
                    },
                    configurable: true
                });

                // 3. Fix navigator.languages
                Object.defineProperty(navigator, 'languages', {
                    get: () => ['en-US', 'en'],
                    configurable: true
                });

                // 4. Fix navigator.platform
                Object.defineProperty(navigator, 'platform', {
                    get: () => 'Win32',
                    configurable: true
                });

                // 5. Fix navigator.hardwareConcurrency (CPU cores)
                Object.defineProperty(navigator, 'hardwareConcurrency', {
                    get: () => 8,
                    configurable: true
                });

                // 6. Fix navigator.deviceMemory
                Object.defineProperty(navigator, 'deviceMemory', {
                    get: () => 8,
                    configurable: true
                });

                // 7. Fix navigator.maxTouchPoints
                Object.defineProperty(navigator, 'maxTouchPoints', {
                    get: () => 0,
                    configurable: true
                });

                // 8. Fix screen properties
                Object.defineProperty(screen, 'availWidth', { get: () => 1920, configurable: true });
                Object.defineProperty(screen, 'availHeight', { get: () => 1040, configurable: true });
                Object.defineProperty(screen, 'width', { get: () => 1920, configurable: true });
                Object.defineProperty(screen, 'height', { get: () => 1080, configurable: true });
                Object.defineProperty(screen, 'colorDepth', { get: () => 24, configurable: true });
                Object.defineProperty(screen, 'pixelDepth', { get: () => 24, configurable: true });

                // 9. Fix window.outerWidth/Height
                Object.defineProperty(window, 'outerWidth', { get: () => 1920, configurable: true });
                Object.defineProperty(window, 'outerHeight', { get: () => 1080, configurable: true });

                // 10. Create realistic chrome object
                window.chrome = {
                    app: {
                        isInstalled: false,
                        InstallState: { DISABLED: 'disabled', INSTALLED: 'installed', NOT_INSTALLED: 'not_installed' },
                        RunningState: { CANNOT_RUN: 'cannot_run', READY_TO_RUN: 'ready_to_run', RUNNING: 'running' }
                    },
                    runtime: {
                        OnInstalledReason: { CHROME_UPDATE: 'chrome_update', INSTALL: 'install', SHARED_MODULE_UPDATE: 'shared_module_update', UPDATE: 'update' },
                        OnRestartRequiredReason: { APP_UPDATE: 'app_update', OS_UPDATE: 'os_update', PERIODIC: 'periodic' },
                        PlatformArch: { ARM: 'arm', ARM64: 'arm64', MIPS: 'mips', MIPS64: 'mips64', X86_32: 'x86-32', X86_64: 'x86-64' },
                        PlatformNaclArch: { ARM: 'arm', MIPS: 'mips', MIPS64: 'mips64', X86_32: 'x86-32', X86_64: 'x86-64' },
                        PlatformOs: { ANDROID: 'android', CROS: 'cros', LINUX: 'linux', MAC: 'mac', OPENBSD: 'openbsd', WIN: 'win' },
                        RequestUpdateCheckStatus: { NO_UPDATE: 'no_update', THROTTLED: 'throttled', UPDATE_AVAILABLE: 'update_available' },
                        id: undefined,
                        connect: function() {},
                        sendMessage: function() {}
                    },
                    csi: function() {},
                    loadTimes: function() {}
                };

                // 11. Fix permissions API
                const originalQuery = window.navigator.permissions.query;
                window.navigator.permissions.query = (parameters) => (
                    parameters.name === 'notifications' ?
                        Promise.resolve({ state: Notification.permission }) :
                        originalQuery(parameters)
                );

                // 12. Fix WebGL vendor and renderer
                const getParameterProxyHandler = {
                    apply: function(target, ctx, args) {
                        const param = args[0];
                        if (param === 37445) return 'Google Inc. (NVIDIA)';
                        if (param === 37446) return 'ANGLE (NVIDIA, NVIDIA GeForce GTX 1060 Direct3D11 vs_5_0 ps_5_0, D3D11)';
                        return Reflect.apply(target, ctx, args);
                    }
                };
                try {
                    const canvas = document.createElement('canvas');
                    const gl = canvas.getContext('webgl') || canvas.getContext('experimental-webgl');
                    if (gl) {
                        const originalGetParameter = gl.getParameter.bind(gl);
                        gl.getParameter = new Proxy(originalGetParameter, getParameterProxyHandler);
                    }
                } catch(e) {}

                // 13. Remove Selenium/WebDriver traces
                delete window.cdc_adoQpoasnfa76pfcZLmcfl_Array;
                delete window.cdc_adoQpoasnfa76pfcZLmcfl_Promise;
                delete window.cdc_adoQpoasnfa76pfcZLmcfl_Symbol;
                delete document.$cdc_asdjflasutopfhvcZLmcfl_;
                delete document.__webdriver_script_fn;
                delete document.__driver_evaluate;
                delete document.__webdriver_evaluate;
                delete document.__selenium_evaluate;
                delete document.__fxdriver_evaluate;
                delete document.__driver_unwrapped;
                delete document.__webdriver_unwrapped;
                delete document.__selenium_unwrapped;
                delete document.__fxdriver_unwrapped;
                delete document.__webdriver_script_function;
                delete document.documentElement.getAttribute;

                // 14. Fix toString to hide modifications
                const originalToString = Function.prototype.toString;
                Function.prototype.toString = function() {
                    if (this === window.navigator.permissions.query) {
                        return 'function query() { [native code] }';
                    }
                    return originalToString.call(this);
                };

                // 15. Add realistic Notification API
                if (!window.Notification) {
                    window.Notification = {
                        permission: 'default',
                        requestPermission: () => Promise.resolve('default')
                    };
                }

                // 16. Fix iframe contentWindow
                Object.defineProperty(HTMLIFrameElement.prototype, 'contentWindow', {
                    get: function() {
                        return window;
                    }
                });
            ");
        }
        catch
        {
            // Ignore errors - page might not be loaded yet
        }
    }

    /// <summary>
    /// Injects stealth script via Chrome DevTools Protocol BEFORE any page loads.
    /// This is critical for bypassing Cloudflare because it runs before page scripts.
    /// </summary>
    private static void InjectStealthViaCDP(ChromeDriver driver)
    {
        try
        {
            // Stealth script that runs before every page load
            var stealthScript = @"
                // Remove webdriver flag immediately
                Object.defineProperty(navigator, 'webdriver', {
                    get: () => undefined,
                    configurable: true
                });

                // Delete webdriver from prototype
                if (navigator.__proto__) {
                    delete navigator.__proto__.webdriver;
                }

                // Fix plugins
                Object.defineProperty(navigator, 'plugins', {
                    get: () => {
                        const p = [
                            { name: 'Chrome PDF Plugin', filename: 'internal-pdf-viewer', description: 'Portable Document Format' },
                            { name: 'Chrome PDF Viewer', filename: 'mhjfbmdgcfjbbpaeojofohoefgiehjai', description: '' },
                            { name: 'Native Client', filename: 'internal-nacl-plugin', description: '' }
                        ];
                        p.length = 3;
                        return p;
                    },
                    configurable: true
                });

                // Fix languages
                Object.defineProperty(navigator, 'languages', {
                    get: () => ['en-US', 'en'],
                    configurable: true
                });

                // Fix platform
                Object.defineProperty(navigator, 'platform', {
                    get: () => 'Win32',
                    configurable: true
                });

                // Create chrome object
                window.chrome = {
                    app: { isInstalled: false },
                    runtime: { id: undefined, connect: function(){}, sendMessage: function(){} },
                    csi: function(){},
                    loadTimes: function(){}
                };

                // Remove Selenium traces
                const props = ['cdc_adoQpoasnfa76pfcZLmcfl_Array', 'cdc_adoQpoasnfa76pfcZLmcfl_Promise',
                               'cdc_adoQpoasnfa76pfcZLmcfl_Symbol', '$cdc_asdjflasutopfhvcZLmcfl_'];
                for (const prop of props) {
                    delete window[prop];
                    delete document[prop];
                }
            ";

            // Use CDP to add script that runs before every document
            var parameters = new Dictionary<string, object>
            {
                { "source", stealthScript }
            };

            driver.ExecuteCdpCommand("Page.addScriptToEvaluateOnNewDocument", parameters);
        }
        catch
        {
            // CDP might not be available in some configurations
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
        try
        {
            var driver = GetDriver();
            return driver?.PageSource ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Gets the current page title from the browser tab (after JavaScript execution)
    /// </summary>
    public string GetPageTitle()
    {
        try
        {
            return GetDriver().Title ?? "";
        }
        catch
        {
            return "";
        }
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
                try
                {
                    var wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(timeoutSeconds));
                    wait.Until(d => d.FindElements(By.CssSelector(cssSelector)).Count > 0);
                    return true;
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }
                catch (WebDriverException)
                {
                    return false;
                }
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
    /// Gets the Chrome user data directory path for the current user.
    /// This contains the user's profile with cookies, history, extensions, etc.
    /// </summary>
    private static string? GetChromeUserDataDirectory()
    {
        try
        {
            // Windows: C:\Users\{username}\AppData\Local\Google\Chrome\User Data
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var chromeUserData = Path.Combine(localAppData, "Google", "Chrome", "User Data");

            if (Directory.Exists(chromeUserData))
            {
                return chromeUserData;
            }

            return null;
        }
        catch
        {
            return null;
        }
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

    /// <summary>
    /// Aggressively kills Chrome processes spawned by this driver using taskkill.
    /// This ensures the browser is fully closed even if graceful quit fails.
    /// DOES NOT try graceful close methods as they can hang on Cloudflare pages.
    /// </summary>
    public void ForceKillChrome()
    {
        // Get process ID before any cleanup attempts
        var processId = _chromeDriverProcessId;

        // Kill by ChromeDriver process ID FIRST (kills entire process tree including Chrome)
        if (processId > 0)
        {
            RunTaskKill($"/F /T /PID {processId}");
        }

        // Set driver to null to prevent further operations
        try
        {
            _driver = null;
            _driverService?.Dispose();
            _driverService = null;
            _chromeDriverProcessId = 0;
        }
        catch { }
    }

    /// <summary>
    /// Runs taskkill with the specified arguments
    /// </summary>
    private static void RunTaskKill(string arguments)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "taskkill",
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            using var proc = Process.Start(startInfo);
            proc?.WaitForExit(5000);
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

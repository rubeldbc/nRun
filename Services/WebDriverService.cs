using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace nRun.Services;

/// <summary>
/// Manages Chrome WebDriver lifecycle for headless browser operations
/// </summary>
public class WebDriverService : IDisposable
{
    private IWebDriver? _driver;
    private readonly object _lock = new();
    private bool _disposed;

    public int TimeoutSeconds { get; set; } = 30;
    public bool UseHeadless { get; set; } = true;

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

    private IWebDriver CreateDriver()
    {
        var options = new ChromeOptions();

        if (UseHeadless)
        {
            options.AddArgument("--headless=new");
        }

        // Anti-detection measures
        options.AddArgument("--disable-blink-features=AutomationControlled");
        options.AddArgument("--disable-infobars");
        options.AddArgument("--disable-extensions");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--window-size=1920,1080");
        options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

        // Exclude automation flags
        options.AddExcludedArgument("enable-automation");
        options.AddAdditionalOption("useAutomationExtension", false);

        var service = ChromeDriverService.CreateDefaultService();
        service.HideCommandPromptWindow = true;
        service.SuppressInitialDiagnosticInformation = true;

        var driver = new ChromeDriver(service, options, TimeSpan.FromSeconds(TimeoutSeconds));
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(TimeoutSeconds);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

        return driver;
    }

    public void NavigateTo(string url)
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

        GetDriver().Navigate().GoToUrl(uri);
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
    /// ????????? ????? ?????? ??????? ??? - ???????????? ????????????
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
    /// ???????? ??? ?????? ???? ??????? ???, ?? ???? false ??????? ???
    /// </summary>
    public bool TryWaitForElement(string cssSelector, int timeoutSeconds = 10)
    {
        try
        {
            var wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(timeoutSeconds));
            wait.Until(d => d.FindElements(By.CssSelector(cssSelector)).Count > 0);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// ??? ???????? ??? ?????? ???? ??????? ???
    /// </summary>
    public void WaitForPageLoad(int timeoutSeconds = 30)
    {
        var wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(timeoutSeconds));
        wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
    }

    /// <summary>
    /// JavaScript ????????? ???
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
    /// ??? ?????? ??? ??? ????????? ???????? ??? ?????? ???? ??????? ???
    /// </summary>
    public void ScrollAndWait(int scrollCount = 2, int waitMs = 500)
    {
        var js = (IJavaScriptExecutor)GetDriver();

        for (int i = 0; i < scrollCount; i++)
        {
            js.ExecuteScript("window.scrollBy(0, 300);");
            Thread.Sleep(waitMs);
        }

        // ???? ???? ?????? ???
        js.ExecuteScript("window.scrollTo(0, 0);");
        Thread.Sleep(waitMs);
    }

    public void ScrollToBottom()
    {
        var js = (IJavaScriptExecutor)GetDriver();
        js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
        Thread.Sleep(500);
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
            if (_driver != null)
            {
                try
                {
                    _driver.Quit();
                    _driver.Dispose();
                }
                catch { }
                finally
                {
                    _driver = null;
                }
            }
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            CloseDriver();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}

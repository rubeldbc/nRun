namespace nRun.Services;

/// <summary>
/// Provides timeout-protected execution of WebDriver operations.
/// Automatically disposes the WebDriver if operation exceeds timeout.
/// </summary>
public static class WebDriverOperationWrapper
{
    /// <summary>
    /// Default timeout for WebDriver operations in milliseconds (2 minutes)
    /// </summary>
    public static int DefaultTimeoutMs { get; set; } = 120000;

    /// <summary>
    /// Executes a WebDriver operation with timeout protection.
    /// Creates a WebDriver, executes the operation, and disposes the driver.
    /// If the operation times out, the driver is forcefully disposed.
    /// </summary>
    /// <typeparam name="T">Return type of the operation</typeparam>
    /// <param name="operation">The async operation to execute</param>
    /// <param name="timeoutMs">Timeout in milliseconds (default: DefaultTimeoutMs)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the operation</returns>
    public static async Task<T> ExecuteAsync<T>(
        Func<WebDriverService, CancellationToken, Task<T>> operation,
        int? timeoutMs = null,
        CancellationToken cancellationToken = default)
    {
        var timeout = timeoutMs ?? DefaultTimeoutMs;
        WebDriverService? driver = null;

        using var timeoutCts = new CancellationTokenSource(timeout);
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken, timeoutCts.Token);

        try
        {
            driver = WebDriverFactory.Create();
            return await operation(driver, linkedCts.Token);
        }
        catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested)
        {
            throw new TimeoutException($"WebDriver operation timed out after {timeout}ms");
        }
        finally
        {
            if (driver != null)
            {
                WebDriverFactory.SafeDispose(driver);
            }
        }
    }

    /// <summary>
    /// Executes a WebDriver operation with timeout protection (no return value).
    /// Creates a WebDriver, executes the operation, and disposes the driver.
    /// </summary>
    /// <param name="operation">The async operation to execute</param>
    /// <param name="timeoutMs">Timeout in milliseconds (default: DefaultTimeoutMs)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public static async Task ExecuteAsync(
        Func<WebDriverService, CancellationToken, Task> operation,
        int? timeoutMs = null,
        CancellationToken cancellationToken = default)
    {
        var timeout = timeoutMs ?? DefaultTimeoutMs;
        WebDriverService? driver = null;

        using var timeoutCts = new CancellationTokenSource(timeout);
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken, timeoutCts.Token);

        try
        {
            driver = WebDriverFactory.Create();
            await operation(driver, linkedCts.Token);
        }
        catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested)
        {
            throw new TimeoutException($"WebDriver operation timed out after {timeout}ms");
        }
        finally
        {
            if (driver != null)
            {
                WebDriverFactory.SafeDispose(driver);
            }
        }
    }

    /// <summary>
    /// Executes a WebDriver operation with a provided driver and timeout protection.
    /// Does NOT dispose the driver - caller is responsible for disposal.
    /// </summary>
    public static async Task<T> ExecuteWithDriverAsync<T>(
        WebDriverService driver,
        Func<WebDriverService, CancellationToken, Task<T>> operation,
        int? timeoutMs = null,
        CancellationToken cancellationToken = default)
    {
        var timeout = timeoutMs ?? DefaultTimeoutMs;

        using var timeoutCts = new CancellationTokenSource(timeout);
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken, timeoutCts.Token);

        try
        {
            return await operation(driver, linkedCts.Token);
        }
        catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested)
        {
            throw new TimeoutException($"WebDriver operation timed out after {timeout}ms");
        }
    }

    /// <summary>
    /// Executes multiple WebDriver operations sequentially with shared driver and overall timeout.
    /// Creates one WebDriver, executes all operations, then disposes.
    /// </summary>
    /// <typeparam name="T">Return type of the final operation</typeparam>
    /// <param name="operations">List of operations to execute in sequence</param>
    /// <param name="timeoutMs">Overall timeout for all operations</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public static async Task<T> ExecuteSequenceAsync<T>(
        Func<WebDriverService, CancellationToken, Task<T>> finalOperation,
        int? timeoutMs = null,
        CancellationToken cancellationToken = default,
        params Func<WebDriverService, CancellationToken, Task>[] preparationOperations)
    {
        var timeout = timeoutMs ?? DefaultTimeoutMs;
        WebDriverService? driver = null;

        using var timeoutCts = new CancellationTokenSource(timeout);
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken, timeoutCts.Token);

        try
        {
            driver = WebDriverFactory.Create();

            // Execute preparation operations
            foreach (var op in preparationOperations)
            {
                await op(driver, linkedCts.Token);
            }

            // Execute final operation
            return await finalOperation(driver, linkedCts.Token);
        }
        catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested)
        {
            throw new TimeoutException($"WebDriver operation sequence timed out after {timeout}ms");
        }
        finally
        {
            if (driver != null)
            {
                WebDriverFactory.SafeDispose(driver);
            }
        }
    }

    /// <summary>
    /// Tries to execute a WebDriver operation with timeout protection.
    /// Returns default(T) on failure instead of throwing.
    /// </summary>
    public static async Task<(bool success, T? result, Exception? error)> TryExecuteAsync<T>(
        Func<WebDriverService, CancellationToken, Task<T>> operation,
        int? timeoutMs = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await ExecuteAsync(operation, timeoutMs, cancellationToken);
            return (true, result, null);
        }
        catch (Exception ex)
        {
            return (false, default, ex);
        }
    }
}

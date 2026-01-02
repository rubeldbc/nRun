namespace nRun.Services;

/// <summary>
/// Handles requests with automatic retry, exponential backoff, and rate limiting
/// </summary>
public class ResilientRequestHandler
{
    private readonly RateLimiter _rateLimiter;
    private readonly int _maxRetries;

    public event EventHandler<string>? StatusChanged;
    public event EventHandler<string>? ErrorOccurred;

    public ResilientRequestHandler(RateLimiter rateLimiter, int maxRetries = 3)
    {
        _rateLimiter = rateLimiter;
        _maxRetries = maxRetries;
    }

    /// <summary>
    /// Executes an action with automatic retry and backoff on failure
    /// </summary>
    public async Task<T?> ExecuteWithRetryAsync<T>(
        Func<Task<T?>> action,
        CancellationToken cancellationToken = default) where T : class
    {
        int attempt = 0;

        while (attempt < _maxRetries)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                attempt++;
                var result = await action().ConfigureAwait(false);

                if (result != null)
                {
                    // Success - reset backoff
                    _rateLimiter.ResetBackoff();
                    return result;
                }

                // Null result - might be rate limited or blocked
                OnStatus($"Attempt {attempt}/{_maxRetries} returned no data");
                _rateLimiter.ApplyBackoff();
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                OnError($"Attempt {attempt}/{_maxRetries} failed: {ex.Message}");

                if (IsRateLimitError(ex))
                {
                    _rateLimiter.ApplyBackoff();
                    OnStatus($"Rate limit detected, backing off ({_rateLimiter.GetDelayDescription()})");
                }
            }

            // Wait before retry (unless it's the last attempt)
            if (attempt < _maxRetries)
            {
                var delay = _rateLimiter.GetNextDelayMs();
                OnStatus($"Waiting {delay / 1000}s before retry...");

                try
                {
                    await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
            }
        }

        OnError($"All {_maxRetries} attempts failed");
        return null;
    }

    /// <summary>
    /// Waits using the rate limiter before proceeding
    /// </summary>
    public async Task WaitBeforeNextRequestAsync(CancellationToken cancellationToken = default)
    {
        var delay = _rateLimiter.GetNextDelayMs();
        OnStatus($"Waiting {delay / 1000}s before next request...");
        await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the current delay description for display
    /// </summary>
    public string GetCurrentDelayInfo() => _rateLimiter.GetDelayDescription();

    private static bool IsRateLimitError(Exception ex)
    {
        var message = ex.Message.ToLowerInvariant();
        return message.Contains("429") ||
               message.Contains("rate limit") ||
               message.Contains("too many requests") ||
               message.Contains("blocked") ||
               message.Contains("captcha");
    }

    private void OnStatus(string message) => StatusChanged?.Invoke(this, message);
    private void OnError(string message) => ErrorOccurred?.Invoke(this, message);
}

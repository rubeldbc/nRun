namespace nRun.Services;

/// <summary>
/// Provides rate limiting with configurable base delay and random jitter
/// to ensure responsible resource consumption
/// </summary>
public class RateLimiter
{
    private readonly Random _random = new();
    private readonly object _lock = new();

    private int _baseDelaySeconds;
    private int _jitterMaxSeconds;
    private int _currentBackoffMultiplier = 1;
    private const int MaxBackoffMultiplier = 32;

    public int BaseDelaySeconds
    {
        get => _baseDelaySeconds;
        set => _baseDelaySeconds = Math.Max(1, value);
    }

    public int JitterMaxSeconds
    {
        get => _jitterMaxSeconds;
        set => _jitterMaxSeconds = Math.Max(0, value);
    }

    public RateLimiter(int baseDelaySeconds = 10, int jitterMaxSeconds = 5)
    {
        BaseDelaySeconds = baseDelaySeconds;
        JitterMaxSeconds = jitterMaxSeconds;
    }

    /// <summary>
    /// Calculates the next delay with jitter: base + random(0, jitterMax)
    /// </summary>
    public int GetNextDelayMs()
    {
        lock (_lock)
        {
            var jitter = _jitterMaxSeconds > 0 ? _random.Next(0, _jitterMaxSeconds * 1000) : 0;
            var baseMs = _baseDelaySeconds * 1000;
            return (baseMs + jitter) * _currentBackoffMultiplier;
        }
    }

    /// <summary>
    /// Waits for the calculated delay with jitter
    /// </summary>
    public async Task WaitAsync(CancellationToken cancellationToken = default)
    {
        var delayMs = GetNextDelayMs();
        await Task.Delay(delayMs, cancellationToken);
    }

    /// <summary>
    /// Applies exponential backoff after encountering a rate limit or error.
    /// Call this when you receive a 429 or similar rate limit response.
    /// </summary>
    public void ApplyBackoff()
    {
        lock (_lock)
        {
            _currentBackoffMultiplier = Math.Min(_currentBackoffMultiplier * 2, MaxBackoffMultiplier);
        }
    }

    /// <summary>
    /// Resets backoff multiplier after a successful request
    /// </summary>
    public void ResetBackoff()
    {
        lock (_lock)
        {
            _currentBackoffMultiplier = 1;
        }
    }

    /// <summary>
    /// Gets the current backoff multiplier for logging/display
    /// </summary>
    public int CurrentBackoffMultiplier
    {
        get { lock (_lock) { return _currentBackoffMultiplier; } }
    }

    /// <summary>
    /// Gets a human-readable description of the current delay settings
    /// </summary>
    public string GetDelayDescription()
    {
        var minDelay = _baseDelaySeconds * _currentBackoffMultiplier;
        var maxDelay = (_baseDelaySeconds + _jitterMaxSeconds) * _currentBackoffMultiplier;

        if (_currentBackoffMultiplier > 1)
            return $"{minDelay}-{maxDelay}s (backoff x{_currentBackoffMultiplier})";

        return $"{minDelay}-{maxDelay}s";
    }
}

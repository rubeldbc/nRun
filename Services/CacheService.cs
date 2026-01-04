using System.Collections.Concurrent;

namespace nRun.Services;

/// <summary>
/// Simple in-memory cache service with TTL (Time-To-Live) support
/// Used to cache frequently accessed data like site lists and profiles
/// </summary>
public class CacheService
{
    private readonly ConcurrentDictionary<string, CacheEntry> _cache = new();
    private readonly TimeSpan _defaultTtl = TimeSpan.FromMinutes(5);

    private class CacheEntry
    {
        public object Value { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    }

    /// <summary>
    /// Get a cached value or compute it if not present/expired
    /// </summary>
    public T GetOrSet<T>(string key, Func<T> factory, TimeSpan? ttl = null)
    {
        if (_cache.TryGetValue(key, out var entry) && !entry.IsExpired)
        {
            return (T)entry.Value;
        }

        var value = factory();
        var expiration = DateTime.UtcNow.Add(ttl ?? _defaultTtl);

        _cache[key] = new CacheEntry { Value = value!, ExpiresAt = expiration };
        return value;
    }

    /// <summary>
    /// Get a cached value or compute it asynchronously if not present/expired
    /// </summary>
    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? ttl = null)
    {
        if (_cache.TryGetValue(key, out var entry) && !entry.IsExpired)
        {
            return (T)entry.Value;
        }

        var value = await factory().ConfigureAwait(false);
        var expiration = DateTime.UtcNow.Add(ttl ?? _defaultTtl);

        _cache[key] = new CacheEntry { Value = value!, ExpiresAt = expiration };
        return value;
    }

    /// <summary>
    /// Try to get a cached value
    /// </summary>
    public bool TryGet<T>(string key, out T? value)
    {
        value = default;
        if (_cache.TryGetValue(key, out var entry) && !entry.IsExpired)
        {
            value = (T)entry.Value;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Set a value in the cache
    /// </summary>
    public void Set<T>(string key, T value, TimeSpan? ttl = null)
    {
        var expiration = DateTime.UtcNow.Add(ttl ?? _defaultTtl);
        _cache[key] = new CacheEntry { Value = value!, ExpiresAt = expiration };
    }

    /// <summary>
    /// Invalidate a specific cache entry
    /// </summary>
    public void Invalidate(string key)
    {
        _cache.TryRemove(key, out _);
    }

    /// <summary>
    /// Invalidate all entries matching a prefix
    /// </summary>
    public void InvalidatePrefix(string prefix)
    {
        var keysToRemove = _cache.Keys.Where(k => k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)).ToList();
        foreach (var key in keysToRemove)
        {
            _cache.TryRemove(key, out _);
        }
    }

    /// <summary>
    /// Clear all cached entries
    /// </summary>
    public void Clear()
    {
        _cache.Clear();
    }

    /// <summary>
    /// Remove expired entries (for periodic cleanup)
    /// </summary>
    public void CleanupExpired()
    {
        var expiredKeys = _cache.Where(kvp => kvp.Value.IsExpired).Select(kvp => kvp.Key).ToList();
        foreach (var key in expiredKeys)
        {
            _cache.TryRemove(key, out _);
        }
    }

    /// <summary>
    /// Cache key constants
    /// </summary>
    public static class Keys
    {
        public const string AllSites = "sites:all";
        public const string ActiveSites = "sites:active";
        public const string AllTkProfiles = "tk:profiles:all";
        public const string ActiveTkProfiles = "tk:profiles:active";
        public const string AllFbProfiles = "fb:profiles:all";
        public const string ActiveFbProfiles = "fb:profiles:active";
    }
}

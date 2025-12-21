using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;

namespace CoreBuilder.Infrastructure.Caching;

/// <summary>
/// In-memory fallback implementation (when Redis is not available)
/// </summary>
public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<MemoryCacheService> _logger;
    private readonly TimeSpan _defaultExpiration;
    private readonly HashSet<string> _keys = new();
    private readonly object _lockObject = new();

    public MemoryCacheService(
        IMemoryCache cache,
        ILogger<MemoryCacheService> logger,
        TimeSpan? defaultExpiration = null)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _defaultExpiration = defaultExpiration ?? TimeSpan.FromMinutes(30);
    }

    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            return Task.FromResult(_cache.Get<T>(key));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cache key: {Key}", key);
            return Task.FromResult<T?>(default);
        }
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var exp = expiration ?? _defaultExpiration;
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = exp
            };

            _cache.Set(key, value, options);

            lock (_lockObject)
            {
                _keys.Add(key);
            }

            _logger.LogDebug("Memory cache set: {Key}, Expiration: {Expiration}", key, exp);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting cache key: {Key}", key);
        }

        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            _cache.Remove(key);
            lock (_lockObject)
            {
                _keys.Remove(key);
            }
            _logger.LogDebug("Memory cache removed: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing cache key: {Key}", key);
        }

        return Task.CompletedTask;
    }

    public Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
    {
        try
        {
            var regex = new Regex(pattern.Replace("*", ".*"));
            List<string> keysToRemove;

            lock (_lockObject)
            {
                keysToRemove = _keys.Where(k => regex.IsMatch(k)).ToList();
            }

            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
            }

            lock (_lockObject)
            {
                foreach (var key in keysToRemove)
                {
                    _keys.Remove(key);
                }
            }

            _logger.LogDebug("Memory cache pattern removed: {Pattern}, Count: {Count}", pattern, keysToRemove.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing cache pattern: {Pattern}", pattern);
        }

        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            return Task.FromResult(_cache.TryGetValue(key, out _));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking cache key exists: {Key}", key);
            return Task.FromResult(false);
        }
    }

    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue<T>(key, out var cached))
        {
            _logger.LogDebug("Memory cache hit: {Key}", key);
            return cached!;
        }

        _logger.LogDebug("Memory cache miss: {Key}", key);
        var value = await factory();
        await SetAsync(key, value, expiration, cancellationToken);
        return value;
    }
}

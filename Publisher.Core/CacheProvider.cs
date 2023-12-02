using Contracts.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Publisher.Interface;

namespace Publisher.Core;

public class CacheProvider : ICacheProvider
{
    private readonly CachingConfiguration _cachingConfiguration;
    private readonly IMemoryCache _memoryCache;
    
    protected CacheProvider(IMemoryCache memoryCache, IOptions<CachingConfiguration> cachingConfiguration)
    {
        _memoryCache = memoryCache;
        _cachingConfiguration = cachingConfiguration.Value;
    }

    /// <summary>
    ///     Get the value from the cache
    /// </summary>
    /// <returns></returns>
    public object? GetValue(object key)
    {
        return _memoryCache.TryGetValue(key, out var result) ? result : default;
    }

    /// <summary>
    ///     Set the value to the cache
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cacheEntry"></param>
    public void SetValue(object key, object cacheEntry)
    {
        var cacheEntryOptions =
            new MemoryCacheEntryOptions().SetSlidingExpiration(
                TimeSpan.FromHours(_cachingConfiguration.CacheExpirationInHours));

        _memoryCache.Set(key, cacheEntry, cacheEntryOptions);
    }

    /// <summary>
    ///     Remove the value from the cache
    /// </summary>
    /// <param name="key"></param>
    public void RemoveValue(object key)
    {
        _memoryCache.Remove(key);
    }

}
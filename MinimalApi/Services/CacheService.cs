using Microsoft.Extensions.Caching.Memory;
using MinimalApi.Services.Abstractions;
using MinimalApi.Shared;

namespace MinimalApi.Services;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    public CacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public T? Get<T>(string key) => _memoryCache.Get<T>(key);

    public void Set<T>(string key, T value, CacheOptions options) =>
        _memoryCache.Set(key, value, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow,
            SlidingExpiration = options.SlidingExpiration
        });
}

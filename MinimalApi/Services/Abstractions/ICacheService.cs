using MinimalApi.Shared;

namespace MinimalApi.Services.Abstractions;

public interface ICacheService
{
    T? Get<T>(string key);
    void Set<T>(string key, T value, CacheOptions options);
}
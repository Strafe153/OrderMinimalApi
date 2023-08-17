using Core.Shared;

namespace Core.Interfaces.Services;

public interface ICacheService
{
    T? Get<T>(string key);
    void Set<T>(string key, T value, CacheOptions options);
}
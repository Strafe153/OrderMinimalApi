namespace MinimalApi.Configurations.Models;

public class CacheOptions
{
    public const string SectionName = "Cache";

    public TimeSpan Expiration { get; init; }
}

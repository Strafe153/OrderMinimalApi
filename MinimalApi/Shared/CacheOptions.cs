namespace MinimalApi.Shared;

public class CacheOptions
{
    public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
    public TimeSpan? SlidingExpiration { get; set; }
}

namespace MinimalApi.Configurations;

public static class OutputCacheConfiguration
{
    public static void ConfigureOutputCache(this IServiceCollection services)
    {
        services.AddOutputCache(options =>
        {
            options.AddBasePolicy(policy =>
            {
                policy.Expire(TimeSpan.FromSeconds(30));
            });
        });
    }
}

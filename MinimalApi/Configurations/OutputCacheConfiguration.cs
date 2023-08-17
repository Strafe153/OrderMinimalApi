namespace MinimalApi.Configurations;

public static class OutputCacheConfiguration
{
    public static void ConfigureOutputCache(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOutputCache(options =>
            {
                options.AddBasePolicy(policy =>
                {
                    policy.Expire(TimeSpan.FromSeconds(30));
                });
            })
            .AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = "OrderMinimalApi";
                options.Configuration = configuration.GetConnectionString("RedisConnection");
            });
    }
}

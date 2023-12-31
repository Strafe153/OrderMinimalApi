using Core.Shared;

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
                options.InstanceName = typeof(Program).Assembly.GetName().Name;
                options.Configuration = configuration.GetConnectionString(ConnectionStringConstants.RedisConnection);
            });
    }
}

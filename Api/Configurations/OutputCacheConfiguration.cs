using Api.Configurations.Models;
using Api.Constants;

namespace Api.Configurations;

public static class OutputCacheConfiguration
{
	public static void ConfigureOutputCache(this IServiceCollection services, IConfiguration configuration) =>
		services
			.AddOutputCache(options =>
			{
				options.AddBasePolicy(policy =>
				{
					var cacheOptions = configuration.GetSection(CacheOptions.SectionName).Get<CacheOptions>()!;
					policy.Expire(cacheOptions.Expiration);
				});
			})
			.AddStackExchangeRedisOutputCache(options =>
			{
				options.InstanceName = typeof(Program).Assembly.GetName().Name;
				options.Configuration = configuration.GetConnectionString(ConnectionStringConstants.RedisConnection);
			});
}

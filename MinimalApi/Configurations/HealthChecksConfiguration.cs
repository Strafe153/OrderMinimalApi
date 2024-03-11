using Domain.Shared;
using Domain.Shared.Constants;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MinimalApi.HealthChecks;

namespace MinimalApi.Configurations;

public static class HealthChecksConfiguration
{
	public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
	{
		var databaseOptions = configuration.GetSection(OrderDatabaseOptions.SectionName).Get<OrderDatabaseOptions>()!;

		services
			.AddHealthChecks()
			.AddMongoDb(databaseOptions.ConnectionString)
			.AddRedis(configuration.GetConnectionString(ConnectionStringConstants.RedisConnection)!)
			.AddCheck<SeqHealthCheck>("Seq");
	}

	public static void UseHealthChecks(this WebApplication application) =>
		application.MapHealthChecks("/_health", new HealthCheckOptions
		{
			ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
		});
}

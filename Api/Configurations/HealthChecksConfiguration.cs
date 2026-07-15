using Api.Configurations.Models;
using Api.Constants;
using Api.HealthChecks.Seq;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Api.Configurations;

public static class HealthChecksConfiguration
{
	public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
	{
		var seqOptions = configuration.GetSection(SeqOptions.SectionName).Get<SeqOptions>()!;
        services.AddHttpClient<SeqClient>(o => o.BaseAddress = new Uri(seqOptions.ConnectionString));

		var databaseOptions = configuration.GetSection(DatabaseOptions.SectionName).Get<DatabaseOptions>()!;

		services
			.AddHealthChecks()
			.AddMongoDb(databaseOptions.ConnectionString)
			.AddRedis(configuration.GetConnectionString(ConnectionStringConstants.RedisConnection)!)
			.AddCheck<SeqHealthCheck>("Seq");
	}

	public static void UseHealthChecks(this WebApplication application) =>
		application.MapHealthChecks(
			"/health",
			new HealthCheckOptions
			{
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
			});
}

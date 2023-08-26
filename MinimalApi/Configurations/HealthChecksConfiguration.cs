using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MinimalApi.HealthChecks;

namespace MinimalApi.Configurations;

public static class HealthChecksConfiguration
{
    public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHealthChecks()
            .AddMongoDb(configuration.GetSection("OrderDatabase:ConnectionString").Value!)
            .AddRedis(configuration.GetConnectionString("RedisConnection")!)
            .AddCheck<SeqHealthCheck>("Seq");
    }

    public static void UseHealthChecks(this WebApplication application)
    {
        application.MapHealthChecks("/_health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }
}

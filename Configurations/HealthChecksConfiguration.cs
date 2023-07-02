using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace OrderMinimalApi.Configurations;

public static class HealthChecksConfiguration
{
    public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHealthChecks()
            .AddMongoDb(configuration.GetSection("OrderDatabase:ConnectionString").Value!);
    }

    public static void UseHealthChecks(this WebApplication application)
    {
        application.MapHealthChecks("/_health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }
}

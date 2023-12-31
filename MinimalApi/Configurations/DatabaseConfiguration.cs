using Core.Shared;

namespace MinimalApi.Configurations;

public static class DatabaseConfiguration
{
    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OrderDatabaseOptions>(configuration.GetSection(OrderDatabaseOptions.SectionName));
    }
}

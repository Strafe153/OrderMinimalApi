using OrderMinimalApi.Shared;

namespace OrderMinimalApi.Configurations;

public static class DatabaseConfiguration
{
    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OrderDatabaseSettings>(configuration.GetSection("OrderDatabase"));
    }
}

using MinimalApi.Services;
using MinimalApi.Services.Abstractions;

namespace MinimalApi.Configurations;

public static class ServicesConfiguration
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IOrderService, OrderService>();
    }
}

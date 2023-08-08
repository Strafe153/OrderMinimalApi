using Core.Interfaces.Services;
using Core.Services;
using MinimalApi.Services;

namespace MinimalApi.Configurations;

public static class ServicesConfiguration
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IOrderService, OrderService>();
    }
}

using MinimalApi.Services;
using MinimalApi.Services.Abstractions;

namespace MinimalApi.Configurations;

public static class CustomServicesConfiguration
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
    }
}

using OrderMinimalApi.Services;

namespace OrderMinimalApi.Configurations;

public static class CustomServicesConfiguration
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
    }
}

using Application.Services.Implementations;
using Application.Services.Interfaces;

namespace MinimalApi.Configurations;

public static class ServicesConfiguration
{
	public static void AddCustomServices(this IServiceCollection services) =>
		services.AddScoped<IOrderService, OrderService>();
}

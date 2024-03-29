﻿using Application.Services;
using Domain.Interfaces.Services;

namespace MinimalApi.Configurations;

public static class ServicesConfiguration
{
	public static void AddCustomServices(this IServiceCollection services) =>
		services.AddScoped<IOrderService, OrderService>();
}

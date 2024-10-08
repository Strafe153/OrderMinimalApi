﻿using Domain.Interfaces.Repositories;
using DataAccess.Repositories;

namespace MinimalApi.Configurations;

public static class RepositoriesConfiguration
{
	public static void AddRepositories(this IServiceCollection services) =>
		services.AddScoped<IOrdersRepository, OrdersRepository>();
}

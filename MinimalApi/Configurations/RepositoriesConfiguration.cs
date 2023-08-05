using MinimalApi.Repositories;
using MinimalApi.Repositories.Abstractions;

namespace MinimalApi.Configurations;

public static class RepositoriesConfiguration
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IOrderRepository, OrderRepository>();
    }
}

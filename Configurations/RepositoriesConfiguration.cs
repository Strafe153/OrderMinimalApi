using OrderMinimalApi.Repositories;

namespace OrderMinimalApi.Configurations;

public static class RepositoriesConfiguration
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IOrderRepository, OrderRepository>();
    }
}

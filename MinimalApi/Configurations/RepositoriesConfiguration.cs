using Core.Interfaces.Repositories;
using DataAccess.Repositories;

namespace MinimalApi.Configurations;

public static class RepositoriesConfiguration
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IOrderRepository, OrderRepository>();
    }
}

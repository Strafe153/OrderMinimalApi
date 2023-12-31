using Core.Shared;
using MinimalApi.HttpClients;

namespace MinimalApi.Configurations;

public static class HttpClientsConfiguration
{
    public static void ConfigureHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<SeqClient>(options =>
            options.BaseAddress = new Uri(configuration.GetConnectionString(ConnectionStringConstants.SeqConnection)!));
    }
}

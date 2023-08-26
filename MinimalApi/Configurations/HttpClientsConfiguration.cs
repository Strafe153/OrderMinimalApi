using MinimalApi.HttpClients;
using Polly;

namespace MinimalApi.Configurations;

public static class HttpClientsConfiguration
{
    public static void ConfigureHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHttpClient<SeqClient>(options =>
            {
                options.BaseAddress = new Uri(configuration.GetConnectionString("SeqConnection")!);
            })
            .AddTransientHttpErrorPolicy(policy =>
                policy.WaitAndRetryAsync(3, retryCount => TimeSpan.FromSeconds(Math.Pow(1.5, retryCount))));
    }
}

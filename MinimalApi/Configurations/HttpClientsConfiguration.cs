using Application.HttpClients;
using MinimalApi.Configurations.Models;

namespace MinimalApi.Configurations;

public static class HttpClientsConfiguration
{
    public static void ConfigureHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        var seqOptions = configuration.GetSection(SeqOptions.SectionName).Get<SeqOptions>()!;

        services.AddHttpClient<SeqClient>(o => o.BaseAddress = new Uri(seqOptions.ConnectionString));
    }
}
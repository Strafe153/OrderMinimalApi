using Domain.Shared.Constants;
using Flurl.Http;
using Flurl.Http.Configuration;
using MinimalApi.Configurations.Models;

namespace MinimalApi.Configurations;

public static class FlurlConfiguration
{
	public static void ConfigureFlurl(this IServiceCollection services, IConfiguration configuration)
	{
        var seqOptions = configuration.GetSection(SeqOptions.SectionName).Get<SeqOptions>()!;

        services.AddSingleton(p =>
            new FlurlClientCache()
                .Add(FlurlConstants.SeqClient, seqOptions.ConnectionString, b => b.WithTimeout(TimeSpan.FromSeconds(.1))));
    }
}

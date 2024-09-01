using MinimalApi.Configurations.Models;
using MinimalApi.HttpClients;

namespace MinimalApi.Configurations;

public static class HttpClientsConfiguration
{
	public static void ConfigureHttpClients(this IServiceCollection services, IConfiguration configuration) =>
		services.AddHttpClient<SeqClient>(options =>
		{
			var seqOptions = configuration.GetSection(SeqOptions.SectionName).Get<SeqOptions>()!;

			options.BaseAddress = new Uri(seqOptions.ConnectionString);
			options.Timeout = TimeSpan.FromSeconds(.1);
		});
}

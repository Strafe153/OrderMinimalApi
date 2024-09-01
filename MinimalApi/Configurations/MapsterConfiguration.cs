using Mapster;
using MapsterMapper;
using System.Reflection;

namespace MinimalApi.Configurations;

public static class MapsterConfiguration
{
	public static void ConfigureMapster(this IServiceCollection services)
	{
		var config = TypeAdapterConfig.GlobalSettings;
		config.Scan(Assembly.GetExecutingAssembly());

		var mapper = new Mapper(config);
		services.AddSingleton<IMapper>(mapper);
	}
}

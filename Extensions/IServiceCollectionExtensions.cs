using Mapster;
using MapsterMapper;
using System.Reflection;

namespace OrderMinimalApi.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddMapster(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        var mapper = new Mapper(config);
        services.AddSingleton<IMapper>(mapper);
    }
}

using MinimalApi.Configurations.Models;
using MongoDB.Driver;

namespace MinimalApi.Configurations;

public static class DatabaseConfiguration
{
	public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
	{
        var mongoOptions = configuration.GetSection(DatabaseOptions.SectionName).Get<DatabaseOptions>()!;
        var mongoClient = new MongoClient(mongoOptions.ConnectionString).GetDatabase(mongoOptions.DatabaseName);

        services.AddSingleton(mongoClient);
    }
}

using Api.Configurations.Models;
using MongoDB.Driver;

namespace Api.Configurations;

public static class DatabaseConfiguration
{
	public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
	{
        var mongoOptions = configuration.GetSection(DatabaseOptions.SectionName).Get<DatabaseOptions>()!;
        var mongoClient = new MongoClient(mongoOptions.ConnectionString).GetDatabase(mongoOptions.DatabaseName);

        services.AddSingleton(mongoClient);
    }
}

using Microsoft.Extensions.Options;
using MinimalApi.Configurations.Models;
using MinimalApi.HostedServices;
using MongoDB.Driver;
using OpenIddict.MongoDb;
using OpenIddict.MongoDb.Models;

namespace MinimalApi.Configurations;

public static class OpenIddictConfiguration
{
    public static void ConfigureOpenIddict(this IServiceCollection services, IConfiguration configuration)
    {
        var openIddictOptionsSection = configuration.GetSection(OpenIddictOptions.SectionName);
        var openIddictOptions = openIddictOptionsSection.Get<OpenIddictOptions>()!;

        services.Configure<OpenIddictOptions>(openIddictOptionsSection);

        services
            .AddOpenIddict()
            .AddCore(options => options.UseMongoDb())
            .AddServer(options => options
                .AllowClientCredentialsFlow()
                .SetTokenEndpointUris($"/{openIddictOptions.TokenEndpoint}")
                .AddDevelopmentEncryptionCertificate()
                .AddDevelopmentSigningCertificate()
                .UseAspNetCore()
                .EnableTokenEndpointPassthrough())
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });

        services.AddHostedService<OpenIddictHostedService>();
    }

    public static async Task CreateIndexes(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<IOpenIddictMongoDbContext>();
        var options = scope.ServiceProvider.GetRequiredService<IOptionsMonitor<OpenIddictMongoDbOptions>>().CurrentValue;

        var database = await context.GetDatabaseAsync(CancellationToken.None);

        await Task.WhenAll(
            CreateApplicationIndexes(database, options),
            CreateAuthorizationIndexes(database, options),
            CreateScopeIndexes(database, options),
            CreateTokenIndexes(database, options));
    }

    private static Task<IEnumerable<string>> CreateApplicationIndexes(IMongoDatabase database, OpenIddictMongoDbOptions options)
    {
        var applications = database.GetCollection<OpenIddictMongoDbApplication>(options.ApplicationsCollectionName);

        return applications.Indexes.CreateManyAsync([
            new CreateIndexModel<OpenIddictMongoDbApplication>(
                Builders<OpenIddictMongoDbApplication>.IndexKeys.Ascending(a => a.ClientId),
                new()
                {
                    Unique = true
                }),
            new CreateIndexModel<OpenIddictMongoDbApplication>(
                Builders<OpenIddictMongoDbApplication>.IndexKeys.Ascending(a => a.PostLogoutRedirectUris),
                new()
                {
                    Background = true
                }),
            new CreateIndexModel<OpenIddictMongoDbApplication>(
                Builders<OpenIddictMongoDbApplication>.IndexKeys.Ascending(a => a.RedirectUris),
                new CreateIndexOptions
                {
                    Background = true
                })
        ]);
    }

    private static Task<string> CreateAuthorizationIndexes(IMongoDatabase database, OpenIddictMongoDbOptions options)
    {
        var authorizations = database.GetCollection<OpenIddictMongoDbAuthorization>(options.AuthorizationsCollectionName);

        return authorizations.Indexes.CreateOneAsync(
            new CreateIndexModel<OpenIddictMongoDbAuthorization>(
                Builders<OpenIddictMongoDbAuthorization>.IndexKeys
                    .Ascending(a => a.ApplicationId)
                    .Ascending(a => a.Scopes)
                    .Ascending(a => a.Status)
                    .Ascending(a => a.Subject)
                    .Ascending(a => a.Type),
                new()
                {
                    Background = true
                }));
    }

    private static Task<string> CreateScopeIndexes(IMongoDatabase database, OpenIddictMongoDbOptions options)
    {
        var scopes = database.GetCollection<OpenIddictMongoDbScope>(options.ScopesCollectionName);

        return scopes.Indexes.CreateOneAsync(new CreateIndexModel<OpenIddictMongoDbScope>(
            Builders<OpenIddictMongoDbScope>.IndexKeys.Ascending(s => s.Name),
            new CreateIndexOptions
            {
                Unique = true
            }));
    }

    private static Task<IEnumerable<string>> CreateTokenIndexes(IMongoDatabase database, OpenIddictMongoDbOptions options)
    {
        var tokens = database.GetCollection<OpenIddictMongoDbToken>(options.TokensCollectionName);

        return tokens.Indexes.CreateManyAsync([
            new(
                Builders<OpenIddictMongoDbToken>.IndexKeys.Ascending(t => t.ReferenceId),
                new CreateIndexOptions<OpenIddictMongoDbToken>
                {
                    PartialFilterExpression = Builders<OpenIddictMongoDbToken>.Filter.Exists(t => t.ReferenceId),
                    Unique = true
                }),
            new(
                Builders<OpenIddictMongoDbToken>.IndexKeys.Ascending(t => t.AuthorizationId),
                new CreateIndexOptions<OpenIddictMongoDbToken>()
                {
                    PartialFilterExpression = Builders<OpenIddictMongoDbToken>.Filter.Exists(t => t.AuthorizationId),
                }),
            new(
                Builders<OpenIddictMongoDbToken>.IndexKeys
                    .Ascending(t => t.ApplicationId)
                    .Ascending(t => t.Status)
                    .Ascending(t => t.Subject)
                    .Ascending(t => t.Type),
                new()
                {
                    Background = true
                })
        ]);
    }
}

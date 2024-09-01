using Microsoft.Extensions.Options;
using MinimalApi.Configurations.Models;
using OpenIddict.Abstractions;

namespace MinimalApi.HostedServices;

public class OpenIddictHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public OpenIddictHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        var clientOptions = scope.ServiceProvider.GetRequiredService<IOptions<OpenIddictOptions>>().Value;

        var client = await manager.FindByClientIdAsync(clientOptions.ClientId, cancellationToken);

        if (client is null)
        {
            OpenIddictApplicationDescriptor descriptor = new()
            {
                ClientId = clientOptions.ClientId,
                ClientSecret = clientOptions.ClientSecret,
                DisplayName = clientOptions.DisplayName
            };

            foreach (var permission in clientOptions.Permissions)
            {
                descriptor.Permissions.Add(permission);
            }

            await manager.CreateAsync(descriptor, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
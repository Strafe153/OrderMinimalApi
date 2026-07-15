using System.Security.Claims;
using Api.Configurations.Models;
using Api.Exceptions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Api.Features.Authorization.Token;

public class TokenEndpoint
{
    public static void Map(WebApplication app)
    {
        var openIddictOptions = app.Configuration
            .GetSection(OpenIddictOptions.SectionName)
            .Get<OpenIddictOptions>()!;

        app.MapPost(openIddictOptions.TokenEndpoint, Handle);
    }

    public static async Task<SignInHttpResult> Handle(
        [FromServices] IOpenIddictApplicationManager openIddictAppManager,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var request = context.GetOpenIddictServerRequest();

        if (request?.ClientId is not null && request.IsClientCredentialsGrantType())
        {
            var application = await openIddictAppManager.FindByClientIdAsync(request.ClientId, cancellationToken)
                ?? throw new OpenIddictApplicationNotFoundException("The application cannot be found.");

            ClaimsIdentity identity = new(TokenValidationParameters.DefaultAuthenticationType, Claims.Name, Claims.Role);

            var clientId = await openIddictAppManager.GetClientIdAsync(application, cancellationToken);
            var displayName = await openIddictAppManager.GetDisplayNameAsync(application, cancellationToken);

            identity
                .SetClaim(Claims.Subject, clientId)
                .SetClaim(Claims.Name, displayName)
                .SetDestinations(_ => [Destinations.AccessToken]);

            ClaimsPrincipal claimsPrincipal = new(identity);

            return TypedResults.SignIn(
                claimsPrincipal,
                authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        throw new GrantNotImplementedException("The specified grant is not implemented.");
    }
}

using Application.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Configurations.Models;
using OpenIddict.Server.AspNetCore;

namespace MinimalApi.Endpoints;

public static class AuthorizationEndpoints
{
    public static void MapAuthorizationEndpoints(this WebApplication app)
    {
        var openIddictOptions = app.Configuration
            .GetSection(OpenIddictOptions.SectionName)
            .Get<OpenIddictOptions>()!;

        app.MapPost(openIddictOptions.TokenEndpoint, Token);
    }

    public static async Task<SignInHttpResult> Token(
        [FromServices] IAuthorizationService authorizationService,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var claimsPrincipal = await authorizationService.GetClaimsPrincipalAsync(context, cancellationToken);

        return TypedResults.SignIn(
            claimsPrincipal,
            authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
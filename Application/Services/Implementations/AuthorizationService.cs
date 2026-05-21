using Application.Services.Interfaces;
using Domain.Exceptions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Application.Services.Implementations;

public class AuthorizationService(IOpenIddictApplicationManager openIddictAppManager) : IAuthorizationService
{
    public async Task<ClaimsPrincipal> GetClaimsPrincipalAsync(
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
                .SetClaim(Claims.Name, displayName);

            identity.SetDestinations(_ => [Destinations.AccessToken]);

            return new(identity);
        }

        throw new GrantNotImplementedException("The specified grant is not implemented.");
    }
}

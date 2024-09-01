using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Services.Interfaces;

public interface IAuthorizationService
{
    Task<ClaimsPrincipal> GetClaimsPrincipalAsync(HttpContext context, CancellationToken cancellationToken);
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using RentIt.Infrastructure.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RentIt.Infrastructure.Authorization;
internal class CustomClaimTransformation : IClaimsTransformation
{
    private readonly IServiceProvider m_ServiceProvider;

    public CustomClaimTransformation(IServiceProvider serviceProvider)
    {
        m_ServiceProvider = serviceProvider;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.HasClaim(claim => claim.Type == ClaimTypes.Role) &&
            principal.HasClaim(claim => claim.Type == JwtRegisteredClaimNames.Sub))
        {
            return principal;
        }

        var scope = m_ServiceProvider.CreateScope();
        var authorizationService = scope.ServiceProvider.GetRequiredService<AuthorizationService>();

        var identityId = principal.GetIdentityId();

        var userRoles = await authorizationService.GetRolesForUserAsync(identityId);

        var claimIdentity = new ClaimsIdentity();

        claimIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, userRoles.Id.ToString()));

        foreach (var role in userRoles.Roles)
        {
            claimIdentity.AddClaim(
                new Claim(
                    ClaimTypes.Role,
                    role.Name));
        }

        principal.AddIdentity(claimIdentity);

        return principal;
    }
}

using Microsoft.EntityFrameworkCore;
using RentIt.Domain.Users;

namespace RentIt.Infrastructure.Authorization;
internal sealed class AuthorizationService
{
    private readonly ApplicationDbContext m_DbContext;

    public AuthorizationService(ApplicationDbContext dbContext)
    {
        m_DbContext = dbContext;
    }

    public async Task<UserRoleResponse> GetRolesForUserAsync(string identityId)
    {
        var roles = await m_DbContext.Set<User>()
            .Where(user => user.IdentityId == identityId)
            .Select(user => new UserRoleResponse
            {
                Id = user.Id,
                Roles = user.Roles.ToList()
            }).FirstAsync();

        return roles;
    }

    public async Task<HashSet<string>> GetPermissionsForUserAsync(string identityId)
    {
        var permissions = await m_DbContext.Set<User>()
            .Where(u => u.IdentityId == identityId)
            .SelectMany(u => u.Roles.Select(r => r.Permissions))
            .FirstAsync();

        return permissions.Select(p => p.Name).ToHashSet();
    }
}

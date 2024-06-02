using RentIt.Domain.Users;

namespace RentIt.Infrastructure.Authorization;

public sealed class UserRoleResponse
{
    public Guid Id { get; init; }

    public List<Role> Roles { get; init; } = [];
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentIt.Domain.Users;

namespace RentIt.Infrastructure.Configurations;
internal class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permissions");

        builder.HasKey(rolePermission => new
        {
            rolePermission.RoleId,
            rolePermission.PermissionId
        });

        builder.HasData(
            new RolePermission
            {
                RoleId = Role.Registered.Id,
                PermissionId = Permission.UsersRead.Id
            });
    }
}
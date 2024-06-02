﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentIt.Domain.Users;

namespace RentIt.Infrastructure.Configurations;
internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(role => role.Id);

        builder.HasMany(role => role.Users)
            .WithMany(user => user.Roles);

        builder.HasData(Role.Registered);
    }
}

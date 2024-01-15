using Abd.CleanArchitecture.Kernel.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.JameahHub.Configurations;

public class RolePermissionsConfiguration : IEntityTypeConfiguration<ApplicationRolePermission<Guid>>
{
    public void Configure(EntityTypeBuilder<ApplicationRolePermission<Guid>> builder)
    {
        builder.HasIndex(e => e.PermissionId, "IX_RolePermissions_PermissionId");
        builder.HasIndex(e => e.RoleId, "IX_RolePermissions_RoleId");
        builder.HasOne(d => d.Permission).WithMany(p => p.RolePermissions).HasForeignKey(d => d.PermissionId);
        builder.HasOne(d => d.Role).WithMany(p => p.RolePermissions).HasForeignKey(d => d.RoleId);
    }
}

public class ApplicationUserGroupConfiguration : IEntityTypeConfiguration<ApplicationUserGroup<Guid>>
{
    public void Configure(EntityTypeBuilder<ApplicationUserGroup<Guid>> builder)
    {
        builder.HasOne(d => d.Group).WithMany(p => p.UserGroups).HasForeignKey(d => d.GroupId);
        builder.HasOne(d => d.User).WithMany(p => p.UserGroups).HasForeignKey(d => d.UserId);
    }
}
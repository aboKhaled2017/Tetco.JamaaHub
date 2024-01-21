using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataPersistence.JameahHub.Configurations;

public class ApplicationRolePermissionsConfiguration : IEntityTypeConfiguration<ApplicationRolePermission<Guid>>
{
    public void Configure(EntityTypeBuilder<ApplicationRolePermission<Guid>> builder)
    {
        builder.ToTable("RolePermissions", "Identity");

        builder.HasIndex(e => e.PermissionId, "IX_RolePermissions_PermissionId");
        builder.HasIndex(e => e.RoleId, "IX_RolePermissions_RoleId");
        builder.HasOne(d => d.Permission).WithMany(p => p.RolePermissions).HasForeignKey(d => d.PermissionId);
        builder.HasOne(d => d.Role).WithMany(p => p.RolePermissions).HasForeignKey(d => d.RoleId);
    }
}

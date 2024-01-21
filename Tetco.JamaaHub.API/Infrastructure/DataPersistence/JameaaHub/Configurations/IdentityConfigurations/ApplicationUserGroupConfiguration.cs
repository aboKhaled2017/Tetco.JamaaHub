using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataPersistence.JameahHub.Configurations;

public class ApplicationUserGroupConfiguration : IEntityTypeConfiguration<ApplicationUserGroup<Guid>>
{
    public void Configure(EntityTypeBuilder<ApplicationUserGroup<Guid>> builder)
    {
        builder.ToTable("UserGroups", "Identity");
        builder.HasOne(d => d.Group).WithMany(p => p.UserGroups).HasForeignKey(d => d.GroupId);
        builder.HasOne(d => d.User).WithMany(p => p.UserGroups).HasForeignKey(d => d.UserId);
    }
}
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataPersistence.JameahHub.Configurations;

public class ApplicationPermissionConfiguration : IEntityTypeConfiguration<ApplicationPermission<Guid>>
{
    public void Configure(EntityTypeBuilder<ApplicationPermission<Guid>> builder)
    {
        builder.ToTable("Permissions", "Identity");

        builder.Property(p => p.NameAr)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.NameEn)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.DescriptionAr)
            .HasMaxLength(500);

        builder.Property(p => p.DescriptionEn)
            .HasMaxLength(500);
    }
}

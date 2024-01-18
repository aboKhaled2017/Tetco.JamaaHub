using Abd.CleanArchitecture.Kernel.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataPersistence.JameahHub.Configurations;
public class ApplicationGroupConfiguration : IEntityTypeConfiguration<ApplicationGroup<Guid>>
{
    public void Configure(EntityTypeBuilder<ApplicationGroup<Guid>> builder)
    {
        builder.ToTable("Groups","Identity");

        builder.Property(g => g.NameAr)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(g => g.NameEn)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(g => g.DescriptionAr)
            .HasMaxLength(500);

        builder.Property(g => g.DescriptionEn)
            .HasMaxLength(500);
    }
}

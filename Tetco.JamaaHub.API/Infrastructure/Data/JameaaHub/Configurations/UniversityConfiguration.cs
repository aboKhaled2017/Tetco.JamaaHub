using Domain.Entities.Hub;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.JameahHub.Configurations;

public sealed class UniversityConfiguration : IEntityTypeConfiguration<University>
{
    public void Configure(EntityTypeBuilder<University> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(g => g.NameAr)
           .HasMaxLength(200)
           .IsRequired();

        builder.Property(g => g.NameEn)
            .HasMaxLength(200)
            .IsRequired();
    }
}

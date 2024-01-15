using Domain.Entities.Hub;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.JameahHub.Configurations;

public sealed class HubUniversityAgentConfiguration : IEntityTypeConfiguration<HubUniversityAgent>
{
    public void Configure(EntityTypeBuilder<HubUniversityAgent> builder)
    {
        builder.ToTable("Agents");
        builder.HasKey(x => x.Id);

        builder.Property(g => g.NameAr)
           .HasMaxLength(200)
           .IsRequired();

        builder.Property(g => g.NameEn)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(g => g.MackAddresses)
            .HasMaxLength(200)
            .HasConversion(x=>string.Join(",",x),y=>y.Split(",",StringSplitOptions.RemoveEmptyEntries))
            .IsRequired();
    }
}

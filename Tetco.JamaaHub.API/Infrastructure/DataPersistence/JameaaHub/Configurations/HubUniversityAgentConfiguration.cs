using Domain.Entities.Hub.UniversityAgent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.JameahHub.Configurations;

public sealed class HubUniversityAgentConfiguration : IEntityTypeConfiguration<HubAgent>
{
    public void Configure(EntityTypeBuilder<HubAgent> builder)
    {
        builder.ToTable("Agents");
        builder.HasKey(x => x.Id);

        builder.Property(g => g.NameAr)
           .HasMaxLength(200)
           .IsRequired();

        builder.Property(g => g.NameEn)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(g => g.InstituteCode)
          .HasMaxLength(10)
          .IsRequired();

        builder.Property(g => g.MackAddresses)
            .HasMaxLength(200)
            .HasConversion(x=>string.Join(",",x),y=>y.Split(",",StringSplitOptions.RemoveEmptyEntries))
            .IsRequired();

        builder.HasIndex(x => x.InstituteCode);

        builder.Property(x => x.AgentServiceUrl)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.AgentApiAccessKey)
            .HasMaxLength(200)
            .IsRequired();
    }
}

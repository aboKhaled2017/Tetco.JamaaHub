using Domain.Entities.Hub.UniversityAgent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataPersistence.JameahHub.Configurations;
public sealed class HubAgentConfiguration : IEntityTypeConfiguration<HubAgent>
{
    public void Configure(EntityTypeBuilder<HubAgent> builder)
    {
        builder.ToTable("Agents", "Hub");
        builder.HasKey(x => x.Id);

        builder.Property(g => g.NameAr)
           .HasMaxLength(50)
           .IsRequired();

        builder.Property(g => g.NameEn)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(g => g.InstituteCode)
          .HasMaxLength(10)
          .IsRequired();

        builder.Property(g => g.IpAddress)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.InstituteCode);

        builder.Property(x => x.AgentServiceUrl)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.AgentApiAccessKey)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasMany(x => x.SchemaTypes)
            .WithOne(x => x.HubAgent)
            .HasForeignKey(x => x.HubAgentId);
    }
}

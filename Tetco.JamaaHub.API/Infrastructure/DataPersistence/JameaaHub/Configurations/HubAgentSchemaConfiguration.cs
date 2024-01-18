using Domain.Entities.Hub.UniversityAgent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataPersistence.JameahHub.Configurations;

public sealed class HubAgentSchemaConfiguration : IEntityTypeConfiguration<HubAgentSchema>
{
    public void Configure(EntityTypeBuilder<HubAgentSchema> builder)
    {
        builder.ToTable("AgentSchemaTypes", "Hub");

        builder.Property(g => g.Name)
           .HasMaxLength(50)
           .IsRequired();

        builder.Property(g => g.Version)
           .HasMaxLength(20)
           .IsRequired();

        builder.HasOne(x => x.HubAgent)
            .WithMany(x => x.SchemaTypes)
            .HasForeignKey(x => x.HubAgentId);
    }
}

using Domain.Entities.Hub.UniversityAgent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataPersistence.JameahHub.Configurations;

public sealed class HubAgentPackageConfiguration : IEntityTypeConfiguration<HubAgentPackage>
{
    public void Configure(EntityTypeBuilder<HubAgentPackage> builder)
    {
        builder.ToTable("AgentPackages", "Hub");

        builder.HasOne(x => x.HubAgentBatch)
            .WithMany()
            .HasForeignKey(x => x.HubAgentBatchId);

        builder.OwnsOne(x => x.MetaData);
    }
}

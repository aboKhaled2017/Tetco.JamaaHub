using Domain.Entities.Hub.UniversityAgent;
using Domain.Entities.Hub.UniversityAgent.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataPersistence.JameahHub.Configurations;
public sealed class HubAgentBatchConfiguration : IEntityTypeConfiguration<HubAgentBatch>
{
    public void Configure(EntityTypeBuilder<HubAgentBatch> builder)
    {
        builder.ToTable("AgentBatches","Hub");

        builder.HasOne(x => x.HubAgentSchema)
            .WithMany()
            .HasForeignKey(x => x.HubAgentSchemaId);

        builder.HasOne(x => x.HubAgent)
            .WithMany()
            .HasForeignKey(x => x.HubAgentId);

        builder.OwnsOne(x => x.AnalysisMetaData, metaBuilder =>
        {
            metaBuilder.Property(x => x.Status)
            .HasMaxLength(20)
            .HasConversion(
                status => status.key,
                key => BatchAnalysisStatus.GetByKey(key)
                );
        });

        builder.OwnsOne(x => x.TransferMetaData, metaBuilder =>
        {
            metaBuilder.Property(x => x.Status)
            .HasMaxLength(20)
            .HasConversion(
                status => status.key,
                key => BatchTransferStatus.GetByKey(key)
                );
        });

        builder.OwnsOne(x => x.SyncMetaData, metaBuilder =>
        {
            metaBuilder.Property(x => x.Status)
            .HasMaxLength(20)
            .HasConversion(
                status => status.key,
                key => BatchSyncStatus.GetByKey(key)
                );
        });
    }
}

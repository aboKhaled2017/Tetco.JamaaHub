using Domain.Entities.Hub;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.JameahHub.Configurations;

public sealed class HubLogConfiguration : IEntityTypeConfiguration<HubLog>
{
    public void Configure(EntityTypeBuilder<HubLog> builder)
    {
        builder.ToTable("Logs");

        builder.HasKey(x => x.Id);
    }
}

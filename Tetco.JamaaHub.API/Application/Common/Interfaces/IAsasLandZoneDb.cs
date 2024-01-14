using Domain.Entities.AsasLandZone;

namespace Application.Common.Interfaces;

public interface IAsasLandZoneDb
{
    public DbSet<AgentBatch> AgentBatchs
    {
        get;
    }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

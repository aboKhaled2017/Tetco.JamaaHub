using Application.Common.Interfaces.AsasLandingzoneDb.Dtos;
using Domain.Entities.AsasLandZone;

namespace Application.Common.Interfaces.AsasLandingzoneDb;

public interface IAsasLandZoneDb
{
    public DbSet<LZAgentBatch> AgentBatchs
    {
        get;
    }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    Task<(bool isValid, string errorMessage)> Execute_StartNewBatchFroAgent_SP(StartNewBatchSPInput input, CancellationToken cancellationToken);
    Task<(bool isValid, string errorMessage)> Execute_StopCurrentForAgentBatch_SP(StopCurrentBatchSPInput input, CancellationToken cancellationToken);
    Task<(bool isValid, string errorMessage)> Execute_InsertNewPackagetForAgentBatch_SP(InsertNewPackageSpInput input, CancellationToken cancellationToken);
}

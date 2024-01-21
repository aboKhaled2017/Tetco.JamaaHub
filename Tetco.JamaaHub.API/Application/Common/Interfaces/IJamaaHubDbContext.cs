using Domain.Entities.Hub.Log;
using Domain.Entities.Hub.UniversityAgent;

namespace Application.Common.Interfaces;

public interface IJamaaHubDbContext
{
    DbSet<HubAgent> Agents
    {
        get;
    }
    DbSet<HubLog> Logs
    {
        get;
    }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

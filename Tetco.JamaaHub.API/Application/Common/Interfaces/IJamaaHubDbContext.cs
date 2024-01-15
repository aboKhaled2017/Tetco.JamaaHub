using Abd.CleanArchitecture.Kernel.Domain.Identity;
using Domain.Entities.Hub;

namespace Application.Common.Interfaces;

public interface IJamaaHubDbContext
{
    DbSet<HubUniversityAgent> Universities
    {
        get;
    }
    DbSet<HubLog> Logs
    {
        get;
    }

    public DbSet<ApplicationGroup<Guid>> Groups
    {
        get;
    }
    public DbSet<ApplicationPermission<Guid>> Permissions
    {
        get;
    }
    public DbSet<ApplicationRolePermission<Guid>> RolePermissions
    {
        get;
    }
    public DbSet<ApplicationUserGroup<Guid>> UserGroups
    {
        get;
    }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

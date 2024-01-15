using Abd.CleanArchitecture.Kernel.Domain.Identity;
using Application.Common.Interfaces;
using Domain.Entities.Hub;
using Infrastructure.Data.JameahHub.Configurations;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.JameahHub;

public class JamaaHubDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>, IJamaaHubDbContext
{
    private readonly ILogger<JamaaHubDbContext> _logger;
    public JamaaHubDbContext(DbContextOptions<JamaaHubDbContext> options, ILogger<JamaaHubDbContext> logger) : base(options)
    {
        _logger = logger;
    }
    public DbSet<HubUniversityAgent> Universities => base.Set<HubUniversityAgent>();
    public DbSet<HubLog> Logs => Set<HubLog>();
    public DbSet<ApplicationGroup<Guid>> Groups => Set<ApplicationGroup<Guid>>();
    public DbSet<ApplicationPermission<Guid>> Permissions => Set<ApplicationPermission<Guid>>();
    public DbSet<ApplicationRolePermission<Guid>> RolePermissions => Set<ApplicationRolePermission<Guid>>();
    public DbSet<ApplicationUserGroup<Guid>> UserGroups => Set<ApplicationUserGroup<Guid>>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.RegisterConfigurationsFromNameSpace(typeof(JamaaHubDbContext).Assembly, typeof(HubUniversityAgentConfiguration
           ).Namespace);
    }

}

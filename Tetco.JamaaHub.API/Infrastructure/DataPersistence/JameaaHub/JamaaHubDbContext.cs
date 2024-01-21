using Application.Common.Interfaces;
using Domain.Entities.Hub.Log;
using Domain.Entities.Hub.UniversityAgent;
using Domain.Entities.Identity;
using Infrastructure.DataPersistence.JameahHub.Configurations;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DataPersistence.JameahHub;

public class JamaaHubDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>, IJamaaHubDbContext
{
    private readonly ILogger<JamaaHubDbContext> _logger;
    public JamaaHubDbContext(DbContextOptions<JamaaHubDbContext> options, ILogger<JamaaHubDbContext> logger) : base(options)
    {
        _logger = logger;
    }
    public DbSet<HubAgent> Agents => base.Set<HubAgent>();
    public DbSet<HubAgentBatch> AgentBatches => base.Set<HubAgentBatch>();
    public DbSet<HubAgentPackage> AgentPackages => base.Set<HubAgentPackage>();
    public DbSet<HubAgentSchema> AgentSchemaTypes => base.Set<HubAgentSchema>();
    public DbSet<HubLog> Logs => Set<HubLog>();


    #region Identity Entities
    public DbSet<ApplicationGroup<Guid>> Groups => Set<ApplicationGroup<Guid>>();
    public DbSet<ApplicationPermission<Guid>> Permissions => Set<ApplicationPermission<Guid>>();
    public DbSet<ApplicationRolePermission<Guid>> RolePermissions => Set<ApplicationRolePermission<Guid>>();
    public DbSet<ApplicationUserGroup<Guid>> UserGroups => Set<ApplicationUserGroup<Guid>>();
    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.RegisterConfigurationsFromNameSpace(typeof(JamaaHubDbContext).Assembly, typeof(HubAgentConfiguration
           ).Namespace);

        string identitySchema = "Identity";
        builder.Entity<ApplicationUser>().ToTable("Users", identitySchema);
        builder.Entity<ApplicationRole>().ToTable("Roles", identitySchema);
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", identitySchema);

        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", identitySchema);


        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", identitySchema);
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", identitySchema)
            .HasKey(x => new { x.LoginProvider, x.ProviderKey });

        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", identitySchema);
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", identitySchema);
    }

}

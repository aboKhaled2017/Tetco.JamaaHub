using Abd.CleanArchitecture.Kernel.Domain.Identity;
using Application.Common.Interfaces;
using Domain.Entities.Hub;
using Infrastructure.Data.JameahHub.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Infrastructure.Data.JameahHub;

public class JamaaHubDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>, IJamaaHubDbContext
{
    private readonly ILogger<JamaaHubDbContext> _logger;
    public JamaaHubDbContext(DbContextOptions<JamaaHubDbContext> options, ILogger<JamaaHubDbContext> logger) : base(options)
    {
        _logger = logger;
    }
    public DbSet<University> Universities => Set<University>();
    public DbSet<HubLog> HubLogs => Set<HubLog>();
    public DbSet<ApplicationGroup<Guid>> Groups => Set<ApplicationGroup<Guid>>();
    public DbSet<ApplicationPermission<Guid>> Permissions => Set<ApplicationPermission<Guid>>();
    public DbSet<ApplicationRolePermission<Guid>> RolePermissions => Set<ApplicationRolePermission<Guid>>();
    public DbSet<ApplicationUserGroup<Guid>> UserGroups => Set<ApplicationUserGroup<Guid>>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        RegisterEntities(builder, typeof(JamaaHubDbContext).Assembly, typeof(UniversityConfiguration
           ).Namespace);
    }

    private void RegisterEntities(ModelBuilder builder, Assembly assembly, string targetNamespace)
    {
        var entityTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
            .Any(interfaceType => interfaceType.IsGenericType &&
                interfaceType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)) && t.Namespace == targetNamespace);
        foreach (var entityType in entityTypes)
        {
            _logger.LogInformation($"Applying configuration for entity type: {entityType.FullName}");
            dynamic instance = Activator.CreateInstance(entityType);
            builder.ApplyConfiguration(instance);
        }
    }


}

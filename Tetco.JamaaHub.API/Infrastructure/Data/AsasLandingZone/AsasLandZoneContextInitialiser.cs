using Infrastructure.AgentDataModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.UniversityAgent;
public static class AsasLandZoneInitialiserExtensions
{
    public static async Task InitialiseDatabaseNaqelAgentAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var initialiser = scope.ServiceProvider.GetRequiredService<AsasLandZoneContextInitialiser>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}
public class AsasLandZoneContextInitialiser
{
    private readonly ILogger<AsasLandZoneContextInitialiser> _logger;
    private readonly AsasLandZoneDb _context;

    public AsasLandZoneContextInitialiser(ILogger<AsasLandZoneContextInitialiser> logger, AsasLandZoneDb context)
    {
        _logger = logger;
        _context = context;

    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
           //TODO : seeding some intial data

            //await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
}

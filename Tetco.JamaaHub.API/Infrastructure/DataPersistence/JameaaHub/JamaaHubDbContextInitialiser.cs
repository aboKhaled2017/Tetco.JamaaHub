using Abd.CleanArchitecture.Kernel.Domain.Identity;
using Domain.Entities.Hub.UniversityAgent;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DataPersistence.JameahHub;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<JamaaHubDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class JamaaHubDbContextInitialiser
{
    private readonly ILogger<JamaaHubDbContextInitialiser> _logger;
    private readonly JamaaHubDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public JamaaHubDbContextInitialiser(ILogger<JamaaHubDbContextInitialiser> logger, JamaaHubDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
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
            if (!await _context.Agents.AnyAsync())
            {
                var agent = new HubAgent
                {
                    NameAr = "جامعه أ",
                    NameEn = "Jamaa A",
                    IpAddress = "192.168.1.1",
                    AgentApiAccessKey = "xxx_asas",
                    AgentServiceUrl = "https://localhost:5000/agentA",
                    InstituteCode = "agent_a"
                };

                await _context.Agents.AddAsync(agent);

                await _context.SaveChangesAsync(cancellationToken);

                agent.SchemaTypes.Add(HubAgentSchema.Create(agent,"Students","Students Schema","v1.0.0"));

                await _context.SaveChangesAsync(cancellationToken);
            }

            if (!await _userManager.Users.AnyAsync())
            {
                var university = await _context.Agents.FirstOrDefaultAsync();

                if (university is null)
                    throw new Exception("faild to find any registed university");

                string username = "mohamed2511995@gmail.com";
                var user = new ApplicationUser
                {
                    UserName = username,
                    Email = username,
                    EmailConfirmed = true,
                    UniversityId = university.Id,
                };
                var res = await _userManager.CreateAsync(user, "Mohamed@12321");

                if (!res.Succeeded)
                    throw new Exception(res.Errors.FirstOrDefault()?.Description);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    //public async Task TrySeedAsync ( )
    //   {
    //   // Default roles
    //   var administratorRole = new IdentityRole ( Roles.Administrator );

    //   if ( _roleManager.Roles.All ( r => r.Name != administratorRole.Name ) )
    //      {
    //      await _roleManager.CreateAsync ( administratorRole );
    //      }

    //   // Default users
    //   var administrator = new ApplicationUser { UserName = "administrator@localhost" , Email = "administrator@localhost" };

    //   if ( _userManager.Users.All ( u => u.UserName != administrator.UserName ) )
    //      {
    //      await _userManager.CreateAsync ( administrator , "Administrator1!" );
    //      if ( !string.IsNullOrWhiteSpace ( administratorRole.Name ) )
    //         {
    //         await _userManager.AddToRolesAsync ( administrator , new [] { administratorRole.Name } );
    //         }
    //      }

    //   // Default data
    //   // Seed, if necessary
    //   if ( !_context.TodoLists.Any ( ) )
    //      {
    //      _context.TodoLists.Add ( new TodoList
    //         {
    //         Title = "Todo List" ,
    //         Items =
    //             {
    //                 new TodoItem { Title = "Make a todo list 📃" },
    //                 new TodoItem { Title = "Check off the first item ✅" },
    //                 new TodoItem { Title = "Realise you've already done two things on the list! 🤯"},
    //                 new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
    //             }
    //         } );

    //      await _context.SaveChangesAsync ( );
    //      }
    //   }
}

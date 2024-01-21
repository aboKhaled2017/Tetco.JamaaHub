using Application.Common.Interfaces;
using Application.Common.Interfaces.AsasLandingzoneDb;
using Application.Common.Settings;
using Ardalis.GuardClauses;
using Domain.BuildingBlocks;
using Domain.Constants;
using Domain.Entities.Identity;
using Infrastructure.AgentDataModels;
using Infrastructure.DataPersistence.JameahHub;
using Infrastructure.DataPersistence.JameahHub.Identity;
using Infrastructure.DataPersistence.JameahHub.Interceptors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure;

public static class DependencyInjection
{
    private static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthSetting>(configuration.GetSection($"{nameof(AuthSetting)}s"));
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<AuthSetting>>().Value);
        return services;
    }
    public static IServiceCollection AddJamaaHubAuthorization(this IServiceCollection services)
    {
        var authSetting = services.BuildServiceProvider().GetRequiredService<AuthSetting>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var keyBytes = new byte[64];
            Encoding.UTF8.GetBytes(authSetting.SecretKey).CopyTo(keyBytes, 0);

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = authSetting.Issuer,
                ValidAudience = authSetting.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
            };
        });

        services.AddAuthorizationBuilder();

        services.AddAuthorization(options =>
         options.AddPolicy(Policies.CanInsertStudents, policy =>
          policy.RequireAuthenticatedUser()
         .RequireRole(Roles.JamaaAgent)));


        return services;
    }
    private static IServiceCollection ConfigureJamaaHubDbWithIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        var connectionString = configuration.GetConnectionString("JamaaHub");
        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddDbContext<IJamaaHubDbContext, JamaaHubDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseSqlServer(connectionString);
        });

        services.AddScoped<JamaaHubDbContextInitialiser>();

        services
           .AddIdentityCore<ApplicationUser>()
           .AddRoles<ApplicationRole>()
           .AddEntityFrameworkStores<JamaaHubDbContext>()
           .AddApiEndpoints();

        return services;
    }
    private static IServiceCollection ConfigureLandingZoneDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        var connectionString = configuration.GetConnectionString("AsasLandingZone");
        Guard.Against.Null(connectionString, message: "Connection string 'NaqelAgentConnection' not found.");

        services.AddDbContext<IAsasLandZoneDb, AsasLandZoneDb>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseSqlServer(connectionString);
        });


        return services;
    }
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureSettings(configuration);

        services.AddDataProtection();

        services.AddScoped<IUser, ApplicationUser>();

        services.ConfigureJamaaHubDbWithIdentity(configuration);
        services.ConfigureLandingZoneDb(configuration);
        services.AddJamaaHubAuthorization();
        
        services.AddLogging(builder =>
        {
            builder.AddConsole(); // Output to console
                                  // Add other logging providers as needed (e.g., AddDebug, AddFile, etc.)
        });
       
        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddHttpClient(HtttClientNames.JamaaAgentHubClientName, client =>
        {
            client.BaseAddress = new Uri(configuration.GetConnectionString("jamaaHubApi"));
        });

        return services;
    }
}

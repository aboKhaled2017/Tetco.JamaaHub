using Abd.CleanArchitecture.Kernel.Domain;
using API.MiddleWares;
using API.Services;
using Infrastructure.DataPersistence.JameahHub;
using Microsoft.AspNetCore.Mvc;


namespace API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPIServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<IUser, CurrentUser>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<JamaaHubDbContext>();

        services.AddTransient<JamaaHubExceptionMiddleWare>();


        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddEndpointsApiExplorer();



        return services;
    }
}


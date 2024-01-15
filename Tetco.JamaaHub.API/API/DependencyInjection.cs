using Abd.CleanArchitecture.Kernel.Domain;
using API.Services;
using Infrastructure.Data.JameahHub;
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

        services.AddTransient<CustomExceptionMiddleWare>();


        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddEndpointsApiExplorer();



        return services;
    }
}


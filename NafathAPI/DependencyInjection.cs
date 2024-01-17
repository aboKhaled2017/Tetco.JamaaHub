using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using NafathAPI.Common.Behaviours;
using NafathAPI.Common.Interfaces;
using NafathAPI.CrossCutting.Middlewares;
using NafathAPI.CrossCutting.OpenApi;
using NafathAPI.CrossCutting.Security;
using NafathAPI.Services;
using StackExchange.Redis;
using System.Reflection;
namespace NafathAPI;

public static class DependencyInjection
    {
    public static IServiceCollection AddWebServices ( this IServiceCollection services , ConfigurationManager configuration )
        {
        services.AddHttpContextAccessor ( );
        services.AddAutoMapper ( Assembly.GetExecutingAssembly ( ) );
        services.AddRazorPages ( );
        services.AddControllersWithViews ( );
        services.AddEndpointsApiExplorer ( );
        //services.AddScoped<IUser , CurrentUser> ( );
        services.AddStackExchangeRedisCache ( delegate ( RedisCacheOptions options )
           {
               options.ConfigurationOptions = new ConfigurationOptions
                   {
                   EndPoints = {
                     {
                       configuration["CacheSettings:ConnectionString"] ,
                       int.Parse (configuration["CacheSettings:ReadPort"] )
                     } ,
                     {
                       configuration["CacheSettings:ConnectionString"] ,
                       int.Parse (configuration["CacheSettings:WritePort"] )
                     }
                    } ,
                   User = configuration ["CacheSettings:UserName"] ,
                   Password = configuration ["CacheSettings:Password"] ,
                   };
               options.InstanceName = configuration ["CacheSettings:InstanceName"];
               } );
        services.AddAutoMapper ( Assembly.GetExecutingAssembly ( ) );

        services.AddValidatorsFromAssembly ( Assembly.GetExecutingAssembly ( ) );

        services.AddMediatR ( cfg =>
        {
            cfg.RegisterServicesFromAssembly ( Assembly.GetExecutingAssembly ( ) );
            cfg.AddBehavior ( typeof ( IPipelineBehavior<,> ) , typeof ( UnhandledExceptionBehaviour<,> ) );
            cfg.AddBehavior ( typeof ( IPipelineBehavior<,> ) , typeof ( ValidationBehaviour<,> ) );
        } );


        services.AddScoped<ISerializer , NewtonsoftJsonSerializer> ( );
        services.AddTransient<IRestClient , RestClient> ( );
        services.AddHttpClient ( );
        services.Configure<ApiBehaviorOptions> ( options =>
            options.SuppressModelStateInvalidFilter = true );
        services.AddScoped<IApiKeyValidationService , ApiKeyValidationService> ( );
        services.AddOpenApiDocs ( );
        services.AddEndpointsApiExplorer ( );

        return services;
        }
    public static IApplicationBuilder UseRequestResponseLogging ( this IApplicationBuilder builder )
        {
        return builder.UseMiddleware<RequestResponseLoggingMiddleware> ( Array.Empty<object> ( ) );
        }
    }

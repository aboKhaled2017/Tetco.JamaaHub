using Microsoft.AspNetCore.Authentication;
using NafathAPI;
using NafathAPI.CrossCutting.Logging;
using NafathAPI.CrossCutting.Middlewares;
using NafathAPI.CrossCutting.OpenApi;
using NafathAPI.CrossCutting.Security;
using NafathAPI.Infrastructure;
using Serilog;
using static NafathAPI.CrossCutting.Middlewares.MiddlewareExtensions;

var builder = WebApplication.CreateBuilder ( args );

// Add services to the container.
//builder.Services.AddKeyVaultIfConfigured ( builder.Configuration );


//builder.Services.AddAutomatedAutorest();
builder.Services.AddControllers ( );

builder.Services.AddWebServices ( builder.Configuration );

builder.Services.AddAuthentication ( "APIKey" )
    .AddScheme<AuthenticationSchemeOptions , ApiKeyAuthenticationHandler> ( "APIKey" , options => { } );

// Use Serilog
builder.Host.UseSerilog ( ( hostContext , services , configuration ) =>
{
    configuration
        .ReadFrom.Configuration ( builder.Configuration )
        .Enrich.FromLogContext ( )
        .WriteTo.Console ( )
        .Enrich.WithSerilogContextEnricher ( );
} );
var app = builder.Build ( );

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment ( ) )
    {
    // await app.InitialiseDatabaseAsync ( );
    }
else
    {
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts ( );
    }
app.UseStaticFiles ( );
if ( bool.TryParse ( builder.Configuration ["EnableRequestResponseLogging"] , out bool enableRequestResponseLogging ) && enableRequestResponseLogging )
    {
    app.UseRequestResponseLogging ( );
    }



app.MapControllerRoute (
    name: "default" ,
    pattern: "{controller}/{action=Index}/{id?}" );

app.MapRazorPages ( );

app.MapFallbackToFile ( "swagger/index.html" );

app.UseExceptionHandler ( options => { } );

app.UseSecurityHeadersMiddleware ( new SecurityHeadersBuilder ( ).AddDefaultSecurePolicy ( ) );
app.MapEndpoints ( );
app.UseOpenApiDocs ( );
app.UseAuthentication ( );
app.UseAuthorization ( );

app.Run ( );


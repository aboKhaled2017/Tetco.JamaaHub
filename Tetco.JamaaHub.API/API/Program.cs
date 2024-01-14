using Application;
using Infrastructure;
using Infrastructure.Data.JameahHub;
using API;
using API.Services;

var builder = WebApplication.CreateBuilder ( args );

// Add services to the container.
builder.Services.AddApplicationServices ( );

builder.Services.AddInfrastructureServices ( builder.Configuration );
builder.Services.AddJamaaHubAuthorization ( );
//builder.Services.AddAutomatedAutorest ( );
builder.Services.AddControllers ( );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddWebServices ( );
builder.Services.AddEndpointsApiExplorer ( );
builder.Services.AddSwaggerGen ( );

var app = builder.Build();

await app.InitialiseDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    
    //app.UseMiddleware<AutoRestMiddleware>();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection ( );

app.UseAuthentication ( );
app.UseAuthorization ( );

app.UseMiddleware<CustomExceptionMiddleWare> ( );

app.MapControllers ( );

app.Run ( );

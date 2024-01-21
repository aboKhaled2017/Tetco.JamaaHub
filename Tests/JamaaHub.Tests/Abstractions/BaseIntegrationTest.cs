using Application.Common.Interfaces;
using Infrastructure.DataPersistence.JameahHub.Identity;
using Infrastructure.DataPersistence.JameahHub;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Infrastructure.AgentDataModels;
using Application.Common.Interfaces.AsasLandingzoneDb;
using Application;

namespace JamaaHub.Tests.Abstractions
{
    public abstract class BaseIntegrationTest: IAsyncDisposable
    {
        private DbContextOptions<JamaaHubDbContext> _jamaaHubOptions;
        protected JamaaHubDbContext _context;
        protected AsasLandZoneDb _asasLandZoneDb;
        private readonly IServiceProvider _sp;
        protected ISender _mediator;
        public BaseIntegrationTest()
        {
            // Initialize the in-memory database
            _jamaaHubOptions = new DbContextOptionsBuilder<JamaaHubDbContext>()
                .UseInMemoryDatabase(databaseName: "JamaahubDb.Test")
                .Options;

            // Create a new context for each test
            _context = new JamaaHubDbContext(_jamaaHubOptions, new Mock<ILogger<JamaaHubDbContext>>().Object);

            //create new DI for registering context & settings
            var services = new ServiceCollection();

            var mockLogger = new Mock<ILogger<object>>();
            services.AddTransient(_ => mockLogger.Object);

            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.Services.AddSingleton<ILoggerProvider>(sp =>
                {
                    var mockLoggerProvider = new Mock<ILoggerProvider>();
                    mockLoggerProvider
                        .Setup(p => p.CreateLogger(It.IsAny<string>()))
                        .Returns((string category) => sp.GetRequiredService<ILogger<object>>()); // Resolve ILogger<object> for any category
                    return mockLoggerProvider.Object;
                });
            });

        
            services.AddScoped<JamaaHubDbContext, JamaaHubDbContext>(_ => _context);

            services.AddDbContext<IAsasLandZoneDb,AsasLandZoneDb>(x=>x.UseSqlServer("Server=localhost;Database=ASASLandingZone.Test;integrated security=false;User Id=sa;Password=123;TrustServerCertificate=true;MultipleActiveResultSets=true"));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //services.AddMediatR(cfg =>
            //{
            //    cfg.RegisterServicesFromAssembly(typeof(IJamaaHubDbContext).Assembly);
            //});

            services.AddApplicationServices();
            services.RemoveService<>();

            SetupAddtionalServices(services);

            _sp = services.BuildServiceProvider();

            _mediator = _sp.GetRequiredService<ISender>();
            _asasLandZoneDb = _sp.GetRequiredService<AsasLandZoneDb>();

        }

        protected abstract void SetupAddtionalServices(ServiceCollection services);
        protected abstract ValueTask DisposeOtherResourcesAsync();
        public async ValueTask DisposeAsync()
        {
            // Clean up the database after each test
            _context.Database.EnsureDeleted();
           
            await _context.DisposeAsync();

            await _asasLandZoneDb.DisposeAsync(); 

            await DisposeOtherResourcesAsync();
        }
    }
}

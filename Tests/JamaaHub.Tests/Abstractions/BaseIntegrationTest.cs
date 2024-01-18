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

namespace JamaaHub.Tests.Abstractions
{
    public abstract class BaseIntegrationTest: IAsyncDisposable
    {
        private DbContextOptions<JamaaHubDbContext> _options;
        protected JamaaHubDbContext _context;
        private readonly IServiceProvider _sp;
        protected ISender _mediator;
        public BaseIntegrationTest()
        {
            // Initialize the in-memory database
            _options = new DbContextOptionsBuilder<JamaaHubDbContext>()
                .UseInMemoryDatabase(databaseName: "TestSeedsDatabase")
                .Options;

            // Create a new context for each test
            _context = new JamaaHubDbContext(_options, new Mock<ILogger<JamaaHubDbContext>>().Object);

            //create new DI for registering context & settings
            var services = new ServiceCollection();

            var loggerMock = new Mock<ILogger<JamaaHubDbContext>>();
            services.AddTransient(_ => loggerMock.Object);

            services.AddScoped<JamaaHubDbContext, JamaaHubDbContext>(_ => _context);
          
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(JamaaHubDbContext).Assembly);
            });

            SetupAddtionalServices(services);

            _sp = services.BuildServiceProvider();

            _mediator = _sp.GetRequiredService<ISender>();
            
        }

        protected abstract void SetupAddtionalServices(ServiceCollection services);
        protected abstract ValueTask DisposeOtherResourcesAsync();
        public async ValueTask DisposeAsync()
        {
            // Clean up the database after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();

            await DisposeOtherResourcesAsync();
        }
    }
}

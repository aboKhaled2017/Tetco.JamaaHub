using Application.Common.Interfaces;
using Application.Features.AgentOperations.Commands.StartNewAgentBatch;
using Domain.Enums;
using Infrastructure.DataPersistence.JameahHub.Identity;
using JamaaHub.Tests.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace JamaaHub.API.Tests.IntegrationTests.Features.AgentOperations.Commands
{
    public class StartNewAgentBatchCommandTests : BaseIntegrationTest
    {
        private int _studebtSchemaType=1;
        public StartNewAgentBatchCommandTests()
        {
        }

        [Fact]
        public async void InsertNewAgentBatch_ValidData_ShouldSuccess()
        {
            //prepare
            Guid newBatchId = Guid.NewGuid();
            string instituteCode = "jamaa_A_agent";
            var insertCommand = new StartNewAgentBatchCommand
            {
                BatchId= newBatchId,
                InstituteCode= instituteCode,
                MigrationType=MigrationType.Full,
                PriorityLevel=PriorityLevel.Heigh,
                SchemaTypeId= _studebtSchemaType,
                SchemaVersion="v.1.0.0",
                StartDate= DateTime.Now,
                TotalRecordsCount=50
            };

            //act
            var res = await _mediator.Send(insertCommand);
            var isBatchInserted = await _asasLandZoneDb.AgentBatchs.AnyAsync(x => x.BatchGUID == newBatchId);

            //assert
            res.ShouldNotBeNull();
            res.Succeeded.ShouldBeTrue();
            isBatchInserted.ShouldBeTrue();
        }

        protected override ValueTask DisposeOtherResourcesAsync()
        {

            return ValueTask.CompletedTask;
        }

        protected override void SetupAddtionalServices(ServiceCollection services)
        {
            var identityServiceMock = new Mock<IdentityService>(null, null, null, null);
            identityServiceMock.Setup(x => x.GetUniversityIdOfCurrentUser()).Returns(1);

            services.AddScoped<IIdentityService>(_ => identityServiceMock.Object);
        }
    }
}

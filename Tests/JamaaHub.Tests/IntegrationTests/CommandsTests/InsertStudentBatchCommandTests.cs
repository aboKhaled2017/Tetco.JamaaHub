using Abd.CleanArchitecture.Kernel.Domain.Identity;
using Application.Common.Behaviours;
using Application.Common.Interfaces;
using Application.JamaaAgents.Commands.InsertStudents;
using Application.NaqelTemplates.Models;
using FluentValidation;
using Infrastructure.Data.JameahHub;
using Infrastructure.Data.JameahHub.Identity;
using JamaaHub.Tests.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JamaaHub.Tests.IntegrationTests.CommandsTests
{
    public class InsertStudentBatchCommandTests: BaseIntegrationTest
    {

        [Fact]
        public async void InsertNewStudentBatch_ValidData_ShouldSuccess()
        {
            //prepare
            int batchNum = 1;
            var insertCommand = new InsertStudentBatchCommand
            {
                BatchNumber = batchNum,
                Priority = true,
                Records = new List<StudentPersonalInfo>
                {
                    new StudentPersonalInfo()
                    {
                        StudentUniqeId="std1",
                        IdentityNumber="123123"
                    },
                    new StudentPersonalInfo()
                    {
                        StudentUniqeId="std2",
                        IdentityNumber="1231567"
                    }
                }
            };

            //act
            var res = await _mediator.Send(insertCommand);
            var isBatchInserted = await _context.JamaaBatches.AnyAsync(x => x.BatchNumber == batchNum);

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

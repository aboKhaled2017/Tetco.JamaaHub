using Application.Common.Interfaces;
using Infrastructure.DataPersistence.JameahHub.Identity;
using JamaaHub.Tests.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace JamaaHub.Tests.IntegrationTests.CommandsTests
{
    public class InsertStudentBatchCommandTests: BaseIntegrationTest
    {

        //[Fact]
        //public async void InsertNewStudentBatch_ValidData_ShouldSuccess()
        //{
        //    //prepare
        //    int batchNum = 1;
        //    var insertCommand = new InsertStudentBatchCommand
        //    {
        //        BatchNumber = batchNum,
        //        Priority = true,
        //        Records = new List<StudentPersonalInfo>
        //        {
        //            new StudentPersonalInfo()
        //            {
        //                StudentUniqeId="std1",
        //                IdentityNumber="123123"
        //            },
        //            new StudentPersonalInfo()
        //            {
        //                StudentUniqeId="std2",
        //                IdentityNumber="1231567"
        //            }
        //        }
        //    };

        //    //act
        //    var res = await _mediator.Send(insertCommand);
        //    var isBatchInserted = await _context.JamaaBatches.AnyAsync(x => x.BatchNumber == batchNum);

        //    //assert
        //    res.ShouldNotBeNull();
        //    res.Succeeded.ShouldBeTrue();
        //    isBatchInserted.ShouldBeTrue();
        //}

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

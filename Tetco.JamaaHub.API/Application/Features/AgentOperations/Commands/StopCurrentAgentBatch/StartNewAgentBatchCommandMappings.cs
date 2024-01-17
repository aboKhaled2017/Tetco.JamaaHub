using Application.Common.Interfaces.AsasLandingzoneDb.Dtos;
using Domain.Enums;

namespace Application.Features.AgentOperations.Commands.StartNewAgentBatch
{
    public sealed class StopCurrentAgentBatchCommandMappings : Profile {
        public StopCurrentAgentBatchCommandMappings()
        {
            CreateMap<StopCurrentAgentBatchCommand, StopCurrentBatchSPInput>()
                 .ForMember(x => x.MigrationTypeId, f => f.MapFrom(s => MigrationType.GetByKey(s.MigrationType).value));
        }
    }
}

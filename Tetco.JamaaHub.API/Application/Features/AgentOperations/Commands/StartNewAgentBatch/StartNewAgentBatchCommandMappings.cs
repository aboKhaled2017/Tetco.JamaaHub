using Application.Common.Interfaces.AsasLandingzoneDb.Dtos;
using Domain.Enums;

namespace Application.Features.AgentOperations.Commands.StartNewAgentBatch
{
    public sealed class StartNewAgentBatchCommandMappings : Profile {
        public StartNewAgentBatchCommandMappings()
        {
            CreateMap<StartNewAgentBatchCommand, StartNewBatchSPInput>()
                 .ForMember(x => x.MigrationTypeId, f => f.MapFrom(s => MigrationType.GetByKey(s.MigrationType).value))
                 .ForMember(x => x.PriorityLevelId, f => f.MapFrom(s => PriorityLevel.GetByKey(s.PriorityLevel).value));
        }
    }
}

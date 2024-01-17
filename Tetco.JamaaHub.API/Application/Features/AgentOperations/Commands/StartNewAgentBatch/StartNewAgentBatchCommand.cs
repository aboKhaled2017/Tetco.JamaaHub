using Application.Common.Interfaces.AsasLandingzoneDb;
using Application.Common.Interfaces.AsasLandingzoneDb.Dtos;
using Domain.Common.Patterns;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.AgentOperations.Commands.StartNewAgentBatch
{
    public sealed record StartNewAgentBatchCommand:IRequest<Result>
    {
        public string SchemaTypeId { get; set; }
        public string MigrationType { get; set; }
        public Guid BatchId { get; set; }
        [FromQuery]
        public string SchemaVersion { get; set; }
        public int TotalRecordsCount { get; set; }
        public DateTime StartDate { get; set; }
        public string InstituteCode { get; set; }
        public int PriorityLevelId { get; set; }
    }
    public sealed class StartNewAgentBatchCommandHandler : IRequestHandler<StartNewAgentBatchCommand, Result>
    {
        private readonly IAsasLandZoneDb _lzDb;
        private readonly IMapper _mapper;
        public StartNewAgentBatchCommandHandler(IAsasLandZoneDb lzDb, IMapper mapper)
        {
            _lzDb = lzDb;
            _mapper = mapper;
        }

        public async Task<Result> Handle(StartNewAgentBatchCommand request, CancellationToken cancellationToken)
        {
            var spInput = _mapper.Map<StartNewBatchSPInput>(request);

            var res=await _lzDb.Execute_StartNewBatchFroAgent_SP(spInput, cancellationToken);

            return res.isValid
                ? Result.Success($"the batach has been started successfully , you can start uploading ackages now")
                : Result.Failure("Batch_Refused", res.errorMessage);
        }
    }
}

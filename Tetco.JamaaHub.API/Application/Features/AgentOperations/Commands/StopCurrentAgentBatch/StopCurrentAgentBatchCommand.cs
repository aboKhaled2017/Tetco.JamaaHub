using Application.Common.Interfaces.AsasLandingzoneDb;
using Application.Common.Interfaces.AsasLandingzoneDb.Dtos;
using Domain.Common.Patterns;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.AgentOperations.Commands.StartNewAgentBatch
{
    public sealed record StopCurrentAgentBatchCommand : IRequest<Result>
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
    public sealed class StopCurrentAgentBatchCommandHandler : IRequestHandler<StopCurrentAgentBatchCommand, Result>
    {
        private readonly IAsasLandZoneDb _lzDb;
        private readonly IMapper _mapper;
        public StopCurrentAgentBatchCommandHandler(IAsasLandZoneDb lzDb, IMapper mapper)
        {
            _lzDb = lzDb;
            _mapper = mapper;
        }

        public async Task<Result> Handle(StopCurrentAgentBatchCommand request, CancellationToken cancellationToken)
        {
            bool isBatchExists = await _lzDb.AgentBatchs.AnyAsync(x => x.BatchGUID == request.BatchId);

            if (!isBatchExists)
                return Result.Failure("Not_Valid_Batch_Id",$"cannot find batch with id {request.BatchId} exists at Jamaa Hub");
           
            var spInput = _mapper.Map<StopCurrentBatchSPInput>(request);

            var res=await _lzDb.Execute_StopCurrentForAgentBatch_SP(spInput, cancellationToken);

            return res.isValid
                ? Result.Success($"the batach has been stopped successfully , then next time you will have to start new batch for uploading data")
                : Result.Failure("Batch_Proccessing_Error", res.errorMessage);
        }
    }
}

using Domain.Enums;

namespace Application.Features.AgentOperations.Commands.StartNewAgentBatch
{
    public sealed class StartNewAgentBatchCommandValidator : AbstractValidator<StartNewAgentBatchCommand>
    {
        public StartNewAgentBatchCommandValidator()
        {
            RuleFor(x => x.SchemaTypeId)
                .NotEmpty().WithMessage("SchemaTypeId is required");

            RuleFor(x => x.MigrationType)
                .NotEmpty().WithMessage("MigrationType is required")
                .Must(x => MigrationType.GetKeys().Contains(x)).WithMessage($"MigrationType value should be one of [{MigrationType.CommaSeperatedkeys()}]");

            RuleFor(x => x.BatchId)
                    .NotEmpty().WithMessage("BatchId is required");

            RuleFor(x => x.SchemaVersion)
                    .NotEmpty().WithMessage("SchemaVersion is required");

            RuleFor(x => x.TotalRecordsCount)
                   .NotEmpty().WithMessage("TotalRecordsCount is required");

            RuleFor(x => x.StartDate)
                   .NotEmpty().WithMessage("StartDate is required");
        }
    }
}

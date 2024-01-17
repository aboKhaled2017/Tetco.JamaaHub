using Application.Features.Login.Commands.NafathLoginJameaAgent;

namespace Application.Features.Login.Commands.LoginJamaagent
    {
    public sealed class NafathLoginJameaAgentCommandValidator : AbstractValidator<NafathLoginJameaAgentCommand>
        {
        public NafathLoginJameaAgentCommandValidator ( )
            {
            RuleFor ( x => x.TransId )
                .NotEmpty ( ).WithMessage ( "TransId address is required" );
            RuleFor ( x => x.AccessToken )
                .NotEmpty ( ).WithMessage ( "AccessToken is required" );
            RuleFor ( x => x.Status )
               .NotEmpty ( ).WithMessage ( "Status is required" );
            }
        }
    }

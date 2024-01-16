using Application.Features.Login.Commands.ADFSLoginJameaAgent;

namespace Application.Features.Login.Commands.LoginJamaagent
    {
    public sealed class ADFSLoginJameaAgentCommandValidator : AbstractValidator<ADFSLoginJameaAgentCommand>
        {
        public ADFSLoginJameaAgentCommandValidator ( )
            {
            RuleFor ( x => x.JWTToken )
              .NotEmpty ( ).WithMessage ( "JWTToken is required" );
            }
        }
    }

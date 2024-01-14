namespace Application.Features.Login.Commands.LoginJamaagent
{
    public sealed class LoginJamaaAgentCommandValidator : AbstractValidator<LoginJamaaAgentCommand>
    {
        public LoginJamaaAgentCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("email address is required")
                .EmailAddress().WithMessage("not valid email address");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("password is required");
        }
    }
}

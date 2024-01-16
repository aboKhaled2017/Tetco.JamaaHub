using FluentValidation;

namespace NafathAPI.Domain.Nafath.Dto
    {
    public class NafathSignInRequest
        {
        public string NationalId
            {
            get; set;
            }
        }
    public class NafathSignInRequestValidator : AbstractValidator<NafathSignInRequest>
        {
        public NafathSignInRequestValidator ( )
            {
            RuleFor ( x => x.NationalId )
          .NotNull ( ).WithMessage ( "National ID is required." )
          .NotEmpty ( ).WithMessage ( "National ID cannot be empty." )
          .Length ( 10 ).WithMessage ( "National ID must be exactly 10 characters long." );
            }
        }
    }

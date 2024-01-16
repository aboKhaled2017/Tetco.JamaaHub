using FluentValidation;

namespace NafathAPI.Domain.Nafath.Dto
    {
    public class NafathChallengeRequest
        {
        public string AccessToken
            {
            get; set;
            }
        public string TransId
            {
            get; set;
            }
        public string Status
            {
            get; set;
            }
        }

    public class NafathCheckStatusRequest
        {
        public string TransId
            {
            get; set;
            }

        }
    public class NafathCheckStatusRequestValidator : AbstractValidator<NafathCheckStatusRequest>
        {
        public NafathCheckStatusRequestValidator ( )
            {
            RuleFor ( x => x.TransId )
       .NotNull ( ).WithMessage ( "TransId  is required." )
       .NotEmpty ( ).WithMessage ( "TransId  cannot be empty." );
            }
        }
    }

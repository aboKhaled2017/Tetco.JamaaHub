namespace NafathAPI.Domain.Nafath.Dto.IntegrationModels
    {
    public class SignInRequest
        {
        public string Action
            {
            get; set;
            }
        public SignInRequestParameters Parameters
            {
            get; set;
            }
        }

    public class SignInRequestParameters
        {
        public string service
            {
            get; set;
            }
        public string id
            {
            get; set;
            }
        }
    }

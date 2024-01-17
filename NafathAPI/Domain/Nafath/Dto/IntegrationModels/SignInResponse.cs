namespace NafathAPI.Domain.Nafath.Dto.IntegrationModels
    {
    public class SignInResponse
        {
        public string transId
            {
            get; set;
            }
        public string random
            {
            get; set;
            }

        public string Trace
            {
            get; set;
            }
        public string Code
            {
            get; set;
            }
        public string RequestedURL
            {
            get; set;
            }
        public string Message
            {
            get; set;
            }
        }
    }

//"{\"Code\":\"400-B005\",\"RequestedURL\":\"https://www.iam.gov.sa/\",\"Message\":\"\",\"Trace\":\"2306041630223004560050\"}"
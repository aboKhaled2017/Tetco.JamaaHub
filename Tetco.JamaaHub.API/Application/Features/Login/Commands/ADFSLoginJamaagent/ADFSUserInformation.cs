namespace Application.Features.Login.Commands.ADFSLoginJameaAgent
    {

    public class ADFSUserInformation
        {
        public string Audience
            {
            get; set;
            }
        public string Issuer
            {
            get; set;
            }
        public DateTime IssuedAt
            {
            get; set;
            }
        public DateTime NotBefore
            {
            get; set;
            }
        public DateTime ExpirationTime
            {
            get; set;
            }
        public string UserPrincipalName
            {
            get; set;
            }
        public string Email
            {
            get; set;
            }
        public string Uniquename
            {
            get; set;
            }
        public string AppType
            {
            get; set;
            }
        public string AppId
            {
            get; set;
            }
        public string AuthMethod
            {
            get; set;
            }
        public DateTime AuthTime
            {
            get; set;
            }
        public string Ver
            {
            get; set;
            }
        public string Scp
            {
            get; set;
            }

        }
    }

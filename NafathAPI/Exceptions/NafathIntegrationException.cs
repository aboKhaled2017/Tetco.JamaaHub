using System.Net;

namespace NafathAPI.Exceptions
    {
    public class NafathIntegrationException : Exception
        {
        public HttpStatusCode HttpStatusCode
            {
            get; private set;
            }

        public NafathIntegrationException ( HttpStatusCode code , string message ) : base ( message )
            {
            HttpStatusCode = code;
            }
        }
    }

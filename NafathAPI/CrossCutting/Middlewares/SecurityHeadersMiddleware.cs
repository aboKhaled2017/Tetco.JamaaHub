namespace NafathAPI.CrossCutting.Middlewares;
public static class MiddlewareExtensions
    {
    public class SecurityHeadersBuilder
        {
        private readonly SecurityHeadersPolicy _policy = new SecurityHeadersPolicy ( );

        /// <summary>
        /// The number of seconds in one year
        /// </summary>
        public const int OneYearInSeconds = 60 * 60 * 24 * 365;

        /// <summary>
        /// Add default headers in accordance with most secure approach
        /// </summary>
        public SecurityHeadersBuilder AddDefaultSecurePolicy ( )
            {
            AddFrameOptionsDeny ( );
            AddXssProtectionBlock ( );
            AddContentTypeOptionsNoSniff ( );
            AddContentSecurityPolicy ( );
            AddStrictTransportSecurityMaxAge ( );
            RemoveServerHeader ( );

            return this;
            }

        /// <summary>
        /// Add X-Frame-Options DENY to all requests.
        /// </summary>
        public SecurityHeadersBuilder AddFrameOptionsDeny ( )
            {
            _policy.SetHeaders ["X-Frame-Options"] = "DENY";
            return this;
            }

        /// <summary>
        /// Add X-XSS-Protection all requests.
        /// </summary>
        public SecurityHeadersBuilder AddXssProtectionBlock ( )
            {
            _policy.SetHeaders ["X-XSS-Protection"] = "1; mode=block";
            return this;
            }

        /// <summary>
        /// Add Strict-Transport-Security max-age=<see cref="maxAge"/> to all requests.
        /// Tells the user-agent to cache the domain in the STS list for the number of seconds provided.
        /// </summary>
        public SecurityHeadersBuilder AddStrictTransportSecurityMaxAge ( int maxAge = OneYearInSeconds )
            {
            _policy.SetHeaders ["Strict-Transport-Security"] = string.Format ( "max-age={0}; includeSubDomains; preload" , maxAge );
            return this;
            }

        /// <summary>
        /// Add X-Content-Type-Options nosniff to all requests.
        /// Can be set to protect against MIME type confusion attacks.
        /// </summary>
        public SecurityHeadersBuilder AddContentTypeOptionsNoSniff ( )
            {
            _policy.SetHeaders ["X-Content-Type-Options"] = "nosniff";
            return this;
            }

        /// <summary>
        /// Add X-Content-Security-Policy to all requests.
        /// </summary>
        public SecurityHeadersBuilder AddContentSecurityPolicy ( )
            {
            var csp = "default-src 'self' 'unsafe-inline' 'unsafe-eval' 'self';";
            _policy.SetHeaders ["Content-Security-Policy"] = csp;
            _policy.SetHeaders ["X-Content-Security-Policy"] = csp;
            return this;
            }

        /// <summary>
        /// Removes the Server header from all responses
        /// </summary>
        public SecurityHeadersBuilder RemoveServerHeader ( )
            {
            _policy.RemoveHeaders.Add ( "Server" );
            return this;
            }

        /// <summary>
        /// Builds a new <see cref="SecurityHeadersPolicy"/> using the entries added.
        /// </summary>
        /// <returns>The constructed <see cref="SecurityHeadersPolicy"/>.</returns>
        public SecurityHeadersPolicy Build ( )
            {
            return _policy;
            }
        }

    public class SecurityHeadersPolicy
        {
        /// <summary>
        /// A dictionary of Header, Value pairs that should be added to all requests
        /// </summary>
        public IDictionary<string , string> SetHeaders { get; } = new Dictionary<string , string> ( );

        /// <summary>
        /// A hashset of Headers that should be removed from all requests
        /// </summary>
        public ISet<string> RemoveHeaders { get; } = new HashSet<string> ( );
        }

    public class SecurityHeadersMiddleware
        {
        private readonly RequestDelegate _next;
        private readonly SecurityHeadersPolicy _policy;

        public SecurityHeadersMiddleware ( RequestDelegate next , SecurityHeadersPolicy policy )
            {
            if ( next == null )
                {
                throw new ArgumentNullException ( nameof ( next ) );
                }

            if ( policy == null )
                {
                throw new ArgumentNullException ( nameof ( policy ) );
                }

            _next = next;
            _policy = policy;
            }

        public async Task Invoke ( HttpContext context )
            {
            if ( context == null )
                {
                throw new ArgumentNullException ( nameof ( context ) );
                }

            var response = context.Response;

            if ( response == null )
                {
                throw new ArgumentNullException ( nameof ( response ) );
                }

            IHeaderDictionary headers = response.Headers;

            foreach ( var headerValuePair in _policy.SetHeaders )
                {
                headers [headerValuePair.Key] = headerValuePair.Value;
                }

            foreach ( var header in _policy.RemoveHeaders )
                {
                headers.Remove ( header );
                }

            await _next ( context );
            }
        }

    public static IApplicationBuilder UseSecurityHeadersMiddleware ( this IApplicationBuilder app , SecurityHeadersBuilder builder )
        {
        if ( app == null )
            {
            throw new ArgumentNullException ( nameof ( app ) );
            }

        if ( builder == null )
            {
            throw new ArgumentNullException ( nameof ( builder ) );
            }

        return app.UseMiddleware<SecurityHeadersMiddleware> ( builder.Build ( ) );
        }
    }

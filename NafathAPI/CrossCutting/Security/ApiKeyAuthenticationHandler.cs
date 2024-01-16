using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Text.Encodings.Web;

namespace NafathAPI.CrossCutting.Security;
public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
    private readonly ILogger<ApiKeyAuthenticationHandler> _logger;

    public ApiKeyAuthenticationHandler (
        IOptionsMonitor<AuthenticationSchemeOptions> options ,
        ILoggerFactory logger ,
        UrlEncoder encoder ,
        ISystemClock clock )
        : base ( options , logger , encoder , clock )
        {
        _logger = logger.CreateLogger<ApiKeyAuthenticationHandler> ( );
        }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync ( )
        {
        if ( !Request.Headers.TryGetValue ( "ApiKey" , out StringValues apiKey ) )
            {
            _logger.LogInformation ( "API Key not found in the request headers." );
            return AuthenticateResult.Fail ( "API Key is missing." );
            }

        // Retrieve a service or component for key validation.
        var apiKeyValidationService = Context.RequestServices.GetRequiredService<IApiKeyValidationService> ( );

        if ( await apiKeyValidationService.ValidateApiKeyAsync ( apiKey ) )
            {
            var claims = new []
            {
                new System.Security.Claims.Claim("ApiKey", apiKey)
            };

            var identity = new System.Security.Claims.ClaimsIdentity ( claims , Scheme.Name );
            var principal = new System.Security.Claims.ClaimsPrincipal ( identity );
            var ticket = new AuthenticationTicket ( principal , Scheme.Name );

            return AuthenticateResult.Success ( ticket );
            }

        _logger.LogWarning ( "Invalid API Key: {ApiKey}" , apiKey );
        return AuthenticateResult.Fail ( "Invalid API Key." );
        }
    }

public interface IApiKeyValidationService
    {
    Task<bool> ValidateApiKeyAsync ( string apiKey );
    }
public class ApiKeyValidationService : IApiKeyValidationService
    {
    private readonly IConfiguration _configuration;

    public ApiKeyValidationService ( IConfiguration configuration )
        {
        _configuration = configuration;
        }

    public Task<bool> ValidateApiKeyAsync ( string apiKey )
        {
        // Retrieve the list of valid API keys from configuration
        var validApiKeys = _configuration ["Jwt:SecretKey"];

        // Check if the provided apiKey is in the list of valid keys
        return Task.FromResult ( validApiKeys.Contains ( apiKey ) );
        }
    }

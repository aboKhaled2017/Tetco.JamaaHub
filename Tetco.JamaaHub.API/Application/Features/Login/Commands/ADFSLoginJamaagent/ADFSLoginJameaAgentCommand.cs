using Abd.CleanArchitecture.Kernel.Domain.Identity;
using Application.Common.Interfaces;
using Application.Common.Settings;
using Domain.Common.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Features.Login.Commands.ADFSLoginJameaAgent
    {
    public record ADFSLoginJameaAgentResponse ( bool success , string token , string message )
        {
        public static ADFSLoginJameaAgentResponse Success ( string token )
            => new ADFSLoginJameaAgentResponse ( true , token , "authorized successfully" );
        public static ADFSLoginJameaAgentResponse Fail ( string errorMess = "Invalid credentials" )
            => new ADFSLoginJameaAgentResponse ( false , null , errorMess );
        }
    public record ADFSLoginJameaAgentCommand : IRequest<ADFSLoginJameaAgentResponse>
        {
        public string JWTToken
            {
            get; set;
            }
        }

    internal sealed class ADFSLoginJameaAgentCommandHandler : IRequestHandler<ADFSLoginJameaAgentCommand , ADFSLoginJameaAgentResponse>
        {
        private readonly ISerializer _serializer;
        private readonly IIdentityService _IdentityService;
        private readonly AuthSetting _authSetting;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ADFSLoginJameaAgentCommandHandler> _logger;

        public ADFSLoginJameaAgentCommandHandler ( AuthSetting authSetting , UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager , IIdentityService identityService , ISerializer serializer , IConfiguration configuration , ILogger<ADFSLoginJameaAgentCommandHandler> logger )
            {
            _authSetting = authSetting;
            _userManager = userManager;
            _signInManager = signInManager;
            _IdentityService = identityService;
            _serializer = serializer;
            _configuration = configuration;
            _logger = logger;
            }

        public async Task<ADFSLoginJameaAgentResponse> Handle ( ADFSLoginJameaAgentCommand request , CancellationToken cancellationToken )
            {
            if ( request is null )
                return ADFSLoginJameaAgentResponse.Fail ( );
            var JWTToken = ExtractToken ( request.JWTToken );
            if ( !string.IsNullOrEmpty ( request.JWTToken ) )
                {

                SecurityToken validatedToken = ValidateToken ( JWTToken );
                if ( validatedToken != null )
                    {
                    var jwtSecurityToken = validatedToken as JwtSecurityToken;
                    var ADFSUser = new ADFSUserInformation
                        {
                        Audience = jwtSecurityToken.Audiences.FirstOrDefault ( ) ,
                        Issuer = jwtSecurityToken.Issuer ,
                        IssuedAt = DateTimeOffset.FromUnixTimeSeconds ( Convert.ToInt64 ( jwtSecurityToken.Claims.FirstOrDefault ( c => c.Type == "iat" )?.Value ) ).UtcDateTime ,
                        NotBefore = DateTimeOffset.FromUnixTimeSeconds ( Convert.ToInt64 ( jwtSecurityToken.Claims.FirstOrDefault ( c => c.Type == "nbf" )?.Value ) ).UtcDateTime ,
                        ExpirationTime = DateTimeOffset.FromUnixTimeSeconds ( Convert.ToInt64 ( jwtSecurityToken.Claims.FirstOrDefault ( c => c.Type == "exp" )?.Value ) ).UtcDateTime ,
                        UserPrincipalName = jwtSecurityToken.Claims.FirstOrDefault ( c => c.Type == "upn" )?.Value ,
                        Email = jwtSecurityToken.Claims.FirstOrDefault ( c => c.Type == "email" )?.Value ,
                        Uniquename = jwtSecurityToken.Claims.FirstOrDefault ( c => c.Type == "unique_name" )?.Value ,
                        AppType = jwtSecurityToken.Claims.FirstOrDefault ( c => c.Type == "apptype" )?.Value ,
                        AppId = jwtSecurityToken.Claims.FirstOrDefault ( c => c.Type == "appid" )?.Value ,
                        AuthMethod = jwtSecurityToken.Claims.FirstOrDefault ( c => c.Type == "authmethod" )?.Value ,
                        AuthTime = DateTime.Parse ( jwtSecurityToken.Claims.FirstOrDefault ( c => c.Type == "auth_time" )?.Value ) ,
                        Ver = jwtSecurityToken.Claims.FirstOrDefault ( c => c.Type == "ver" )?.Value ,
                        Scp = jwtSecurityToken.Claims.FirstOrDefault ( c => c.Type == "scp" )?.Value

                        };
                    if ( !string.IsNullOrEmpty ( ADFSUser.Email ) )
                        {
                        var user = await _userManager.FindByEmailAsync ( ADFSUser.Email );

                        if ( user is not null )
                            {
                            var token = GenerateJwtToken ( user , ADFSUser.ExpirationTime );
                            return ADFSLoginJameaAgentResponse.Success ( token );
                            }
                        }
                    }
                }
            return ADFSLoginJameaAgentResponse.Fail ( );
            }

        private string GenerateJwtToken ( ApplicationUser user , DateTime ExpirationTime )
            {
            var keyBytes = new byte [64];
            Encoding.UTF8.GetBytes ( _authSetting.SecretKey ).CopyTo ( keyBytes , 0 );
            var key = new SymmetricSecurityKey ( keyBytes );
            var creds = new SigningCredentials ( key , SecurityAlgorithms.HmacSha512 );
            var token = new JwtSecurityToken (
                _authSetting.Issuer ,
                _authSetting.Audience ,
                claims: CreateJamaaAgentClaimsFor ( user ) ,
                expires: ExpirationTime ,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler ( ).WriteToken ( token );
            }
        private IEnumerable<Claim> CreateJamaaAgentClaimsFor ( ApplicationUser user )
           => new [] {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, Roles.JamaaAgent),
                    new Claim(JamaaHubClaims.UniversityId, user.UniversityId.ToString()),
               };

        private string ExtractToken ( string input )
            {
            string jwtPattern = @"(?:\bey[A-Za-z0-9-_=]+\.[A-Za-z0-9-_=]+(?:\.[A-Za-z0-9-_.+/=]+)?)";

            Regex regex = new Regex ( jwtPattern );
            Match match = regex.Match ( input );

            if ( match.Success )
                {
                string jwtToken = match.Value;
                return jwtToken;
                }
            return string.Empty;
            }
        private SecurityToken ValidateToken ( string token )
            {
            var tokenHandler = new JwtSecurityTokenHandler ( );
            SecurityToken validatedToken;

            try
                {
                tokenHandler.ValidateToken ( token , GetTokenValidationParameters ( ) , out validatedToken );
                return validatedToken;
                }
            catch ( SecurityTokenExpiredException ex )
                {
                _logger.LogError ( $"Token expired: {ex.Message}" );
                }
            catch ( SecurityTokenNotYetValidException ex )
                {
                _logger.LogError ( $"Token not yet valid: {ex.Message}" );
                }
            catch ( SecurityTokenInvalidSignatureException ex )
                {
                // Handle invalid signature.
                _logger.LogError ( $"Invalid token signature: {ex.Message}" );
                }
            catch ( Exception ex )
                {
                _logger.LogError ( $"Token validation failed: {ex.Message}" );
                }

            return null;
            }

        private TokenValidationParameters GetTokenValidationParameters ( )
            {
            var jwksUri = "https://stsstaging.moe.gov.sa/adfs/discovery/keys";
            var signingKeys = FetchSigningKeys ( jwksUri );
            return new TokenValidationParameters
                {
                ValidateIssuerSigningKey = true ,
                IssuerSigningKeys = signingKeys ,
                ValidateIssuer = true ,
                ValidIssuer = "http://stsstaging.moe.gov.sa/adfs/services/trust" ,
                ValidateAudience = true ,
                ValidAudience = "microsoft:identityserver:0f812887-7429-4778-89d2-992f82e6907c" ,
                ValidateLifetime = true
                };
            }

        private IEnumerable<SecurityKey> FetchSigningKeys ( string jwksUri )
            {
            var handler = new HttpClientHandler
                {
                ServerCertificateCustomValidationCallback = ( message , cert , chain , errors ) => true
                };

            using ( var httpClient = new HttpClient ( handler ) )
                {
                var jwksJson = httpClient.GetStringAsync ( jwksUri ).Result;

                var signingKeys = ConvertJwksToSecurityKeys ( jwksJson );

                return signingKeys;
                }
            }

        public IEnumerable<SecurityKey> ConvertJwksToSecurityKeys ( string jwksJson )
            {
            var keys = new List<SecurityKey> ( );
            var jwks = JObject.Parse ( jwksJson );
            var keyArray = jwks ["keys"] as JArray;

            foreach ( var keyObj in keyArray )
                {
                var kty = keyObj.Value<string> ( "kty" );
                var kid = keyObj.Value<string> ( "kid" );

                switch ( kty )
                    {
                    case "RSA":
                        var rsa = new RSAParameters
                            {
                            Modulus = Base64UrlEncoder.DecodeBytes ( keyObj.Value<string> ( "n" ) ) ,
                            Exponent = Base64UrlEncoder.DecodeBytes ( keyObj.Value<string> ( "e" ) )
                            };

                        var rsaSecurityKey = new RsaSecurityKey ( rsa ) { KeyId = kid };
                        keys.Add ( rsaSecurityKey );
                        break;

                    case "EC":
                        // Extract the curve, X, and Y parameters for the EC key
                        // EC key handling will depend on the specific curve being used
                        // You'll need to map the 'crv' parameter to the appropriate curve in .NET
                        // and then create an ECDsa object with the curve and X, Y points
                        // For example:
                        // var curve = keyObj.Value<string>("crv");
                        // var x = Base64UrlEncoder.DecodeBytes(keyObj.Value<string>("x"));
                        // var y = Base64UrlEncoder.DecodeBytes(keyObj.Value<string>("y"));
                        // ECDsa ecdsa = ...; // Create ECDsa object based on curve, x, and y
                        // var ecdsaSecurityKey = new ECDsaSecurityKey(ecdsa) { KeyId = kid };
                        // keys.Add(ecdsaSecurityKey);
                        break;

                        // Additional key types can be added here.
                    }
                }

            return keys;
            }
        }
    }

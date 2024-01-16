using Abd.CleanArchitecture.Kernel.Domain.Identity;
using Application.Common.Interfaces;
using Application.Common.Settings;
using Domain.Common.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Features.Login.Commands.NafathLoginJameaAgent
    {
    public record NafathLoginJameaAgentResponse ( bool success , string token , string message )
        {
        public static NafathLoginJameaAgentResponse Success ( string token )
            => new NafathLoginJameaAgentResponse ( true , token , "authorized successfully" );
        public static NafathLoginJameaAgentResponse Fail ( string errorMess = "Invalid credentials" )
            => new NafathLoginJameaAgentResponse ( false , null , errorMess );
        }
    public record NafathLoginJameaAgentCommand : IRequest<NafathLoginJameaAgentResponse>
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

    internal sealed class NafathLoginJameaAgentCommandHandler : IRequestHandler<NafathLoginJameaAgentCommand , NafathLoginJameaAgentResponse>
        {
        private readonly ISerializer _serializer;
        private readonly IIdentityService _IdentityService;
        private readonly AuthSetting _authSetting;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IRestClient _restClient;
        public NafathLoginJameaAgentCommandHandler ( AuthSetting authSetting , UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager , IIdentityService identityService , ISerializer serializer , IConfiguration configuration , IRestClient restClient )
            {
            _authSetting = authSetting;
            _userManager = userManager;
            _signInManager = signInManager;
            _IdentityService = identityService;
            _serializer = serializer;
            _configuration = configuration;
            _restClient = restClient;
            }

        public async Task<NafathLoginJameaAgentResponse> Handle ( NafathLoginJameaAgentCommand request , CancellationToken cancellationToken )
            {
            if ( request is null )
                return NafathLoginJameaAgentResponse.Fail ( );

            var tokenHandler = new JwtSecurityTokenHandler ( );
            NafathUserInformation nafathUserInformation = null;
            if ( request.Status == "COMPLETED" )
                {
                var JWTToken = await GetNafathJWTTokenAsync ( request.AccessToken );
                var jsonToken = tokenHandler.ReadToken ( JWTToken ) as JwtSecurityToken;
                if ( jsonToken != null )
                    {
                    var userInfoJson = jsonToken.Payload ["user_info"].ToString ( );
                    nafathUserInformation = _serializer.Deserialize<NafathUserInformation> ( userInfoJson );
                    }
                if ( !string.IsNullOrEmpty ( nafathUserInformation?.UserId ) )
                    {
                    var user = await _IdentityService.GetUserByNationalIdAsync ( nafathUserInformation.UserId );

                    if ( user is not null )
                        {
                        var token = GenerateJwtToken ( user );
                        return NafathLoginJameaAgentResponse.Success ( token );
                        }
                    }
                }

            return NafathLoginJameaAgentResponse.Fail ( );
            }

        private string GenerateJwtToken ( ApplicationUser user )
            {
            var keyBytes = new byte [64];
            Encoding.UTF8.GetBytes ( _authSetting.SecretKey ).CopyTo ( keyBytes , 0 );
            var key = new SymmetricSecurityKey ( keyBytes );
            var creds = new SigningCredentials ( key , SecurityAlgorithms.HmacSha512 );
            var token = new JwtSecurityToken (
                _authSetting.Issuer ,
                _authSetting.Audience ,
                claims: CreateJamaaAgentClaimsFor ( user ) ,
                expires: DateTime.Now.AddHours ( 1 ) ,
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

        private async Task<string> GetNafathJWTTokenAsync ( string AccessToken )
            {
            var apiUrl = _configuration ["Nafath:ServiceURL"];
            var bearerToken = AccessToken;
            var headers = new Dictionary<string , string> ( )
                     {
                         { "Authorization", $"Bearer {bearerToken}" }
                     };
            var jwtToken = await _restClient.GetAsync<string> ( apiUrl , headers );
            return jwtToken;

            }
        }
    }

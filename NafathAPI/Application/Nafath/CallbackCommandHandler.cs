using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using NafathAPI.Common;
using NafathAPI.Common.Interfaces;
using NafathAPI.Domain.Nafath.Dto;
using NafathAPI.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using JwtPayload = NafathAPI.Domain.Nafath.Dto.JwtPayload;

namespace NafathAPI.Application.Nafath
    {

    public class CallbackCommand : IRequest<Result>
        {
        public NafathCallbackRequest Request
            {
            get; set;
            }
        }

    // CheckStatusCommandHandler.cs
    public class CallbackCommandHandler : IRequestHandler<CallbackCommand , Result>
        {
        private readonly ILogger<CallbackCommandHandler> _logger;
        private readonly IDistributedCache _cache;
        private readonly ISerializer _serializer;
        private const string CacheKey = "Naqel_NafathAuth_";
        private readonly IConfiguration _configuration;
        public CallbackCommandHandler ( ILogger<CallbackCommandHandler> logger , IDistributedCache cache , ISerializer serializer , IConfiguration configuration )
            {
            _logger = logger;
            _cache = cache;
            _serializer = serializer;
            _configuration = configuration;
            }

        public async Task<Result> Handle ( CallbackCommand request , CancellationToken cancellationToken )
            {
            try
                {
                // Log the received callback request for auditing and debugging purposes
                _logger.LogInformation ( "Received NafathCallback request: {@CallbackRequest}" , request.Request.Response );

                // Perform the necessary logic for setting Nafath status
                await SetNafathStatus ( request.Request );

                return Result.Success ( "Callback is successfully " );
                }
            catch ( Exception ex )
                {
                // Log the exception
                _logger.LogError ( ex , "An error occurred in NafathCallback." );
                return Result.Failure ( "CallbackFailed" , "Callback is Failed " );
                }
            }



        private async Task SetNafathStatus ( NafathCallbackRequest nafathCallbackRequest )
            {
            #region Read & Validate Token
            var certificateStore = new X509Store ( StoreName.My , StoreLocation.LocalMachine );
            certificateStore.Open ( OpenFlags.ReadOnly );
            //"0de8..........................93c1" Nafath__CertificateThumbprint
            var certificateCollection = certificateStore.Certificates.Find ( X509FindType.FindByThumbprint , _configuration ["Nafath:CertificateThumbprint"] /*Environment.GetEnvironmentVariable ( "Nafath__CertificateThumbprint" )*/ , false );

            certificateStore.Close ( );
            if ( certificateCollection.Count > 0 )
                {
                var key = new X509SecurityKey ( certificateCollection [0] );
                var validationParameters = new TokenValidationParameters ( )
                    {
                    ValidateAudience = false ,
                    ValidateIssuer = false ,
                    ValidateLifetime = true ,
                    ValidateIssuerSigningKey = true ,
                    IssuerSigningKey = key
                    };
                var handler = new JwtSecurityTokenHandler ( );
                var claims = handler.ValidateToken ( nafathCallbackRequest.Response , validationParameters , out var validatedToken );
                if ( claims != null && validatedToken != null )
                    {
                    var tokenS = validatedToken as JwtSecurityToken;
                    //Deserialize decodedString to JwtPayload
                    var jwtPayload = new JwtPayload ( ) { Status = tokenS.Payload ["status"]?.ToString ( ) , TransId = tokenS.Payload ["transId"]?.ToString ( ) };
                    if ( jwtPayload.Status == "COMPLETED" )
                        {
                        jwtPayload.AccessToken = tokenS.Payload ["accessToken"]?.ToString ( );
                        // Update Data Store with New Nafath Status
                        var nafathChallengeRequest = new NafathChallengeRequest ( )
                            {
                            AccessToken = jwtPayload.AccessToken ,
                            TransId = jwtPayload.TransId ,
                            Status = jwtPayload.Status
                            };
                        _logger.LogInformation ( "Nafath Callback AccessToken : " + tokenS.Payload ["accessToken"]?.ToString ( ) );
                        await _cache.SetRecordAsync ( CacheKey + nafathChallengeRequest.TransId.ToString ( ) , nafathChallengeRequest );
                        }
                    }
                }
            #endregion



            }
        }

    }

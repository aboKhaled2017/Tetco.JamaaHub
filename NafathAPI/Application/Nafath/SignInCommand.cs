using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using NafathAPI.Common;
using NafathAPI.Common.Interfaces;
using NafathAPI.Domain.Nafath.Dto;
using NafathAPI.Domain.Nafath.Dto.IntegrationModels;
using NafathAPI.Exceptions;
using NafathAPI.Extensions;


namespace NafathAPI.Application.Nafath
    {
    public class SignInCommand : IRequest<Result<NafathSignInResponse>>
        {
        public NafathSignInRequest Request
            {
            get; set;
            }
        }

    public class SignInCommandHandler : IRequestHandler<SignInCommand , Result<NafathSignInResponse>>
        {
        private readonly IDistributedCache _cache;
        private readonly IRestClient _restClient;
        private readonly ISerializer _serializer;
        private readonly ILogger<SignInCommandHandler> _logger;
        private readonly IConfiguration _configuration;

        private const string CacheKey = "Naqel_NafathAuth_";
        public SignInCommandHandler ( IDistributedCache cache , IRestClient restClient , ISerializer serializer , ILogger<SignInCommandHandler> logger , IConfiguration configuration )
            {
            _cache = cache;
            _restClient = restClient;
            _serializer = serializer;
            _logger = logger;
            _configuration = configuration;
            }

        public async Task<Result<NafathSignInResponse>> Handle ( SignInCommand request , CancellationToken cancellationToken )
            {
            // Send a sign-in response as an SSE event
            NafathSignInResponse signinResponse = await NafathChallengeAsync ( request.Request );

            if ( signinResponse.IsSuccess && !string.IsNullOrWhiteSpace ( signinResponse.Code ) )
                {
                await _cache.SetRecordAsync ( CacheKey + signinResponse.TransId , signinResponse , TimeSpan.FromMinutes ( 1 ) );
                return Result<NafathSignInResponse>.Success ( "token retrived successfully" )
                    .WithData ( signinResponse );
                }
            else
                {
                return Result<NafathSignInResponse>.Failure ( "NafathError" , "error while token  retrived " );
                }
            }
        private async Task<NafathSignInResponse> NafathChallengeAsync ( NafathSignInRequest request )
            {
            //Captcha Validation
            try
                {
                var apiUrl = _configuration ["Nafath:ServiceURL"]; //Environment.GetEnvironmentVariable ( "Nafath__ServiceURL" );
                SignInRequest signInRequest = new SignInRequest ( )
                    {
                    Action = "SpRequest" ,
                    Parameters = new SignInRequestParameters ( )
                        {
                        service = "AdvancedLogin" ,
                        id = request.NationalId
                        }
                    };
                var headers = new Dictionary<string , string> ( )
                    {
                        { "Authorization", _configuration ["Nafath:ApiKe"] } //Environment.GetEnvironmentVariable("Nafath__ApiKey") 
                    };
                var userResponse = await _restClient.PostAsync<SignInResponse , SignInRequest> ( apiUrl , signInRequest , headers );
                if ( userResponse != null && !string.IsNullOrWhiteSpace ( userResponse.transId ) && !string.IsNullOrWhiteSpace ( userResponse.random ) )
                    {
                    _logger.LogInformation ( "Random : " + userResponse.random );

                    //Persist Nafath Response in Data Store

                    var nafathChallengeRequest = new NafathChallengeRequest ( )
                        {
                        TransId = userResponse.transId ,
                        AccessToken = string.Empty ,
                        Status = "WAITING"
                        };
                    await _cache.SetRecordAsync ( CacheKey + nafathChallengeRequest.TransId.ToString ( ) , nafathChallengeRequest , TimeSpan.FromMinutes ( 1 ) );

                    return new NafathSignInResponse ( ) { IsSuccess = true , TransId = userResponse.transId , Code = userResponse.random };
                    }
                else
                    {
                    throw new NafathIntegrationException ( System.Net.HttpStatusCode.BadRequest , _serializer.Serialize ( userResponse ) );
                    }
                }
            catch ( Exception ex )
                {
                throw new NafathIntegrationException ( System.Net.HttpStatusCode.BadRequest , "NAFATHERROR : " + ex.Message );
                }
            }

        }

    }

using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using NafathAPI.Common;
using NafathAPI.Common.Interfaces;
using NafathAPI.Domain.Nafath.Dto;
using NafathAPI.Extensions;

namespace NafathAPI.Application.Nafath
    {
    public class CheckStatusResult
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
    public class CheckStatusCommand : IRequest<Result<CheckStatusResult>>
        {
        public NafathCheckStatusRequest Request
            {
            get; set;
            }
        }

    public class CheckStatusCommandHandler : IRequestHandler<CheckStatusCommand , Result<CheckStatusResult>>
        {
        private readonly IDistributedCache _cache;
        private readonly IRestClient _restClient;
        private readonly ISerializer _serializer;
        private readonly IConfiguration _configuration;
        private const string CacheKey = "Naqel_NafathAuth_";
        public CheckStatusCommandHandler ( IDistributedCache cache , IConfiguration configuration , ISerializer serializer , IRestClient restClient )
            {
            _cache = cache;
            _configuration = configuration;
            _serializer = serializer;
            _restClient = restClient;
            }

        public async Task<Result<CheckStatusResult>> Handle ( CheckStatusCommand request , CancellationToken cancellationToken )
            {
            // Check Status in data store using TransId
            var challengeObj = await _cache.GetRecordAsync<NafathChallengeRequest> ( CacheKey + request.Request.TransId );

            if ( challengeObj != null )
                {
                CheckStatusResult res = new CheckStatusResult ( )
                    {
                    AccessToken = challengeObj.AccessToken ,
                    TransId = challengeObj.TransId ,
                    Status = challengeObj.Status ,
                    };
                return Result<CheckStatusResult>.Success ( "Check status  successfully" )
                .WithData ( res );
                }
            else
                {
                return Result<CheckStatusResult>.Failure ( "CheckStatusError" , "error while token  retrived " );
                }
            }



        }

    }

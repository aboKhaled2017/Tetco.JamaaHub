using Microsoft.IO;
using Newtonsoft.Json;
namespace NafathAPI.CrossCutting.Middlewares
    {
    public class RequestResponseLoggingMiddleware
        {
        private readonly RequestDelegate _next;

        private readonly ILogger _requestLogger;

        private readonly ILogger _responseLogger;

        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public RequestResponseLoggingMiddleware ( RequestDelegate next , ILoggerFactory loggerFactory )
            {
            _next = next;
            _requestLogger = loggerFactory.CreateLogger ( "Request" );
            _responseLogger = loggerFactory.CreateLogger ( "Response" );
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager ( );
            }

        public async Task Invoke ( HttpContext context )
            {
            if ( context.Request.Path.Value.Contains ( "swagger" ) || context.Request.Path.Value.Contains ( ".html" ) )
                {
                await _next ( context );
                return;
                }

            await LogRequest ( context );
            await LogResponse ( context );
            }

        private async Task LogRequest ( HttpContext context )
            {
            context.Request.EnableBuffering ( );
            await using MemoryStream requestStream = _recyclableMemoryStreamManager.GetStream ( );
            await context.Request.Body.CopyToAsync ( requestStream );
            string message = JsonConvert.SerializeObject ( new
                {
                Schema = context.Request.Scheme ,
                context.Request.Host ,
                context.Request.Path ,
                context.Request.QueryString ,
                RequestBody = ReadStreamInChunks ( requestStream )
                } );
            _requestLogger.LogInformation ( message );
            context.Request.Body.Position = 0L;
            }

        private static string ReadStreamInChunks ( Stream stream )
            {
            stream.Seek ( 0L , SeekOrigin.Begin );
            using StringWriter stringWriter = new StringWriter ( );
            using StreamReader streamReader = new StreamReader ( stream );
            char [] buffer = new char [4096];
            int num;
            do
                {
                num = streamReader.ReadBlock ( buffer , 0 , 4096 );
                stringWriter.Write ( buffer , 0 , num );
                }
            while ( num > 0 );
            return stringWriter.ToString ( );
            }

        private async Task LogResponse ( HttpContext context )
            {
            Stream originalBodyStream = context.Response.Body;
            await using MemoryStream responseBody = _recyclableMemoryStreamManager.GetStream ( );
            context.Response.Body = responseBody;
            await _next ( context );
            context.Response.Body.Seek ( 0L , SeekOrigin.Begin );
            string requestBody = await new StreamReader ( context.Response.Body ).ReadToEndAsync ( );
            context.Response.Body.Seek ( 0L , SeekOrigin.Begin );
            string message = JsonConvert.SerializeObject ( new
                {
                Schema = context.Request.Scheme ,
                context.Request.Host ,
                context.Request.Path ,
                context.Request.QueryString ,
                context.Response.Headers ,
                context.Response.StatusCode ,
                RequestBody = requestBody
                } );
            _responseLogger.LogInformation ( message );
            await responseBody.CopyToAsync ( originalBodyStream );
            }
        }
    }

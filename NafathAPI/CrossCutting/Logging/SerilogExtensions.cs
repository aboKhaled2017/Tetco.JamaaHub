using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace NafathAPI.CrossCutting.Logging;
public static class SerilogExtensions
    {
    public static LoggerConfiguration WithSerilogContextEnricher ( this LoggerEnrichmentConfiguration enrichmentConfiguration )
        {
        if ( enrichmentConfiguration == null )
            throw new ArgumentNullException ( nameof ( enrichmentConfiguration ) );
        return enrichmentConfiguration.With<SerilogContextEnricher> ( );
        }
    }

public class SerilogContextEnricher : ILogEventEnricher
    {
    private readonly IHttpContextAccessor _contextAccessor;

    public SerilogContextEnricher ( ) : this ( new HttpContextAccessor ( ) )
        {
        }

    public SerilogContextEnricher ( IHttpContextAccessor contextAccessor )
        {
        _contextAccessor = contextAccessor;
        }

    public void Enrich ( LogEvent logEvent , ILogEventPropertyFactory propertyFactory )
        {
        if ( _contextAccessor.HttpContext == null )
            return;

        //logEvent.AddOrUpdateProperty ( propertyFactory.CreateProperty ( "AppName" , "Nafath-API" ) );

        //UserName
        // string userName = _contextAccessor.HttpContext?.User?.FindFirstValue ( "idn" ).ToString ( );
        //var userNameProp = propertyFactory.CreateProperty ( "UserName" , userName );
        //logEvent.AddOrUpdateProperty ( userNameProp );

        //ServerName
        var serverNameProp = propertyFactory.CreateProperty ( "ServerName" , Environment.MachineName );
        logEvent.AddOrUpdateProperty ( serverNameProp );

        //Client IP
        var clientIPProp = propertyFactory.CreateProperty ( "ClientIP" , _contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4 ( ) );
        logEvent.AddOrUpdateProperty ( clientIPProp );

        //Claims
        var claimsProp = propertyFactory.CreateProperty ( "Claims" , _contextAccessor?.HttpContext?.User?.Claims?.Select ( c => new
            {
            c.Type ,
            c.Value
            } ).ToList ( ) );
        logEvent.AddOrUpdateProperty ( claimsProp );

        var request = _contextAccessor?.HttpContext?.Request;
        if ( request != null )
            {
            //Form Data
            if ( request.HasFormContentType )
                {
                var finaleFormData = new Dictionary<string , string> ( );
                var formData = _contextAccessor?.HttpContext.Request.Form.ToDictionary ( v => v.Key , v => v.Value.ToString ( ) );
                foreach ( var formDataItem in formData )
                    {
                    if ( formDataItem.Key.Trim ( ).ToLower ( ).Contains ( "password" ) )
                        finaleFormData.Add ( formDataItem.Key , "*******" );
                    else
                        finaleFormData.Add ( formDataItem.Key , formDataItem.Value );
                    }
                var requestFormProp = propertyFactory.CreateProperty ( "RequestForm" , finaleFormData );
                logEvent.AddOrUpdateProperty ( requestFormProp );
                }

            //Request Headers
            var requestHeaders = request.Headers
                                .ToDictionary ( h => h.Key , h => h.Value.ToString ( ) )
                                .Where ( d => d.Key.ToLower ( ) != "cookie" )
                                .Select ( d => new
                                    {
                                    Name = d.Key ,
                                    Content = d.Value
                                    } ).ToList ( );
            var requestHeaderProp = propertyFactory.CreateProperty ( "RequestHeaders" , requestHeaders );
            logEvent.AddOrUpdateProperty ( requestHeaderProp );

            //Cookie
            var cookieProp = propertyFactory.CreateProperty ( "Cookies" , request.Cookies.Select ( c => new
                {
                Name = c.Key ,
                Content = c.Value
                } ).ToList ( ) );
            logEvent.AddOrUpdateProperty ( cookieProp );

            var userAgent = request.Headers ["User-Agent"].FirstOrDefault ( );
            if ( userAgent != default )
                {
                //User-Agent
                var userAgentProp = propertyFactory.CreateProperty ( "User-Agent" , userAgent );
                logEvent.AddOrUpdateProperty ( userAgentProp );

                //Referer
                var refererProp = propertyFactory.CreateProperty ( "Referer" , request.Headers ["Referer"].FirstOrDefault ( ) );
                logEvent.AddOrUpdateProperty ( refererProp );
                }

            //Host
            var hostProp = propertyFactory.CreateProperty ( "Host" , request.Host );
            logEvent.AddOrUpdateProperty ( hostProp );

            //Protocol
            var protocolProp = propertyFactory.CreateProperty ( "Protocol" , request.Protocol );
            logEvent.AddOrUpdateProperty ( protocolProp );

            //Scheme
            var schemeProp = propertyFactory.CreateProperty ( "Scheme" , request.Scheme );
            logEvent.AddOrUpdateProperty ( schemeProp );

            //QueryString
            if ( request.QueryString.HasValue )
                {
                var queryStringProp = propertyFactory.CreateProperty ( "QueryString" , request.QueryString.Value );
                logEvent.AddOrUpdateProperty ( queryStringProp );
                }

            //EndPoint Name
            var endpoint = _contextAccessor?.HttpContext.GetEndpoint ( );
            if ( endpoint is object )
                {
                var endPointProp = propertyFactory.CreateProperty ( "EndpointName" , endpoint.DisplayName );
                logEvent.AddOrUpdateProperty ( endPointProp );
                }

            //Content-Type
            var response = _contextAccessor?.HttpContext?.Response;
            if ( response != null )
                {
                var contentTypeProp = propertyFactory.CreateProperty ( "ContentType" , response?.ContentType ?? string.Empty );
                logEvent.AddOrUpdateProperty ( contentTypeProp );
                }
            }
        }
    }

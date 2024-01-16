using NafathAPI.Common.Interfaces;
using System.Text;

namespace NafathAPI.Services
    {
    public class RestClient : IRestClient
        {
        private readonly ISerializer _serializer;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<RestClient> _logger;

        private Encoding _encoding;

        public RestClient ( ISerializer serializer , IHttpClientFactory httpClientFactory , ILogger<RestClient> logger )
            {
            _serializer = serializer;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _encoding = Encoding.UTF8;
            }

        #region GET

        public async Task<TResponse> GetAsync<TResponse> ( string url )
            {
            var start = DateTime.Now;
            var response = await SendAsync<TResponse> ( client => client.GetAsync ( url ) , url );

            return response;
            }
        public async Task<TResponse> GetAsync<TResponse> ( string url , Dictionary<string , string> headers )
            {
            var start = DateTime.Now;
            var response = await SendAsync<TResponse> ( client => client.GetAsync ( url ) , url , headers );

            return response;
            }

        public async Task GetAndForgetAsync<TResponse> ( string url )
            {
            var start = DateTime.Now;
            var response = await SendAsync<TResponse> ( client => client.GetAsync ( url ) , url );
            }
        #endregion

        #region POST

        public async Task<TResponse> PostAsync<TResponse, TRequest> ( string url , TRequest integrationRequest )
            {
            var start = DateTime.Now;
            var content = await SerializeToStringContentAsync ( integrationRequest );
            var response = await SendAsync<TResponse> ( client => client.PostAsync ( url , content ) , url );

            return response;
            }
        public async Task<TResponse> PostAsync<TResponse, TRequest> ( string url , TRequest integrationRequest , Dictionary<string , string> headers )
            {
            var start = DateTime.Now;
            var content = await SerializeToStringContentAsync ( integrationRequest );
            var response = await SendAsync<TResponse> ( client => client.PostAsync ( url , content ) , url , headers );

            return response;
            }
        public async Task<TResponse> PostAsync<TResponse> ( string url )
            {
            var start = DateTime.Now;
            var content = new StringContent ( string.Empty , _encoding , "application/json" );
            var response = await SendAsync<TResponse> ( client => client.PostAsync ( url , content ) , url );

            return response;
            }
        public async Task<TResponse> PostAsync<TResponse> ( string url , Dictionary<string , string> headers )
            {
            var start = DateTime.Now;
            var content = new StringContent ( string.Empty , _encoding , "application/json" );
            var response = await SendAsync<TResponse> ( client => client.PostAsync ( url , content ) , url , headers );

            return response;
            }
        public async Task PostAsync<TRequest> ( string url , TRequest integrationRequest )
            {
            var start = DateTime.Now;
            var content = await SerializeToStringContentAsync ( integrationRequest );
            var response = await SendAsync<TRequest> ( client => client.PostAsync ( url , content ) , url );
            }
        public async Task<TResponse?> PostAsync<TResponse> ( string url , IFormFile file )
            {
            var start = DateTime.Now;
            using ( var ms = new MemoryStream ( ) )
                {
                var length = file.Length;
                var fileStream = file.OpenReadStream ( );
                byte [] bytes = new byte [length];
                fileStream.Read ( bytes , 0 , ( int ) file.Length );
                string fileContent = Convert.ToBase64String ( bytes , 0 , bytes.Length );
                var content = new FormUrlEncodedContent ( new []
                {
                        new KeyValuePair<string, string>("content", fileContent),
                        new KeyValuePair<string, string>("file", file.FileName)
                    } );
                var response = await SendAsync<TResponse> ( client => client.PostAsync ( url , content ) , url );
                return response;
                }
            }
        public async Task<TResponse?> PostAsync<TResponse> ( string url , IFormFile file , Dictionary<string , string> headers )
            {
            var start = DateTime.Now;
            using ( var ms = new MemoryStream ( ) )
                {
                var length = file.Length;
                var fileStream = file.OpenReadStream ( );
                byte [] bytes = new byte [length];
                fileStream.Read ( bytes , 0 , ( int ) file.Length );
                string fileContent = Convert.ToBase64String ( bytes , 0 , bytes.Length );
                var content = new FormUrlEncodedContent ( new []
                {
                        new KeyValuePair<string, string>("content", fileContent),
                        new KeyValuePair<string, string>("file", file.FileName),
                        new KeyValuePair<string, string>("contentType", file.ContentType),
                    } );
                var response = await SendAsync<TResponse> ( client => client.PostAsync ( url , content ) , url , headers );
                return response;
                }
            }
        #endregion

        #region PUT

        public async Task<TResponse> PutAsync<TResponse, TRequest> ( string url , TRequest integrationRequest )
            {
            var start = DateTime.Now;
            var content = await SerializeToStringContentAsync ( integrationRequest );
            var response = await SendAsync<TResponse> ( client => client.PutAsync ( url , content ) , url );

            return response;
            }
        public async Task<TResponse> PutAsync<TResponse, TRequest> ( string url , TRequest integrationRequest , Dictionary<string , string> headers )
            {
            var start = DateTime.Now;
            var content = await SerializeToStringContentAsync ( integrationRequest );
            var response = await SendAsync<TResponse> ( client => client.PutAsync ( url , content ) , url , headers );

            return response;
            }
        public async Task PutAsync<TRequest> ( string url , TRequest integrationRequest )
            {
            var start = DateTime.Now;
            var content = await SerializeToStringContentAsync ( integrationRequest );
            var response = await SendAsync<TRequest> ( client => client.PutAsync ( url , content ) , url );
            }
        #endregion

        #region PATCH

        public async Task<TResponse> PatchAsync<TResponse, TRequest> ( string url , TRequest integrationRequest )
            {
            var start = DateTime.Now;
            var content = await SerializeToStringContentAsync ( integrationRequest );
            var response = await SendAsync<TResponse> ( client => client.PatchAsync ( url , content ) , url );

            return response;
            }
        public async Task PatchAsync<TRequest> ( string url , TRequest integrationRequest )
            {
            var start = DateTime.Now;
            var content = await SerializeToStringContentAsync ( integrationRequest );
            var response = await SendAsync<TRequest> ( client => client.PatchAsync ( url , content ) , url );
            }
        #endregion

        #region DELETE
        public async Task<TResponse> DeleteAsync<TResponse> ( string url )
            {
            var start = DateTime.Now;
            var response = await SendAsync<TResponse> ( client => client.DeleteAsync ( url ) , url );

            return response;
            }
        public async Task<TResponse> DeleteAsync<TResponse> ( string url , Dictionary<string , string> headers )
            {
            var start = DateTime.Now;
            var response = await SendAsync<TResponse> ( client => client.DeleteAsync ( url ) , url , headers );

            return response;
            }
        public async Task DeleteAndForgetAsync<TResponse> ( string url )
            {
            var start = DateTime.Now;
            var response = await SendAsync<TResponse> ( client => client.DeleteAsync ( url ) , url );
            }
        #endregion

        #region Helpers
        private async Task<T> SendAsync<T> ( Func<HttpClient , Task<HttpResponseMessage>> senderFunc , string targetUrl , Dictionary<string , string> headers = null )
            {
            HttpClient client = _httpClientFactory.CreateClient ( );

            foreach ( var header in headers )
                {
                client.DefaultRequestHeaders.TryAddWithoutValidation ( header.Key , header.Value );
                }


            var response = await senderFunc ( client );

            if ( response.IsSuccessStatusCode )
                {
                return await DeserializeFromHttpContent<T> ( response.Content , true );
                }
            else
                {
                var errorAsJson = await DeserializeFromHttpContent<string> ( response.Content );

                throw new Exception ( errorAsJson );
                }

            }
        private async Task<StringContent> SerializeToStringContentAsync<T> ( T data )
            {
            var json = await _serializer.SerializeAsync ( data );

            return new StringContent ( json , _encoding , "application/json" );
            }

        private async Task<T> DeserializeFromHttpContent<T> ( HttpContent content , bool propertyNameCaseInsensitive = false )
            {
            var contentAsString = await content.ReadAsStringAsync ( );

            if ( typeof ( T ) == typeof ( string ) )
                {
                return ( T ) Convert.ChangeType ( contentAsString , typeof ( T ) );
                }
            if ( !string.IsNullOrEmpty ( contentAsString ) )
                {
                var data = await _serializer.DeserializeAsync<T> ( contentAsString );

                return data;
                }

            return default;
            }

        #endregion
        }
    }

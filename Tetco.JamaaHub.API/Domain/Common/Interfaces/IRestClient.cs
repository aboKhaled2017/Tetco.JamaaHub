using Microsoft.AspNetCore.Http;

namespace Domain.Common.Interfaces
    {
    public interface IRestClient
        {
        #region GET

        Task<TResponse> GetAsync<TResponse> ( string url );
        Task<TResponse> GetAsync<TResponse> ( string url , Dictionary<string , string> headers );

        Task GetAndForgetAsync<TResponse> ( string url );

        #endregion

        #region POST

        Task<TResponse> PostAsync<TResponse, TRequest> ( string url , TRequest request );
        Task<TResponse> PostAsync<TResponse, TRequest> ( string url , TRequest request , Dictionary<string , string> headers );

        Task<TResponse> PostAsync<TResponse> ( string url );
        Task<TResponse> PostAsync<TResponse> ( string url , Dictionary<string , string> headers );

        Task PostAsync<TRequest> ( string url , TRequest request );

        Task<TResponse> PostAsync<TResponse> ( string url , IFormFile file );
        Task<TResponse> PostAsync<TResponse> ( string url , IFormFile file , Dictionary<string , string> headers );


        #endregion

        #region PUT

        Task<TResponse> PutAsync<TResponse, TRequest> ( string url , TRequest request );
        Task<TResponse> PutAsync<TResponse, TRequest> ( string url , TRequest request , Dictionary<string , string> headers );

        Task PutAsync<TRequest> ( string url , TRequest request );

        #endregion

        #region PATCH

        Task<TResponse> PatchAsync<TResponse, TRequest> ( string url , TRequest request );

        Task PatchAsync<TRequest> ( string url , TRequest request );

        #endregion

        #region DELETE

        Task<TResponse> DeleteAsync<TResponse> ( string url );
        Task<TResponse> DeleteAsync<TResponse> ( string url , Dictionary<string , string> headers );

        Task DeleteAndForgetAsync<TResponse> ( string url );

        #endregion
        }
    }

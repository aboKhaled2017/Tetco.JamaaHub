using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace NafathAPI.Extensions
    {
    public static class DistributedCachExtensions
        {
        /// <summary>
        /// Set an item to the distributed cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="recordId"></param>
        /// <param name="data"></param>
        /// <param name="absoluteExpireTime"></param>
        /// <param name="unusedExpireTime"></param>
        /// <returns></returns>
        public static async Task SetRecordAsync<T> ( this IDistributedCache cache ,
            string recordId ,
            T data ,
            TimeSpan? absoluteExpireTime = null ,
            TimeSpan? unusedExpireTime = null )
            {
            DistributedCacheEntryOptions options = new ( )
                {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes ( 10 ) ,
                SlidingExpiration = unusedExpireTime
                };

            var jsonData = JsonConvert.SerializeObject ( data , Formatting.Indented ,
                new JsonSerializerSettings
                    {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    } );
            await cache.SetStringAsync ( recordId , jsonData , options );
            }

        /// <summary>
        /// Get an item from the distributed cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public static async Task<T> GetRecordAsync<T> ( this IDistributedCache cache , string recordId )
            {
            var jsonData = await cache.GetStringAsync ( recordId );

            if ( string.IsNullOrWhiteSpace ( jsonData ) )
                {
                return default;
                }

            return JsonConvert.DeserializeObject<T> ( jsonData );
            }
        }
    }

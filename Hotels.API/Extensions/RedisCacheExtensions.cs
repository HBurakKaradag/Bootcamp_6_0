using System.Collections.Generic;
using System.Globalization;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Hotels.API.Extensions
{
    public static class RedisCacheExtensions
    {

        private static DistributedCacheEntryOptions CreateOptionOnMunite(int munite)
            => new DistributedCacheEntryOptions{ AbsoluteExpiration = DateTime.Now.AddMinutes(munite) };

        public static async Task<TResult> GetCachedValueAsync<TResult>(this IDistributedCache cache, string key)
            where TResult : class
        {

            if(string.IsNullOrWhiteSpace(key))
                return null;

            var cachedItem = await cache.GetStringAsync(key);
            if(string.IsNullOrWhiteSpace(cachedItem))
                return null;
            
            return JsonConvert.DeserializeObject<TResult>(cachedItem);
        }

        public static async Task<TResult> SetCacheAsync<TResult>(this IDistributedCache cache, 
                                                                      string key,
                                                                      Func<TResult> action,
                                                                      DistributedCacheEntryOptions cacheOption = null)
            where TResult : class
        {
            if(string.IsNullOrWhiteSpace(key))
            return null;

            var item = action();
            if(item != null && item != default(TResult))
            {
                var value = JsonConvert.SerializeObject(item);

                if(cacheOption != null )
                {
                   await  cache.SetStringAsync(key,value,cacheOption);
                }
                else 
                {
                    await cache.SetStringAsync(key,value);
                }
            }

            return item;

        }

        public static async Task<bool> SetCacheAsync<TResult>(this IDistributedCache cache,
                                                                   string key,
                                                                   TResult item,
                                                                   int expireMunite
                                                                )
            where  TResult : class
            {
                if(string.IsNullOrWhiteSpace(key))
                    return false;
                
                if(item == null || item == default(TResult))
                    return false;

                var value  = JsonConvert.SerializeObject(item);
                await cache.SetStringAsync(key,value, CreateOptionOnMunite(expireMunite) );

                return true;
            }


        public static async Task<TResult> GetOrSetCacheAsync<TResult>(this IDistributedCache cache,
                                                                           string key,
                                                                           Func<TResult> action,
                                                                           DistributedCacheEntryOptions cacheOption = null)
                where TResult : class
        {
            var cachedItem = await cache.GetCachedValueAsync<TResult>(key);
            if(cachedItem == null)
                return await cache.SetCacheAsync(key,action,cacheOption);

            return cachedItem;
        }
   
    }
}

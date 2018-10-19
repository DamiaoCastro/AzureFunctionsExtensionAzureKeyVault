using System;
using System.Collections.Concurrent;

namespace AzureFunctions.Extensions.KeyVault.Services
{
    internal class ObjectCacheService<T> where T : class
    {

        private ConcurrentDictionary<string, ExpiringObject> objectCache = new ConcurrentDictionary<string, ExpiringObject>();

        public T GetObject(string key, int totalMinutesToCache)
        {
            if (objectCache.ContainsKey(key))
            {
                var expiringBigQueryService = objectCache[key];
                if ((DateTime.UtcNow - expiringBigQueryService.CreatedUtc).TotalMinutes > totalMinutesToCache)
                {
                    //log expired
                }
                else
                {
                    return expiringBigQueryService.CachedObject;
                }
            }

            return null;
        }

        internal void SetObject(string key, T objectToCache)
        {
            var expiringObject = new ExpiringObject(DateTime.UtcNow, objectToCache);
            objectCache.AddOrUpdate(key, expiringObject,
                (newkey, oldValue) =>
                {

                    if (oldValue.CachedObject is IDisposable)
                    {
                        ((IDisposable)oldValue.CachedObject).Dispose();
                    }

                    return expiringObject;
                });
        }

        private class ExpiringObject
        {

            public ExpiringObject(DateTime createdUtc, T objectToCache)
            {
                CreatedUtc = createdUtc;
                CachedObject = objectToCache;
            }

            public DateTime CreatedUtc { get; }
            public T CachedObject { get; }

        }

    }
}

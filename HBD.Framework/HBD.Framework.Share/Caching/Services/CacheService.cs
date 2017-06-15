#region using

using System;
using System.Collections;
using System.Collections.Generic;
using HBD.Framework.Core;
using HBD.Framework.Caching.Providers;

#endregion

namespace HBD.Framework.Caching.Services
{
    public class CacheService<TCache> : ICacheService where TCache : ICacheProvider, new()
    {
        private readonly TCache _cache;

        public CacheService() : this(new TCache())
        {
        }

        public CacheService(TCache cacheProvider)
        {
            Guard.ArgumentIsNotNull(cacheProvider, nameof(cacheProvider));
            _cache = cacheProvider;
        }

        /// <summary>
        ///     Default TImeSpan is 8 hours.
        /// </summary>
        public virtual TimeSpan ExpirationTime { get; set; } = new TimeSpan(8, 0, 0);

        public virtual void AddOrUpdate(string key, object item, string regionName = null)
            => AddOrUpdate(key, item, TimeSpan.MinValue, regionName);

        public virtual void AddOrUpdate(string key, object item, TimeSpan expiry, string regionName = null)
        {
            Guard.ArgumentIsNotNull(key, nameof(key));
            Guard.ArgumentIsNotNull(item, nameof(item));

            _cache.Set(key, item, expiry != TimeSpan.MinValue ? expiry : ExpirationTime, regionName);
        }

        public virtual object Get(string key, string regionName = null) => _cache.Get(key, regionName);

        public virtual T Get<T>(string key, string regionName = null) => _cache.Get<T>(key, regionName);

        public virtual void Remove(string key, string regionName = null)
        {
            Guard.ArgumentIsNotNull(key, nameof(key));
            _cache.Remove(key, regionName);
        }

        public bool IsContains(string key, string regionName = null) => _cache.Contains(key, regionName);

        public virtual void Dispose() => Dispose(true);

        protected virtual void Dispose(bool isDisposing)
            => _cache?.Dispose();

        public virtual IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            => _cache.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public virtual IEnumerable<string> GetAllKeys(string regionName = null)
            => _cache.GetAllKeys(regionName);
    }
}
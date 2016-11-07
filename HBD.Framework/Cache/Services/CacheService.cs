using HBD.Framework.Cache.Providers;
using HBD.Framework.Core;
using System;
using System.Collections;
using System.Collections.Generic;

namespace HBD.Framework.Cache.Services
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

        public virtual void Dispose() => _cache?.Dispose();

        public virtual IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            => this._cache.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public virtual IEnumerable<string> GetAllKeys(string regionName = null)
            => this._cache.GetAllKeys(regionName);
    }
}
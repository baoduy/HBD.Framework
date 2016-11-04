using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace HBD.Framework.Cache.Providers
{
    public class RegionMemoryCache : ObjectCache, ICacheProvider
    {
        private bool _isDisposed = false;
        private volatile IDictionary<string, MemoryCache> _regions = new Dictionary<string, MemoryCache>();
        public override DefaultCacheCapabilities DefaultCacheCapabilities => GetCache(null).DefaultCacheCapabilities;
        public override string Name => "RegionMemoryCache";

        public override object this[string key]
        {
            get { return GetCache(null)[key]; }
            set { GetCache(null)[key] = value; }
        }

        public override bool Contains(string key, string regionName = null) => GetCache(regionName).Contains(key);

        public override object Get(string key, string regionName = null) => GetCache(regionName).Get(key);

        public virtual T Get<T>(string key, string regionName = null)
        {
            try
            {
                var value = Get(key, regionName);
                if (value.IsNull()) return default(T);
                return (T)value;
            }
            catch
            {
                return default(T);
            }
        }

        public void Set(string key, object value, TimeSpan expiration, string regionName = null)
            => Set(key, value, DateTimeOffset.Now.Add(expiration), regionName);

        public override object Remove(string key, string regionName = null) => GetCache(regionName).Remove(key);

        public override long GetCount(string regionName = null) => GetCache(regionName).GetCount();

        public IEnumerable<string> GetAllKeys(string regionName = null)
        {
            var cahe = GetCache(regionName);
            foreach (var item in cahe)
                yield return item.Key;
        }

        public virtual void Dispose()
        {
            _isDisposed = true;
            foreach (var region in _regions)
                region.Value.Dispose();
            _regions.Clear();
        }

        private MemoryCache GetCache(string regionName)
        {
            if (_isDisposed) throw new ObjectDisposedException(this.GetType().FullName);

            if (regionName.IsNull()) regionName = Name;
            if (_regions.ContainsKey(regionName)) return _regions[regionName];
            return _regions[regionName] = new MemoryCache(regionName);
        }

        public override CacheEntryChangeMonitor CreateCacheEntryChangeMonitor(IEnumerable<string> keys,
            string regionName = null) => GetCache(regionName).CreateCacheEntryChangeMonitor(keys);

        protected override IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            => _regions.SelectMany(m => m.Value).GetEnumerator();

        public override object AddOrGetExisting(string key, object value, DateTimeOffset absoluteExpiration,
            string regionName = null) => GetCache(regionName).AddOrGetExisting(key, value, absoluteExpiration);

        public override CacheItem AddOrGetExisting(CacheItem value, CacheItemPolicy policy)
            => GetCache(value.RegionName).AddOrGetExisting(value, policy);

        public override object AddOrGetExisting(string key, object value, CacheItemPolicy policy,
            string regionName = null) => GetCache(regionName).AddOrGetExisting(key, value, policy);

        public override CacheItem GetCacheItem(string key, string regionName = null)
            => GetCache(regionName).GetCacheItem(key);

        public override void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
            => GetCache(regionName).Set(key, value, absoluteExpiration);

        public override void Set(CacheItem item, CacheItemPolicy policy) => GetCache(item.RegionName).Set(item, policy);

        public override void Set(string key, object value, CacheItemPolicy policy, string regionName = null)
            => GetCache(regionName).Set(key, value, policy);

        public override IDictionary<string, object> GetValues(IEnumerable<string> keys, string regionName = null)
            => GetCache(regionName).GetValues(keys);
    }
}
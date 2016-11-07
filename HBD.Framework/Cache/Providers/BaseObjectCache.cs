using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using HBD.Framework.Core;

namespace HBD.Framework.Cache.Providers
{
    public abstract class BaseObjectCache : ObjectCache
    {
        private ICacheStorageProvider _storeageProvider;
        public ICacheStorageProvider StoreageProvider => _storeageProvider ?? (_storeageProvider = CreateCacheStorageProvider(this.Name));

        protected BaseObjectCache(string name) { this.Name = name; }

        protected BaseObjectCache(ICacheStorageProvider storeageProvider)
        {
            Guard.ArgumentIsNotNull(storeageProvider, nameof(storeageProvider));
            this.Name = storeageProvider.Name;
            _storeageProvider = storeageProvider;
        }

        protected abstract ICacheStorageProvider CreateCacheStorageProvider(string name);

        public override CacheEntryChangeMonitor CreateCacheEntryChangeMonitor(IEnumerable<string> keys, string regionName = null)
        {
            Guard.ArgumentIsNotNull(keys, nameof(keys));
            var list = new List<string>(keys);
            Guard.CollectionMustNotEmpty(list, nameof(list));
            Guard.AllItemsMustNotEmpty(list, nameof(list));

            return new CacheStorageEntryChangeMonitor(list.AsReadOnly(), regionName, this);
        }

        protected override IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            => StoreageProvider.GetEnumerator();

        public override bool Contains(string key, string regionName = null)
        {
            throw new NotImplementedException();
        }

        public override object AddOrGetExisting(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            throw new NotImplementedException();
        }

        public override CacheItem AddOrGetExisting(CacheItem value, CacheItemPolicy policy)
        {
            throw new NotImplementedException();
        }

        public override object AddOrGetExisting(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            throw new NotImplementedException();
        }

        public override object Get(string key, string regionName = null)
        {
            throw new NotImplementedException();
        }

        public override CacheItem GetCacheItem(string key, string regionName = null)
        {
            throw new NotImplementedException();
        }

        public override void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = absoluteExpiration;
            Set(key, value, policy);

        }

        public override void Set(CacheItem item, CacheItemPolicy policy) => Set(item.Key, item.Value, policy);

        public override void Set(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            Guard.ArgumentIsNotNull(key, nameof(key));
            var absExp = ObjectCache.InfiniteAbsoluteExpiration;
            var slidingExp = ObjectCache.NoSlidingExpiration;
            var priority = CacheItemPriority.Default;
            Collection<ChangeMonitor> changeMonitors = null;
            CacheEntryRemovedCallback removedCallback = null;

            if (policy != null)
            {
                //ValidatePolicy(policy);
                if (policy.UpdateCallback != null)
                {
                    Set(key, value, policy.ChangeMonitors, policy.AbsoluteExpiration, policy.SlidingExpiration, policy.UpdateCallback);
                    return;
                }
                absExp = policy.AbsoluteExpiration;
                slidingExp = policy.SlidingExpiration;
                priority = policy.Priority;
                changeMonitors = policy.ChangeMonitors;
                removedCallback = policy.RemovedCallback;
            }

            //MemoryCacheKey cacheKey = new MemoryCacheKey(key);
            //MemoryCacheStore store = GetStore(cacheKey);
            //store.Set(cacheKey, new MemoryCacheEntry(key, value, absExp, slidingExp, priority, changeMonitors, removedCallback, this));
        }

        public override IDictionary<string, object> GetValues(IEnumerable<string> keys, string regionName = null)
        {
            throw new NotImplementedException();
        }

        public override object Remove(string key, string regionName = null)
        {
            throw new NotImplementedException();
        }

        public override long GetCount(string regionName = null)
        {
            throw new NotImplementedException();
        }

        public override DefaultCacheCapabilities DefaultCacheCapabilities
            => DefaultCacheCapabilities.InMemoryProvider | DefaultCacheCapabilities.CacheEntryChangeMonitors
            | DefaultCacheCapabilities.AbsoluteExpirations | DefaultCacheCapabilities.SlidingExpirations
            | DefaultCacheCapabilities.CacheEntryUpdateCallback | DefaultCacheCapabilities.CacheEntryRemovedCallback;

        public override string Name { get; }

        public override object this[string key]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}

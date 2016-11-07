using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace HBD.Framework.Cache.Providers
{
    internal sealed class CacheStorageEntryChangeMonitor: CacheEntryChangeMonitor
    {
        private ReadOnlyCollection<string> readOnlyCollection;
        private string regionName;
        private BaseObjectCache baseObjectCache;

        public CacheStorageEntryChangeMonitor(ReadOnlyCollection<string> readOnlyCollection, string regionName, BaseObjectCache baseObjectCache)
        {
            this.readOnlyCollection = readOnlyCollection;
            this.regionName = regionName;
            this.baseObjectCache = baseObjectCache;
        }

        protected override void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }

        public override string UniqueId { get; }
        public override ReadOnlyCollection<string> CacheKeys { get; }
        public override DateTimeOffset LastModified { get; }
        public override string RegionName { get; }
    }
}

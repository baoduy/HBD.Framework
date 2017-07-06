using System;
using System.Collections.Generic;

namespace HBD.Framework.Caching.Providers
{
    public interface ICacheStorageProvider : IEnumerable<KeyValuePair<string, object>>, IDisposable
    {
        string Name { get; }
        void AddOrUpdate(string key, object value, string regionName = null);
        object Get(string key, string regionName = null);
        void Remove(string key, string regionName = null);
        void Delete();
    }
}

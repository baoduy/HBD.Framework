#region

using System;
using System.Collections.Generic;

#endregion

namespace HBD.Framework.Cache.Providers
{
    public interface ICacheProvider : IEnumerable<KeyValuePair<string, object>>, IDisposable
    {
        bool Contains(string key, string regionName = null);

        object Get(string key, string regionName = null);

        T Get<T>(string key, string regionName = null);

        void Set(string key, object value, TimeSpan expiration, string regionName = null);

        object Remove(string key, string regionName = null);

        long GetCount(string regionName = null);

        IEnumerable<string> GetAllKeys(string regionName = null);
    }
}
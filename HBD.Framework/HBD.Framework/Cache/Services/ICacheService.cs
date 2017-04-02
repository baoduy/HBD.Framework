#region using

using System;
using System.Collections.Generic;

#endregion

namespace HBD.Framework.Cache.Services
{
    public interface ICacheService : IEnumerable<KeyValuePair<string, object>>, IDisposable
    {
        TimeSpan ExpirationTime { get; set; }

        object Get(string key, string regionName = null);

        T Get<T>(string key, string regionName = null);

        IEnumerable<string> GetAllKeys(string regionName = null);

        void AddOrUpdate(string key, object item, string regionName = null);

        void AddOrUpdate(string key, object item, TimeSpan expiry, string regionName = null);

        bool IsContains(string key, string regionName = null);

        void Remove(string key, string regionName = null);
    }
}
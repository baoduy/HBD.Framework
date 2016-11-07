using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBD.Framework.Cache.Providers
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

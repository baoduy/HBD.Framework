#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

#endregion

namespace HBD.Framework.Dynamic
{
    public class DynamicDictionary : DynamicObject
    {
        private readonly IDictionary<string, object> _cacheDictionary;

        public DynamicDictionary(params object[] objs)
        {
            _cacheDictionary = new ConcurrentDictionary<string, object>();
            Objects = objs;
        }

        private object[] Objects { get; }

        public object this[string name]
        {
            get
            {
                object result;
                TryGetMember(name, out result);
                return result;
            }
            set { _cacheDictionary[name] = value; }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _cacheDictionary[binder.Name] = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
            => TryGetMember(binder.Name, out result);

        private bool TryGetMember(string propertName, out object result)
        {
            result = null;

            var k =
                _cacheDictionary.FirstOrDefault(
                    i => string.Compare(i.Key, propertName, StringComparison.OrdinalIgnoreCase) == 0);
            if (k.IsNotDefault())
            {
                result = k.Value;
                return true;
            }

            if (Objects.NotAnyItem()) return false;

            result = (from b in Objects
                let val = b.GetValueFromProperty(propertName)
                where val != null
                select val).FirstOrDefault();

            if (result != null)
                _cacheDictionary[propertName] = result;

            return true;
        }
    }
}
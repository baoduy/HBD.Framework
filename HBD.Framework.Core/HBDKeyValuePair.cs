using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HBD.Framework.Core
{
    [Serializable]
    public class HBDKeyValuePair<TKey,TValue>
    {
        public TKey Key { get; private set; }
        public TValue Value { get; private set; }

        public HBDKeyValuePair(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }

        public override string ToString()
        {
            if (this.Key != null)
                return Key.ToString();
            else if (this.Value != null)
                return this.Value.ToString();
            else return "Empty HBDKeyValuePair";
        }
    }
}

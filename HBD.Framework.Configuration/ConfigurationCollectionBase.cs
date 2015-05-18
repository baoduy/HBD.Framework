using HBD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace HBD.Framework.Configuration
{
    public class ConfigurationCollectionBase<TElement> : ConfigurationElementCollection, IEnumerable<TElement> where TElement : ConfigurationElementBase, new()
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TElement)element).Name;
        }

        public TElement this[int index]
        { get { return (TElement)base.BaseGet(index);  } }

        public TElement this[string name]
        {
            get
            {
                foreach (TElement temp in this)
                    if (temp.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                        return temp;
                return null;
            }
        }

        public new IEnumerator<TElement> GetEnumerator()
        {
            return new EnumeratorSwaper<TElement>(base.GetEnumerator());
        }
    }
}

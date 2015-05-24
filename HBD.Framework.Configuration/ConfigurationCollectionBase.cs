using HBD.Framework.Core;
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
        { get { return base.BaseGet(index) as TElement; } }

        public new TElement this[string name]
        {
            get { return base.BaseGet(name) as TElement; }
        }

        public new IEnumerator<TElement> GetEnumerator()
        {
            return new EnumeratorSwaper<TElement>(base.GetEnumerator());
        }
    }
}

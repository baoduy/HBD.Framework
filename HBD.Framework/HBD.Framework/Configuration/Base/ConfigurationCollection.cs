#region

using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Configuration.Base
{
    public class ConfigurationCollection<TElement> : ConfigurationElementCollection, IEnumerable<TElement>
        where TElement : ConfigurationElement, new()
    {
        public TElement this[int index] => BaseGet(index) as TElement;
        public new TElement this[string name] => BaseGet(name) as TElement;

        public new IEnumerator<TElement> GetEnumerator() => new Enumerator<TElement>(base.GetEnumerator());

        protected override ConfigurationElement CreateNewElement() => new TElement();

        protected override object GetElementKey(ConfigurationElement element)
        {
            var @base = element as ConfigurationElementBase;
            if (@base != null) return @base.Name;

            var p = element.GetProperties<ConfigurationPropertyAttribute>().FirstOrDefault(a => a.Attribute.IsKey);
            // ReSharper disable once AssignNullToNotNullAttribute
            return p?.PropertyInfo.GetValue(element);
        }
    }
}
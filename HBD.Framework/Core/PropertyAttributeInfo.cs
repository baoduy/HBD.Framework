using System;
using System.Reflection;

namespace HBD.Framework.Core
{
    public class PropertyAttributeInfo<TAttribute> where TAttribute : Attribute
    {
        public PropertyInfo PropertyInfo { get; protected internal set; }
        public TAttribute Attribute { get; protected internal set; }
    }
}
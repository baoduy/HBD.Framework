using System;
using System.Linq;
using System.Reflection;

namespace HBD.Framework
{
    public static class AttributeExtensions
    {
        public static bool HasAttribute<TAttribute>(this object @this, bool inherit = true) where TAttribute : Attribute
            => @this?.GetAttribute<TAttribute>(inherit) != null;

        public static bool HasAttribute<TAttribute>(this PropertyInfo @this, bool inherit = true)
            where TAttribute : Attribute
            => @this?.GetAttribute<TAttribute>(inherit) != null;

        public static bool HasAttributeOnProperty<TAttribute>(this object @this, string propertyName,
            bool inherit = true) where TAttribute : Attribute
        {
            var prop = @this.GetProperty(propertyName);
            return prop.HasAttribute<TAttribute>(inherit);
        }

        public static TAttribute GetAttribute<TAttribute>(this PropertyInfo @this, bool inherit = true)
            where TAttribute : Attribute
            => (TAttribute)@this?.GetCustomAttribute(typeof(TAttribute), inherit);

        public static TAttribute GetAttribute<TAttribute>(this object @this, bool inherit = true)
            where TAttribute : Attribute
        {
            if (@this == null) return default(TAttribute);
            return (TAttribute)Attribute.GetCustomAttribute(@this.GetType(), typeof(TAttribute), inherit);
        }

        public static Attribute GetAttribute(this Type @this, Type typeOfAttribute)
            => @this?.GetCustomAttributes(typeOfAttribute, true).Cast<Attribute>().FirstOrDefault();

        public static TAttribute GetAttribute<TAttribute>(this Type @this) where TAttribute : Attribute
            => (TAttribute)@this.GetAttribute(typeof(TAttribute));
    }
}
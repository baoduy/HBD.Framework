using HBD.Framework.Extensions.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HBD.Framework.Extensions
{
    public static class PropertyExtensions
    {
        #region Public Methods

        public static IEnumerable<PropertyAttributeInfo<TAttribute>> GetProperties<TAttribute>(this object @this,
            bool inherit = true) where TAttribute : Attribute
        {
            if (@this == null) yield break;
            foreach (var p in @this.GetType().GetTypeInfo().GetProperties())
            {
                var att = p.GetCustomAttribute<TAttribute>(inherit);
                if (att == null) continue;
                yield return new PropertyAttributeInfo<TAttribute> { Attribute = att, PropertyInfo = p };
            }
        }

        //    foreach (var p in list)
        //        if (p != null)
        //            yield return p;
        //}
        public static PropertyInfo GetProperty<T>(this T @this, string propertyName) where T : class
        {
            if (@this == null || propertyName.IsNullOrEmpty()) return null;
            if (@this is Type)
                throw new NotSupportedException("Type");

            return @this.GetType()
                .GetTypeInfo()
                .GetProperty(propertyName,
                    BindingFlags.IgnoreCase
                    | BindingFlags.Public
                    | BindingFlags.NonPublic
                    | BindingFlags.Instance);
        }

        // var list = propertyNames.Length <= 0 ? @this.GetType().GetTypeInfo().GetProperties() : propertyNames.Select(@this.GetProperty);
        public static object PropertyValue<T>(this T obj, string propertyName) where T : class
        {
            if (obj == null || propertyName.IsNullOrEmpty()) return null;
            var props = propertyName.Contains(".") ? propertyName.Split('.') : new[] { propertyName };

            var currentObj =
                props.Aggregate<string, object>(obj, (current, p) => current.GetProperty(p)?.GetValue(current));
            return currentObj == obj ? null : currentObj;
        }

        public static bool SetPropertyValue(this object @this, PropertyInfo property, object value)
        {
            if (@this == null || property == null) return false;
            try
            {
                value = property.PropertyType.GetTypeInfo().IsEnum
                    ? Enum.Parse(property.PropertyType, value.ToString())
                    : Convert.ChangeType(value, property.PropertyType);

                property.SetValue(@this, value, null);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //public static IEnumerable<PropertyInfo> GetProperties<T>(this T @this, params string[] propertyNames)
        //    where T : class
        //{
        //    if (@this == null) yield break;
        //    if (@this is Type)
        //        throw new NotSupportedException("Type");
        public static bool SetPropertyValue(this object @this, string propertyName, object value)
        {
            if (@this == null || propertyName.IsNullOrEmpty()) return false;
            var property = @this.GetProperty(propertyName);
            return @this.SetPropertyValue(property, value);
        }

        #endregion Public Methods
    }
}
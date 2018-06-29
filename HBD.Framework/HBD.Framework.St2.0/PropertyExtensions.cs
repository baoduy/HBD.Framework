#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework
{
    public static class PropertyExtensions
    {
        public static IEnumerable<PropertyInfo> GetProperties<T>(this T @this, params string[] propertyNames)
            where T : class
        {
            if (@this == null) yield break;
            if (@this is Type)
                throw new NotSupportedException("Type");

            var list = propertyNames.Length <= 0 ? @this.GetType().GetTypeInfo().GetProperties() : propertyNames.Select(@this.GetProperty);

            foreach (var p in list)
                if (p != null) yield return p;
        }

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

        #region GetValues

        public static object PropertyValue<T>(this T obj, string propertyName) where T : class
        {
            if (obj == null || propertyName.IsNullOrEmpty()) return null;
            var props = propertyName.Contains(".") ? propertyName.Split('.') : new[] { propertyName };

            object currentObj = obj;
            foreach (var p in props)
            {
                Guard.ArgumentIsNotNull(p, "PropertyName");
                currentObj = currentObj.GetProperty(p)?.GetValue(currentObj);
            }
            return currentObj == obj ? null : currentObj;
        }

        #endregion GetValues

        #region SetValues

        public static bool SetPropertyValue(this object @this, string propertyName, object value)
        {
            if (@this == null || propertyName.IsNullOrEmpty()) return false;
            var property = @this.GetProperty(propertyName);
            return @this.SetPropertyValue(property, value);
        }

        internal static bool SetPropertyValue(this object @this, PropertyInfo property, object value)
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

        #endregion SetValues
    }
}
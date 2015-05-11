using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using HBD.Framework.Core;
using System.ComponentModel;
using System.Collections;
using System.Linq.Expressions;
using HBD.Framework.Data.Utilities;

namespace HBD.Framework.Extension
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null||obj==DBNull.Value)
                return true;
            if (obj is string)
                return string.IsNullOrEmpty(obj as string);
            return string.IsNullOrEmpty(obj.ToString());
        }

        public static bool IsNullOrLessThanZeo(this object obj)
        {
            if (obj.IsNullOrEmpty())
                return true;
            return obj.Compare(0, CompareOperation.LessThanOrEquals);
        }

        public static object CreateInstance(this Type type)
        {
            Guard.ArgumentNotNull(type, "Type");
            return Activator.CreateInstance(type);
        }

        public static bool IsNull<T>(this T value) where T : struct
        {
            return value.Equals(default(T));
        }

        public static string Replace(this string value, string[] oldValues, string newValue)
        {
            if (oldValues != null && oldValues.Length > 0 && !string.IsNullOrEmpty(value))
            {
                value = oldValues.Aggregate(value, (current, s) => current.Replace(s, newValue));
            }
            return value;
        }

        public static bool IsEquals(this object objA, object objB)
        {
            return objA.Compare(objB) == 0;
        }

        public static bool IsContains(this object objA, object objB)
        {
            if (objA == null || objB == null) return false;
            var strA = objA.ToString();
            var strB = objB.ToString();
            return strA.Trim().ToLower().Contains(strB.Trim().ToLower());
        }

        public static bool IsStartsWith(this object objA, object objB)
        {
            if (objA == null || objB == null) return false;
            var strA = objA.ToString();
            var strB = objB.ToString();
            return strA.Trim().ToLower().StartsWith(strB.Trim().ToLower());
        }

        public static bool IsEndsWith(this object objA, object objB)
        {
            if (objA == null || objB == null) return false;
            var strA = objA.ToString();
            var strB = objB.ToString();
            return strA.Trim().ToLower().EndsWith(strB.Trim().ToLower());
        }

        public static bool IsIn(this object objA, object objB)
        {
            if (objA == null || objB == null) return false;
            if (!(objB is IEnumerable)) return false;

            return ((IEnumerable)objB).Cast<object>().Any(objA.IsEquals);
        }

        public static bool IsStringOrValueType(this object obj)
        {
            if (obj == null) return false;
            var type = obj as Type;
            return type != null ? type.IsStringOrValueType() : obj.GetType().IsStringOrValueType();
        }

        public static bool IsStringOrValueType(this PropertyInfo property)
        {
            return property != null && property.PropertyType.IsStringOrValueType();
        }

        public static bool IsStringOrValueType(this Type type)
        {
            Guard.ArgumentNotNull(type, "Type");
            return type == typeof(string) || (!type.IsGenericType && type.IsValueType);//Nullable is IsValueType and IsGenericType
        }

        public static int Compare(this object objA, object objB)
        {
            if (objA != null && objB != null)
            {
                if ((!(objA is string) && !(objB is string)) &&
                    (objA is IComparable && objB is IComparable))// If not is string and is IComparable
                {
                    return ((IComparable)objA).CompareTo((IComparable)objB);
                }
                return String.Compare(objA.ToString().Trim(), objB.ToString().Trim(), StringComparison.CurrentCultureIgnoreCase);// Compare as string.
            }
            if (objA == null && objB == null)
                return 0;
            if (objA != null)
                return 1;
            return -1;
        }

        public static bool Compare(this object objA, object objB, CompareOperation operation)
        {
            switch (operation)
            {
                case CompareOperation.GreaterThan:
                    return objA.Compare(objB) > 0;
                case CompareOperation.LessThan:
                    return objA.Compare(objB) < 0;
                case CompareOperation.GreaterThanOrEquals:
                    return objA.Compare(objB) >= 0;
                case CompareOperation.LessThanOrEquals:
                    return objA.Compare(objB) <= 0;
                case CompareOperation.Contains:
                    return objA.IsContains(objB);
                case CompareOperation.NotContains:
                    return !objA.IsContains(objB);
                case CompareOperation.StartsWith:
                    return objA.IsStartsWith(objB);
                case CompareOperation.EndsWith:
                    return objA.IsEndsWith(objB);
                case CompareOperation.In:
                    return objA.IsIn(objB);
                case CompareOperation.NotIn:
                    return !objA.IsIn(objB);
                case CompareOperation.IsNull:
                    return objA == null && objB == null;
                case CompareOperation.NotNull:
                    return objA != null || objB != null;
                case CompareOperation.NotEquals:
                    return !objA.IsEquals(objB);
                default:
                case CompareOperation.Equals:
                    return objA.IsEquals(objB);
            }
        }

        public static string GetAlphabetCharacters(this string value)
        {
            var buil = new StringBuilder();
            foreach (var c in value)
            {
                if (c >= 'a' && c <= 'z')
                    buil.Append(c);
                else if (c >= 'A' && c <= 'Z')
                    buil.Append(c);
                else if (c >= '0' && c <= '9')
                    buil.Append(c);
            }
            return buil.ToString();
            //return value.ToLower().Replace(" ", string.Empty).Replace("_", string.Empty).Replace("//", string.Empty).Replace("\\", string.Empty);
        }

        public static PropertyInfo GetProperty(this object obj, string propertyName)
        {
            Guard.ArgumentNotNull(obj, "obj");
            Guard.ArgumentNotNull(propertyName, "propertyName");

            try { return obj.GetType().GetProperty(propertyName); }
            catch { return null; }
        }

        public static IEnumerable<PropertyInfo> GetProperties<T>(BindingFlags flags = BindingFlags.Instance|BindingFlags.Public)
        {
            return GetProperties(typeof(T), flags);
        }

        public static IEnumerable<PropertyInfo> GetProperties(Type type, BindingFlags flags = BindingFlags.Instance|BindingFlags.Public)
        {
            return type.GetProperties(flags).Where(p => p.CanRead);
        }
        public static IEnumerable<PropertyInfo> GetProperties<T, TKey>(params Expression<Func<T, TKey>>[] propertiesLambda) where T : class
        {
            return propertiesLambda.Select(lamdar => lamdar.Body as MemberExpression ?? ((UnaryExpression)lamdar.Body).Operand as MemberExpression).Select(member => member.Member).OfType<PropertyInfo>();
        }

        public static string[] GetPropertiesName<T, TKey>(params Expression<Func<T, TKey>>[] propertiesLambda) where T : class
        {
            var properties = GetProperties(propertiesLambda);
            return properties != null ? properties.Select(p => p.Name).ToArray() : null;
        }

        public static object[] GetValue<T, TKey>(this T obj, params Expression<Func<T, TKey>>[] propertiesLambda) where T : class
        {
            var properties = GetProperties(propertiesLambda);
            return (from prop in properties where prop != null select obj.GetValue(prop)).ToArray();
        }

        public static PropertyInfo GetProperty<T, TKey>(Expression<Func<T, TKey>> propertyLambda) where T : class
        {
            var list = GetProperties(propertyLambda).ToArray();
            return (list.Length > 0) ? list[0] : null;
        }

        public static PropertyInfo GetProperty<T>(Expression<Func<T, object>> propertyLambda) where T : class
        {
            return GetProperty<T, object>(propertyLambda);
        }

        public static string GetpropertyName<T, TKey>(Expression<Func<T, TKey>> propertyLambda) where T : class
        {
            var property = GetProperty(propertyLambda);
            return property != null ? property.Name : null;
        }

        public static object GetValue<T, TKey>(this T obj, Expression<Func<T, TKey>> propertyLambda) where T : class
        {
            var property = GetProperty(propertyLambda);
            return property != null ? obj.GetValue(property) : null;
        }

        public static IEnumerable<PropertyInfo> GetPropertiesByAttribute<TAttribute>(this object obj) where TAttribute : Attribute
        {
            if (obj == null)
                return null;

            return (from p in obj.GetType().GetProperties()
                    select new
                    {
                        Property = p,
                        Attributes = (from a in p.GetCustomAttributes(true)
                                      where a is TAttribute
                                      select a as TAttribute).ToList()
                    }).Where(p => p.Attributes.Count > 0 && p.Property.CanRead).Select(p => p.Property);
        }

        public static IEnumerable<PropertyInfo> GetProperties(this object obj, BindingFlags flags = BindingFlags.Instance|BindingFlags.Public)
        {
            return GetProperties(obj.GetType(), flags);
        }

        public static string[] GetPropertiesName<TAttribute>(this object obj) where TAttribute : Attribute
        {
            return GetPropertiesByAttribute<TAttribute>(obj).Select(p => p.Name).ToArray();
        }

        public static string GetDefaultPropertyName(this object obj)
        {
            var attributes = TypeDescriptor.GetAttributes(obj);
            var attribute = (DefaultPropertyAttribute)attributes[typeof(DefaultPropertyAttribute)];
            return attribute == null ? string.Empty : attribute.Name;
        }

        public static PropertyInfo GetDefaultProperty(this object obj)
        {
            var name = GetDefaultPropertyName(obj);
            return string.IsNullOrEmpty(name) ? null : obj.GetProperty(name);
        }

        public static object GetDefaultValue(this object obj)
        {
            var p = obj.GetDefaultProperty();
            return obj.GetValue(p);
        }

        public static object GetValue(this object obj, PropertyInfo property)
        {
            Guard.ArgumentNotNull(obj, "object");
            Guard.ArgumentNotNull(property, "PropertyInfo");

            var value = property.GetValue(obj, null);
            if (property.PropertyType.IsEnum && value != null)
                return ((Enum)value).ToString();
            return value;
        }

        public static object GetValue(this object obj, string propertyName)
        {
            var prop = obj.GetProperty(propertyName);
            return prop == null ? null : obj.GetValue(prop);
        }

        /// <summary>
        /// Set value to property
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="propertyName">name of property</param>
        /// <param name="value">value will be set</param>
        /// <returns></returns>
        public static bool SetValue(this object obj, string propertyName, object value)
        {
            Guard.ArgumentNotNull(obj, "obj");
            Guard.ArgumentNotNull(propertyName, "propertyName");

            var prop = obj.GetProperty(propertyName);
            return obj.SetValue(prop, value);
        }

        public static bool SetValue(this object obj, PropertyInfo property, object value)
        {
            if (property != null)
            {
                try
                {
                    value = property.PropertyType.IsEnum ? Enum.Parse(property.PropertyType, value.ToString()) : Convert.ChangeType(value, property.PropertyType);

                    property.SetValue(obj, value, null);
                    return true;
                }
                catch { return false; }
            }
            return false;
        }

        /// <summary>
        /// Set value to default property of object
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="value">value</param>
        /// <returns></returns>
        public static bool SetDefaultValue(this object obj, object value)
        {
            Guard.ArgumentNotNull(obj, "obj");

            var name = obj.GetDefaultPropertyName();
            return !string.IsNullOrEmpty(name) && obj.SetValue(name, value);
        }

        public static bool SetValue<T, TKey>(this T obj, Expression<Func<T, TKey>> propertyLambda, object value) where T : class
        {
            if (obj == null) return false;
            if (propertyLambda == null) return false;

            var property = GetProperty(propertyLambda);
            return obj.SetValue(property, value);
        }

        public static bool SetValue<T>(this T obj, Expression<Func<T, object>> propertyLambda, object value) where T : class
        {
            return obj.SetValue<T, object>(propertyLambda, value);
        }

        public static FieldInfo GetField(this object obj, string fieldName)
        {
            Guard.ArgumentNotNull(obj, "object");
            Guard.ArgumentNotNull(fieldName, "fieldName");

            try { return obj.GetType().GetField(fieldName); }
            catch { return null; }
        }

        public static object GetFieldValue(this object obj, string fieldName)
        {
            var field = obj.GetField(fieldName);
            if (field == null) return null;

            var val = field.GetValue(obj);
            if (field.FieldType.IsEnum && val != null)
                return ((Enum)val).ToString();
            return val;
        }
    }
}

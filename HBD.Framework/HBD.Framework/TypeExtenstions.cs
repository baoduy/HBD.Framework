#region

using System;

#endregion

namespace HBD.Framework
{
    public static class TypeExtenstions
    {
        public static bool IsNotAssignableFrom(this Type @this, Type type)
        {
            if (@this == null || type == null) return false;
            return !@this.IsAssignableFrom(type);
        }

        public static bool IsAssignableFrom<T>(this Type @this)
        {
            if (@this == null) return false;
            return @this.IsAssignableFrom(typeof(T));
        }

        public static bool IsNotAssignableFrom<T>(this Type @this)
            => !@this.IsAssignableFrom<T>();

        public static T ChangeType<T>(this object @this)
        {
            if (typeof(T) == typeof(object)) return (T) @this;
            if (@this.IsNull()) return default(T);
            if (typeof(T) == typeof(bool) && !(@this is bool))
            {
                var str = @this.ToString();
                @this = str.Equals("1") || string.Compare(str, "Yes", StringComparison.CurrentCultureIgnoreCase) == 0;
            }

            object value = null;

            value = Convert.ChangeType(@this, typeof(T));

            if (value == null) return default(T);
            return (T) value;
        }

        public static object ChangeType(this object @this, Type newType)
        {
            if (@this == null) return null;
            if (@this.GetType() == newType) return @this;

            try
            {
                return Convert.ChangeType(@this, newType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool IsNumericType(this Type @this)
        {
            if (@this == null) return false;

            switch (Type.GetTypeCode(@this))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsNumericType(this object @this)
            => @this?.GetType().IsNumericType() ?? false;

        public static bool IsNotNumericType(this object @this)
            => !@this.IsNumericType();
    }
}
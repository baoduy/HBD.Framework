#region using

using System;
using System.ComponentModel;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework
{
    public static class CommonExtensions
    {
        public static bool IsNull(this object @this)
        {
            if (@this == null || @this == DBNull.Value) return true;
            // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
            if (@this is string) return string.IsNullOrEmpty((string) @this);
            return false;
        }

        public static bool IsNotNull(this object @this) => !@this.IsNull();

        //public static bool IsNullOrEmpty(this object @this)
        //{
        //    if (@this.IsNull()) return true;
        //    // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
        //    if (@this is string) return string.IsNullOrWhiteSpace((string)@this);
        //    if (@this.IsStringOrValueType()) return false;
        //    // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
        //    if (@this is IEnumerable) return ((IEnumerable)@this).IsEmpty();
        //    return false;
        //}

        //public static bool IsNotNullOrEmpty(this object @this) => !@this.IsNullOrEmpty();

        public static string GetName(this Enum @this)
        {
            var type = @this.GetType();
            var name = Enum.GetName(type, @this);
            var field = type.GetField(name);

            var attr = field.GetAttribute<DescriptionAttribute>();
            return attr == null ? name.ConsolidateWords() : attr.Description;
        }

        #region Assembly Extension

        public static object CreateInstance(this Type @this, params object[] args)
        {
            Guard.ArgumentIsNotNull(@this, "Type");
            if (@this.IsAbstract || @this.IsInterface) return null;
            return Activator.CreateInstance(@this, args);
        }

        /// <summary>
        ///     Check the struct value is default value or not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsDefault<T>(this T @this) where T : struct => @this.Equals(default(T));

        /// <summary>
        ///     Check the struct value is difference with default value or not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsNotDefault<T>(this T @this) where T : struct => !@this.IsDefault();

        #endregion Assembly Extension
    }
}
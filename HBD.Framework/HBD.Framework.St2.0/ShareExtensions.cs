#region using

using System;
using System.Reflection;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework
{
    public static class ShareExtensions
    {
        public static bool IsNull(this object @this)
        {
            if (@this == null || @this == DBNull.Value) return true;
            // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
            if (@this is string) return string.IsNullOrEmpty((string) @this);
            return false;
        }

        public static bool IsNotNull(this object @this)
        {
            return !@this.IsNull();
        }

        #region Assembly Extension

        public static object CreateInstance(this Type @this, params object[] args)
        {
            Guard.ArgumentIsNotNull(@this, "Type");
            if (@this.GetTypeInfo().IsAbstract || @this.GetTypeInfo().IsInterface) return null;
            return Activator.CreateInstance(@this, args);
        }

        /// <summary>
        ///     Check the struct value is default value or not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsDefault<T>(this T @this) where T : struct
        {
            return @this.Equals(default(T));
        }

        /// <summary>
        ///     Check the struct value is difference with default value or not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsNotDefault<T>(this T @this) where T : struct
        {
            return !@this.IsDefault();
        }

        #endregion Assembly Extension
    }
}
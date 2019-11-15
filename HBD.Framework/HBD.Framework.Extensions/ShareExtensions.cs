using System;
using System.Reflection;

namespace HBD.Framework.Extensions
{
    public static class ShareExtensions
    {
        #region Public Methods

        public static object CreateInstance(this Type @this, params object[] args)
        {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (@this.GetTypeInfo().IsAbstract || @this.GetTypeInfo().IsInterface) return null;
            return Activator.CreateInstance(@this, args);
        }

        /// <summary>
        /// Check the struct value is default value or not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsDefault<T>(this T @this) where T : struct => @this.Equals(default(T));

        /// Check the struct value is difference with default value or not. </summary> <typeparam
        /// name="T"></typeparam> <param name="this"></param> <returns></returns>
        public static bool IsNotDefault<T>(this T @this) where T : struct => !@this.IsDefault();

        public static bool IsNotNull(this object @this) => !@this.IsNull();

        public static bool IsNull(this object @this)
        {
            if (@this == null || @this == DBNull.Value) return true;
            // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
            return @this is string && string.IsNullOrEmpty((string)@this);
        }

        #endregion Public Methods
    }
}
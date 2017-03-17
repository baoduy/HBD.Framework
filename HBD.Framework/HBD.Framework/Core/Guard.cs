#region

using HBD.Framework.Core.Exceptions;
using HBD.Framework.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#endregion

namespace HBD.Framework.Core
{
    public static class Guard
    {
        public static void ValueIsNotNull(object obj, string name)
        {
            if (obj.IsNull()) throw new NoNullAllowedException(name);
        }

        public static void ArgumentIsNotNull(object obj, string name)
        {
            if (obj.IsNull()) throw new ArgumentNullException(name);
        }

        public static void ArgumentIsTypeOf<T>(object obj, string name) => ArgumentIsTypeOf(obj, typeof(T), name);

        public static void ShouldGreaterThan<T>(this T @this, T value, string name) where T : IComparable<T>
        {
            if (@this.CompareTo(value) <= 0)
                throw new ArgumentException($"{name} must be greater than {value}.");
        }

        public static void ArgumentIsTypeOf(object obj, Type expectedType, string name)
        {
            ArgumentIsNotNull(obj, name);
            ArgumentIsNotNull(expectedType, "expectedType");
            if (!expectedType.IsInstanceOfType(obj))
                throw new ArgumentException($"{name} must be an instance of {expectedType.FullName}");
        }

        /// <summary>
        ///     Ensure that the file or directory path must exisited.
        ///     PathNotFoundException will be throw if the path isnot existing.
        /// </summary>
        /// <param name="path"></param>
        public static void PathMustExisted(string path)
        {
            ArgumentIsNotNull(path, "path");

            if (!PathEx.IsPathExisted(path))
                throw new PathNotFoundException(path);
        }

        public static void ShouldNotBeEmpty<T>(IEnumerable<T> collection, string name) where T : class
        {
            if (collection.NotAnyItem())
                throw new ArgumentException($"{name} cannot be empty.");
        }

        public static void AllItemsShouldNotBeNull<T>(IEnumerable<T> collection, string name)
        {
            if (collection?.Any(c => c.IsNull()) == true)
                throw new ArgumentException("item in collection cannot be empty.");
        }
    }
}
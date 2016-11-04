using HBD.Framework.Core.Exceptions;
using HBD.Framework.Exceptions;
using HBD.Framework.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HBD.Framework.Core
{
    public static class Guard
    {
        public static void ValueIsNotNull(object obj, string name)
        {
            if (obj.IsNull()) throw new ObjectNullException(name);
        }

        public static void ArgumentIsNotNull(object obj, string name)
        {
            if (obj.IsNull()) throw new ArgumentNullException(name);
        }

        public static void ArgumentIs<T>(object obj, string name) => ArgumentIs(obj, typeof(T), name);

        public static void MustGreaterThan<T>(this T @this, T value, string name) where T : IComparable<T>
        {
            if (@this.CompareTo(value) <= 0)
                throw new ArgumentException($"{name} must be greater than {value}.");
        }

        public static void MustGreaterThanOrEquals<T>(this T @this, T value, string name) where T : IComparable<T>
        {
            if (@this.CompareTo(value) < 0)
                throw new ArgumentException($"{name} must be greater than or equals {value}.");
        }

        public static void MustGreaterThanOrEqualsZero<T>(this T @this, string name) where T : IComparable<T>
            => MustGreaterThanOrEquals(@this, (T)Convert.ChangeType(0, typeof(T)), name);

        public static void MustIsIn<T>(this T @this, T minValue, T maxValue, string name) where T : IComparable<T>
        {
            maxValue.MustGreaterThanOrEquals(minValue, nameof(minValue));
            if (@this.CompareTo(minValue) < 0 || @this.CompareTo(maxValue) > 0)
                throw new ArgumentException($"{name} must be from {minValue} to {maxValue}.");
        }

        public static void ArgumentIs(object obj, Type expectedType, string name)
        {
            ArgumentIsNotNull(obj, name);
            ArgumentIsNotNull(expectedType, "expectedType");
            if (!expectedType.IsInstanceOfType(obj))
                throw new ArgumentException($"{name} must be an instance of {expectedType.FullName}");
        }

        /// <summary>
        /// Ensure that the file or directory path must exisited.
        /// PathNotFoundException will be throw if the path isnot existing.
        /// </summary>
        /// <param name="path"></param>
        public static void PathMustExisted(string path)
        {
            ArgumentIsNotNull(path, "path");

            if (!PathEx.IsPathExisted(path))
                throw new PathNotFoundException(path);
        }

        public static void MustBeValueType(object obj, string name)
        {
            if (obj?.GetType().IsValueType == true || obj is string)
                throw new ArgumentException($"{name} must be Value Type.");
        }

        public static void MustBeValuesType(IEnumerable<object> collection)
        {
            ArgumentIsNotNull(collection, "Collection");
            if (collection.Any(obj => obj?.GetType().IsValueType == true || obj is string))
            {
                throw new ArgumentException("Object in collection must be Value Type");
            }
        }

        public static void CollectionMustNotEmpty<T>(IEnumerable<T> collection, string name) where T : class
        {
            ArgumentIsNotNull(collection, name);
            if (!collection.Any())
                throw new ArgumentException($"{name} cannot be empty.");
        }

        public static void CollectionMustNotEmpty<T>(T[] collection, string name)
        {
            ArgumentIsNotNull(collection, name);
            if (collection.Length <= 0)
                throw new ArgumentException($"{name} cannot be empty.");
        }

        public static void AllItemsMustNotEmpty<T>(IEnumerable<T> collection, string name)
        {
            ArgumentIsNotNull(collection, name);
            if (collection.Any(c => c.IsNull()))
                throw new ArgumentException("item in collection cannot be empty.");
        }
    }
}
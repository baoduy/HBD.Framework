#region using

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HBD.Framework.Core;
using HBD.Framework.Data.EntityConverters;

#endregion

namespace HBD.Framework.Data
{
    public static class EntityConverterExtensions
    {
        /// <summary>
        ///     Get Writable property column mapping.
        /// </summary>
        /// <returns></returns>
        internal static IEnumerable<ColumnMappingInfo> GetColumnMapping(Type type)
        {
            foreach (var p in type.GetProperties().Where(p => p.CanWrite))
            {
                var fieldName = p.Name;

#if !NETSTANDARD2_0 && !NETSTANDARD1_6 
                var att = p.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.ColumnAttribute>();

                if (att != null)
                    fieldName = att.Name;
#endif
                yield return new ColumnMappingInfo(p, fieldName);
            }
        }

        internal static IEnumerable<ColumnMappingInfo> GetColumnMapping<T>() where T : class
            => GetColumnMapping(typeof(T));

        internal static IEnumerable<ColumnMappingInfo> GetColumnMapping(this object @this)
            => GetColumnMapping(@this.GetType());

        /// <summary>
        ///     Mapping a DataReader to entity. If entity property name is difference with Database Field using ColumnAttribute to
        ///     custonmize it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="ignoreEmptyRows"></param>
        /// <returns></returns>
        public static IEnumerable<T> MappingTo<T>(this IDataReader @this, bool ignoreEmptyRows = true)
            where T : class, new()
        {
            Guard.ArgumentIsNotNull(@this, "IDataReader");

            var fieldNames = Enumerable.Range(0, @this.FieldCount).Select(@this.GetName).ToList();
            var columnMapping = GetColumnMapping<T>().ToList();

            while (@this.Read())
            {
                var entity = new T();
                var hasValue = false;

                foreach (var p in columnMapping.Where(t => fieldNames.Any(f => f.EqualsIgnoreCase(t.FieldName))))
                    try
                    {
                        var value = @this[p.FieldName];
                        if (value.IsNull()) continue;

                        entity.SetValueToProperty(p.PropertyInfo, value);
                        hasValue = true;
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                if (!hasValue && ignoreEmptyRows)
                    continue;

                yield return entity;
            }
        }

        public static Task<IEnumerable<T>> MappingToAsync<T>(this IDataReader @this, bool ignoreEmptyRows = true)
            where T : class, new()
        {
            var task = new TaskCompletionSource<IEnumerable<T>>();
            task.SetResult(@this.MappingTo<T>(ignoreEmptyRows));
            return task.Task;
        }

#if !NETSTANDARD1_6
        public static IEnumerable<T> MappingTo<T>(this DataTable @this, bool ignoreEmptyRows = true)
            where T : class, new()
        {
            Guard.ArgumentIsNotNull(@this, "DataTable");
            return @this.CreateDataReader().MappingTo<T>(ignoreEmptyRows);
        }

        public static Task<IEnumerable<T>> MappingToAsync<T>(this DataTable @this, bool ignoreEmptyRows = true)
            where T : class, new()
        {
            var task = new TaskCompletionSource<IEnumerable<T>>();
            task.SetResult(@this.MappingTo<T>(ignoreEmptyRows));
            return task.Task;
        }
#endif
    }
}
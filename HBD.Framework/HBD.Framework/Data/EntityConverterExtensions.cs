#region using

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
                var att = p.GetCustomAttribute<ColumnAttribute>();

                if (att != null)
                    fieldName = att.Name;

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

        public static IEnumerable<T> MappingTo<T>(this DataTable @this, bool ignoreEmptyRows = true)
            where T : class, new()
        {
            Guard.ArgumentIsNotNull(@this, "DataTable");
            return @this.CreateDataReader().MappingTo<T>(ignoreEmptyRows);

            //if (ignoreEmptyRows)
            //{
            //    var newtb = @this.Clone();
            //    foreach (DataRow row in @this.Rows)
            //    {
            //        if (row.ItemArray.Any(v => v.IsNotNull()))
            //            newtb.Rows.Add(row.ItemArray);
            //    }

            //    //Copy to the new Table so that the existing table is not change from outside.
            //    @this = newtb;
            //}

            //var setting = new JsonSerializerSettings();
            //setting.NullValueHandling = NullValueHandling.Ignore;

            //var json = JsonConvert.SerializeObject(@this, Formatting.None);
            //return JsonConvert.DeserializeObject<T[]>(json, setting);
        }

        public static Task<IEnumerable<T>> MappingToAsync<T>(this DataTable @this, bool ignoreEmptyRows = true)
            where T : class, new()
        {
            var task = new TaskCompletionSource<IEnumerable<T>>();
            task.SetResult(@this.MappingTo<T>(ignoreEmptyRows));
            return task.Task;
        }
    }
}
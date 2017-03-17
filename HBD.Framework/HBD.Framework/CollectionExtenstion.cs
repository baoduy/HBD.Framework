#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#endregion

namespace HBD.Framework
{
    public static class CollectionExtenstion
    {
        public static IReadOnlyList<T> ToReadOnly<T>(this IEnumerable<T> @this)
        {
            if (@this == null) return null;
            var list = @this as IList<T> ?? @this.ToList();
            return new ReadOnlyCollection<T>(list);
        }

        public static void AddRange<T>(this ICollection<T> @this, IEnumerable<T> collection)
        {
            if (@this == null || collection == null) return;
            if (@this.IsReadOnly)
                throw new NotSupportedException("The read-only collection can't add more items.");

            foreach (var i in collection)
                @this.Add(i);
        }

        public static void RemoveRange<T>(this ICollection<T> @this, IEnumerable<T> collection)
        {
            if (@this == null || collection == null) return;
            if (@this.IsReadOnly)
                throw new NotSupportedException("The read-only collection can't remove items.");

            foreach (var i in collection)
                @this.Remove(i);
        }

        /// <summary>
        /// Check whether IEnumerable has NO item or not.
        /// If IEnumerable is NULL then return FALSE.
        /// </summary>
        public static bool NotAnyItem<T>(this IEnumerable<T> @this) => !@this.AnyItem();

        /// <summary>
        /// Check whether IEnumerable has any item or not.
        /// If IEnumerable is NULL then return FALSE.
        /// </summary>
        public static bool AnyItem<T>(this IEnumerable<T> @this) => @this?.Any() == true;

        /// <summary>
        ///  Check whether IDictionary has NO item or not.
        /// If IDictionary is NULL then return FALSE.
        /// </summary>
        public static bool NotAnyItem<TKey, TValue>(this IDictionary<TKey, TValue> @this) => !@this.AnyItem();

        /// <summary>
        /// Check whether IDictionary has any item or not.
        /// If IDictionary is NULL then return FALSE.
        /// </summary>
        public static bool AnyItem<TKey, TValue>(this IDictionary<TKey, TValue> @this) => @this?.Count > 0;

        /// <summary>
        /// Check whether the collection whether it has duplicate items.
        /// </summary>
        public static bool DuplicatedItems<T, TKey>(this ICollection<T> @this, Func<T, TKey> keySelector)
            where T : class where TKey : struct
        {
            if (@this == null || keySelector == null) return false;

            return (from i in @this
                    from y in @this
                    where i != null && y != null && i != y && keySelector(i).Equals(keySelector(y))
                    select i).Any();
        }

        public static bool DuplicatedItems<T>(this ICollection<T> @this, Func<T, string> keySelector) where T : class
        {
            if (@this == null || keySelector == null) return false;

            return (from i in @this
                    from y in @this
                    where i != null && y != null && i != y && keySelector(i).Equals(keySelector(y))
                    select i).Any();
        }

        public static IDictionary<TKey, TValue> ToDictionary<T, TKey, TValue>(this ICollection<T> @this,
            Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
        {
            var dic = new Dictionary<TKey, TValue>();
            if (@this == null || keySelector == null || valueSelector == null) return dic;

            foreach (var item in @this)
            {
                var key = keySelector(item);
                var value = valueSelector(item);
                if (key == null) continue;
                if (dic.ContainsKey(key)) continue;

                dic.Add(key, value);
            }

            return dic;
        }

        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> @this,
            params KeyValuePair<TKey, TValue>[] items)
        {
            if (@this == null) return;
            foreach (var i in items.Where(i => !@this.ContainsKey(i.Key)))
                @this.Add(i);
        }

        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> @this, IDictionary<TKey, TValue> items)
        {
            if (@this == null || items == null) return;
            foreach (var i in items.Where(i => !@this.ContainsKey(i.Key)))
                @this.Add(i);
        }

        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> @this,
            IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            if (@this == null || items == null) return;

            foreach (var i in items.Where(i => !@this.ContainsKey(i.Key)))
                @this.Add(i);
        }

        public static void EnqueueArrange<T>(this Queue<T> @this, IEnumerable<T> items)
        {
            if (@this == null || items == null || @this.Equals(items)) return;

            foreach (var t in items)
                @this.Enqueue(t);
        }

        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (var i in @this) action(i);
        }

        #region Merge Collection Extensions

        public static void MergeFrom<T>(this IList<T> @this, IList<T> list, Func<T, object> keySelector)
        {
            if (@this == null || list == null || keySelector == null || @this.IsReadOnly) return;
            foreach (var item in list)
            {
                if (@this.Any(i => keySelector(i).Equals(keySelector(item)))) continue;
                @this.Add(item);
            }
        }

        public static void MergeFrom<T, TKey>(this IList<T> @this, IDictionary<TKey, T> dic, Func<T, TKey> keySelector)
        {
            if (@this == null || dic == null || keySelector == null || @this.IsReadOnly) return;

            foreach (var item in dic)
            {
                if (@this.Any(i => keySelector(i).Equals(item.Key))) continue;
                @this.Add(item.Value);
            }
        }

        public static void MergeFrom<T, TKey>(this IDictionary<TKey, T> @this, IList<T> list, Func<T, TKey> keySelector)
        {
            if (@this == null || list == null || keySelector == null || @this.IsReadOnly) return;

            foreach (var item in list)
            {
                var key = keySelector(item);
                if (@this.ContainsKey(key)) continue;
                @this.Add(key, item);
            }
        }

        public static void MergeFrom<T, TKey>(this IDictionary<TKey, T> @this, IDictionary<TKey, T> dic)
        {
            if (@this == null || dic == null || @this.IsReadOnly) return;

            foreach (var item in dic)
            {
                if (@this.ContainsKey(item.Key)) continue;
                @this.Add(item.Key, item.Value);
            }
        }

        #endregion Merge Collection Extensions
    }
}
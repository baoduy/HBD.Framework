﻿using HBD.Framework.Core;
using HBD.Framework.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HBD.Framework.Collections
{
    /// <summary>
    /// The collection that won't allow to add duplication item that have the same key. They key is
    /// one of property of T and will be retrieved by getKeyFunc.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public class DistinctCollection<TKey, T> : ICollection<T>
    {
        protected Dictionary<TKey, T> InternalDic { get; }
        protected Func<T, TKey> GetKeyByItem { get; }

        public DistinctCollection(Func<T, TKey> getKeyFunc)
        {
            Guard.ArgumentIsNotNull(getKeyFunc, nameof(getKeyFunc));
            this.GetKeyByItem = getKeyFunc;
            InternalDic = new Dictionary<TKey, T>();
        }

        public virtual T this[TKey key]
        {
            get { return !InternalDic.ContainsKey(key) ? default(T) : InternalDic[key]; }
            set { InternalDic[key] = value; }
        }

        public virtual T this[int index]
        {
            get
            {
                var key = GetKeyByIndex(index);
                return key == null ? default(T) : InternalDic[key];
            }
            set
            {
                var key = GetKeyByIndex(index);
                if (key == null) return;
                InternalDic[key] = value;
            }
        }

        public int Count => InternalDic.Count;
        public bool IsReadOnly => false;

        public virtual void Add(T item)
        {
            lock (InternalDic)
            {
                if (item.IsNull()) return;
                var key = GetKeyByItem(item);

                try
                {
                    InternalDic.Add(key, item);
                }
                catch (ArgumentException) { throw new DuplicatedException(key); }
            }
        }

        public virtual void Clear() => InternalDic.Clear();

        public virtual bool Contains(T item) => InternalDic.Values.Contains(item);

        public virtual bool ContainsKey(TKey key) => InternalDic.ContainsKey(key);

        public void CopyTo(T[] array, int arrayIndex) => InternalDic.Values.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => InternalDic.Values.GetEnumerator();

        public bool Remove(T item)
        {
            if (item.IsNull()) return false;
            var key = GetKeyByItem(item);

            T tmp;
            return this.TryRemove(key, out tmp);
        }

        public bool RemoveAt(int index)
        {
            var key = GetKeyByIndex(index);
            if (key.IsNull()) return false;
            T item;
            return this.TryRemove(key, out item);
        }

        protected virtual bool TryRemove(TKey key, out T item)
        {
            item = InternalDic[key];
            return InternalDic.Remove(key);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected virtual TKey GetKeyByIndex(int index)
        {
            if (index < 0 || index >= Count) return default(TKey);
            return InternalDic.Keys.ToList()[index];
        }
    }
}
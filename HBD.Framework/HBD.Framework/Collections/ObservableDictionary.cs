#region using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Collections
{
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged,
        INotifyPropertyChanged
    {
        private const string CountString = "Count";
        private const string IndexerName = "Item[]";
        private const string KeysName = "Keys";
        private const string ValuesName = "Values";
        protected SimpleMonitor Monitor { get; }

        protected IDictionary<TKey, TValue> InternalDictionary { get; }

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => InternalDictionary.GetEnumerator();

        #endregion IEnumerable<KeyValuePair<TKey,TValue>> Members

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) InternalDictionary).GetEnumerator();

        #endregion IEnumerable Members

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion INotifyCollectionChanged Members

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged Members

        protected virtual void Insert(TKey key, TValue value, bool add)
        {
            Guard.ArgumentIsNotNull(key, nameof(key));
            Monitor.CheckReentrancy(CollectionChanged);

            TValue item;
            if (InternalDictionary.TryGetValue(key, out item))
            {
                if (add) throw new ArgumentException("An item with the same key has already been added.");
                if (Equals(item, value)) return;
                InternalDictionary[key] = value;

                OnCollectionChanged(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, value),
                    new KeyValuePair<TKey, TValue>(key, item));
            }
            else
            {
                InternalDictionary[key] = value;
                OnCollectionChanged(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value));
            }
        }

        private void RaisePropertyChanged()
        {
            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnPropertyChanged(KeysName);
            OnPropertyChanged(ValuesName);
        }

        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            using (Monitor.BlockReentrancy())
            {
                CollectionChanged?.Invoke(this, e);
            }
        }

        private void OnCollectionChanged()
        {
            RaisePropertyChanged();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> changedItem)
        {
            RaisePropertyChanged();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, changedItem));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> newItem,
            KeyValuePair<TKey, TValue> oldItem)
        {
            RaisePropertyChanged();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem));
        }

        //private void OnCollectionChanged(NotifyCollectionChangedAction action, IList newItems)
        //{
        //    RaisePropertyChanged();
        //    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItems));

        //}

        #region Constructors

        public ObservableDictionary() : this(new Dictionary<TKey, TValue>())
        {
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary) : this(dictionary, null)
        {
        }

        public ObservableDictionary(IEqualityComparer<TKey> comparer) : this(null, comparer)
        {
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            Monitor = new SimpleMonitor();
            InternalDictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        #endregion Constructors

        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value) => Insert(key, value, true);

        public bool ContainsKey(TKey key) => InternalDictionary.ContainsKey(key);

        public ICollection<TKey> Keys => InternalDictionary.Keys;

        public bool Remove(TKey key)
        {
            Guard.ArgumentIsNotNull(key, nameof(key));
            Monitor.CheckReentrancy(CollectionChanged);

            TValue value;
            if (!InternalDictionary.TryGetValue(key, out value))
                return false;

            var removed = InternalDictionary.Remove(key);
            if (removed)
                OnCollectionChanged(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, value));

            return removed;
        }

        public bool TryGetValue(TKey key, out TValue value) => InternalDictionary.TryGetValue(key, out value);

        public ICollection<TValue> Values => InternalDictionary.Values;

        public TValue this[TKey key]
        {
            get { return InternalDictionary[key]; }
            set { Insert(key, value, false); }
        }

        #endregion IDictionary<TKey,TValue> Members

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        public void Add(KeyValuePair<TKey, TValue> item) => Insert(item.Key, item.Value, true);

        public void Clear()
        {
            Monitor.CheckReentrancy(CollectionChanged);

            if (InternalDictionary.Count <= 0) return;
            InternalDictionary.Clear();
            OnCollectionChanged();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) => InternalDictionary.Contains(item);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
            => InternalDictionary.CopyTo(array, arrayIndex);

        public int Count => InternalDictionary.Count;
        public bool IsReadOnly => InternalDictionary.IsReadOnly;

        public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);

        #endregion ICollection<KeyValuePair<TKey,TValue>> Members
    }
}
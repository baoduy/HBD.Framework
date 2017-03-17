#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Collections
{
    /// <summary>
    /// The Observable SortedList
    /// Note: TKey is not allows to duplicated.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class ObservableSortedCollection<TKey, T> : ICollection<T>, ICollection, IReadOnlyList<T>, INotifyCollectionChanged, INotifyPropertyChanged where T:class, INotifyPropertyChanged
    {
        protected bool StopRaisingEvent { get; set; } = false;
        private const string CountString = "Count";
        private const string IndexerName = "Item[]";
        private readonly string _keyPropertyName;
        private readonly Func<T, TKey> _keySelector;

        public ObservableSortedCollection(Expression<Func<T, TKey>> keySelector) : this(new List<T>(), keySelector)
        {

        }

        public ObservableSortedCollection(List<T> list, Expression<Func<T, TKey>> keySelector)
        {
            Guard.ArgumentIsNotNull(keySelector, nameof(keySelector));
            Guard.ArgumentIsNotNull(list, nameof(list));

            Monitor = new SimpleMonitor();
            _keyPropertyName = keySelector.ExtractPropertyNames().FirstOrDefault();
            _keySelector = keySelector.Compile();
            InternalList = list;
        }

        protected SimpleMonitor Monitor { get; }

        protected List<T> InternalList { get; private set; }

        public void CopyTo(Array array, int index) => ((ICollection)InternalList).CopyTo(array, index);

        public object SyncRoot => ((ICollection)InternalList).SyncRoot;

        public bool IsSynchronized => ((ICollection)InternalList).IsSynchronized;

        public IEnumerator<T> GetEnumerator() => InternalList.OrderBy(_keySelector).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public virtual void Add(T item)
        {
            Monitor.CheckReentrancy(CollectionChanged);

            InternalList.Add(item);
            item.PropertyChanged += Item_PropertyChanged;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            OnPropertyChanged();
        }

        public virtual void Clear()
        {
            Monitor.CheckReentrancy(CollectionChanged);

            foreach (var item in InternalList)
                item.PropertyChanged -= Item_PropertyChanged;

            InternalList.Clear();

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnPropertyChanged();
        }

        public virtual bool Contains(T item) => InternalList.Contains(item);

        public virtual void CopyTo(T[] array, int arrayIndex) => InternalList.CopyTo(array, arrayIndex);

        public virtual bool Remove(T item)
        {
            Monitor.CheckReentrancy(CollectionChanged);
            if (!InternalList.Remove(item)) return false;

            item.PropertyChanged -= Item_PropertyChanged;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            OnPropertyChanged();

            return true;
        }

        public virtual int Count => InternalList.Count;

        public bool IsReadOnly => false;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        int IReadOnlyCollection<T>.Count => Count;

        public virtual T this[int index] => InternalList[index];

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (StopRaisingEvent) return;
            if (e.Action == NotifyCollectionChangedAction.Add && this.Count > 1)
                this.InternalList = InternalList.OrderBy(_keySelector).ToList();

            using (Monitor.BlockReentrancy())
                CollectionChanged?.Invoke(this, e);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (StopRaisingEvent) return;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnPropertyChanged()
        {
            OnPropertyChanged(IndexerName);
            OnPropertyChanged(CountString);
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != _keyPropertyName||StopRaisingEvent) return;

            lock (Monitor)
                this.InternalList = InternalList.OrderBy(_keySelector).ToList();
        }
    }
}

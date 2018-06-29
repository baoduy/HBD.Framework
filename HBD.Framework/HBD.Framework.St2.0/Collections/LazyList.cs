#region using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Collections
{
    /// <summary>
    ///     The LazyList will execute loadItemAction method at the first item accessing.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class LazyList<T> : IList<T>, INotifyCollectionChanging, INotifyCollectionChanged
    {
        private readonly Func<bool> _canLoadItems;
        private readonly ChangingObservableCollection<T> _internalCollection;
        private readonly Func<IEnumerable<T>> _valuesFactory;
        private bool _raiseEvent = true;

        public LazyList(Func<IEnumerable<T>> valuesFactory, Func<bool> canLoadItems = null)
        {
            Guard.ArgumentIsNotNull(valuesFactory, nameof(valuesFactory));
            _valuesFactory = valuesFactory;
            _canLoadItems = canLoadItems;

            _internalCollection = new ChangingObservableCollection<T>();
            _internalCollection.CollectionChanging += _items_CollectionChanging;
            _internalCollection.CollectionChanged += _items_CollectionChanged;
        }

        public bool IsInitialized { get; protected set; }

        protected bool IsInitializing => _internalCollection.IsInitializing;

        public IEnumerator<T> GetEnumerator()
        {
            EnsureInitialized();
            return _internalCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(T item) => _internalCollection.Add(item);

        public void Clear() => _internalCollection.Clear();

        public bool Contains(T item)
        {
            EnsureInitialized();
            return _internalCollection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            EnsureInitialized();
            _internalCollection.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            EnsureInitialized();
            return _internalCollection.Remove(item);
        }

        public int Count
        {
            get
            {
                EnsureInitialized();
                return _internalCollection.Count;
            }
        }

        public bool IsReadOnly => false;

        public int IndexOf(T item)
        {
            EnsureInitialized();
            return _internalCollection.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            EnsureInitialized();
            _internalCollection.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            EnsureInitialized();
            _internalCollection.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                EnsureInitialized();
                return _internalCollection[index];
            }
            set => _internalCollection[index] = value;
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event EventHandler<NotifyCollectionChangingEventArgs> CollectionChanging;

        /// <summary>
        ///     Clear all items and re-set the auto load setting. The valuesFactory will be re-call to reload the items again.
        /// </summary>
        public void ClearAndReset(bool raiseChangeEvent = false)
        {
            _raiseEvent = raiseChangeEvent;

            Clear();

            IsInitialized = false;
            _raiseEvent = true;
        }

        private void _items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            => OnCollectionChanged(e);

        private void _items_CollectionChanging(object sender, NotifyCollectionChangingEventArgs e)
            => OnCollectionChanging(e);

        public virtual void EnsureInitialized()
        {
            if (_canLoadItems?.Invoke() == false) return;
            if (IsInitialized || _valuesFactory == null) return;

            lock (_internalCollection)
            {
                IsInitialized = true;

                _internalCollection.BeginInit();
                _internalCollection.AddRange(_valuesFactory.Invoke());
                _internalCollection.EndInit();
            }
        }

        protected virtual void OnCollectionChanging(NotifyCollectionChangingEventArgs e)
        {
            if (_raiseEvent)
                CollectionChanging?.Invoke(this, e);
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (_raiseEvent)
                CollectionChanged?.Invoke(this, e);
        }
    }
}
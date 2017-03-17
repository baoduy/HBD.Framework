#region

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#endregion

namespace HBD.Framework.Collections
{
    public class ObservableDistinctCollection<TKey, T> : DistinctCollection<TKey, T>, INotifyCollectionChanged,
        INotifyPropertyChanged
    {
        private const string CountString = "Count";
        private const string IndexerName = "Item[]";

        public ObservableDistinctCollection(Func<T, TKey> getKeyFunc) : base(getKeyFunc)
        {
            Monitor = new SimpleMonitor();
        }

        protected SimpleMonitor Monitor { get; }

        public override T this[TKey key]
        {
            get { return base[key]; }
            set
            {
                Monitor.CheckReentrancy(CollectionChanged);
                base[key] = value;
                OnCollectionChanged(NotifyCollectionChangedAction.Replace, value);
            }
        }

        public override T this[int index]
        {
            get { return base[index]; }
            set
            {
                Monitor.CheckReentrancy(CollectionChanged);
                base[index] = value;
                OnCollectionChanged(NotifyCollectionChangedAction.Replace, value);
            }
        }

        public override void Add(T item)
        {
            Monitor.CheckReentrancy(CollectionChanged);
            base.Add(item);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
        }

        public override void Clear()
        {
            Monitor.CheckReentrancy(CollectionChanged);
            base.Clear();
            OnCollectionChanged();
        }

        protected override bool TryRemove(TKey key, out T item)
        {
            Monitor.CheckReentrancy(CollectionChanged);
            var s = base.TryRemove(key, out item);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, item);
            return s;
        }

        private void RaisePropertyChanged()
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged(CountString);
            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged(IndexerName);
        }

        private void OnCollectionChanged()
        {
            RaisePropertyChanged();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, T changedItem)
        {
            RaisePropertyChanged();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, changedItem));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, T newItem, T oldItem)
        {
            RaisePropertyChanged();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem));
        }

        #region Events

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            using (Monitor.BlockReentrancy())
            {
                CollectionChanged?.Invoke(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion Events
    }
}
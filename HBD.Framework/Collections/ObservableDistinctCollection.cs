using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HBD.Framework.Collections
{
    public class ObservableDistinctCollection<TKey, T> : DistinctCollection<TKey, T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        protected SimpleMonitor Monitor { get; } = new SimpleMonitor();
        private const string CountString = "Count";
        private const string IndexerName = "Item[]";

        public ObservableDistinctCollection(Func<T, TKey> getKeyFunc) : base(getKeyFunc)
        {
        }

        public override T this[TKey key]
        {
            get { return base[key]; }
            set
            {
                this.Monitor.CheckReentrancy(this.CollectionChanged);
                base[key] = value;
                this.OnCollectionChanged(NotifyCollectionChangedAction.Replace, value);
            }
        }

        public override T this[int index]
        {
            get { return base[index]; }
            set
            {
                this.Monitor.CheckReentrancy(this.CollectionChanged);
                base[index] = value;
                this.OnCollectionChanged(NotifyCollectionChangedAction.Replace, value);
            }
        }

        public override void Add(T item)
        {
            this.Monitor.CheckReentrancy(this.CollectionChanged);
            base.Add(item);
            this.OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
        }

        public override void Clear()
        {
            this.Monitor.CheckReentrancy(this.CollectionChanged);
            base.Clear();
            this.OnCollectionChanged();
        }

        protected override bool TryRemove(TKey key, out T item)
        {
            this.Monitor.CheckReentrancy(this.CollectionChanged);
            var s = base.TryRemove(key, out item);
            this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, item);
            return s;
        }

        #region Events

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            using (Monitor.BlockReentrancy())
                CollectionChanged?.Invoke(this, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion Events

        private void RaisePropertyChanged()
        {
            this.OnPropertyChanged(CountString);
            this.OnPropertyChanged(IndexerName);
        }

        private void OnCollectionChanged()
        {
            RaisePropertyChanged();
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, T changedItem)
        {
            RaisePropertyChanged();
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, changedItem));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, T newItem, T oldItem)
        {
            RaisePropertyChanged();
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem));
        }
    }
}
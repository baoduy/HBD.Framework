#region using

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

#endregion

namespace HBD.Framework.Collections
{
    /// <summary>
    ///     The extension of ObservableCollection that support INotifyCollectionChanging,
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class ChangingObservableCollection<T> : ObservableCollection<T>,
        INotifyCollectionChanging, ISupportInitialize
    {
        public bool IsInitializing { get; private set; }

        public event EventHandler<NotifyCollectionChangingEventArgs> CollectionChanging;

        public virtual void BeginInit() => IsInitializing = true;

        public virtual void EndInit() => IsInitializing = false;

        protected virtual void OnCollectionChanging(NotifyCollectionChangingEventArgs e)
        {
            using (BlockReentrancy())
            {
                CollectionChanging?.Invoke(this, e);
            }
        }

        protected override void RemoveItem(int index)
        {
            var obj = this[index];
            var args = new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Remove, obj, index);
            OnCollectionChanging(args);
            if (args.Cancel) return;
            base.RemoveItem(index);
        }

        /// <summary>
        ///     Inserts an item into the collection at the specified index.
        /// </summary>
        /// <param name="index">
        ///     The zero-based index at which <paramref name="item" /> should be inserted.
        /// </param>
        /// <param name="item">The object to insert.</param>
        protected override void InsertItem(int index, T item)
        {
            var args = new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Add, item, index);
            OnCollectionChanging(args);
            if (args.Cancel) return;
            base.InsertItem(index, item);
        }

        /// <summary>
        ///     Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index.</param>
        protected override void SetItem(int index, T item)
        {
            var obj = this[index];
            var args = new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Replace, obj, item, index);
            OnCollectionChanging(args);
            if (args.Cancel) return;

            base.SetItem(index, item);
        }

        /// <summary>
        ///     Moves the item at the specified index to a new location in the collection.
        /// </summary>
        /// <param name="oldIndex">
        ///     The zero-based index specifying the location of the item to be moved.
        /// </param>
        /// <param name="newIndex">The zero-based index specifying the new location of the item.</param>
        protected override void MoveItem(int oldIndex, int newIndex)
        {
            var obj = this[oldIndex];
            var args = new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Move, obj, newIndex, oldIndex);
            OnCollectionChanging(args);
            if (args.Cancel) return;
            base.MoveItem(oldIndex, newIndex);
        }
    }
}
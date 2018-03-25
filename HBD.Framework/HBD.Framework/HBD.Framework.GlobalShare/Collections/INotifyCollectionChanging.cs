#region using

using System;
using System.Collections;
using System.Collections.Specialized;

#endregion

namespace HBD.Framework.Collections
{
    public interface INotifyCollectionChanging
    {
        event EventHandler<NotifyCollectionChangingEventArgs> CollectionChanging;
    }

    public class NotifyCollectionChangingEventArgs : NotifyCollectionChangedEventArgs
    {
        public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action) : base(action)
        {
        }

        public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, IList changedItems)
            : base(action, changedItems)
        {
        }

        public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, object changedItem)
            : base(action, changedItem)
        {
        }

        public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems)
            : base(action, newItems, oldItems)
        {
        }

        public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem)
            : base(action, newItem, oldItem)
        {
        }

        public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, IList changedItems,
            int startingIndex) : base(action, changedItems, startingIndex)
        {
        }

        public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, object changedItem, int index)
            : base(action, changedItem, index)
        {
        }

        public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, IList changedItems, int index,
            int oldIndex) : base(action, changedItems, index, oldIndex)
        {
        }

        public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, object changedItem, int index,
            int oldIndex) : base(action, changedItem, index, oldIndex)
        {
        }

        public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems,
            int startingIndex) : base(action, newItems, oldItems, startingIndex)
        {
        }

        public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem,
            int index) : base(action, newItem, oldItem, index)
        {
        }

        public bool Cancel { get; set; }
    }
}
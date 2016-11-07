using System;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace HBD.Framework.Core
{
    public class ChangeTrackingEntry<TEntity> : IUnChangableTracking where TEntity : class, INotifyPropertyChanging, INotifyPropertyChanged
    {
        public ChangeTrackingEntry(TEntity entity)
        {
            Guard.ArgumentIsNotNull(entity, nameof(entity));
            Entity = entity;
            Entity.PropertyChanging += Entity_PropertyChanging;
            Entity.PropertyChanged += Entity_PropertyChanged;
        }

        private void Entity_PropertyChanged(object sender, PropertyChangedEventArgs e)
            => IsChanged = true;

        private void Entity_PropertyChanging(object sender, PropertyChangingEventArgs e)
            => this.OriginalValues.GetOrAdd(e.PropertyName, this.Entity.GetValueFromProperty(e.PropertyName));

        public TEntity Entity { get; }
        private ConcurrentDictionary<string, object> OriginalValues { get; } = new ConcurrentDictionary<string, object>();
        public bool IsChanged { get; private set; }

        public event EventHandler<CancelEventArgs> AcceptChange;

        public event EventHandler<CancelEventArgs> UndoChange;

        protected virtual void OnAcceptChange(CancelEventArgs e)
            => this.AcceptChange?.Invoke(this, e);

        protected virtual void OnUndoChange(CancelEventArgs e)
           => this.UndoChange?.Invoke(this, e);

        /// <summary>
        /// This will be remove the original of properties accept the current values as latest one.
        /// </summary>
        public void AcceptChanges()
        {
            if (!this.IsChanged) return;

            lock (this.OriginalValues)
            {
                var evnArg = new CancelEventArgs();
                this.OnAcceptChange(evnArg);
                if (evnArg.Cancel) return;

                this.OriginalValues.Clear();
                this.IsChanged = false;
            }
        }

        public void UndoChanges()
        {
            if (!this.IsChanged) return;

            lock (this.OriginalValues)
            {
                var evnArg = new CancelEventArgs();
                this.OnUndoChange(evnArg);
                if (evnArg.Cancel) return;

                Entity.PropertyChanging -= Entity_PropertyChanging;
                Entity.PropertyChanged -= Entity_PropertyChanged;

                foreach (var item in this.OriginalValues) this.Entity.SetValueToProperty(item.Key, item.Value);

                Entity.PropertyChanging += Entity_PropertyChanging;
                Entity.PropertyChanged += Entity_PropertyChanged;

                this.IsChanged = false;
            }
        }
    }
}
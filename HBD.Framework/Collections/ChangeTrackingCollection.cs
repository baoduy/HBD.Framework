using HBD.Framework.Core;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace HBD.Framework.Collections
{
    public class ChangeTrackingCollection<TEntity> : ICollection<TEntity>, IUnChangableTracking where TEntity : class, INotifyPropertyChanging, INotifyPropertyChanged
    {
        //protected SimpleMonitor Monitor { get; } = new SimpleMonitor();

        public ChangeTrackingCollection(IEnumerable<TEntity> collection = null)
        {
            this.InternalList = new List<ChangeTrackingEntry<TEntity>>();
            this.InternalList.AddRange(collection?.Select(e => new ChangeTrackingEntry<TEntity>(e)));
        }

        public IList<TEntity> ChangedItems => InternalList.Where(e => e.IsChanged).Select(e => e.Entity).ToList();

        public TEntity this[int index]
        {
            get { return this.InternalList[index].Entity; }
            set { this.InternalList[index] = new ChangeTrackingEntry<TEntity>(value); }
        }

        public int Count => InternalList.Count;

        public bool IsReadOnly => InternalList.IsReadOnly;

        private IList<ChangeTrackingEntry<TEntity>> InternalList { get; }

        public bool IsChanged => InternalList.Any(e => e.IsChanged);

        public void Add(TEntity item)
        {
            if (this.Contains(item)) return;
            InternalList.Add(new ChangeTrackingEntry<TEntity>(item));
        }

        public void Clear() => InternalList.Clear();

        public bool Contains(TEntity item) => this.InternalList.Any(i => i.Entity == item);

        public void CopyTo(TEntity[] array, int arrayIndex)
        {
            var list = InternalList.Select(e => e.Entity).ToList();
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TEntity> GetEnumerator() => InternalList.Select(e => e.Entity).GetEnumerator();

        public bool Remove(TEntity item)
        {
            var entry = this.InternalList.FirstOrDefault(e => e.Entity == item);
            return entry != null && InternalList.Remove(entry);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Undo changes for all items
        /// </summary>
        public void UndoChanges()
        {
            foreach (var source in InternalList.Where(e => e.IsChanged))
                source.UndoChanges();
        }

        /// <summary>
        /// Accept changes for all items
        /// </summary>
        public void AcceptChanges()
        {
            foreach (var source in InternalList.Where(e => e.IsChanged))
                source.AcceptChanges();
        }

        /// <summary>
        /// Undo changes for all items
        /// </summary>
        public void UndoChanges(params TEntity[] items)
        {
            foreach (var item in items)
            {
                var entry = InternalList.FirstOrDefault(e => e.Entity == item);
                if (entry == null || !entry.IsChanged) continue;
                entry.UndoChanges();
            }
        }

        /// <summary>
        /// Accept changes for all items
        /// </summary>
        public void AcceptChanges(params TEntity[] items)
        {
            foreach (var item in items)
            {
                var entry = InternalList.FirstOrDefault(e => e.Entity == item);
                if (entry == null || !entry.IsChanged) continue;
                entry.AcceptChanges();
            }
        }
    }
}
#region using

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Collections
{
    public class ChangeTrackingCollection<TEntity> : ICollection<TEntity>
        where TEntity : class, ICancelableNotifyPropertyChanging, INotifyPropertyChanged
    {
        public ChangeTrackingCollection(IEnumerable<TEntity> collection = null)
        {
            InternalList = new List<ChangeTrackingEntry<TEntity>>();
            InternalList.AddRange(collection?.Select(e => new ChangeTrackingEntry<TEntity>(e)));
        }

        public IList<TEntity> ChangedItems => InternalList.Where(e => e.IsChanged).Select(e => e.Entity).ToList();

        public TEntity this[int index]
        {
            get => InternalList[index].Entity;
            set => InternalList[index] = new ChangeTrackingEntry<TEntity>(value);
        }

        private IList<ChangeTrackingEntry<TEntity>> InternalList { get; }

        public bool IsChanged => InternalList.Any(e => e.IsChanged);

        public int Count => InternalList.Count;

        public bool IsReadOnly => InternalList.IsReadOnly;

        public void Add(TEntity item)
        {
            if (Contains(item)) return;
            InternalList.Add(new ChangeTrackingEntry<TEntity>(item));
        }

        public void Clear() => InternalList.Clear();

        public bool Contains(TEntity item) => InternalList.Any(i => i.Entity == item);

        public void CopyTo(TEntity[] array, int arrayIndex)
        {
            var list = InternalList.Select(e => e.Entity).ToList();
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TEntity> GetEnumerator() => InternalList.Select(e => e.Entity).GetEnumerator();

        public bool Remove(TEntity item)
        {
            var entry = InternalList.FirstOrDefault(e => e.Entity == item);
            return entry != null && InternalList.Remove(entry);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        ///     Undo changes for all items
        /// </summary>
        public void UndoChanges()
        {
            foreach (var source in InternalList.Where(e => e.IsChanged))
                source.UndoChanges();
        }

        /// <summary>
        ///     Accept changes for all items
        /// </summary>
        public void AcceptChanges()
        {
            foreach (var source in InternalList.Where(e => e.IsChanged))
                source.AcceptChanges();
        }

        public virtual ChangeTrackingEntry<TEntity> Entry(TEntity item)
            => InternalList.FirstOrDefault(e => e.Entity == item);

        /// <summary>
        ///     Undo changes for all items
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
        ///     Accept changes for all items
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
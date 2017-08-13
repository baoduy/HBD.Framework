#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Collections
{
    /// <summary>
    ///     The Observable SortedList
    ///     Note: TKey is not allows to duplicated.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class ObservableSortedCollection<TKey, T> : System.Collections.ObjectModel.ObservableCollection<T> where T : class, INotifyPropertyChanged
    {
        private readonly IComparer<TKey> _compare;
        private readonly string _keyPropertyName;
        private readonly Func<T, TKey> _keySelector;

        public ObservableSortedCollection(Expression<Func<T, TKey>> keySelector, IComparer<TKey> compare = null)
        {
            _compare = compare;
            Guard.ArgumentIsNotNull(keySelector, nameof(keySelector));

            _keyPropertyName = keySelector.ExtractPropertyNames().FirstOrDefault();
            _keySelector = keySelector.Compile();
        }

        private int GetIndex(T item)
        {
            //No item to compare then just add the item in.
            if (this.Count <= 0) return 0;

            var key = _keySelector.Invoke(item);
            var compare = _compare ?? Comparer<TKey>.Default;

            if (!ReferenceEquals(this[this.Count - 1], item))
            {
                var lastKey = _keySelector.Invoke(this[this.Count - 1]);

                //If the key of Item is greater than or equals last item in the list.
                //Then add item to the end.
                if (compare.Compare(key, lastKey) >= 0)
                    return this.Contains(item) ? this.Count - 1 : this.Count;
            }

            var current = this.Count - 2;
            while (current >= 0)
            {
                var k = _keySelector.Invoke(this[current]);

                //if k <= key
                var r = compare.Compare(k, key);
                if (r < 0)
                    return current + 1;
                else if (r == 0)
                    return current;

                current--;
            }

            //Insert to the first position.
            return 0;
        }

        protected override void InsertItem(int index, T item)
        {
            var newIndex = GetIndex(item);

            if (newIndex < 0)
                newIndex = index;

            item.PropertyChanged += Item_PropertyChanged;

            base.InsertItem(newIndex, item);
        }

        protected override void SetItem(int index, T item)
        {
            this.RemoveAt(index);
            this.InsertItem(index, item);
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.NotEqualsIgnoreCase(_keyPropertyName)) return;

            var item = sender as T;
            if (item == null) return;

            var current = this.IndexOf(item);
            var index = GetIndex(item);

            if (current == index) return;
            base.Move(current, index);
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];
            item.PropertyChanged -= Item_PropertyChanged;

            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            foreach (var item in this)
                item.PropertyChanged -= Item_PropertyChanged;
            base.ClearItems();
        }
    }

    public class ObservableSortedCollection<T> : ObservableSortedCollection<int, T> where T : class, INotifyPropertyChanged
    {
        public ObservableSortedCollection(Expression<Func<T, int>> keySelector, IComparer<int> compare = null)
            : base(keySelector, compare)
        {
        }
    }
}
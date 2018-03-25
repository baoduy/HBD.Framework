#region

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using HBD.Framework.Collections;
using HBD.Framework.Exceptions;

#endregion

namespace HBD.Framework.Security.Azman.Base
{
    public class AzItemCollection<T> : LazyList<T> where T : AzItem
    {
        private readonly AzItem _parent;

        public AzItemCollection(AzItem parent, Func<IEnumerable<T>> loadItemAction, Func<bool> canAutoLoad = null)
            : base(loadItemAction, canAutoLoad)
        {
            _parent = parent;
            CollectionChanging += AzItemCollection_CollectionChanging;
            CollectionChanged += AzItemCollection_CollectionChanged;
        }

        public T this[string name] => this.FirstOrDefault(i => i.Name.EqualsIgnoreCase(name));

        private void AzItemCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Call Delete method with removed ones.
            e.OldItems?.OfType<T>().ForEach(i =>
            {
                i.Delete();
                i.Parent = null;
            });

            //Update parent
            if (_parent == null) return;

            if (!IsInitializing)
                _parent.HasChanged = true;

            e.NewItems?.OfType<T>().ForEach(i => i.Parent = _parent);
        }

        private void AzItemCollection_CollectionChanging(object sender, NotifyCollectionChangingEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add
                || e.Action == NotifyCollectionChangedAction.Replace)
            {
                var list = e.NewItems.OfType<T>().ToList();

                foreach (var item in list.Where(item => this.Any(i => i.Name.EqualsIgnoreCase(item.Name))))
                    if (IsInitializing) e.Cancel = true;
                    else throw new DuplicatedException(item.Name);
            }
        }

        public void Remove(string name)
        {
            var item = this[name];
            if (item == null) return;
            Remove(item);
        }
    }
}
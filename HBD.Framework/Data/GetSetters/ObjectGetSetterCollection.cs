using HBD.Framework.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HBD.Framework.Data.GetSetters
{
    public class ObjectGetSetterCollection<T> : IGetSetterCollection where T : class
    {
        public IList<T> OriginalCollection { get; }

        public string Name => typeof(T).Name;
        public IGetSetter Header => new ObjectPropertyGetSetter(this.OriginalCollection.FirstOrDefault());

        public ObjectGetSetterCollection(IEnumerable<T> collection)
        {
            Guard.ArgumentIsNotNull(collection, nameof(collection));
            this.OriginalCollection = collection.ToList();
            Guard.CollectionMustNotEmpty(this.OriginalCollection, nameof(collection));
        }

        public ObjectGetSetterCollection(params T[] collection)
        {
            Guard.ArgumentIsNotNull(collection, nameof(collection));
            Guard.CollectionMustNotEmpty(collection, nameof(collection));
            this.OriginalCollection = collection;
        }

        public IEnumerator<IGetSetter> GetEnumerator()
        {
            foreach (var obj in this.OriginalCollection)
                yield return new ObjectValueGetSetter(obj);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
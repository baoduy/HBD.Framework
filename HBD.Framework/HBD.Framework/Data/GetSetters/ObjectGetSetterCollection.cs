#region using

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Data.GetSetters
{
    public class ObjectGetSetterCollection<T> : IGetSetterCollection where T : class
    {
        public ObjectGetSetterCollection(IEnumerable<T> collection)
        {
            Guard.ArgumentIsNotNull(collection, nameof(collection));
            OriginalCollection = collection.ToList();
            Guard.ShouldNotBeEmpty(OriginalCollection, nameof(collection));
        }

        public ObjectGetSetterCollection(params T[] collection)
        {
            Guard.ArgumentIsNotNull(collection, nameof(collection));
            Guard.ShouldNotBeEmpty(collection, nameof(collection));
            OriginalCollection = collection;
        }

        public IList<T> OriginalCollection { get; }

        public string Name => typeof(T).Name;
        public IGetSetter Header => new ObjectPropertyGetSetter(OriginalCollection.FirstOrDefault());

        public IEnumerator<IGetSetter> GetEnumerator()
        {
            foreach (var obj in OriginalCollection)
                yield return new ObjectValueGetSetter(obj);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
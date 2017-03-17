#region

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

namespace HBD.Framework.Core
{
    public class Enumerator<TItem> : IEnumerator<TItem>
    {
        private readonly IEnumerator _items;

        public Enumerator(IEnumerator items)
        {
            Guard.ArgumentIsNotNull(items, "Enumerable interface");
            _items = items;
        }

        public TItem Current => (TItem) _items.Current;

        public void Dispose()
        {
            var items = _items as IDisposable;
            items?.Dispose();
        }

        object IEnumerator.Current => _items.Current;

        public bool MoveNext() => _items.MoveNext();

        public void Reset() => _items.Reset();
    }
}
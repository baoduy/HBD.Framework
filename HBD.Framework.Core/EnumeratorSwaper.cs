using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace HBD.Framework.Core
{
    public class EnumeratorSwaper<TItem> : IEnumerator<TItem>
    {
        IEnumerator _items = null;
        public EnumeratorSwaper(IEnumerator items)
        {
            this._items = items;
            Guard.ArgumentNotNull(items, "Enumerable interface");
        }

        public TItem Current
        {
            get { return (TItem)this._items.Current; }
        }

        public void Dispose()
        { }

        object System.Collections.IEnumerator.Current
        {
            get { return this._items.Current; }
        }

        public bool MoveNext()
        {
            return this._items.MoveNext();
        }

        public void Reset()
        {
            this._items.Reset();
        }
    }
}

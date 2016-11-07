﻿using HBD.Framework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HBD.Framework.Data.GetSetters
{
    public class ArrayGetSetter : IGetSetter
    {
        private readonly IList _collection;

        public ArrayGetSetter(IList collection)
        {
            Guard.ArgumentIsNotNull(collection, nameof(collection));
            this._collection = collection;
        }

        public IEnumerator<object> GetEnumerator() => _collection.Cast<object>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        object IGetSetter.this[string name]
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public object this[int index]
        {
            get { return _collection[index]; }
            set { _collection[index] = value; }
        }
    }
}
using HBD.Framework.Core;
using HBD.Framework.Data.EntityConverters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HBD.Framework.Data.GetSetters
{
    internal class ObjectPropertyGetSetter : IGetSetter
    {
        public object OriginalObject { get; }
        private ColumnMappingInfo[] _columnInfos = null;

        public ObjectPropertyGetSetter(object obj)
        {
            Guard.ArgumentIsNotNull(obj, nameof(obj));
            this.OriginalObject = obj;
        }

        private void EnsureColumnInfos()
        {
            if (_columnInfos == null)
                _columnInfos = OriginalObject.GetColumnMapping().ToArray();
        }

        public object this[string name]
        {
            get
            {
                this.EnsureColumnInfos();
                return this._columnInfos.FirstOrDefault(c => c.PropertyName.EqualsIgnoreCase(name) || c.FieldName.EqualsIgnoreCase(name))?.FieldName;
            }
            set { throw new NotSupportedException("Set Property"); }
        }

        public object this[int index]
        {
            get
            {
                this.EnsureColumnInfos();
                return this._columnInfos[index].FieldName;
            }
            set { throw new NotSupportedException("Set Property"); }
        }

        public IEnumerator<object> GetEnumerator()
        {
            this.EnsureColumnInfos();
            foreach (var p in _columnInfos)
                yield return p.FieldName;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
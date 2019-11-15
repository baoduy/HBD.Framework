#region using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HBD.Framework.Core;
using HBD.Framework.Data.EntityConverters;
using HBD.Framework.Extensions;

#endregion

namespace HBD.Framework.Data.GetSetters
{
    internal class ObjectPropertyGetSetter : IGetSetter
    {
        private ColumnMappingInfo[] _columnInfos;

        public ObjectPropertyGetSetter(object obj)
        {
            Guard.ArgumentIsNotNull(obj, nameof(obj));
            OriginalObject = obj;
        }

        public object OriginalObject { get; }

        public int Count { get { EnsureColumnInfos(); return _columnInfos.Length; } }

        public object this[string name]
        {
            get
            {
                EnsureColumnInfos();
                return
                    _columnInfos.FirstOrDefault(
                        c => c.PropertyName.EqualsIgnoreCase(name) || c.FieldName.EqualsIgnoreCase(name))?.FieldName;
            }
            set => throw new NotSupportedException("Set Property");
        }

        public object this[int index]
        {
            get
            {
                EnsureColumnInfos();
                return _columnInfos[index].FieldName;
            }
            set => throw new NotSupportedException("Set Property");
        }

        public IEnumerator<object> GetEnumerator()
        {
            EnsureColumnInfos();
            foreach (var p in _columnInfos)
                yield return p.FieldName;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void EnsureColumnInfos()
        {
            if (_columnInfos == null)
                _columnInfos = OriginalObject.GetColumnMapping().ToArray();
        }
    }
}
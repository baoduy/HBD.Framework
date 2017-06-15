#region using

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HBD.Framework.Core;
using HBD.Framework.Data.EntityConverters;

#endregion

namespace HBD.Framework.Data.GetSetters
{
    internal class ObjectValueGetSetter : IGetSetter
    {
        private ColumnMappingInfo[] _columnInfos;

        public ObjectValueGetSetter(object obj)
        {
            Guard.ArgumentIsNotNull(obj, nameof(obj));
            OriginalObject = obj;
        }

        public object OriginalObject { get; }

        public object this[string name]
        {
            get
            {
                EnsureColumnInfos();
                var col =
                    _columnInfos.FirstOrDefault(
                        c => c.PropertyName.EqualsIgnoreCase(name) || c.FieldName.EqualsIgnoreCase(name));
                return col?.PropertyInfo.GetValue(OriginalObject);
            }
            set
            {
                EnsureColumnInfos();
                var col =
                    _columnInfos.FirstOrDefault(
                        c => c.PropertyName.EqualsIgnoreCase(name) || c.FieldName.EqualsIgnoreCase(name));
                col?.PropertyInfo.SetValue(OriginalObject, value);
            }
        }

        public object this[int index]
        {
            get
            {
                EnsureColumnInfos();
                var col = _columnInfos[index];
                return col.PropertyInfo.GetValue(OriginalObject);
            }
            set
            {
                EnsureColumnInfos();
                var col = _columnInfos[index];
                col.PropertyInfo.SetValue(OriginalObject, value);
            }
        }

        public IEnumerator<object> GetEnumerator()
        {
            EnsureColumnInfos();

            foreach (var p in _columnInfos)
                yield return p.PropertyInfo.GetValue(OriginalObject);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void EnsureColumnInfos()
        {
            if (_columnInfos == null)
                _columnInfos = OriginalObject.GetColumnMapping().ToArray();
        }
    }
}
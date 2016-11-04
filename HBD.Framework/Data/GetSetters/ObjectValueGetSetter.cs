using HBD.Framework.Core;
using HBD.Framework.Data.EntityConverters;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HBD.Framework.Data.GetSetters
{
    internal class ObjectValueGetSetter : IGetSetter
    {
        public object OriginalObject { get; }
        private ColumnMappingInfo[] _columnInfos = null;

        public ObjectValueGetSetter(object obj)
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
                var col = this._columnInfos.FirstOrDefault(c => c.PropertyName.EqualsIgnoreCase(name) || c.FieldName.EqualsIgnoreCase(name));
                return col?.PropertyInfo.GetValue(this.OriginalObject);
            }
            set
            {
                this.EnsureColumnInfos();
                var col = this._columnInfos.FirstOrDefault(c => c.PropertyName.EqualsIgnoreCase(name) || c.FieldName.EqualsIgnoreCase(name));
                col?.PropertyInfo.SetValue(this.OriginalObject, value);
            }
        }

        public object this[int index]
        {
            get
            {
                this.EnsureColumnInfos();
                var col = this._columnInfos[index];
                return col.PropertyInfo.GetValue(this.OriginalObject);
            }
            set
            {
                this.EnsureColumnInfos();
                var col = this._columnInfos[index];
                col.PropertyInfo.SetValue(this.OriginalObject, value);
            }
        }

        public IEnumerator<object> GetEnumerator()
        {
            this.EnsureColumnInfos();

            foreach (var p in _columnInfos)
                yield return p.PropertyInfo.GetValue(this.OriginalObject);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
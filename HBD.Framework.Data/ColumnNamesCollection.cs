using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using HBD.Framework.Data;
using HBD.Framework.Core;

namespace HBD.Framework.Data
{
    public class ColumnNamesCollection : List<string>, ICloneable<ColumnNamesCollection>
    {
        public ColumnNamesCollection() { }
        public ColumnNamesCollection(DataColumnCollection columns) : this(columns != null ? columns.Cast<DataColumn>() : null) { }

        public ColumnNamesCollection(IEnumerable<DataColumn> columns)
        {
            if (columns == null) return;
            foreach (DataColumn col in columns)
                this.Add(col.ColumnName);
        }

        public ColumnNamesCollection(IEnumerable<string> columns)
        {
            foreach (var col in columns)
                this.Add(col);
        }

        public virtual ColumnNamesCollection Copy()
        {
            return new ColumnNamesCollection(this);
        }

        public static implicit operator ColumnNamesCollection(DataColumnCollection columns)
        {
            return new ColumnNamesCollection(columns);
        }

        public ColumnNamesCollection Clone()
        {
            throw new NotImplementedException();
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}

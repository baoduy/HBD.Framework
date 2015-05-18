using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using HBD.Framework.Data;
using HBD.Framework.Core;

namespace HBD.Framework.Data
{
    public class ColumnItemCollection : List<ColumnItem>, ICloneable<ColumnItemCollection>
    {
        public ColumnItemCollection() { }
        public ColumnItemCollection(DataColumnCollection columns) : this(columns != null ? columns.Cast<DataColumn>() : null) { }

        public ColumnItemCollection(IEnumerable<DataColumn> columns)
        {
            if (columns == null) return;
            foreach (DataColumn col in columns)
                this.Add(col.ColumnName, col.DataType);
        }

        public ColumnItemCollection(IEnumerable<ColumnItem> columns)
        {
            foreach (var col in columns)
                this.Add(col.Name, col.DataType);
        }

        public ColumnItem this[string name]
        { get { return this.FirstOrDefault(i => i.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)); } }

        public void Add(string name, Type dataType)
        {
            Guard.ArgumentNotNull(name, "Name");
            Guard.ArgumentNotNull(dataType, "DataType");

            if (this[name] != null) return;
            base.Add(new ColumnItem() { Name = name, DataType = dataType });
        }

        public void Remove(string name)
        {
            var item = this[name];
            if (item != null)
                this.Remove(item);
        }

        public static implicit operator ColumnItemCollection(DataColumnCollection columns)
        {
            return new ColumnItemCollection(columns);
        }

        public ColumnItemCollection Clone()
        {
            return new ColumnItemCollection(this);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}

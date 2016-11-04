using HBD.Framework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace HBD.Framework.Data.GetSetters
{
    internal class DataColumnGetSetter : IGetSetter
    {
        public DataTable OriginalTable { get; }

        public DataColumnGetSetter(DataTable table)
        {
            Guard.ArgumentIsNotNull(table, nameof(table));
            this.OriginalTable = table;
        }

        public object this[int index]
        {
            get { return this.OriginalTable.Columns[index].ColumnName; }
            set { this.OriginalTable.Columns[index].ColumnName = value.ToString(); }
        }

        public object this[string name]
        {
            get { return this.OriginalTable.Columns.IndexOf(name); }
            set { throw new NotSupportedException(); }
        }

        public int Count => this.OriginalTable.Columns.Count;

        public IEnumerator<object> GetEnumerator()
            => (from DataColumn col in this.OriginalTable.Columns select col.ColumnName).Cast<object>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
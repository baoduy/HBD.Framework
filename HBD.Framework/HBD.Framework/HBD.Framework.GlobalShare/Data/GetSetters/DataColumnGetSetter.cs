#region using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Data.GetSetters
{
#if !NETSTANDARD1_6
    internal class DataColumnGetSetter : IGetSetter
    {
        public DataColumnGetSetter(DataTable table)
        {
            Guard.ArgumentIsNotNull(table, nameof(table));
            OriginalTable = table;
        }

        public DataTable OriginalTable { get; }

        public int Count => OriginalTable.Columns.Count;

        public object this[int index]
        {
            get => OriginalTable.Columns[index].ColumnName;
            set => OriginalTable.Columns[index].ColumnName = value.ToString();
        }

        public object this[string name]
        {
            get => OriginalTable.Columns.IndexOf(name);
            set => throw new NotSupportedException();
        }

        public IEnumerator<object> GetEnumerator()
            => (from DataColumn col in OriginalTable.Columns select col.ColumnName).Cast<object>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
#endif
}
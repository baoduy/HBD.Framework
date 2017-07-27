#region using

using System.Collections;
using System.Collections.Generic;
using System.Data;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Data.GetSetters
{
#if !NETSTANDARD1_6
    public class DataTableGetSetterCollection : IGetSetterCollection
    {
        public DataTableGetSetterCollection(DataTable table)
        {
            Guard.ArgumentIsNotNull(table, nameof(table));
            OriginalTable = table;
        }

        public DataTable OriginalTable { get; }

        public string Name => OriginalTable?.TableName;
        public IGetSetter Header => new DataColumnGetSetter(OriginalTable);

        public int Count => OriginalTable.Rows.Count;

        public IEnumerator<IGetSetter> GetEnumerator()
        {
            foreach (DataRow row in OriginalTable.Rows)
                yield return new DataRowGetSetter(row);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
#endif
}
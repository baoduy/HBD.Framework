using HBD.Framework.Core;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace HBD.Framework.Data.GetSetters
{
    public class DataTableGetSetterCollection : IGetSetterCollection
    {
        public DataTable OriginalTable { get; }

        public string Name => this.OriginalTable?.TableName;
        public IGetSetter Header => new DataColumnGetSetter(this.OriginalTable);

        public DataTableGetSetterCollection(DataTable table)
        {
            Guard.ArgumentIsNotNull(table, nameof(table));
            this.OriginalTable = table;
        }

        public IEnumerator<IGetSetter> GetEnumerator()
        {
            foreach (DataRow row in this.OriginalTable.Rows)
                yield return new DataRowGetSetter(row);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
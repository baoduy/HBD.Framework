#region using

using System.Collections.Generic;
using HBD.Framework.Collections;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Data.SqlClient.Base
{
    public class ColumnInfoCollection : DistinctCollection<string, ColumnInfo>
    {
        internal ColumnInfoCollection(TableInfo parentTable) : this(parentTable, null)
        {
        }

        internal ColumnInfoCollection(TableInfo parentTable, IEnumerable<ColumnInfo> collection) : base(c => c.Name)
        {
            ParentTable = parentTable;
            AddRange(collection);
        }

        private TableInfo ParentTable { get; }

        public new void Add(ColumnInfo item)
        {
            Guard.ArgumentIsNotNull(item, nameof(item));
            Guard.ArgumentIsNotNull(item.Name, "ColumnName");

            item.Table = ParentTable;
            base.Add(item);
        }

        public void AddRange(IEnumerable<ColumnInfo> collection)
        {
            if (collection == null) return;
            foreach (var c in collection) Add(c);
        }
    }
}
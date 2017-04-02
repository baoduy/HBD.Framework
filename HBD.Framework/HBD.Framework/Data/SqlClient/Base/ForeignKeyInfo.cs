#region using

using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Data.SqlClient.Base
{
    public class ForeignKeyInfo
    {
        public ForeignKeyInfo(string name, ColumnInfo column, ReferencedColumnInfo referencedColumn)
        {
            Guard.ArgumentIsNotNull(referencedColumn, nameof(referencedColumn));
            Column = column;
            ReferencedColumn = referencedColumn;
            Name = name;
        }

        public string Name { get; }
        public ColumnInfo Column { get; }
        public ReferencedColumnInfo ReferencedColumn { get; }
    }
}
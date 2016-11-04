namespace HBD.Framework.Data.SqlClient.Base
{
    public class ReferencedColumnInfo
    {
        public ReferencedColumnInfo(string referencedTableSchema, string referencedTableName, string referencedColumn)
            : this(new DbName(referencedTableSchema, referencedTableName), referencedColumn)
        {
        }

        public ReferencedColumnInfo(DbName referencedTable, string referencedColumn)
        {
            ReferencedTable = referencedTable;
            ReferencedColumn = referencedColumn;
        }

        public DbName ReferencedTable { get; }
        public string ReferencedColumn { get; }
    }
}
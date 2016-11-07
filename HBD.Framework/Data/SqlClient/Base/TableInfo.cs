using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HBD.Framework.Data.SqlClient.Base
{
    [DebuggerDisplay("Table Name = {Name}, Columns={Columns.Count}")]
    public class TableInfo : IDbInfo
    {
        private int _dependenceIndex = -1;
        private IList<TableInfo> _referenceTables;

        public TableInfo(string tableSchema, string tableName) : this(new DbName(tableSchema, tableName))
        {
        }

        public TableInfo(DbName name)
        {
            Name = name;
            Columns = new ColumnInfoCollection(this);
        }

        public ColumnInfoCollection Columns { get; }
        public long RowCount { get; set; }

        public IList<ForeignKeyInfo> ForeignKeys { get; set; } = new List<ForeignKeyInfo>();

        /// <summary>
        ///     This is the index of table which has dependency to the other table.
        ///     The dependency define by Foreign key. Ex: B depend on A then B will have a foreign key to A.
        ///     When insert, update a foreign key of B that key must existed on A.
        ///     Then the dependenceIndex of B will greater than A.
        /// </summary>
        public int DependenceIndex
        {
            get
            {
                if (_dependenceIndex > -1) return _dependenceIndex;

                var refTables = ReferenceTables;
                if (ForeignKeys.Count == 0 || refTables.Count == 0)
                    _dependenceIndex = 0;
                else _dependenceIndex = refTables.Max(t => t.DependenceIndex) + 1;

                return _dependenceIndex;
            }
        }

        /// <summary>
        ///     Get all reference tables of this table.
        ///     Table A is reference table of B once B has a foreign key to A.
        ///     Self-Reference will be excluded.
        /// </summary>
        public IList<TableInfo> ReferenceTables => _referenceTables ??
                                                   (_referenceTables = (from f in ForeignKeys
                                                                        where f.ReferencedColumn.ReferencedTable != Name
                                                                        select Schema.Tables[f.ReferencedColumn.ReferencedTable]
                                                       ).ToList());

        /// <summary>
        ///     Get all name of dependence tables of this table.
        ///     Table B is dependent on A once B has a foreign key to A.
        ///     Self-Reference will be excluded.
        /// </summary>
        public IList<TableInfo> DependenceTables
            => Schema.Tables.Where(t => t.ReferenceTables.Contains(this)).ToList();

        public DbName Name { get; }
        public SchemaInfo Schema { get; set; }

        public override string ToString() => Name;
    }
}
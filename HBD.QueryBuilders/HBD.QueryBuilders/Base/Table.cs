#region

using System.Collections.Generic;
using HBD.Framework.Data.SqlClient.Base;

#endregion

namespace HBD.QueryBuilders.Base
{
    public class Table : Aliasable
    {
        public Table(string tableName) : this(DbName.Parse(tableName))
        {
        }

        public Table(DbName tableName)
        {
            Name = tableName;
        }

        public DbName Name { get; }
        public IList<Join> Joins { get; } = new List<Join>();
    }
}
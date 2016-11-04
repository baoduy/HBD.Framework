using System.Collections.Generic;
using System.Diagnostics;

namespace HBD.Framework.Data.SqlClient.Base
{
    [DebuggerDisplay("Table Name = {Name}, Columns={Columns.Count}")]
    public class ViewInfo : IDbInfo
    {
        public ViewInfo(string schema, string name) : this(new DbName(schema, name))
        {
        }

        public ViewInfo(DbName name)
        {
            Name = name;
        }

        public IList<string> Columns { get; } = new List<string>();

        public DbName Name { get; }
        public SchemaInfo Schema { get; set; }

        public override string ToString() => Name;
    }
}
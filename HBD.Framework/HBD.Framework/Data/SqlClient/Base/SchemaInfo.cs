#region

using System.Diagnostics;

#endregion

namespace HBD.Framework.Data.SqlClient.Base
{
    [DebuggerDisplay("Schema Name = {Name}, Tables= {Tables.Count}")]
    public class SchemaInfo
    {
        protected internal SchemaInfo(string name)
        {
            Name = name;
            Tables = new TableInfoCollection(this);
            Views = new ViewInfoCollection(this);
        }

        public string Name { get; set; }
        public TableInfoCollection Tables { get; protected internal set; }
        public ViewInfoCollection Views { get; protected internal set; }
    }
}
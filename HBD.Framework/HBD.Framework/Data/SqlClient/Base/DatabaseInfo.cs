#region

using System.Diagnostics;

#endregion

namespace HBD.Framework.Data.SqlClient.Base
{
    [DebuggerDisplay("Schema Name = {Name}")]
    public class DatabaseInfo
    {
        private readonly SqlClientAdapterBase _sqlClient;

        protected internal DatabaseInfo(SqlClientAdapterBase sqlClient, string name)
        {
            _sqlClient = sqlClient;
            Name = name;
        }

        public string Name { get; }

        public SchemaInfo GetSchemaInfo() => _sqlClient?.GetSchemaInfo(Name);
    }
}
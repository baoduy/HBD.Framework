#region using

using System.Collections.Generic;

#endregion

namespace HBD.Framework.Data.SqlClient.Base
{
    public class TableInfoCollection : DbInfoCollection<TableInfo>
    {
        internal TableInfoCollection(SchemaInfo parentSchema) : base(parentSchema, null)
        {
        }

        internal TableInfoCollection(SchemaInfo parentSchema, IEnumerable<TableInfo> collection)
            : base(parentSchema, collection)
        {
        }
    }
}
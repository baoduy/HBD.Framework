using System.Collections.Generic;

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
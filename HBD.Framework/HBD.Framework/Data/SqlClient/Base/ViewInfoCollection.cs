#region

using System.Collections.Generic;

#endregion

namespace HBD.Framework.Data.SqlClient.Base
{
    public class ViewInfoCollection : DbInfoCollection<ViewInfo>
    {
        internal ViewInfoCollection(SchemaInfo parentSchema) : base(parentSchema, null)
        {
        }

        internal ViewInfoCollection(SchemaInfo parentSchema, IEnumerable<ViewInfo> collection)
            : base(parentSchema, collection)
        {
        }
    }
}
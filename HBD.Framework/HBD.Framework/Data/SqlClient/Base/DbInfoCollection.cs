#region

using System.Collections.Generic;
using HBD.Framework.Collections;

#endregion

namespace HBD.Framework.Data.SqlClient.Base
{
    public class DbInfoCollection<TDbInfo> : DistinctCollection<DbName, TDbInfo> where TDbInfo : IDbInfo
    {
        public DbInfoCollection(SchemaInfo parentSchema) : this(parentSchema, null)
        {
        }

        public DbInfoCollection(SchemaInfo parentSchema, IEnumerable<TDbInfo> collection) : base(t => t.Name)
        {
            ParentSchema = parentSchema;
            AddRange(collection);
        }

        private SchemaInfo ParentSchema { get; }

        public new void Add(TDbInfo item)
        {
            item.Schema = ParentSchema;
            base.Add(item);
        }

        public void AddRange(IEnumerable<TDbInfo> collection)
        {
            if (collection == null) return;
            foreach (var c in collection) Add(c);
        }
    }
}
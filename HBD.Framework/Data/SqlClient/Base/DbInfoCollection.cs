using HBD.Framework.Collections;
using System.Collections.Generic;

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

        public new void AddRange(IEnumerable<TDbInfo> collection)
        {
            if (collection == null) return;
            foreach (var c in collection) Add(c);
        }
    }
}
#region

using System.Collections.Generic;
using HBD.QueryBuilders.Context;

#endregion

namespace HBD.QueryBuilders.Base
{
    public abstract class SetQueryBuilder : QueryBuilder
    {
        public SetQueryBuilder(SqlQueryBuilderContext context) : base(context)
        {
        }

        internal IDictionary<string, object> Sets { get; } = new Dictionary<string, object>();
    }
}
#region

using System.Collections.Generic;
using HBD.Data.Comparisons.Base;
using HBD.QueryBuilders.Providers;

#endregion

namespace HBD.QueryBuilders.Base
{
    public abstract class QueryBuilder
    {
        protected QueryBuilder(IQueryBuilderContext context)
        {
            Context = context;
        }

        protected IQueryBuilderContext Context { get; }
        internal ICondition WhereConditions { get; set; }
        internal IList<Table> Tables { get; } = new List<Table>();

        public virtual QueryInfo Build() => Context.Build(this);
    }
}
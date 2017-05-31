#region

using System;
using System.Collections.Generic;
using HBD.Data.Comparisons.Base;
using HBD.QueryBuilders.Context;

#endregion

namespace HBD.QueryBuilders.Base
{
    public class SelectQueryBuilder : QueryBuilder
    {
        public SelectQueryBuilder(SqlQueryBuilderContext context) : base(context)
        {
        }

        internal IList<Field> Fields { get; } = new List<Field>();
        internal uint TopAmount { get; private set; }
        internal bool IsDistinct { get; private set; }
        internal IList<OrderByField> OrderBy { get; } = new List<OrderByField>();
        internal IList<Field> GroupBy { get; } = new List<Field>();

        internal ICondition HavingConditions { get; set; }

        public SelectQueryBuilder Top(uint amount)
        {
            if (amount <= 0) throw new ArgumentException("Top");
            TopAmount = amount;
            return this;
        }

        public SelectQueryBuilder Distinct()
        {
            IsDistinct = true;
            return this;
        }
    }
}
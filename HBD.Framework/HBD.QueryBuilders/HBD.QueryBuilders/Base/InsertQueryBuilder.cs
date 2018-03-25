#region

using HBD.QueryBuilders.Context;

#endregion

namespace HBD.QueryBuilders.Base
{
    public class InsertQueryBuilder : SetQueryBuilder
    {
        public InsertQueryBuilder(SqlQueryBuilderContext context) : base(context)
        {
        }
    }
}
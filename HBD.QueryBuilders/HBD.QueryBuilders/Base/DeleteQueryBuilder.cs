#region

using HBD.QueryBuilders.Context;

#endregion

namespace HBD.QueryBuilders.Base
{
    public class DeleteQueryBuilder : QueryBuilder
    {
        public DeleteQueryBuilder(SqlQueryBuilderContext context) : base(context)
        {
        }
    }
}
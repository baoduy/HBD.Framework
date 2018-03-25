#region

using HBD.QueryBuilders.Context;

#endregion

namespace HBD.QueryBuilders.Base
{
    public class UpdateQueryBuilder : SetQueryBuilder
    {
        public UpdateQueryBuilder(SqlQueryBuilderContext context) : base(context)
        {
        }
    }
}
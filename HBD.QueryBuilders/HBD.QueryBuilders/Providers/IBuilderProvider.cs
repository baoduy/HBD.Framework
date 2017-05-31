#region

using HBD.QueryBuilders.Base;

#endregion

namespace HBD.QueryBuilders.Providers
{
    public interface IBuilderProvider
    {
        QueryInfo Build(QueryBuilder query);
    }
}
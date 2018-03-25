#region

using System.Data;
using HBD.QueryBuilders.Providers;

#endregion

namespace HBD.QueryBuilders.Base
{
    public interface IQueryBuilderContext
    {
        QueryInfo Build(QueryBuilder query);

        DeleteQueryBuilder CreateDeleteQuery();

        InsertQueryBuilder CreateInsertQuery();

        SelectQueryBuilder CreateSelectQuery();

        UpdateQueryBuilder CreateUpdateQuery();

        int ExecuteNonQuery(QueryInfo query);

        int ExecuteNonQuery(QueryBuilder query);

        IDataReader ExecuteReader(QueryInfo query);

        IDataReader ExecuteReader(QueryBuilder query);

        object ExecuteScalar(QueryInfo query);

        object ExecuteScalar(QueryBuilder query);

        DataTable ExecuteTable(QueryInfo query);

        DataTable ExecuteTable(QueryBuilder query);
    }
}
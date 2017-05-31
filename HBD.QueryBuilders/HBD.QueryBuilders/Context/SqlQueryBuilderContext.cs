#region

using System.Data;
using System.Data.Common;
using HBD.Framework.Data.SqlClient;
using HBD.QueryBuilders.Base;
using HBD.QueryBuilders.Providers;

#endregion

namespace HBD.QueryBuilders.Context
{
    public class SqlQueryBuilderContext : SqlClientAdapter, IQueryBuilderContext
    {
        public SqlQueryBuilderContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Provider = CreateBuilderProvider();
        }

        public SqlQueryBuilderContext(DbConnectionStringBuilder connectionString) : base(connectionString)
        {
            Provider = CreateBuilderProvider();
        }

        public SqlQueryBuilderContext(string nameOrConnectionString, IBuilderProvider provider)
            : base(nameOrConnectionString)
        {
            Provider = provider;
        }

        public SqlQueryBuilderContext(DbConnectionStringBuilder connectionString, IBuilderProvider provider)
            : base(connectionString)
        {
            Provider = provider;
        }

        public SqlQueryBuilderContext(IDbConnection connection) : base(connection)
        {
            Provider = CreateBuilderProvider();
        }

        public SqlQueryBuilderContext(IDbConnection connection, IBuilderProvider provider) : base(connection)
        {
            Provider = provider;
        }

        protected IBuilderProvider Provider { get; }

        public virtual QueryInfo Build(QueryBuilder query) => Provider.Build(query);

        protected virtual IBuilderProvider CreateBuilderProvider() => new SqlBuilderProvider();

        #region QueryInfo Executes

        public virtual int ExecuteNonQuery(QueryInfo query) => ExecuteNonQuery(query.Query, query.Parameters);

        public virtual object ExecuteScalar(QueryInfo query) => ExecuteScalar(query.Query, query.Parameters);

        public virtual IDataReader ExecuteReader(QueryInfo query) => ExecuteReader(query.Query, query.Parameters);

        public virtual DataTable ExecuteTable(QueryInfo query) => ExecuteTable(query.Query, query.Parameters);

        #endregion QueryInfo Executes

        #region QueryInfo Executes

        public virtual int ExecuteNonQuery(QueryBuilder query) => ExecuteNonQuery(Build(query));

        public virtual object ExecuteScalar(QueryBuilder query) => ExecuteScalar(Build(query));

        public virtual IDataReader ExecuteReader(QueryBuilder query) => ExecuteReader(Build(query));

        public virtual DataTable ExecuteTable(QueryBuilder query) => ExecuteTable(Build(query));

        #endregion QueryInfo Executes

        #region Query Builders

        public virtual SelectQueryBuilder CreateSelectQuery() => new SelectQueryBuilder(this);

        public virtual InsertQueryBuilder CreateInsertQuery() => new InsertQueryBuilder(this);

        public virtual UpdateQueryBuilder CreateUpdateQuery() => new UpdateQueryBuilder(this);

        public virtual DeleteQueryBuilder CreateDeleteQuery() => new DeleteQueryBuilder(this);

        #endregion Query Builders
    }
}
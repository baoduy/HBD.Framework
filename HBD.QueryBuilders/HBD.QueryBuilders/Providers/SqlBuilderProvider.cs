#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBD.QueryBuilders.Base;
using HBDCommon = HBD.Framework.Data.Common;

#endregion

namespace HBD.QueryBuilders.Providers
{
    public class SqlBuilderProvider : IBuilderProvider
    {
        private static readonly SqlConditionRender SqlRender = new SqlConditionRender();

        public QueryInfo Build(QueryBuilder query)
        {
            var parameters = new Dictionary<string, object>();
            string result = null;

            var select = query as SelectQueryBuilder;
            if (select != null)
                result = BuildSelect(parameters, select);

            var insert = query as InsertQueryBuilder;
            if (insert != null)
                result = BuildInsert(parameters, insert);

            var update = query as UpdateQueryBuilder;
            if (update != null)
                result = BuildUpdate(parameters, update);

            var delete = query as DeleteQueryBuilder;
            if (delete != null)
                result = BuildDelete(parameters, delete);

            return new QueryInfo(result, parameters);
        }

        private string BuildInsert(IDictionary<string, object> parameters, InsertQueryBuilder query)
        {
            if (query == null) return null;
            if (query.Tables.Count == 0) return null;
            if (query.Sets.Count == 0) return null;

            var builder = new StringBuilder();
            var result = BuildInsertSet(parameters, query.Sets);

            builder.Append("INSERT INTO ")
                .Append(Environment.NewLine)
                .Append(BuildTables(parameters, query.Tables))
                .Append($"({string.Join(",", result.Select(n => HBDCommon.GetSqlName(n.Key)))})")
                .Append(Environment.NewLine)
                .Append("VALUES (")
                .Append($"{string.Join(",", result.Select(v => v.Value))}")
                .Append(")");

            return builder.ToString();
        }

        private string BuildSelect(IDictionary<string, object> parameters, SelectQueryBuilder query)
        {
            if (query == null) return null;
            if (query.Tables.Count == 0) return null;

            var builder = new StringBuilder();

            builder.Append("SELECT ")
                .Append(query.IsDistinct ? "DISTINCT " : string.Empty)
                .Append(query.TopAmount > 0 ? $"TOP {query.TopAmount} " : string.Empty)
                .Append(Environment.NewLine).Append(BuildFields(query.Fields))
                .Append(Environment.NewLine).Append("FROM ")
                .Append(BuildTables(parameters, query.Tables));

            var where = SqlRender.BuildCondition(query.WhereConditions, parameters);
            if (!string.IsNullOrWhiteSpace(where))
                builder.Append(Environment.NewLine).Append($"WHERE ({where})");

            if (query.GroupBy.Count > 0)
                builder.Append(Environment.NewLine).Append($"GROUP BY {BuildFields(query.GroupBy)}");

            var having = SqlRender.BuildCondition(query.HavingConditions, parameters);
            if (!string.IsNullOrWhiteSpace(having))
                builder.Append(Environment.NewLine).Append($"HAVING ({having})");

            if (query.OrderBy.Count > 0)
                builder.Append(Environment.NewLine).Append($"ORDER BY {BuildOrderBy(query.OrderBy)}");

            return builder.ToString();
        }

        private string BuildUpdate(IDictionary<string, object> parameters, UpdateQueryBuilder query)
        {
            if (query == null) return null;
            if (query.Tables.Count == 0) return null;
            if (query.Sets.Count == 0) return null;

            var builder = new StringBuilder();

            builder.Append("UPDATE ")
                .Append(BuildTables(parameters, query.Tables))
                .Append(Environment.NewLine).Append("SET ")
                .Append(string.Join("," + Environment.NewLine, query.Sets.Select(s => BuildUpdateSet(parameters, s))));

            var where = SqlRender.BuildCondition(query.WhereConditions, parameters);
            if (!string.IsNullOrWhiteSpace(where))
                builder.Append(Environment.NewLine).Append($"WHERE ({where})");

            return builder.ToString();
        }

        private string BuildDelete(IDictionary<string, object> parameters, DeleteQueryBuilder query)
        {
            if (query == null) return null;
            if (query.Tables.Count == 0) return null;

            var builder = new StringBuilder();

            builder.Append("DELETE FROM ")
                .Append(BuildTables(parameters, query.Tables));

            var where = SqlRender.BuildCondition(query.WhereConditions, parameters);
            if (!string.IsNullOrWhiteSpace(where))
                builder.Append(Environment.NewLine).Append($"WHERE ({where})");

            return builder.ToString();
        }

        private static object BuildUpdateSet(IDictionary<string, object> parameters, KeyValuePair<string, object> set)
        {
            var builder = new StringBuilder();

            var paraName = SqlRender.GetParamertName(parameters, set.Key);
            builder.Append(HBDCommon.GetSqlName(set.Key)).Append(" = ").Append(paraName);
            parameters.Add(paraName, set.Value);

            return builder.ToString();
        }

        private IDictionary<string, string> BuildInsertSet(IDictionary<string, object> parameters,
            IDictionary<string, object> sets)
        {
            var builder = new Dictionary<string, string>();

            foreach (var k in sets)
            {
                var paraName = SqlRender.GetParamertName(parameters, k.Key);
                builder.Add(k.Key, paraName);
                parameters.Add(paraName, k.Value);
            }

            return builder;
        }

        private static string BuildOrderBy(IList<OrderByField> orderBy)
            =>
            string.Join(",",
                orderBy.Select(
                    f =>
                            HBDCommon.GetSqlName(f.Name) + (f.OrderType == OrderType.Ascending ? string.Empty : " DESC")));

        private string BuildTables(IDictionary<string, object> parameters, IList<Table> tables)
            => string.Join(",", tables.Select(t => BuildTable(parameters, t)));

        private string BuildTable(IDictionary<string, object> parameters, Table table)
        {
            var builder = new StringBuilder();

            var query = table as SubQuery;
            builder.Append(query != null ? $"({Build(query.Query)})" : table.Name.FullName);

            builder.Append(BuildAlias(table));

            if (table.Joins.Count > 0)
            {
                builder.Append(Environment.NewLine);
                builder.Append(string.Join(Environment.NewLine, table.Joins.Select(j => BuildJoin(parameters, j))));
            }

            return builder.ToString();
        }

        private static string BuildJoin(IDictionary<string, object> parameters, Join join)
        {
            var builder = new StringBuilder();
            switch (join.JoinOperator)
            {
                case JoinOperation.FullOuterJoin:
                    builder.Append("FULL OUTER JOIN ");
                    break;

                case JoinOperation.InnerJoin:
                    builder.Append("INNER JOIN ");
                    break;

                case JoinOperation.LeftJoin:
                    builder.Append("LEFT JOIN ");
                    break;

                case JoinOperation.RightJoin:
                    builder.Append("RIGHT JOIN ");
                    break;
            }

            builder.Append(HBDCommon.GetSqlName(join.JoinTable))
                .Append(BuildAlias(join))
                .Append(" ON (")
                .Append(SqlRender.BuildCondition(join.Condition, parameters))
                .Append(")");

            return builder.ToString();
        }

        private static string BuildSqlValueList(object conditionValue)
        {
            var builder = new StringBuilder();
            if (conditionValue == null) return builder.ToString();
            var objs = (object[]) conditionValue;
            if (objs.Length == 0) return builder.ToString();

            foreach (var obj in objs)
            {
                if (builder.Length > 0) builder.Append(",");
                builder.AppendFormat(obj is string ? "N'{0}'" : "{0}", obj);
            }

            return builder.ToString();
        }

        private static string BuildFields(IList<Field> fields)
        {
            if (fields == null || fields.Count == 0)
                return "*";
            return string.Join(",", fields.Select(BuildField));
        }

        private static string BuildField(Field field)
        {
            if (field is FunctionField)
                return BuildFunctionField((FunctionField) field);
            return HBDCommon.GetSqlName(field.Name) + BuildAlias(field);
        }

        private static string BuildFunctionField(FunctionField field)
        {
            var builder = new StringBuilder();
            if (field is CustomFunction)
            {
                var c = (CustomFunction) field;
                builder.Append(c.FunctionName.ToUpper())
                    .Append($"({BuildSqlValueList(c.Parameters)}) ");
                return builder.ToString();
            }

            if (field is AverageField)
                builder.Append("AVG");
            if (field is CountField)
                builder.Append("COUNT");
            if (field is LeftField)
                builder.Append("LEFT");
            if (field is RightField)
                builder.Append("RIGHT");
            if (field is MaxField)
                builder.Append("MAX");
            if (field is MinField)
                builder.Append("MIN");
            if (field is SumField)
                builder.Append("SUM");

            builder.Append("(")
                .Append(field.Type == FunctionType.All ? string.Empty : "DISTINCT ")
                .Append(HBDCommon.GetSqlName(field.Name))
                .Append(") ")
                .Append(BuildAlias(field));

            return builder.ToString();
        }

        private static string BuildAlias(Aliasable alias)
            => string.IsNullOrWhiteSpace(alias.Alias) ? string.Empty : HBDCommon.GetSqlName(alias.Alias);
    }
}
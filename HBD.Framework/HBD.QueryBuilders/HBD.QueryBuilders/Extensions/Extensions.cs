#region

using System;
using System.Collections.Generic;
using System.Linq;
using HBD.Data.Comparisons;
using HBD.Data.Comparisons.Base;
using HBD.QueryBuilders.Base;

#endregion

namespace HBD.QueryBuilders
{
    public static class Extensions
    {
        public static void AddRange<T>(this IList<T> @this, IEnumerable<T> items)
        {
            foreach (var item in items)
                @this.Add(item);
        }

        public static TQuery Where<TQuery>(this TQuery @this, Func<FieldBuilder, ICondition> conditions)
            where TQuery : QueryBuilder
        {
            var con = conditions.Invoke(FieldBuilder.Current);
            return @this.Where(con);
        }

        public static TQuery Where<TQuery>(this TQuery @this, ICondition conditions) where TQuery : QueryBuilder
        {
            if (conditions != null)
                @this.WhereConditions = @this.WhereConditions == null
                    ? conditions
                    : @this.WhereConditions.And(conditions);
            return @this;
        }

        #region Delete

        public static DeleteQueryBuilder From(this DeleteQueryBuilder @this, string tableName)
        {
            @this.Tables.Clear();
            @this.Tables.Add(new Table(tableName));
            return @this;
        }

        #endregion Delete

        #region Select Query Builder

        public static SelectQueryBuilder Fields(this SelectQueryBuilder @this, params string[] fieldNames)
            => @this.Fields(fieldNames.Select(s => FieldBuilder.Current.Field(s)).ToArray());

        public static SelectQueryBuilder Fields(this SelectQueryBuilder @this, params Field[] fieldNames)
        {
            foreach (var fieldName in fieldNames)
                @this.Fields.Add(fieldName);
            return @this;
        }

        public static SelectQueryBuilder Fields(this SelectQueryBuilder @this, Func<FieldBuilder, Field> field)
            => @this.Fields(field.Invoke(FieldBuilder.Current));

        public static SelectQueryBuilder Fields(this SelectQueryBuilder @this, Func<FieldBuilder, Field[]> fields)
        {
            var fs = fields.Invoke(FieldBuilder.Current);
            foreach (var f in fs)
                @this.Fields.Add(f);
            return @this;
        }

        public static SelectQueryBuilder From(this SelectQueryBuilder @this, Func<TableBuilder, Table> table)
        {
            @this.Tables.Add(table.Invoke(TableBuilder.Current));
            return @this;
        }

        public static SelectQueryBuilder From(this SelectQueryBuilder @this, params string[] tableNames)
        {
            @this.Tables.AddRange(tableNames.Select(TableBuilder.Current.Table));
            return @this;
        }

        //public static SelectQueryBuilder From(this SelectQueryBuilder @this, Func<SubQueryBuilder, SubQuery> subQuery)
        //=> @this.From(subQuery.Invoke(SubQueryBuilder.Current));

        //public static SelectQueryBuilder From(this SelectQueryBuilder @this, SubQuery subQuery)
        //{
        //    @this.Tables.Add(subQuery);
        //    return @this;
        //}

        public static SubQuery From(this SelectQueryBuilder @this, SelectQueryBuilder subQuery)
        {
            var sub = SubQueryBuilder.Current.SubQuery(subQuery);
            @this.Tables.Add(sub);
            return sub;
        }

        public static SelectQueryBuilder OrderBy(this SelectQueryBuilder @this, params string[] fieldNames)
        {
            @this.OrderBy.AddRange(fieldNames.Select(f => new OrderByField(f, OrderType.Ascending)));
            return @this;
        }

        public static SelectQueryBuilder OrderByDescending(this SelectQueryBuilder @this, params string[] fieldNames)
        {
            @this.OrderBy.AddRange(fieldNames.Select(f => new OrderByField(f, OrderType.Descending)));
            return @this;
        }

        public static SelectQueryBuilder GroupBy(this SelectQueryBuilder @this, params string[] fieldNames)
        {
            @this.GroupBy.AddRange(fieldNames.Select(FieldBuilder.Current.Field));
            return @this;
        }

        public static SelectQueryBuilder Having(this SelectQueryBuilder @this, Func<FieldBuilder, ICondition> conditions)
        {
            if (@this.GroupBy.Count <= 0)
                throw new InvalidOperationException("GroupBy cannot be empty when apply Having condition.");
            var con = conditions.Invoke(FieldBuilder.Current);
            @this.HavingConditions = @this.HavingConditions == null ? con : @this.HavingConditions.And(con);
            return @this;
        }

        #endregion Select Query Builder

        #region Update

        public static UpdateQueryBuilder Table(this UpdateQueryBuilder @this, string tableName)
        {
            @this.Tables.Clear();
            @this.Tables.Add(new Table(tableName));
            return @this;
        }

        //public static UpdateQueryBuilder Set(this UpdateQueryBuilder @this, Func<FieldBuilder, FieldSet> field)
        //{
        //    var f = field.Invoke(FieldBuilder.Current);
        //    return @this.Set(f);
        //}

        //public static UpdateQueryBuilder Set(this UpdateQueryBuilder @this, Func<FieldBuilder, FieldSet[]> fields)
        //{
        //    var fs = fields.Invoke(FieldBuilder.Current);
        //    return @this.Set(fs);
        //}

        public static FieldSet<UpdateQueryBuilder> Set(this UpdateQueryBuilder @this, string fieldName)
            => new FieldSet<UpdateQueryBuilder>(@this, fieldName);

        #endregion Update

        #region Insert

        public static InsertQueryBuilder Into(this InsertQueryBuilder @this, string tableName)
        {
            @this.Tables.Clear();
            @this.Tables.Add(new Table(tableName));
            return @this;
        }

        //public static InsertQueryBuilder Values(this InsertQueryBuilder @this, Func<FieldBuilder, FieldSet> field)
        //{
        //    var f = field.Invoke(FieldBuilder.Current);
        //    return @this.Values(f);
        //}

        //public static InsertQueryBuilder Values(this InsertQueryBuilder @this, Func<FieldBuilder, FieldSet[]> fields)
        //{
        //    var fs = fields.Invoke(FieldBuilder.Current);
        //    return @this.Values(fs);
        //}

        public static FieldSet<InsertQueryBuilder> Values(this InsertQueryBuilder @this, string fieldName)
            => new FieldSet<InsertQueryBuilder>(@this, fieldName);

        #endregion Insert
    }
}
#region

using System;
using HBD.Data.Comparisons.Base;
using HBD.QueryBuilders.Base;

#endregion

namespace HBD.QueryBuilders
{
    public static class JoinExtensions
    {
        public static Table On(this Join @this, Func<FieldBuilder, ICondition> conditions)
        {
            if (@this == null || conditions == null) return null;
            var con = conditions.Invoke(FieldBuilder.Current);
            return @this.On(con);
        }

        public static Table On(this Join @this, ICondition condition)
        {
            if (@this == null || condition == null) return null;
            @this.Condition = condition;
            return @this.ParentTable;
        }

        #region Joins

        public static Join LeftJoin(this Table @this, Func<TableBuilder, Table> table)
        {
            if (@this == null || table == null) return null;
            var tb = table.Invoke(TableBuilder.Current);
            return @this.LeftJoin(tb);
        }

        public static Join LeftJoin(this Table @this, string tableName)
            => @this.LeftJoin(TableBuilder.Current.Table(tableName));

        public static Join LeftJoin(this Table @this, Table table)
        {
            if (@this == null || table == null) return null;
            var join = new Join(@this, table.Name, JoinOperation.LeftJoin).As(table.Alias);
            @this.Joins.Add(join);
            return join;
        }

        public static Join RightJoin(this Table @this, Func<TableBuilder, Table> table)
        {
            if (@this == null || table == null) return null;
            var tb = table.Invoke(TableBuilder.Current);
            return @this.RightJoin(tb);
        }

        public static Join RightJoin(this Table @this, string tableName)
            => @this.RightJoin(TableBuilder.Current.Table(tableName));

        public static Join RightJoin(this Table @this, Table table)
        {
            if (@this == null || table == null) return null;
            var join = new Join(@this, table.Name, JoinOperation.RightJoin).As(table.Alias);
            @this.Joins.Add(join);
            return join;
        }

        public static Join FullOuterJoin(this Table @this, Func<TableBuilder, Table> table)
        {
            if (@this == null || table == null) return null;
            var tb = table.Invoke(TableBuilder.Current);
            return @this.FullOuterJoin(tb);
        }

        public static Join FullOuterJoin(this Table @this, string tableName)
            => @this.FullOuterJoin(TableBuilder.Current.Table(tableName));

        public static Join FullOuterJoin(this Table @this, Table table)
        {
            if (@this == null || table == null) return null;
            var join = new Join(@this, table.Name, JoinOperation.FullOuterJoin).As(table.Alias);
            @this.Joins.Add(join);
            return join;
        }

        public static Join InnerJoin(this Table @this, Func<TableBuilder, Table> table)
        {
            if (@this == null || table == null) return null;
            var tb = table.Invoke(TableBuilder.Current);
            return @this.InnerJoin(tb);
        }

        public static Join InnerJoin(this Table @this, string tableName)
            => @this.InnerJoin(TableBuilder.Current.Table(tableName));

        public static Join InnerJoin(this Table @this, Table table)
        {
            if (@this == null || table == null) return null;
            var join = new Join(@this, table.Name, JoinOperation.InnerJoin).As(table.Alias);
            @this.Joins.Add(join);
            return join;
        }

        #endregion Joins
    }
}
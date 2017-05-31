#region

using HBD.Data.Comparisons.Base;

#endregion

namespace HBD.QueryBuilders.Base
{
    public class Join : Aliasable
    {
        internal Join(Table parentTable, string tableName, JoinOperation joinOperator)
        {
            ParentTable = parentTable;
            JoinOperator = joinOperator;
            JoinTable = tableName;
        }

        internal Join(Table parentTable, SelectQueryBuilder joinQuery, JoinOperation joinOperator)
        {
            ParentTable = parentTable;
            JoinOperator = joinOperator;
            this.joinQuery = joinQuery;
        }

        internal Table ParentTable { get; }
        internal string JoinTable { get; }
        internal ICondition Condition { get; set; }
        internal JoinOperation JoinOperator { get; }
        internal SelectQueryBuilder joinQuery { get; }
    }

    public enum JoinOperation
    {
        LeftJoin,
        RightJoin,
        InnerJoin,
        FullOuterJoin
    }
}
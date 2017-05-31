namespace HBD.QueryBuilders.Base
{
    public class SubQuery : Table
    {
        internal SubQuery(QueryBuilder subquery) : base("SubQuery")
        {
            Query = subquery;
        }

        internal QueryBuilder Query { get; private set; }
    }
}
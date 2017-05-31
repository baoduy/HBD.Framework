namespace HBD.QueryBuilders
{
    public static class SqlContextExtension
    {
        //public static int Execute(this SqlBuilderContext @this, QueryBuilder query)
        //{
        //    if (@this == null || query == null) return -1;
        //    var result = @this.Build(query);
        //    return string.IsNullOrWhiteSpace(result?.Query) ? -1 : @this.Execute(result);
        //}

        //public static IDataReader ExecuteReader(this SqlBuilderContext @this, QueryBuilder query)
        //{
        //    if (@this == null || query == null) return null;
        //    var result = @this.Build(query);
        //    return string.IsNullOrWhiteSpace(result?.Query) ? null : @this.ExecuteReader(result);
        //}

        //public static DataTable ExecuteTable(this SqlBuilderContext @this, QueryBuilder query)
        //{
        //    if (@this == null || query == null) return null;
        //    var result = @this.Build(query);
        //    return string.IsNullOrWhiteSpace(result?.Query) ? null : @this.ExecuteTable(result);
        //}
    }
}
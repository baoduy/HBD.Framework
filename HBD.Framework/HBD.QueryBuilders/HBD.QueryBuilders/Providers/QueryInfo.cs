#region

using System.Collections.Generic;
using HBD.QueryBuilders.Base;

#endregion

namespace HBD.QueryBuilders.Providers
{
    public class QueryInfo
    {
        public QueryInfo(string query, IDictionary<string, object> parameters)
        {
            Query = query;
            Parameters = parameters;
        }

        public string Query { get; }
        public IDictionary<string, object> Parameters { get; }

        public IList<string> Validate() => SqlSyntaxValidation.Parse(Query);
    }

    //public enum QueryResultType
    //{ Unknown, Select, Insert, Update, Delete }
}
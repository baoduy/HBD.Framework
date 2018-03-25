#region

using System.Collections.Generic;
using HBD.Data.Comparisons;

#endregion

namespace HBD.QueryBuilders.Providers
{
    internal class SqlConditionRender : DataTableExpressionRender
    {
        protected override string ContainsPattern => "%{0}%";
        protected override string StartsWithPattern => "{0}%";
        protected override string EndsWithPattern => "%{0}";
        protected override string StringPattern => "{0}";

        //Republic the GetParamertName method from DataTableExpressionRender
        public new string GetParamertName(IDictionary<string, object> parameters, string name)
            => base.GetParamertName(parameters, name);
    }
}
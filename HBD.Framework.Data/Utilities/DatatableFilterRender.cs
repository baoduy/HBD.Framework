using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBD.Framework.Core;

namespace HBD.Framework.Data.Utilities
{
    public class DatatableFilterRender : FilterRenderBase
    {
        protected virtual string GetFilterValue(object value)
        {
            Guard.MustBeValueType(value, "Value");
            if (value == null)
                return "IS NULL";
            if (value is string || value is DateTime)
                return string.Format("'{0}'", value);
            var val = value.ToString();
            
            if (val.Contains("'"))
                throw new Exception(string.Format("The filter value must not contains (') characters: {0}", val));

            return val;
        }

        protected virtual string GetFilterValue(IEnumerable<object> collection)
        {
            var build = new StringBuilder();

            foreach (var obj in collection)
            {
                if (obj == null) continue;

                if (build.Length > 0)
                    build.Append(",");
                build.Append(GetFilterValue(obj));
            }

            return build.ToString();
        }

        protected override string RenderFilter(FilterClause filter)
        {
            Guard.ArgumentNotNull(filter, "FilterItem");

            if (filter.Value == null)
                return string.Format("[{0}] {1}", filter.FieldName, GetFilterValue(filter.Value));

            switch (filter.Operation)
            {
                case CompareOperation.GreaterThan:
                    return string.Format("[{0}] > {1}", filter.FieldName, GetFilterValue(filter.Value));
                case CompareOperation.LessThan:
                    return string.Format("[{0}] < {1}", filter.FieldName, GetFilterValue(filter.Value));
                case CompareOperation.GreaterThanOrEquals:
                    return string.Format("{0} >= {1}", filter.FieldName, GetFilterValue(filter.Value));
                case CompareOperation.LessThanOrEquals:
                    return string.Format("[{0}] <= {1}", filter.FieldName, GetFilterValue(filter.Value));
                case CompareOperation.Contains:
                    return string.Format("[{0}] LIKE '%{1}%'", filter.FieldName, filter.Value);
                case CompareOperation.NotContains:
                    return string.Format("[{0}] NOT LIKE '%{1}%'", filter.FieldName, filter.Value);
                case CompareOperation.StartsWith:
                    return string.Format("[{0}] LIKE '{1}%'", filter.FieldName, filter.Value);
                case CompareOperation.EndsWith:
                    return string.Format("[{0}] LIKE '%{1}'", filter.FieldName, filter.Value);
                case CompareOperation.In:
                    return string.Format("[{0}] IN ({1})", filter.FieldName, GetFilterValue(filter.Value as IEnumerable<object>));
                case CompareOperation.NotIn:
                    return string.Format("[{0}] NOT IN ({1})", filter.FieldName, GetFilterValue(filter.Value as IEnumerable<object>));
                case CompareOperation.IsNull:
                    return string.Format("[{0}] IS NULL", filter.FieldName);
                case CompareOperation.NotNull:
                    return string.Format("[{0}] IS NOT NULL", filter.FieldName);
                case CompareOperation.NotEquals:
                    return string.Format("[{0}] <> {1}", filter.FieldName, GetFilterValue(filter.Value));
                case CompareOperation.Equals:
                default:
                    return string.Format("[{0}] = {1}", filter.FieldName, GetFilterValue(filter.Value));
            }
        }

        protected override string RenderFilter(BinaryFilterItem filter)
        {
            return string.Format("({0}) {1} ({2})", this.RenderFilter(filter.LeftClause), filter.Operation, this.RenderFilter(filter.RightClause));
        }
    }
}

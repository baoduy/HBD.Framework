using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBD.Framework.Data.Utilities;
using HBD.Framework.Core;

namespace HBD.Framework.Data.Sharepoint
{
    public class SPCamlQueryRender : FilterRenderBase
    {
        protected const string ViewFormat = "<View>{0}</View>";
        protected const string ViewFieldsFormat = "<ViewFields>{0}</ViewFields>";
        protected const string QueryFormat = "<Query>{0}</Query>";
        protected const string WhereFormat = "<Where>{0}</Where>";
        protected const string AndFormat = "<And>{0}</And>";
        protected const string OrFormat = "<Or>{0}</Or>";

        protected const string FieldFormat = "<FieldRef Name='{0}' />";
        protected const string ValueFormat = "<Value Type='{0}'>{1}</Value>";
        protected const string ValueArrayFormat = "<Values>{0}</Values>";

        protected const string EqualsFormat = "<Eq>{0}</Eq>";
        protected const string NotEqualsFormat = "<Neq>{0}</Neq>";
        protected const string GreaterThanFormat = "<Gt>{0}</Gt>";
        protected const string GreaterThanOrEqualsFormat = "<Geq>{0}</Geq>";
        protected const string LessThanFormat = "<Lt>{0}</Lt>";
        protected const string LessThanOrEqualsFormat = "<Leq>{0}</Leq>";
        protected const string IsNullFormat = "<IsNull>{0}</IsNull>";
        protected const string IsNotNullFormat = "<IsNotNull>{0}</IsNotNull>";
        protected const string StartsWithFormat = "<BeginsWith>{0}</BeginsWith>";
        protected const string ContainsFormat = "<Contains>{0}</Contains>";
        protected const string InFormat = "<In>{0}</In>";

        protected const string DateRangesOverlapFormat = "<DateRangesOverlap>{0}</DateRangesOverlap>";

        private string GetSPValueType(object value)
        {
            if (value == null)
                return string.Empty;
            if (value is int)
                return "Integer";
            if (value is string)
            {
                if (value.ToString().ToLower().StartsWith("http"))
                    return "URL";
                return "Text";
            }
            if (value is DateTime)
                return "DateTime";
            if (value is bool)
                return "Boolean";
            if (value is decimal || value is double || value is float || value is long)
                return "Number";
            if (value is Guid)
                return "Guid";
            return string.Empty;

            //Invalid
            //Integer
            //Text
            //Note
            //DateTime
            //Counter
            //Choice
            //Lookup
            //Boolean
            //Number
            //Currency
            //URL
            //Computed
            //Threading
            //Guid
            //MultiChoice
            //GridChoice
            //Calculated
            //File
            //Attachments
            //User
            //Recurrence
            //CrossProjectLink
            //ModStat
            //Error
            //ContentTypeId
            //PageSeparator
            //ThreadIndex
            //WorkflowStatus
            //AllDayEvent
            //WorkflowEventType
            //Geolocation
            //OutcomeChoice
            //MaxItems
        }

        private string GetSPValue(object value)
        {
            Guard.ArgumentNotNull(value, "Value");

            if (value is IEnumerable<object>)
            {
                var build = new StringBuilder();
                foreach (var b in value as IEnumerable<object>)
                {
                    build.AppendFormat(ValueFormat, GetSPValueType(b), b);
                }
                return string.Format(ValueArrayFormat, build.ToString());
            }
            return string.Format(ValueFormat, this.GetSPValueType(value), value);
        }

        protected override string RenderFilter(FilterClause filter)
        {
            Guard.ArgumentNotNull(filter, "FilterClause");
            Guard.ArgumentNotNull(filter.FieldName, "FilterClause.FiledName");

            var field = string.Format(FieldFormat, filter.FieldName);
            if (filter.Value == null)
                return string.Format(IsNullFormat, field);
            var value = this.GetSPValue(filter.Value);

            switch (filter.Operation)
            {
                case CompareOperation.Equals:
                    return string.Format(EqualsFormat, field + value);
                case CompareOperation.NotContains:
                case CompareOperation.NotEquals:
                    return string.Format(NotEqualsFormat, field + value);
                case CompareOperation.GreaterThan:
                    return string.Format(GreaterThanFormat, field + value);
                case CompareOperation.LessThan:
                    return string.Format(LessThanFormat, field + value);
                case CompareOperation.GreaterThanOrEquals:
                    return string.Format(GreaterThanOrEqualsFormat, field + value);
                case CompareOperation.LessThanOrEquals:
                    return string.Format(LessThanOrEqualsFormat, field + value);
                case CompareOperation.StartsWith:
                    return string.Format(StartsWithFormat, field + value);
                case CompareOperation.IsNull:
                    return string.Format(IsNullFormat, field);
                case CompareOperation.NotNull:
                    return string.Format(IsNotNullFormat, field);
                case CompareOperation.In:
                    return string.Format(InFormat, field + value);
                case CompareOperation.NotIn:
                    {
                        if (filter.Value is IEnumerable<object>)
                        {
                            var build = new StringBuilder();
                            foreach (var b in filter.Value as IEnumerable<object>)
                            {
                                var val = GetSPValue(b);
                                build.AppendFormat(NotEqualsFormat, field + val);
                            }
                            return string.Format(AndFormat, build.ToString());
                        }
                        return string.Format(NotEqualsFormat, field + value);
                    }
                case CompareOperation.EndsWith:
                case CompareOperation.Contains:
                default:
                    return string.Format(ContainsFormat, field + value);
            }
        }

        protected override string RenderFilter(BinaryFilterItem filter)
        {
            Guard.ArgumentNotNull(filter, "BinaryFilterItem");

            switch (filter.Operation)
            {
                case BinaryOperation.AND:
                    return string.Format(AndFormat, RenderFilter(filter.LeftClause) + RenderFilter(filter.RightClause));
                case BinaryOperation.OR:
                    return string.Format(OrFormat, RenderFilter(filter.LeftClause) + RenderFilter(filter.RightClause));
            }
            return string.Empty;
        }

        public virtual string RenderWhereClause(IFilterClause filter)
        {
            return string.Format(WhereFormat, this.RenderFilter(filter));
        }

        public virtual string RenderQueryClause(IFilterClause filter)
        {
            return string.Format(QueryFormat, this.RenderWhereClause(filter));
        }

        public virtual string RenderFields(params string[] fields)
        {
            var build = new StringBuilder();

            //Alway add ID to the selection fields
            if (!fields.Contains(DefaultSPValues.InternalFieldName.ID))
                build.AppendFormat(FieldFormat, DefaultSPValues.InternalFieldName.ID);

            foreach (var f in fields)
                build.AppendFormat(FieldFormat, f);

            return build.ToString();
        }

        public virtual string RenderViewFields(params string[] fields)
        {
            return string.Format(ViewFieldsFormat, this.RenderFields(fields));
        }

        public virtual string RenderViewXml(IFilterClause filter, params string[] fields)
        {
            var filterString = string.Empty;
            var fieldsString = string.Empty;

            if (filter != null)
                filterString = this.RenderQueryClause(filter);

            if (fields != null && fields.Length > 0)
                fieldsString = this.RenderViewFields(fields);

            return string.Format(ViewFormat, fieldsString + filterString);
        }
    }
}

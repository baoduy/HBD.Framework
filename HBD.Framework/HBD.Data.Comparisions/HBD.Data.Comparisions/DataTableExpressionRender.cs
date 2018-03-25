#region

using HBD.Data.Comparisons.Base;
using HBD.Framework;
using HBD.Framework.Data;
using System.Collections.Generic;
using System.Text;

#endregion

namespace HBD.Data.Comparisons
{
    public class DataTableExpressionRender : ConditionRender
    {
        protected virtual string ContainsPattern => "'%{0}%'";
        protected virtual string StartsWithPattern => "'{0}%'";
        protected virtual string EndsWithPattern => "'%{0}'";
        protected virtual string StringPattern => "'{0}'";

        /// <summary>
        ///     Check whether the value
        /// </summary>
        public bool CheckMethodString { get; set; } = true;

        protected virtual string GetParamertName(IDictionary<string, object> parameterCollection, string name)
        {
            name = "@" + name.Replace(".", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
            return parameterCollection.ContainsKey(name) ? name + parameterCollection.Count : name;
        }

        private void AddToBuilder(StringBuilder builder, IDictionary<string, object> parameterCollection,
            string fieldName, object value)
        {
            if (parameterCollection.IsNotNull())
            {
                var pname = GetParamertName(parameterCollection, fieldName);
                parameterCollection.Add(pname, value);
                builder.Append(pname);
            }
            else
            {
                if (value.IsNull())
                    builder.Append("NULL");
                else if (value is string)
                    if (value.ToString().Contains("'"))
                        builder.Append(value);
                    else builder.AppendFormat(StringPattern, value);
                else builder.Append(value);
            }
        }

        private string GetFilterPattern(CompareOperation operation)
        {
            switch (operation)
            {
                case CompareOperation.Contains:
                case CompareOperation.NotContains:
                    return ContainsPattern;

                case CompareOperation.StartsWith:
                    return StartsWithPattern;

                case CompareOperation.EndsWith:
                    return EndsWithPattern;

                default:
                    return string.Empty;
            }
        }

        protected override string BuildFieldCondition(FieldConditionBase fieldCondition,
                IDictionary<string, object> outPutParameters = null)
            =>
            fieldCondition is FieldCondition
                ? BuildFieldCondition((FieldCondition)fieldCondition)
                : BuildValueCondition((ValueCondition)fieldCondition, outPutParameters);

        protected virtual string BuildValueCondition(ValueCondition fieldCondition,
            IDictionary<string, object> outPutParameters = null)
        {
            var builder = new StringBuilder()
                .Append(Common.GetSqlName(fieldCondition.Field))
                .Append(GetOperator(fieldCondition.Operation));

            switch (fieldCondition.Operation)
            {
                case CompareOperation.Contains:
                case CompareOperation.NotContains:
                case CompareOperation.StartsWith:
                case CompareOperation.EndsWith:
                    {
                        var value = string.Format(GetFilterPattern(fieldCondition.Operation), fieldCondition.Value);
                        AddToBuilder(builder, outPutParameters, fieldCondition.Field, value);
                    }
                    break;

                case CompareOperation.In:
                case CompareOperation.NotIn:
                    {
                        builder.Append("(");

                        var values = fieldCondition.Value as object[] ?? new[] { fieldCondition.Value };

                        for (var i = 0; i < values.Length; i++)
                        {
                            if (i > 0) builder.Append(",");
                            AddToBuilder(builder, outPutParameters, fieldCondition.Field, values[i]);
                        }

                        builder.Append(")");
                    }
                    break;

                case CompareOperation.IsNull:
                case CompareOperation.NotNull:
                    break;

                case CompareOperation.Equals:
                case CompareOperation.NotEquals:
                case CompareOperation.GreaterThan:
                case CompareOperation.LessThan:
                case CompareOperation.GreaterThanOrEquals:
                case CompareOperation.LessThanOrEquals:
                    AddToBuilder(builder, outPutParameters, fieldCondition.Field, fieldCondition.Value);
                    break;
            }

            return builder.ToString();
        }

        protected virtual string BuildFieldCondition(FieldCondition fieldCondition)
        {
            var builder = new StringBuilder()
                .Append(Common.GetSqlName(fieldCondition.Field))
                .Append(GetOperator(fieldCondition.Operation));

            if (Common.IsMethod(fieldCondition.ConditionField))
            {
                builder.Append(fieldCondition.ConditionField);
                return builder.ToString();
            }

            switch (fieldCondition.Operation)
            {
                case CompareOperation.NotIn:
                case CompareOperation.In:
                    builder.Append("(").Append(Common.GetSqlName(fieldCondition.ConditionField)).Append(")");
                    break;

                case CompareOperation.IsNull:
                case CompareOperation.NotNull:
                    break;

                case CompareOperation.Equals:
                case CompareOperation.NotEquals:
                case CompareOperation.GreaterThan:
                case CompareOperation.LessThan:
                case CompareOperation.GreaterThanOrEquals:
                case CompareOperation.LessThanOrEquals:

                case CompareOperation.NotContains:
                case CompareOperation.Contains:
                case CompareOperation.StartsWith:
                case CompareOperation.EndsWith:
                default:
                    builder.Append(Common.GetSqlName(fieldCondition.ConditionField));
                    break;
            }

            return builder.ToString();
        }
    }
}
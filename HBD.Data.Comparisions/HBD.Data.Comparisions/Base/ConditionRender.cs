#region

using System.Collections.Generic;
using System.Text;

#endregion

namespace HBD.Data.Comparisons.Base
{
    public abstract class ConditionRender : IConditionRender
    {
        public virtual string BuildCondition(ICondition condition, IDictionary<string, object> outPutParameters = null)
        {
            if (condition == null) return null;

            var binaryCondition = condition as BinaryCondition;
            return binaryCondition != null
                ? BuildBinaryCondition(binaryCondition, outPutParameters)
                : BuildFieldCondition((FieldConditionBase)condition, outPutParameters);
        }

        protected virtual string BuildBinaryCondition(BinaryCondition condition,
            IDictionary<string, object> outPutParameters = null)
        {
            var builder = new StringBuilder();
            var left = BuildCondition(condition.LeftCondition, outPutParameters);
            var right = BuildCondition(condition.RightCondition, outPutParameters);

            builder.Append($"({left})")
                .Append($" {condition.Operation} ")
                .Append($"({right})");

            return builder.ToString();
        }

        protected virtual string BuildCondition(ConditionBase condition,
                IDictionary<string, object> outPutParameters = null)
            =>
            condition is FieldConditionBase
                ? BuildFieldCondition((FieldConditionBase)condition, outPutParameters)
                : null;

        protected abstract string BuildFieldCondition(FieldConditionBase fieldCondition,
            IDictionary<string, object> outPutParameters = null);

        protected virtual string GetOperator(CompareOperation operation)
        {
            switch (operation)
            {
                case CompareOperation.NotEquals:
                    return " <> ";

                case CompareOperation.GreaterThanOrEquals:
                    return " >= ";

                case CompareOperation.GreaterThan:
                    return " > ";

                case CompareOperation.LessThanOrEquals:
                    return " <= ";

                case CompareOperation.LessThan:
                    return " < ";

                case CompareOperation.In:
                    return " IN ";

                case CompareOperation.NotIn:
                    return " NOT IN ";

                case CompareOperation.Contains:
                case CompareOperation.StartsWith:
                case CompareOperation.EndsWith:
                    return " LIKE ";

                case CompareOperation.NotContains:
                    return " NOT LIKE ";

                case CompareOperation.IsNull:
                    return " IS NULL ";

                case CompareOperation.NotNull:
                    return " IS NOT NULL ";

                default:
                    return " = ";
            }
        }
    }
}
namespace HBD.Data.Comparisons.Base
{
    public sealed class FieldCondition : FieldConditionBase
    {
        public FieldCondition(string field, CompareOperation operation, string conditionField)
        {
            Field = field;
            Operation = operation;
            ConditionField = conditionField;
        }

        public string ConditionField { get; private set; }
    }
}
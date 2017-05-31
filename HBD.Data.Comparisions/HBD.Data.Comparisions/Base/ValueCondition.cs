namespace HBD.Data.Comparisons.Base
{
    public sealed class ValueCondition : FieldConditionBase
    {
        public ValueCondition(string field, CompareOperation operation, object value)
        {
            Field = field;
            Operation = operation;
            Value = value;
        }

        public object Value { get; private set; }
    }
}
namespace HBD.Data.Comparisons.Base
{
    public interface ICondition
    {
    }

    public abstract class ConditionBase : ICondition
    {
        public static ICondition operator &(ConditionBase left, ICondition right) => left.And(right);

        public static ICondition operator |(ConditionBase left, ICondition right) => left.Or(right);
    }
}
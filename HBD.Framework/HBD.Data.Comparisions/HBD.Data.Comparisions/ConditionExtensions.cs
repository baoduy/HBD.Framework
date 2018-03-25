#region

using HBD.Data.Comparisons.Base;

#endregion

namespace HBD.Data.Comparisons
{
    public static class ConditionExtensions
    {
        public static ICondition And<TCondition>(this TCondition @this, TCondition condition)
            where TCondition : ICondition => new BinaryCondition(@this, BinaryOperation.And, condition);

        public static ICondition Or<TCondition>(this TCondition @this, TCondition condition)
            where TCondition : ICondition => new BinaryCondition(@this, BinaryOperation.Or, condition);
    }
}
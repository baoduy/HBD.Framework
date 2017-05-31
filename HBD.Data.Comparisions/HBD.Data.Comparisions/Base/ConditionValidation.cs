#region

using HBD.Framework.Core;

#endregion

namespace HBD.Data.Comparisons.Base
{
    public abstract class ConditionValidation<TData> : IConditionValidation where TData : class
    {
        public abstract void Validate(ICondition condition);

        protected virtual bool IsSatisfy(TData value, ICondition condition)
        {
            Guard.ArgumentIsNotNull(condition, nameof(condition));

            var binaryCondition = condition as BinaryCondition;
            return binaryCondition != null
                ? IsSatisfy(value, binaryCondition)
                : IsSatisfy(value, (FieldConditionBase) condition);
        }

        protected virtual bool IsSatisfy(TData value, BinaryCondition condition)
        {
            var leftSatisfy = IsSatisfy(value, condition.LeftCondition);
            var rightSatisfy = IsSatisfy(value, condition.RightCondition);

            switch (condition.Operation)
            {
                case BinaryOperation.And:
                    return leftSatisfy && rightSatisfy;

                case BinaryOperation.Or:
                    return leftSatisfy || rightSatisfy;

                default:
                    return false;
            }
        }

        protected virtual bool IsSatisfy(TData value, FieldConditionBase condition)
        {
            if (condition is ValueCondition)
                return IsSatisfy(value, (ValueCondition) condition);
            return IsSatisfy(value, (FieldCondition) condition);
        }

        protected abstract bool IsSatisfy(TData value, FieldCondition condition);

        protected abstract bool IsSatisfy(TData value, ValueCondition condition);
    }
}
#region

using HBD.Framework.Core;

#endregion

namespace HBD.Data.Comparisons.Base
{
    public class BinaryCondition : ConditionBase
    {
        internal BinaryCondition(ICondition leftCondition, BinaryOperation operation, ICondition rightCondition)
        {
            Guard.ArgumentIsNotNull(leftCondition, nameof(leftCondition));
            Guard.ArgumentIsNotNull(rightCondition, nameof(rightCondition));

            Operation = operation;
            LeftCondition = leftCondition;
            RightCondition = rightCondition;
        }

        public ICondition LeftCondition { get; protected set; }
        public ICondition RightCondition { get; protected set; }
        public BinaryOperation Operation { get; protected set; }
    }
}
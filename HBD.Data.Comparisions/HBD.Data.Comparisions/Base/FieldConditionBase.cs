namespace HBD.Data.Comparisons.Base
{
    public abstract class FieldConditionBase : ConditionBase
    {
        public virtual string Field { get; protected set; }
        public virtual CompareOperation Operation { get; protected set; }
    }
}
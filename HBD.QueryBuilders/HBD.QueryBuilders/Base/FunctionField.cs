namespace HBD.QueryBuilders.Base
{
    public abstract class FunctionField : Field
    {
        //internal string PartitionBy { get; private set; }
        //internal string OrderBy { get; private set; }

        protected FunctionField(string fiedName) : base(fiedName)
        {
        }

        protected FunctionField(FunctionType type, string fiedName) : base(fiedName)
        {
            Type = type;
        }

        internal FunctionType Type { get; } = FunctionType.All;
        //    this.Over(null, orderByField);
        //{
        //public void Over(string orderByField)
        //}

        //public void Over(string partitionByField, string orderByField)
        //{
        //    this.PartitionBy = partitionByField;
        //    this.OrderBy = OrderBy;
        //}
    }

    public enum FunctionType
    {
        All,
        Distinct
    }
}
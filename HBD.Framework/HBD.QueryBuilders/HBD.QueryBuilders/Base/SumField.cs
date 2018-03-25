namespace HBD.QueryBuilders.Base
{
    public class SumField : FunctionField
    {
        public SumField(string fiedName) : base(fiedName)
        {
        }

        public SumField(FunctionType type, string fiedName) : base(type, fiedName)
        {
        }
    }
}
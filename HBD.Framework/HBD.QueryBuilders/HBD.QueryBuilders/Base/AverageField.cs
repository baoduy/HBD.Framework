namespace HBD.QueryBuilders.Base
{
    public class AverageField : FunctionField
    {
        public AverageField(string fiedName) : base(fiedName)
        {
        }

        public AverageField(FunctionType type, string fiedName) : base(type, fiedName)
        {
        }
    }
}
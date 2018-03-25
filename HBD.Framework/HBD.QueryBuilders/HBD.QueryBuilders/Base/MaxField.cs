namespace HBD.QueryBuilders.Base
{
    public class MaxField : FunctionField
    {
        public MaxField(string fiedName) : base(fiedName)
        {
        }

        public MaxField(FunctionType type, string fiedName) : base(type, fiedName)
        {
        }
    }
}
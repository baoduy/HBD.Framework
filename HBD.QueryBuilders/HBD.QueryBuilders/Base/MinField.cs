namespace HBD.QueryBuilders.Base
{
    public class MinField : FunctionField
    {
        public MinField(string fiedName) : base(fiedName)
        {
        }

        public MinField(FunctionType type, string fiedName) : base(type, fiedName)
        {
        }
    }
}
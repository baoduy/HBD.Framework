namespace HBD.QueryBuilders.Base
{
    public class CountField : FunctionField
    {
        public CountField() : base("*")
        {
        }

        public CountField(string fiedName) : this(FunctionType.All, fiedName)
        {
        }

        public CountField(FunctionType type, string fiedName)
            : base(type, string.IsNullOrWhiteSpace(fiedName) ? "*" : fiedName)
        {
        }
    }
}
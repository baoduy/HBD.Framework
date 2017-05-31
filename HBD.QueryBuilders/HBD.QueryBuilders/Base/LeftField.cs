namespace HBD.QueryBuilders.Base
{
    public class LeftField : FunctionField
    {
        public LeftField(string fiedName, int length) : base(fiedName)
        {
            Length = length;
        }

        internal int Length { get; private set; }
    }
}
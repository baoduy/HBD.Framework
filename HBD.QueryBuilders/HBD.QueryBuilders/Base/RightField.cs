namespace HBD.QueryBuilders.Base
{
    public class RightField : FunctionField
    {
        public RightField(string fiedName, int length) : base(fiedName)
        {
            Length = length;
        }

        internal int Length { get; private set; }
    }
}
namespace HBD.QueryBuilders.Base
{
    public class CustomFunction : FunctionField
    {
        public CustomFunction(string functionName, params object[] parameters) : base(functionName)
        {
            Parameters = parameters;
        }

        internal string FunctionName => Name;
        internal object[] Parameters { get; }
    }
}
namespace HBD.QueryBuilders.Base
{
    public class FieldBuilder
    {
        private FieldBuilder()
        {
        }

        public static FieldBuilder Current { get; } = new FieldBuilder();

        public Field Field(string fileName) => new Field(fileName);

        public AverageField Avg(string fieldName) => new AverageField(fieldName);

        public AverageField Avg(FunctionType type, string fieldName) => new AverageField(type, fieldName);

        public CountField Count(string fieldName = null) => new CountField(fieldName);

        public CountField Count(FunctionType type, string fieldName) => new CountField(type, fieldName);

        public SumField Sum(string fieldName) => new SumField(fieldName);

        public SumField Sum(FunctionType type, string fieldName) => new SumField(type, fieldName);

        public MinField Min(string fieldName) => new MinField(fieldName);

        public MinField Min(FunctionType type, string fieldName) => new MinField(type, fieldName);

        public MaxField Max(string fieldName) => new MaxField(fieldName);

        public MaxField Max(FunctionType type, string fieldName) => new MaxField(type, fieldName);

        public LeftField Left(string fieldName, int length) => new LeftField(fieldName, length);

        public RightField Right(string fieldName, int length) => new RightField(fieldName, length);

        public CustomFunction Func(string functionName, params object[] parameters)
            => new CustomFunction(functionName, parameters);
    }

    public class TableBuilder
    {
        private TableBuilder()
        {
        }

        public static TableBuilder Current { get; } = new TableBuilder();

        public Table Table(string tableName) => new Table(tableName);

        /// <summary>
        ///     The shorter of Table method
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Table Field(string fileName) => Table(fileName);
    }

    public class SubQueryBuilder
    {
        private SubQueryBuilder()
        {
        }

        public static SubQueryBuilder Current { get; } = new SubQueryBuilder();

        public SubQuery SubQuery(QueryBuilder query) => new SubQuery(query);

        /// <summary>
        ///     The shortern of SubQuery method
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public SubQuery _(QueryBuilder query) => SubQuery(query);
    }
}
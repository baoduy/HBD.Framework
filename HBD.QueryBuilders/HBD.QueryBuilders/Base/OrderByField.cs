namespace HBD.QueryBuilders.Base
{
    public class OrderByField
    {
        internal OrderByField(string fieldName, OrderType orderType)
        {
            Name = fieldName;
            OrderType = orderType;
        }

        internal string Name { get; private set; }
        internal OrderType OrderType { get; private set; }
    }

    public enum OrderType
    {
        Ascending,
        Descending
    }
}
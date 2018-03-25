namespace HBD.Framework.Data.SqlClient.Base
{
    public interface IDbInfo
    {
        DbName Name { get; }
        SchemaInfo Schema { get; set; }
    }
}
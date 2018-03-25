#region

using System;
using System.Data.SqlClient;

#endregion

namespace HBD.Framework.Security.Azman
{
    public static class ConnectionStringExtensions
    {
        public static SqlConnectionStringBuilder ToSqlConnectionString(this AzSqlConnectionStringBuilder @this)
        {
            var conn = new SqlConnectionStringBuilder
            {
                DataSource = @this.ServerName,
                InitialCatalog = @this.DataBaseName,
                IntegratedSecurity = @this.UserName.IsNull()
            };

            if (!conn.IntegratedSecurity)
            {
                conn.UserID = @this.UserName;
                conn.Password = @this.Password;
            }

            return conn;
        }

        public static SqlConnectionStringBuilder ToSqlConnectionString(this IAzConnectionStringBuilder @this)
        {
            if (@this is AzXmlConnectionStringBuilder) throw new NotSupportedException(@this.GetType().FullName);
            return ((AzSqlConnectionStringBuilder) @this).ToSqlConnectionString();
        }
    }
}
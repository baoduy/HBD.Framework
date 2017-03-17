#region

using System;
using System.Linq;
using System.Text;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Security.Azman
{
    /// <summary>
    ///     - SQL Storage:
    ///     Trusted:   mssql://Driver={SQL Server};Server={[Server Name]};/[Database Name]/[AzMan Store Name]
    ///     Untrusted: mssql://Driver={SQL Server};Server={[Server Name]};Uid=[User Id];Pwd=[Password];/[Database Name]/[AzMan
    ///     Store Name]
    /// </summary>
    public class AzSqlConnectionStringBuilder : BaseAzConnectionStringBuilder
    {
        public AzSqlConnectionStringBuilder(string connectionString = null)
        {
            Parse(connectionString);
        }

        public string Driver => "mssql://Driver={SQL Server};";
        public string ServerName { get; set; }
        public string DataBaseName { get; set; }
        public string AzManStoreName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        protected override void Parse(string connectionString)
        {
            if (connectionString.IsNullOrEmpty())
            {
                ServerName = DataBaseName = AzManStoreName = UserName = Password = null;
            }
            else
            {
                var splits = connectionString.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);

                try
                {
                    foreach (var s in splits)
                    {
                        if (s.StartsWith("mssql")) continue;

                        if (s.StartsWith("Server", StringComparison.OrdinalIgnoreCase))
                        {
                            ServerName = s.ExtractBraces().FirstOrDefault()?.Value.Trim();
                        }
                        else if (s.StartsWith("Uid", StringComparison.OrdinalIgnoreCase))
                        {
                            UserName = s.Split('=')[1].Trim();
                        }
                        else if (s.StartsWith("Pwd", StringComparison.OrdinalIgnoreCase))
                        {
                            Password = s.Split('=')[1].Trim();
                        }
                        else if (s.StartsWith("/"))
                        {
                            var ss = s.Split('/');

                            DataBaseName = ss[1]?.Trim();
                            AzManStoreName = ss[2]?.Trim();
                        }
                    }
                }
                catch
                {
                    throw new ArgumentException(connectionString);
                }

                Validate();
            }
        }

        protected override string BuildConnectionString()
        {
            Validate();

            var builder = new StringBuilder(Driver)
                .Append($"Server={{{ServerName}}};");

            if (UserName.IsNotNullOrEmpty() && Password.IsNotNullOrEmpty())
                builder.AppendFormat("Uid={0};", UserName)
                    .AppendFormat("Pwd={0};", Password);

            builder.AppendFormat("/{0}/{1}", DataBaseName, AzManStoreName);

            return builder.ToString();
        }

        public override void Validate()
        {
            Guard.ArgumentIsNotNull(ServerName, nameof(ServerName));
            Guard.ArgumentIsNotNull(DataBaseName, nameof(DataBaseName));
            Guard.ArgumentIsNotNull(AzManStoreName, nameof(AzManStoreName));
        }
    }
}
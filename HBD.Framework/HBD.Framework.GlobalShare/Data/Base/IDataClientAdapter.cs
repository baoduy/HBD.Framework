#region using

using System;
using System.Collections.Generic;
using System.Data;

#endregion

namespace HBD.Framework.Data.Base
{
    public interface IDataClientAdapter : IDisposable
    {
        ConnectionState State { get; }

        bool Close();

        int ExecuteNonQuery(IDbCommand command);

        int ExecuteNonQuery(string query, IDictionary<string, object> parameters = null);

        IDataReader ExecuteReader(IDbCommand command);

        IDataReader ExecuteReader(string query, IDictionary<string, object> parameters = null);

        object ExecuteScalar(IDbCommand command);

        object ExecuteScalar(string query, IDictionary<string, object> parameters = null);

#if !NETSTANDARD1_6
        DataTable ExecuteTable(string query, IDictionary<string, object> parameters = null);
#endif
        bool Open();
    }
}
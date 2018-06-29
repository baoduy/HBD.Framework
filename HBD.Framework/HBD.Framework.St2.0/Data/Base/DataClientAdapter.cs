#region using

using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using HBD.Framework.Attributes;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Data.Base
{
    public abstract class DataClientAdapter : IDataClientAdapter
    {
        #region Properties
        private readonly string _connectionString;

        private DbConnectionStringBuilder _dbConnectionStringBuilder;
        public DbConnectionStringBuilder ConnectionString
        {
            get
            {
                if (_dbConnectionStringBuilder == null)
                    _dbConnectionStringBuilder = CreateConnectionString(_connectionString);
                return _dbConnectionStringBuilder;
            }

            private set => _dbConnectionStringBuilder = value;
        }

        IDbConnection _connection;
        public IDbConnection Connection
        {
            get
            {
                if (_connection == null)
                    _connection = CreateConnection();
                return _connection;
            }

            private set => _connection = value;
        }

        public ConnectionState State => Connection?.State ?? ConnectionState.Broken;

        #endregion Properties

        #region Constructors

        protected DataClientAdapter([NotNull] string connectionString)
        {
            _connectionString = connectionString;
        }

        protected DataClientAdapter([NotNull] DbConnectionStringBuilder connectionStringBuilder)
        {
            ConnectionString = connectionStringBuilder;
        }

        protected DataClientAdapter([NotNull] IDbConnection connection)
        {
            Guard.ArgumentIsNotNull(connection, nameof(connection));
            Connection = connection;
        }

        #endregion Constructors

        #region Protected Virtual Methods

        protected abstract DbConnectionStringBuilder CreateConnectionString(string connectionString);

        protected abstract IDbConnection CreateConnection();

        protected abstract IDataParameter CreateParameter(string name, object value);

        /// <summary>
        ///     Build Command with CommandType is Text
        /// </summary>
        /// <param name="queryOrStoreProcedure">Query or Store Procedure</param>
        /// <param name="parameters">The parameters</param>
        /// <returns></returns>
        protected virtual IDbCommand CreateCommand(string queryOrStoreProcedure = null,
            IDictionary<string, object> parameters = null)
        {
            var command = Connection.CreateCommand();
            command.CommandText = queryOrStoreProcedure;
            if (parameters != null)
                foreach (var parameter in parameters)
                    command.Parameters.Add(CreateParameter(parameter.Key, parameter.Value));
            return command;
        }

        #endregion Protected Virtual Methods

        #region Public Methods

        public bool Open() => Connection.TryOpen();

        public bool Close() => Connection.TryClose();

        public virtual int ExecuteNonQuery(IDbCommand command)
        {
            try
            {
                Connection.TryOpen();
                command.Connection = Connection;
                return command.ExecuteNonQuery();
            }
            finally
            {
                Connection.TryClose();
            }
        }

        public virtual object ExecuteScalar(IDbCommand command)
        {
            try
            {
                Connection.TryOpen();
                command.Connection = Connection;
                return command.ExecuteScalar();
            }
            finally
            {
                Connection.TryClose();
            }
        }

        public virtual IDataReader ExecuteReader(IDbCommand command)
        {
            Connection.TryOpen();
            command.Connection = Connection;
            return command.ExecuteReader(CommandBehavior.CloseConnection | CommandBehavior.KeyInfo);
        }

        public virtual int ExecuteNonQuery(string query, IDictionary<string, object> parameters = null)
        {
            using (var command = CreateCommand(query, parameters))
            {
                return ExecuteNonQuery(command);
            }
        }

        public virtual object ExecuteScalar(string query, IDictionary<string, object> parameters = null)
        {
            using (var command = CreateCommand(query, parameters))
            {
                return ExecuteScalar(command);
            }
        }

        public virtual IDataReader ExecuteReader(string query, IDictionary<string, object> parameters = null)
        {
            using (var command = CreateCommand(query, parameters))
            {
                return ExecuteReader(command);
            }
        }

#if !NETSTANDARD1_6
        public virtual DataTable ExecuteTable(string query, IDictionary<string, object> parameters = null)
        {
            using (var reader = ExecuteReader(query, parameters))
            {
                var tb = new DataTable();
                tb.Load(reader);
                return tb;
            }
        }

#endif

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool isDisposing) => Connection?.Dispose();
        #endregion Public Methods
    }
}
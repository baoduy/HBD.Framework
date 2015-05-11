using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HBD.Framework.Data.SQL
{
    public class SQLAdapter : IDisposable
    {
        public SQLAdapter() { }
        public SQLAdapter(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        public virtual string ConnectionString { get; set; }

        private IDbConnection Connection { get; set; }
        public void Open()
        {
            if (this.Connection == null)
                this.Connection = new SqlConnection();

            if (this.Connection.State == ConnectionState.Open)
            {
                try { this.Connection.Close(); }
                catch (Exception ex) { Log.LogManager.Write(ex); }
            }

            this.Connection.ConnectionString = this.ConnectionString;
            this.Connection.Open();
        }

        public IDataReader ExcecuteReader(string commandText, CommandType commandType)
        {
            using (var cmd = this.Connection.CreateCommand())
            {
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;
                cmd.Connection = this.Connection;
                return cmd.ExecuteReader();
            }
        }

        public string[] GetAllDatabaseNames()
        {
            var list = new List<string>();
            using (var reader = this.ExcecuteReader("sp_databases", CommandType.StoredProcedure))
            {
                while (reader.Read())
                    list.Add(reader["DATABASE_NAME"] as string);
            }
            return list.ToArray();
        }

        public DataTable ExceuteQuery(string query)
        {
            var data = new DataTable();

            using (var reader = this.ExcecuteReader(query, CommandType.Text))
            {
                data.Load(reader);
            }

            return data;
        }

        public DataTable ExceuteStore(string storeProcedure)
        {
            var data = new DataTable();

            using (var reader = this.ExcecuteReader(storeProcedure, CommandType.StoredProcedure))
            {
                data.Load(reader);
            }

            return data;
        }
        public void Dispose()
        {
            if (this.Connection != null)
            {
                try
                {
                    this.Connection.Close();
                    this.Connection.Dispose();
                }
                catch (Exception ex) { Log.LogManager.Write(ex); }
                finally { this.Connection = null; }
            }
        }
    }
}

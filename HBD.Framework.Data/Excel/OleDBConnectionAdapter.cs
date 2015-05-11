using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using HBD.Framework.Data.Excel.Core;

namespace HBD.Framework.Data.Excel
{
    internal class OleDBConnectionAdapter : ConnectionAdapterBase
    {
        public OleDBConnectionAdapter(string fileName) : base(fileName) { }

        OleDbConnection _connection;

        public override void Open()
        {
            if (string.IsNullOrEmpty(this.FileName))
                return;

            if (this._connection != null && this._connection.State == ConnectionState.Closed)
            {
                this.Dispose();
            }

            if (this._connection == null)
            {
                var constr = new OleDbConnectionStringBuilder();
                constr.DataSource = this.FileName;
                constr.Provider = "Microsoft.ACE.OLEDB.12.0";
                constr["Extended Properties"] = "Excel 12.0;HDR=Yes;IMEX=1";

                this._connection = new OleDbConnection(constr.ToString());
                this._connection.Open();
            }
        }

        public override string[] GetSheetNames()
        {
            if (this._connection == null)
                return null;

            var list = new List<string>();
            using (var dt = this._connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null))
            {
                if (dt != null)
                {
                    // Add the sheet name to the string array.
                    foreach (DataRow row in dt.Rows)
                        list.Add(row["TABLE_NAME"].ToString().Replace("'", string.Empty).Replace("$", string.Empty));
                }
            }

            list.Sort();
            return list.ToArray();
        }

        public virtual DataTable Excecute(string query)
        {
            var dataTable = new System.Data.DataTable();

            var command = this._connection.CreateCommand();
            command.CommandText = query;
            command.CommandType = CommandType.TableDirect;

            var adapter = new OleDbDataAdapter(command);
            adapter.Fill(dataTable);

            return dataTable;
        }

        public override DataTable GetTableBySheetName(string sheetName)
        {
            var data = this.Excecute(string.Format("SELECT * FROM [{0}$]", sheetName));
            data.TableName = sheetName;
            return data;
        }

        public override void Dispose()
        {
            try
            {
                if (this._connection != null)
                {
                    this._connection.Close();
                    this._connection = null;
                }
            }
            finally { this._connection = null; }
        }
    }
}

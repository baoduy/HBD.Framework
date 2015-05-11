using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using HBD.Framework.Data.Excel.Core;
using DocumentFormat.OpenXml.Packaging;
using HBD.Framework.OpenXML;

namespace HBD.Framework.Data.Excel
{
    internal class OpenXMLConnectionAdapter : ConnectionAdapterBase
    {
        public OpenXMLConnectionAdapter(string fileName) : base(fileName) { }

        SpreadsheetDocument _connection;
        public override void Open()
        {
            if (string.IsNullOrEmpty(this.FileName))
                return;

            if (this._connection == null)
                this._connection = OpenXMLHelper.OpenExcelFile(this.FileName);
        }

        public override string[] GetSheetNames()
        {
            if (this._connection == null)
                return null;

            return OpenXMLHelper.GetSheetNames(this._connection);
        }

        public override DataTable GetTableBySheetName(string sheetName)
        {
            return OpenXMLHelper.ReadDataFromWorksheet(this._connection, sheetName);
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

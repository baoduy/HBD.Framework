using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Data.OleDb;
using DocumentFormat.OpenXml.Packaging;
using HBD.Framework.OpenXML;
using HBD.Framework.Data.Excel.Core;
using HBD.Framework.Core;

namespace HBD.Framework.Data.Excel
{
    public class ExcelAdapter : FileDataConverterBase
    {
        public ExcelAdapter(string fileName, bool isUsingOpenXML = false)
            : base(fileName)
        { this.IsUsingOpenXML = isUsingOpenXML; }

        public ExcelAdapter(FileInfo file, bool isUsingOpenXML = false)
            : base(file)
        { this.IsUsingOpenXML = IsUsingOpenXML; }

        public bool IsUsingOpenXML { get; private set; }

        string[] sheetNames = null;
        public string[] SheetNames
        {
            get
            {
                if (this.sheetNames == null)
                {
                    if (this.Connection == null)
                        return null;
                    this.sheetNames = this.Connection.GetSheetNames();
                }
                return this.sheetNames;
            }
        }

        #region private methods
        IConnectionAdapter _connection;
        private IConnectionAdapter Connection
        {
            get
            {
                if (string.IsNullOrEmpty(this.FileName))
                    return null;

                if (this._connection == null)
                {
                    if (this.IsUsingOpenXML)
                    {
                        this._connection = new OpenXMLConnectionAdapter(this.FileName);
                        this._connection.Open();
                    }
                    else
                    {
                        try
                        {
                            this._connection = new OleDBConnectionAdapter(this.FileName);
                            this._connection.Open();
                        }
                        catch
                        {
                            this._connection = new OpenXMLConnectionAdapter(this.FileName);
                            this._connection.Open();
                        }
                    }
                }

                return this._connection;
            }
        }

        #endregion

        private string EnsureSheetName(string sheetName)
        {
            if (string.IsNullOrEmpty(sheetName)
                && this.SheetNames != null)
            {
                sheetName = this.SheetNames[0];
            }

            Guard.ArgumentNotNull(sheetName, "Sheet Name");

            return sheetName;
        }

        /// <summary>
        /// Convert To DataTable
        /// </summary>
        /// <param name="sheetName">The sheet name</param>
        /// <returns></returns>
        public override DataTable ToDataTable(string sheetName = null)
        {
            sheetName = this.EnsureSheetName(sheetName);
            return this.Connection.GetTableBySheetName(sheetName);
        }

        /// <summary>
        /// Convert All Sheets to Data Set
        /// </summary>
        /// <returns></returns>
        public override DataSet ToDataSet()
        {
            var dataSet = new DataSet();

            if (this.SheetNames.Length == 0)
                return dataSet;

            foreach (var sheet in this.SheetNames)
                dataSet.Tables.Add(this.ToDataTable(sheet));
            return dataSet;
        }


        private void WriteFile(SpreadsheetDocument spreadsheet, DataTable table, CellStyleCollection styles = null)
        {
            var worksheetPart = OpenXMLHelper.AddSheet(spreadsheet, table.TableName);
            OpenXMLHelper.FillDataToWorksheet(worksheetPart, table, spreadsheet, styles);
        }

        public void WriteFile(DataSet dataSet, string fileName = null)
        {
            fileName = this.EnsureFileName(fileName);

            using (var spreadsheet = OpenXMLHelper.CreateExcelFile(fileName))
            {
                foreach (DataTable table in dataSet.Tables)
                    this.WriteFile(spreadsheet, table);

                spreadsheet.WorkbookPart.Workbook.Save();
            }
        }

        public void WriteFile(DataTable table, string fileName = null, CellStyleCollection styles = null)
        {
            fileName = this.EnsureFileName(fileName);

            using (var spreadsheet = OpenXMLHelper.CreateExcelFile(fileName))
            {
                this.WriteFile(spreadsheet, table, styles);
                spreadsheet.WorkbookPart.Workbook.Save();
            }
        }

        public override void WriteFile(DataTable data)
        {
            this.WriteFile(data, string.Empty);
        }

        public override void Dispose()
        {
            base.Dispose();

            try
            {
                if (this._connection != null)
                {
                    this._connection.Dispose();
                    this._connection = null;
                }
            }
            finally { this._connection = null; }
        }
    }
}

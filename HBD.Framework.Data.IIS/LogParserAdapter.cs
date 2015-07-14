using System;
using System.Data;
using System.IO;
using System.Linq;
using MSUtil;

using LogQuery = MSUtil.LogQueryClass;
using LogRecordSet = MSUtil.ILogRecordset;
using LogRecord = MSUtil.ILogRecord;
using IISW3CInputFormat = MSUtil.COMIISW3CInputContextClass;

namespace HBD.Framework.Data.IIS
{
    public class LogParserAdapter : DataConverterBase
    {
        public string RootFolder { get; private set; }

        public LogParserAdapter(string rootFolder)
        {
            this.RootFolder = rootFolder;
        }

        private Type GetColumnType(int type)
        {
            switch (type)
            {
                case 1: return typeof(int);
                case 2: return typeof(double);
                case 4: return typeof(DateTime);
                default: return typeof(string);
            }
        }

        private void VerifyAppPoolName(string name)
        {
            if (!this.AppPoolsId.Contains(name))
                throw new ArgumentException(string.Format("The Log Folder {0} is not a subfolder of Root folder {1}", name, this.RootFolder));
        }

        string[] _appPoolsId = null;
        public string[] AppPoolsId
        {
            get
            {
                if (!Directory.Exists(this.RootFolder))
                    return this._appPoolsId;

                var list = new DirectoryInfo(this.RootFolder).GetDirectories().Select(d => d.Name).ToList();

                list.Sort();

                this._appPoolsId = list.ToArray();
                return this._appPoolsId;
            }
        }

        public LogRecordSet Execute(string query)
        {
            var logQuery = new LogQuery();
            return logQuery.Execute(query, new IISW3CInputFormat());
        }

        public DataTable ConvertToDataTable(ILogRecordset recordSet)
        {
            var data = new DataTable();

            var columnCount = recordSet.getColumnCount();
            for (int i = 0; i < columnCount; i++)
                data.Columns.Add(recordSet.getColumnName(i), this.GetColumnType(recordSet.getColumnType(i)));

            // Traverse the result set
            for (; !recordSet.atEnd(); recordSet.moveNext())
            {
                // Get the current record
                var logRecord = recordSet.getRecord();
                var row = data.NewRow();

                for (int i = 0; i < columnCount; i++)
                    row[i] = logRecord.getValue(i);

                data.Rows.Add(row);
            }

            return data;
        }

        public override DataTable ToDataTable(string name = null)
        {
            if (string.IsNullOrEmpty(name))
                name = this.AppPoolsId[0];
            VerifyAppPoolName(name);

            var query = "SELECT * FROM {0}\\{1}\\u_ex*.log";
            query = string.Format(query, this.RootFolder, name);

            var recordSet = this.Execute(query);
            return ConvertToDataTable(recordSet);
        }

        public override DataSet ToDataSet()
        {
            var dataSet = new DataSet();
            foreach (var d in this.AppPoolsId)
                dataSet.Tables.Add(this.ToDataTable(d));
            return dataSet;
        }

        public virtual DataTable GetAccountLastLoginDate(string appPoolFolder)
        {
            VerifyAppPoolName(appPoolFolder);

            var query = "SELECT to_uppercase(cs-username) as Account, max(date) as LastLoginDate FROM {0}\\{1}\\u_ex*.log GROUP BY to_uppercase(cs-username)";
            query = string.Format(query, this.RootFolder, appPoolFolder);

            var recordSet = this.Execute(query);
            return ConvertToDataTable(recordSet);
        }
    }
}

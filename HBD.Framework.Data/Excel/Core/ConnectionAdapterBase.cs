using System;
namespace HBD.Framework.Data.Excel.Core
{
    internal abstract class ConnectionAdapterBase : IConnectionAdapter
    {
        public string FileName { get; private set; }

        public ConnectionAdapterBase(string fileName)
        {
            this.FileName = fileName;
        }

        public abstract void Open();

        public abstract string[] GetSheetNames();

        public abstract System.Data.DataTable GetTableBySheetName(string sheetName);

        public abstract void Dispose();
    }
}

using System;
namespace HBD.Framework.Data.Excel.Core
{
    internal interface IConnectionAdapter : IDisposable
    {
        string FileName { get; }
        string[] GetSheetNames();
        System.Data.DataTable GetTableBySheetName(string sheetName);
        void Open();
    }
}

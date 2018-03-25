#region

using System;
using System.Collections.Generic;
using System.Data;

#endregion

namespace HBD.Data.Comparisons.Base
{
    public interface IDataTableComparision : IDisposable
    {
        DataTable CompareTable { get; }
        DataTable Table { get; }

        ICompareColumnInfo Column(string columnName);

        IDataTableComparision Columns(IEnumerable<ICompareColumnInfo> columns);

        IComparisionResult Execute();

        ICompareColumnInfo PrimaryKey(string keyName);

        IDataTableComparision PrimaryKeys(IEnumerable<ICompareColumnInfo> keys);

        object GetValue(ICompareColumnInfo column, DataRow row);
    }
}
#region

using System.Collections.Generic;
using System.Data;

#endregion

namespace HBD.Data.Comparisons.Base
{
    public interface ICompareColumnInfo : IEqualityComparer<ICompareColumnInfo>
    {
        IDataTableComparision ParentComparision { get; set; }
        string Column { get; }
        string CompareColumn { get; }

        object GetValue(DataRow row);
    }
}
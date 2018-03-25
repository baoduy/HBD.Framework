#region

using HBD.Framework.Core;

#endregion

namespace HBD.Data.Comparisons.Base
{
    public class DifferenceCell
    {
        public DifferenceCell(string column, string compareColumn)
        {
            Guard.ArgumentIsNotNull(column, nameof(column));
            Guard.ArgumentIsNotNull(compareColumn, nameof(compareColumn));

            Column = column;
            CompareColumn = compareColumn;
        }

        public DifferenceCell(ICompareColumnInfo compareColumn)
        {
            Guard.ArgumentIsNotNull(compareColumn, nameof(compareColumn));

            Column = compareColumn.Column;
            CompareColumn = compareColumn.CompareColumn;
        }

        public string Column { get; private set; }
        public string CompareColumn { get; private set; }
    }
}
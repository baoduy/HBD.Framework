#region

using System.Collections.Generic;
using System.Data;
using System.Linq;
using HBD.Framework.Core;

#endregion

namespace HBD.Data.Comparisons.Base
{
    public class DifferenceRow
    {
        public DifferenceRow(DataRow row, DataRow compareRow, DifferenceCell[] cells)
        {
            Guard.ArgumentIsNotNull(row, nameof(row));
            Guard.ArgumentIsNotNull(compareRow, nameof(compareRow));

            Row = row;
            CompareRow = compareRow;
            Cells = cells ?? new DifferenceCell[] {};
        }

        public DataRow Row { get; }
        public DataRow CompareRow { get; }
        public DifferenceCell[] Cells { get; private set; }
    }

    public class DifferenceRowCollection : List<DifferenceRow>
    {
        public bool ContainsRow(DataRow row) => this.Any(r => r.Row == row);

        public bool ContainsCompareRow(DataRow compareRow) => this.Any(r => r.CompareRow == compareRow);
    }
}
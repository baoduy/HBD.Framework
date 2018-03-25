#region

using System;
using System.Collections.Generic;
using System.Data;
using HBD.Data.Comparisons.Base;
using HBD.Framework;

#endregion

namespace HBD.Data.Comparisons
{
    public class DataTablesResult : IDisposable
    {
        public DataTablesResult(ComparisionResult comparisionResult, DataTablesResultType resultType)
        {
            ComparisionResult = comparisionResult;
            ResultType = resultType;

            Table = comparisionResult.Source.Table.Clone().AllowDbNull();
            CompareTable = comparisionResult.Source.CompareTable.Clone().AllowDbNull();
        }

        public ComparisionResult ComparisionResult { get; }
        public DataTablesResultType ResultType { get; }

        public DataTable Table { get; }
        public DataTable CompareTable { get; }

        public IList<DifferenceRow> DifferenceCells { get; } = new List<DifferenceRow>();
        public NotFoundRows NotFoundRows { get; } = new NotFoundRows();

        public void Dispose()
        {
            Table?.Dispose();
            CompareTable?.Dispose();
        }

        public void Add(DifferenceRow row)
        {
            if (row == null) return;

            var r = Table.Rows.Add(row.Row.ItemArray);
            var cr = CompareTable.Rows.Add(row.CompareRow.ItemArray);
            DifferenceCells.Add(new DifferenceRow(r, cr, row.Cells));
        }

        public void AddRange(IEnumerable<DifferenceRow> rows)
        {
            foreach (var row in rows) Add(row);
        }

        public void Add(NotFoundRows notfoundRows)
        {
            if (notfoundRows == null) return;

            foreach (var row in notfoundRows.Rows)
            {
                var r = Table.Rows.Add(row.ItemArray);
                var cr = CompareTable.Rows.Add();

                NotFoundRows.Rows.Add(r);
                NotFoundRows.CompareRows.Add(cr);
            }

            foreach (var row in notfoundRows.CompareRows)
            {
                var r = Table.Rows.Add();
                var cr = CompareTable.Rows.Add(row.ItemArray);

                NotFoundRows.Rows.Add(r);
                NotFoundRows.CompareRows.Add(cr);
            }
        }
    }
}
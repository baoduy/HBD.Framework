#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using HBD.Data.Comparisons.Base;
using HBD.Framework.Core;

#endregion

namespace HBD.Data.Comparisons
{
    public static class DataTableComparisionExtensions
    {
        public static IConditionRender DefaultDataTableExpressionRender { get; } = new DataTableExpressionRender();

        /// <summary>
        ///     Count the number of cells that have values the same with the cell in @this.
        /// </summary>
        /// <param name="this">The row</param>
        /// <param name="row">comparison row</param>
        /// <param name="columns">Columns comparison</param>
        /// <returns>The number of cells.</returns>
        public static DifferenceCell[] GetDiffrenceCells(this DataRow @this, DataRow row,
            IList<ICompareColumnInfo> columns = null)
        {
            var list = new List<DifferenceCell>();
            if (@this == row) return list.ToArray();

            if (columns == null)
                for (var i = 0; i < @this.ItemArray.Length && i < row.ItemArray.Length; i++)
                {
                    if (@this[i].IsEquals(row[i])) continue;
                    list.Add(new DifferenceCell(@this.Table.Columns[i].ColumnName, row.Table.Columns[i].ColumnName));
                }
            else
                list.AddRange(from col in columns
                    let val = col.GetValue(@this)
                    let comVal = col.GetValue(row)
                    where !val.IsEquals(comVal)
                    select new DifferenceCell(col));

            return list.ToArray();
        }

        public static bool IsEquals(this DataRow @this, DataRow row, IList<ICompareColumnInfo> columns = null)
        {
            var diffCell = @this.GetDiffrenceCells(row, columns);
            return diffCell.Length == 0;
        }

        /// <summary>
        ///     Find the row in rows array exactly the same with @this.
        /// </summary>
        /// <param name="this">the row</param>
        /// <param name="rows">the row array.</param>
        /// <param name="columns">The CompareColumnInfos</param>
        /// <returns>The identity row</returns>
        public static DataRow FindEqualsRow(this DataRow @this, DataRow[] rows, IList<ICompareColumnInfo> columns = null)
        {
            if (@this == null || rows == null || rows.Length <= 0) return null;
            return rows.FirstOrDefault(row => @this.IsEquals(row, columns));
        }

        /// <summary>
        ///     Find the most equals row in the Array.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static DifferenceRow FindMostEqualsRow(this DataRow @this, DataRow[] rows,
            IList<ICompareColumnInfo> columns = null)
        {
            if (@this == null || rows == null || rows.Length <= 0) return null;

            DifferenceCell[] diffCells = null;
            DataRow foundRow = null;

            foreach (var row in rows)
            {
                var cells = @this.GetDiffrenceCells(row, columns);
                if (diffCells != null && cells.Length >= diffCells.Length) continue;
                foundRow = row;
                diffCells = cells;
            }

            return new DifferenceRow(@this, foundRow, diffCells);
        }

        /// <summary>
        ///     Compare 2 DataTables
        /// </summary>
        /// <param name="this">1st DataTable</param>
        /// <param name="table">2st DataTable</param>
        /// <param name="keys">The Primary Keys Columns</param>
        /// <param name="columns">Compare Columns Collection</param>
        /// <returns></returns>
        public static bool IsEquals(this DataTable @this, DataTable table, IList<ICompareColumnInfo> keys = null,
            IList<ICompareColumnInfo> columns = null)
        {
            Guard.ArgumentIsNotNull(@this, "DataTable");
            Guard.ArgumentIsNotNull(table, "CompareDataTale");

            return @this.CompareTo(table)
                .PrimaryKeys(keys)
                .Columns(columns)
                .Execute().IsIdentity;
        }

        /// <summary>
        ///     Compare 2 DataTables
        /// </summary>
        /// <param name="this">1st DataTable</param>
        /// <param name="table">2st DataTable</param>
        /// <param name="keys">The Primary Keys Columns</param>
        /// <param name="columns">Compare Columns Collection</param>
        /// <returns></returns>
        public static bool IsNotEquals(this DataTable @this, DataTable table, IList<ICompareColumnInfo> keys = null,
                IList<ICompareColumnInfo> columns = null)
            => !@this.IsEquals(table, keys, columns);

        public static DataTableComparision CompareTo(this DataTable @this, DataTable table)
            => new DataTableComparision(@this, table);

        public static DataRow[] Select(this DataTable @this, ICondition condition)
        {
            var filter = DefaultDataTableExpressionRender.BuildCondition(condition);
            try
            {
                return @this.Select(filter);
            }
            catch (Exception ex)
            {
                throw new Exception(filter, ex);
            }
        }
    }
}
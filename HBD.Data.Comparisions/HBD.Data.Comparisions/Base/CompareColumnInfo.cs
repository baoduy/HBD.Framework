#region

using System;
using System.Data;
using HBD.Framework.Core;

#endregion

namespace HBD.Data.Comparisons.Base
{
    public class CompareColumnInfo : ICompareColumnInfo
    {
        /// <summary>
        ///     ColumnComparisionInfo with ColumnName.
        ///     This constructor will assume that both Tables have the same ColumnName.
        /// </summary>
        /// <param name="column"></param>
        public CompareColumnInfo(string column) : this(column, column)
        {
        }

        /// <summary>
        ///     ColumnComparisionInfo with ColumnName in Table and compareColumnName in ComparisionTable.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="compareColumn"></param>
        public CompareColumnInfo(string column, string compareColumn)
        {
            Guard.ArgumentIsNotNull(column, nameof(column));

            Column = column;
            CompareColumn = compareColumn ?? column;
        }

        public IDataTableComparision ParentComparision { get; set; }

        public string Column { get; }
        public string CompareColumn { get; protected set; }

        public object GetValue(DataRow row)
        {
            if (row == null) return null;
            if (ParentComparision != null)
                return ParentComparision.GetValue(this, row);

            if (row.Table.Columns.Contains(Column))
                return row[Column];
            if (row.Table.Columns.Contains(CompareColumn))
                return row[CompareColumn];

            return null;
        }

        public bool Equals(ICompareColumnInfo x, ICompareColumnInfo y) => x == y;

        public int GetHashCode(ICompareColumnInfo obj) => obj.GetHashCode();

        public IDataTableComparision CompareTo(string compareColumn)
        {
            Guard.ArgumentIsNotNull(ParentComparision, nameof(ParentComparision));
            Guard.ArgumentIsNotNull(CompareColumn, nameof(CompareColumn));

            if (!ParentComparision.CompareTable.Columns.Contains(CompareColumn))
                throw new ArgumentException($"Column {CompareColumn} is not found in ComparisionTable.");

            CompareColumn = compareColumn;
            return ParentComparision;
        }

        public override int GetHashCode() => (Column + CompareColumn).GetHashCode();

        public override bool Equals(object obj)
        {
            var info = obj as CompareColumnInfo;
            if (info != null)
                return Column == info.Column && CompareColumn == info.CompareColumn;
            return false;
        }

        public static bool operator ==(CompareColumnInfo @this, ICompareColumnInfo column)
        {
            if ((object) @this == null && column == null) return true;
            return @this?.Equals(column) ?? false;
        }

        public static bool operator !=(CompareColumnInfo @this, ICompareColumnInfo column) => !(@this == column);
    }
}
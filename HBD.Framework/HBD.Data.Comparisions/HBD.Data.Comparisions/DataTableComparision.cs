#region

using HBD.Data.Comparisons.Base;
using HBD.Framework;
using HBD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#endregion

namespace HBD.Data.Comparisons
{
    public class DataTableComparision : IDataTableComparision
    {
        public DataTableComparision(DataTable table, DataTable compareTable)
        {
            Guard.ArgumentIsNotNull(table, nameof(table));
            Guard.ArgumentIsNotNull(compareTable, nameof(compareTable));

            Table = table;
            CompareTable = compareTable;

            CompareColumns = new CompareColumnCollection(this);
            CompareKeys = new CompareColumnCollection(this);
            DataTableExpressionRender = new DataTableExpressionRender();
        }

        protected IConditionRender DataTableExpressionRender { get; }

        protected CompareColumnCollection CompareColumns { get; }
        protected CompareColumnCollection CompareKeys { get; }
        public DataTable Table { get; }
        public DataTable CompareTable { get; }

        public ICompareColumnInfo Column(string columnName)
        {
            Guard.ArgumentIsNotNull(columnName, nameof(columnName));

            if (!Table.Columns.Contains(columnName))
                throw new ArgumentException($"Column {columnName} is not found in Table.");

            return CompareColumns.Add(columnName, null);
        }

        public IDataTableComparision Columns(IEnumerable<ICompareColumnInfo> columns)
        {
            if (columns != null)
                CompareColumns.AddRange(columns);

            return this;
        }

        public ICompareColumnInfo PrimaryKey(string keyName)
        {
            if (!Table.Columns.Contains(keyName))
                throw new ArgumentException($"Column {keyName} is not found in Table.");

            return CompareKeys.Add(keyName, null);
        }

        public IDataTableComparision PrimaryKeys(IEnumerable<ICompareColumnInfo> keys)
        {
            if (keys != null)
                CompareKeys.AddRange(keys);

            return this;
        }

        public virtual IComparisionResult Execute()
        {
            if (Table == CompareTable) return null;
            Validate();

            if (CompareColumns.Count <= 0)
                for (var i = 0; i < Table.Columns.Count && i < CompareTable.Columns.Count; i++)
                    CompareColumns.Add(Table.Columns[i].ColumnName, CompareTable.Columns[i].ColumnName);

            if (CompareKeys.Count > 0)
                return CompareWithPrimaryKeys();
            return CompareWithColumns();
        }

        public virtual object GetValue(ICompareColumnInfo column, DataRow row)
        {
            if (row.Table == Table) return row[column.Column];
            return row[column.CompareColumn];
        }

        public void Dispose()
        {
            Table?.Dispose();
            CompareTable?.Dispose();
        }

        private void Validate()
        {
            //Validate Compare columns.
            for (var i = 0; i < CompareColumns.Count; i++)
            {
                var col = CompareColumns[i];
                if (!Table.Columns.Contains(col.Column))
                    throw new Exception($"Column {col.Column} at index {i} isn't existed in Table.");
                if (!Table.Columns.Contains(col.CompareColumn))
                    throw new Exception($"Column {col.CompareColumn} at index {i} isn't existed in CompareTable.");
            }

            //Validate Primary keys.
            for (var i = 0; i < CompareKeys.Count; i++)
            {
                var col = CompareKeys[i];
                if (!Table.Columns.Contains(col.Column))
                    throw new Exception($"Primary Key {col.Column} at index {i} isn't existed in Table.");
                if (!Table.Columns.Contains(col.CompareColumn))
                    throw new Exception($"Primary Key {col.CompareColumn} at index {i} isn't existed in CompareTable.");
            }
        }

        #region Comparison Methods

        protected virtual IComparisionResult CompareWithColumns()
        {
            var result = new ComparisionResult(this);

            for (var i = 0; i < Table.Rows.Count && i < CompareTable.Rows.Count; i++)
            {
                var row = Table.Rows[i];
                var crow = CompareTable.Rows[i];

                result.Rows.Add(new DifferenceRow(row, crow, row.GetDiffrenceCells(crow)));
            }

            CollectNotFoundRows(result);

            return result;
        }

        protected virtual IComparisionResult CompareWithPrimaryKeys()
        {
            var result = new ComparisionResult(this);

            foreach (DataRow row in Table.Rows)
            {
                var compareRows = FindRowsByPrimaryKeys(row, CompareTable);
                var foundRow = row.FindMostEqualsRow(compareRows, CompareColumns);

                if (foundRow != null) result.Rows.Add(foundRow);
            }

            CollectNotFoundRows(result);

            return result;
        }

        protected virtual DataRow[] FindRowsByPrimaryKeys(DataRow row, DataTable compareTable)
        {
            Guard.ArgumentIsNotNull(CompareKeys, nameof(CompareKeys));

            ICondition filter = null;
            foreach (var col in CompareKeys)
            {
                var con = new ValueCondition(col.Column, CompareOperation.Equals, GetValue(col, row));
                filter = filter == null ? con : filter.And(con);
            }

            return compareTable.Select(DataTableExpressionRender.BuildCondition(filter));
        }

        protected virtual void CollectNotFoundRows(IComparisionResult result)
        {
            result.NotFoundRows.Rows.AddRange(Table.Select().Where(r => !result.Rows.ContainsRow(r)));
            result.NotFoundRows.CompareRows.AddRange(CompareTable.Select()
                .Where(r => !result.Rows.ContainsCompareRow(r)));
        }

        #endregion Comparison Methods
    }
}
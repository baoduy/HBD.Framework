using HBD.Framework.Core;
using HBD.Framework.Data.Csv;
using HBD.Framework.Data.Csv.Base;
using HBD.Framework.Data.Excel;
using System;
using System.Data;

namespace HBD.Framework
{
    public static class DataTableExtensions
    {
        /// <summary>
        ///     Set AllowDBNull = true to all columns in a DataTable.
        /// </summary>
        /// <param name="this"></param>
        public static DataTable AllowDbNull(this DataTable @this)
        {
            if (@this == null) return null;
            foreach (DataColumn column in @this.Columns)
                column.AllowDBNull = true;
            return @this;
        }

        /// <summary>
        ///     Set AllowDBNull = false to all columns in a DataTable.
        /// </summary>
        /// <param name="this"></param>
        public static DataTable NotAllowDbNull(this DataTable @this)
        {
            if (@this == null) return null;
            foreach (DataColumn column in @this.Columns)
                column.AllowDBNull = false;
            return @this;
        }

        //public static ValueRepleacementItem Replace(this DataTable @this, string key)
        //    => new DataTableValueReplacementJob(@this).Replace(key);

        public static DataColumn AddAutoIncrement(this DataColumnCollection @this, string columnName = null)
        {
            Guard.ArgumentIsNotNull(@this, "DataColumnCollection");
            if (columnName.IsNullOrEmpty()) columnName = ExcelAdapter.GetColumnLabel(@this.Count);

            var col = new DataColumn(columnName, typeof(int)) { AutoIncrement = true };
            @this.Add(col);
            return col;
        }

        public static void AddMoreColumns(this DataTable @this, int expectedColumns, ColumnNamingType namingType = ColumnNamingType.FieldType)
            => @this.Columns.AddMoreColumns(expectedColumns, namingType);

        public static void AddMoreColumns(this DataColumnCollection @this, int expectedColumns,
            ColumnNamingType namingType = ColumnNamingType.FieldType)
        {
            Guard.ArgumentIsNotNull(@this, nameof(@this));
            if (@this.Count >= expectedColumns) return;

            for (var i = @this.Count; i < expectedColumns; i++)
            {
                string name;

                switch (namingType)
                {
                    case ColumnNamingType.ExcelType:
                        name = ExcelAdapter.GetColumnLabel(i);
                        break;

                    case ColumnNamingType.FieldType:
                    case ColumnNamingType.Auto:
                        name = $"F{i + 1}";
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(namingType), namingType, null);
                }
                @this.Add(name, typeof(object));
            }
        }

        #region DataFiles

        public static DataTable LoadFromCsv(this DataTable @this, string fileName, Action<ReadCsvOption> options = null)
        {
            new CsvAdapter(fileName).ReadInto(@this, options);
            return @this;
        }

        public static void SaveToCsv(this DataTable @this, string fileName, Action<WriteCsvOption> options = null)
            => new CsvAdapter(fileName).Write(@this, options);

        #endregion DataFiles
    }

    public enum ColumnNamingType
    {
        Auto = 0,

        /// <summary>
        /// Column Name: A, B, C,...
        /// </summary>
        ExcelType = 1,

        /// <summary>
        /// Column Name: F1, F2, F3,...
        /// </summary>
        FieldType = 2
    }
}
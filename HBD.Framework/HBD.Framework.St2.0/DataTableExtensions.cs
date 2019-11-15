#region using

using System;
using System.Data;
using HBD.Framework.Core;
using HBD.Framework.Data.GetSetters;
using HBD.Framework.Extensions;

#endregion

namespace HBD.Framework
{
#if !NETSTANDARD1_6
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

        public static DataColumn AddAutoIncrement(this DataColumnCollection @this, string columnName = null)
        {
            Guard.ArgumentIsNotNull(@this, "DataColumnCollection");
            if (columnName.IsNullOrEmpty()) columnName = CommonFunctions.GetExcelColumnName(@this.Count);

            var col = new DataColumn(columnName, typeof(int)) {AutoIncrement = true};
            @this.Add(col);
            return col;
        }

        public static void AddMoreColumns(this DataTable @this, int expectedColumns,
            ColumnNamingType namingType = ColumnNamingType.FieldType)
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
                        name = CommonFunctions.GetExcelColumnName(i);
                        break;

                    case ColumnNamingType.FieldType:
                    case ColumnNamingType.Auto:
                        name = CommonFunctions.GetColumnName(i);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(namingType), namingType, null);
                }
                @this.Add(name, typeof(object));
            }
        }

        public static IGetSetterCollection CreateGetSetter(this DataTable @this)
            => new DataTableGetSetterCollection(@this);
    }
#endif
}
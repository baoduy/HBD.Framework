using HBD.Framework.Data.SqlClient.Base;
using HBD.Framework.Data.SqlClient.Extensions;
using HBD.Framework.Exceptions;
using System;
using System.Data;

namespace HBD.Framework.Randoms
{
    public partial class RandomGenerator
    {
        public static DataTable TableInfo(TableInfo tableInfo, int numberOfRows = 0)
        {
            if (numberOfRows <= 0)
                numberOfRows = Int(10, 100);

            var table = tableInfo.CreateDataTable();

            for (var i = 0; i < numberOfRows; i++)
            {
                var dataRow = table.NewRow();
                RandomRow(tableInfo.Columns, dataRow);
                table.Rows.Add(dataRow);
            }
            return table;
        }

        private static void RandomRow(ColumnInfoCollection columns, DataRow dataRow)
        {
            foreach (var column in columns)
            {
                var val = RandomValue(column);
                if (val.IsNull()) continue;
                dataRow[column.Name] = val;
            }
        }

        internal static object GetPrimaryValue(ColumnInfo column)
        {
            switch (column.DataType)
            {
                case SqlDbType.Int:
                case SqlDbType.BigInt:
                case SqlDbType.SmallInt:
                case SqlDbType.TinyInt:
                    {
                        var value = 0;
                        if (!column.MaxPrimaryKeyValue.IsNull())
                        {
                            value = (int)Convert.ChangeType(column.MaxPrimaryKeyValue, typeof(int));
                            if (value == int.MaxValue)
                                throw new OutOfCapacityException(typeof(int));
                        }
                        value += 1;
                        column.MaxPrimaryKeyValue = value;
                        return value;
                    }
                default:
                    {
                        if (column.MaxPrimaryKeyValue.IsNull())
                        {
                            if (column.MaxLengh <= 0) column.MaxLengh = 100;
                            var key = new string('A', column.MaxLengh > 100 ? 100 : column.MaxLengh);
                            column.MaxPrimaryKeyValue = key;
                            return key;
                        }

                        var values = column.MaxPrimaryKeyValue.ToString().ToCharArray();
                        var index = values.Length - 1;

                        while (index >= 0)
                        {
                            values[index] = (char)(values[index] + 1);
                            if (values[index] <= 'Z') break;
                            if (index == 0) //The worst case all Character is ZZZZZ
                                throw new OutOfCapacityException(column.MaxLengh);
                            values[index] = 'A';
                            index--;
                        }

                        return column.MaxPrimaryKeyValue = new string(values);
                    }
            }
        }

        internal static object RandomValue(ColumnInfo column)
        {
            if (column.IsComputed || column.IsIdentity) return null;
            return column.IsPrimaryKey ? GetPrimaryValue(column) : RandomValue(column.GetRuntimeType());
        }
    }
}

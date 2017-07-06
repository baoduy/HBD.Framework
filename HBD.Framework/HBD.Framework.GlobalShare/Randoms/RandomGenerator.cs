#region using

using System;
using System.Data;
using System.Text;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Randoms
{
    public partial class RandomGenerator
    {
        private const string Chars = "qwertyuiopasdfghjklzxcvbnm QWERTYUIOPASDFGHJKLZXCVBNM 0123456789";
        private static readonly Random Random = new Random();

        public static string String(int length = 0)
        {
            if (length <= 0)
                length = Int(10, byte.MaxValue);

            var sb = new StringBuilder(length);
            for (var i = 0; i < length; i++)
                sb.Append(Chars[Random.Next(0, Chars.Length)]);
            return sb.ToString();
        }

        public static bool Boolean() => Int(0, 2) != 0;

        public static int Int() => Int(int.MinValue, int.MaxValue);

        public static int Int(int max) => Int(int.MinValue, max);

        public static int Int(int min, int max) => min > max ? min : Random.Next(min, max);

        public static double Decimal() => Decimal(double.MinValue, double.MaxValue);

        public static double Decimal(double max) => Decimal(int.MinValue, max);

        public static double Decimal(double min, double max)
            => Math.Round(Random.NextDouble() + Int((int) min, (int) max), 3);

        public static DateTime DateTime() => DateTime(System.DateTime.MinValue, System.DateTime.MaxValue);

        public static DateTime DateTime(DateTime endDate) => DateTime(System.DateTime.MinValue, endDate);

        public static DateTime DateTime(DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate) return startDate;
            var dateTime = new DateTime(Int(startDate.Year, endDate.Year),
                Int(startDate.Month, endDate.Month),
                Int(startDate.Day, endDate.Day),
                Int(startDate.Hour, 24), Int(startDate.Minute, 60),
                Int(startDate.Second, 60));
            return dateTime;
        }

        public static byte[] ByteArray(int length = 0)
        {
            if (length <= 0)
                length = Int(10, byte.MaxValue);

            var buffer = new byte[length];
            Random.NextBytes(buffer);
            return buffer;
        }

#if !NETSTANDARD1_6
        public static DataTable DataTable(DataTable table = null, int numberOfColumn = 0, int numberOfRows = 0)
        {
            if (numberOfRows <= 0)
                numberOfRows = Int(10, 100);

            if (table == null)
                table = new DataTable();

            if (table.Columns.Count == 0)
                GenerateColumns(table, numberOfColumn);

            for (var i = 0; i < numberOfRows; i++)
            {
                var dataRow = table.NewRow();
                RandomRow(table.Columns, dataRow);
                table.Rows.Add(dataRow);
            }
            return table;
        }

        private static void GenerateColumns(DataTable dt, int numberOfColumn = 0)
        {
            if (numberOfColumn <= 0)
                numberOfColumn = Int(2, 51);
            //First Column is AutoIncrement
            var col = dt.Columns.Add(CommonFuncs.GetExcelColumnName(0), typeof(int));
            col.AutoIncrement = true;

            for (var i = 1; i < numberOfColumn; i++)
                dt.Columns.Add(CommonFuncs.GetExcelColumnName(i));
        }

        private static void RandomRow(DataColumnCollection columns, DataRow dataRow)
        {
            Guard.ArgumentIsNotNull(columns, nameof(columns));
            foreach (DataColumn column in columns)
            {
                var val = RandomValue(column);
                if (val.IsNull()) continue;
                dataRow[column] = val;
            }
        }

        internal static object RandomValue(DataColumn column)
            => column.AutoIncrement || column.ReadOnly ? null : RandomValue(column.DataType);

        internal static object RandomValue(Type type)
        {
            if (type == null) type = typeof(string);

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return Boolean();

                case TypeCode.Byte:
                    return Int(byte.MinValue, byte.MaxValue);

                case TypeCode.DateTime:
                    return DateTime(System.DateTime.Parse(System.Data.SqlTypes.SqlDateTime.MinValue.ToString()), System.DateTime.Now);

                case TypeCode.DBNull:
                    return DBNull.Value;

                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return Decimal();

                case TypeCode.Int16:
                    return Int(short.MinValue, short.MaxValue);

                case TypeCode.Int32:
                case TypeCode.Int64:
                    return Int();

                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return Int(ushort.MaxValue);

                case TypeCode.Char:
                    return String(1);

                case TypeCode.SByte:
                    return Int(sbyte.MinValue, sbyte.MaxValue);

                default:
                    return String();
            }
        }

#endif
    }
}
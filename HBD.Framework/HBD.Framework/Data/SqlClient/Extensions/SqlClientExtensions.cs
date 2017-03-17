#region

using System;
using System.Data;
using HBD.Framework.Core;
using HBD.Framework.Data.SqlClient.Base;

#endregion

namespace HBD.Framework.Data.SqlClient
{
    public static class SqlClientExtensions
    {
        public static T GetValue<T>(this IDataReader @this, string columnName)
        {
            Guard.ArgumentIsNotNull(@this, "IDataReader");
            Guard.ArgumentIsNotNull(columnName, "ColumnName");

            var value = @this[columnName];
            return value.ChangeType<T>();
        }

        public static T GetValue<T>(this IDataReader @this, int columnIndex)
        {
            Guard.ArgumentIsNotNull(@this, "IDataReader");

            var value = @this[columnIndex];
            return value.ChangeType<T>();
        }

        public static SqlDbType ToSqlDbType(this Type @this)
        {
            switch (Type.GetTypeCode(@this))
            {
                case TypeCode.Boolean:
                    return SqlDbType.Bit;

                case TypeCode.Char:
                    return SqlDbType.Char;

                case TypeCode.SByte:
                case TypeCode.Byte:
                    return SqlDbType.Binary;

                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return SqlDbType.Int;

                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return SqlDbType.BigInt;

                case TypeCode.Single:
                    return SqlDbType.Float;

                case TypeCode.Double:
                case TypeCode.Decimal:
                    return SqlDbType.Decimal;

                case TypeCode.DateTime:
                    return SqlDbType.DateTime;

                case TypeCode.String:
                    return SqlDbType.NVarChar;

                case TypeCode.Empty:
                case TypeCode.Object:
                case TypeCode.DBNull:
                default:
                    return SqlDbType.VarBinary;
            }
        }

        public static Type ToRuntimeType(SqlDbType sqlType)
        {
            switch (sqlType)
            {
                case SqlDbType.BigInt:
                    return typeof(long);

                case SqlDbType.Binary:
                case SqlDbType.Image:
                case SqlDbType.Timestamp:
                case SqlDbType.VarBinary:
                    return typeof(byte[]);

                case SqlDbType.Bit:
                    return typeof(bool);

                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.VarChar:
                case SqlDbType.Xml:
                    return typeof(string);

                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                case SqlDbType.Date:
                case SqlDbType.Time:
                case SqlDbType.DateTime2:
                    return typeof(DateTime);

                case SqlDbType.Decimal:
                case SqlDbType.Money:
                case SqlDbType.SmallMoney:
                    return typeof(decimal);

                case SqlDbType.Float:
                    return typeof(double);

                case SqlDbType.Int:
                    return typeof(int);

                case SqlDbType.Real:
                    return typeof(float);

                case SqlDbType.UniqueIdentifier:
                    return typeof(Guid);

                case SqlDbType.SmallInt:
                    return typeof(short);

                case SqlDbType.TinyInt:
                    return typeof(byte);

                case SqlDbType.DateTimeOffset:
                    return typeof(DateTimeOffset);

                //case SqlDbType.UserDefinedTableType:
                //    return typeof(DataTable);

                //case SqlDbType.UserDefinedDataType:
                //case SqlDbType.UserDefinedType:
                case SqlDbType.Variant:
                default:
                    return typeof(object);
            }
        }

        public static SqlDbType ToSqlDbType(this string @this)
        {
            if (@this.IsNullOrEmpty()) return SqlDbType.NVarChar;

            switch (@this)
            {
                case "bigint":
                    return SqlDbType.BigInt;
                case "binary":
                    return SqlDbType.Binary;
                case "bit":
                    return SqlDbType.Bit;
                case "char":
                    return SqlDbType.Char;
                case "date":
                    return SqlDbType.Date;
                case "datetime":
                    return SqlDbType.DateTime;
                case "datetime2":
                    return SqlDbType.DateTime2;
                case "datetimeoffset":
                    return SqlDbType.DateTimeOffset;
                case "decimal":
                    return SqlDbType.Decimal;
                case "float":
                    return SqlDbType.Float;
                case "image":
                    return SqlDbType.Image;
                case "int":
                    return SqlDbType.Int;
                case "money":
                    return SqlDbType.Money;
                case "nchar":
                    return SqlDbType.NChar;
                case "ntext":
                    return SqlDbType.NText;
                case "numeric":
                    return SqlDbType.Decimal;
                case "nvarchar":
                    return SqlDbType.NVarChar;
                case "real":
                    return SqlDbType.Real;
                case "rowversion":
                    return SqlDbType.Binary;
                case "smalldatetime":
                    return SqlDbType.SmallDateTime;
                case "smallint":
                    return SqlDbType.SmallInt;
                case "smallmoney":
                    return SqlDbType.SmallMoney;
                case "text":
                    return SqlDbType.Text;
                case "time":
                    return SqlDbType.Time;
                case "timestamp":
                    return SqlDbType.Timestamp;
                case "tinyint":
                    return SqlDbType.TinyInt;
                case "uniqueidentifier":
                    return SqlDbType.UniqueIdentifier;
                case "varbinary":
                    return SqlDbType.VarBinary;
                case "varchar":
                    return SqlDbType.VarChar;
                default:
                    return SqlDbType.VarBinary;
            }
        }

        public static Type GetRuntimeType(this ColumnInfo @this) => ToRuntimeType(@this.DataType);
    }
}
#region

using System;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

#endregion

namespace HBD.Framework.Data.Excel
{
    public static class ExcelExtensions
    {
        public static CellValues GetCellValues(this Type @this)
        {
            if (@this == null) return CellValues.String;
            switch (Type.GetTypeCode(@this))
            {
                case TypeCode.Boolean:
                    return CellValues.Boolean;

                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return CellValues.Number;

                case TypeCode.DateTime:
                    return CellValues.Date;

                case TypeCode.Empty:
                case TypeCode.Object:
                case TypeCode.Char:
                case TypeCode.DBNull:
                case TypeCode.String:
                default:
                    return CellValues.String;
            }
        }

        public static string GetNumberFormatCode(this Stylesheet @this, int formatId)
        {
            switch (formatId)
            {
                case 0:
                    return "General";

                case 1:
                    return "0";

                case 2:
                    return "0.00";

                case 3:
                    return "#,##0";

                case 4:
                    return "#,##0.00";

                case 9:
                    return "0%";

                case 10:
                    return "0.00%";

                case 11:
                    return "0.00E+00";

                case 12:
                    return "# ?/?";

                case 13:
                    return "# ??/??";

                case 14:
                    return "mm-dd-yy";

                case 15:
                    return "d-mmm-yy";

                case 16:
                    return "d-mmm";

                case 17:
                    return "mmm-yy";

                case 18:
                    return "h:mm AM/PM";

                case 19:
                    return "h:mm:ss AM/PM";

                case 20:
                    return "h:mm";

                case 21:
                    return "h:mm:ss";

                case 22:
                    return "m/d/yy h:mm";

                case 37:
                    return "#,##0 ;(#,##0)";
                case 38:
                    return "#,##0 ;[Red](#,##0)";
                case 39:
                    return "#,##0.00;(#,##0.00)";
                case 40:
                    return "#,##0.00;[Red](#,##0.00)";

                case 44:
                    return "_(\"$\"* #,##0.00_);_(\"$\"* \\(#,##0.00\\);_(\"$\"* \"-\"??_);_(@_)";
                case 45:
                    return "mm:ss";

                case 46:
                    return "[h]:mm:ss";

                case 47:
                    return "mmss.0";

                case 48:
                    return "##0.0E+0";

                case 49:
                    return "@";

                case 27:
                    return "[$-404]e/m/d";

                case 30:
                    return "m/d/yy";

                case 36:
                    return "[$-404]e/m/d";

                case 50:
                    return "[$-404]e/m/d";

                case 57:
                    return "[$-404]e/m/d";

                case 59:
                    return "t0";

                case 60:
                    return "t0.00";

                case 61:
                    return "t#,##0";

                case 62:
                    return "t#,##0.00";

                case 67:
                    return "t0%";

                case 68:
                    return "t0.00%";

                case 69:
                    return "t# ?/?";

                case 70:
                    return "t# ??/??";
            }

            var numberFormat =
                @this?.NumberingFormats.Elements<NumberingFormat>().FirstOrDefault(f => f.NumberFormatId == formatId);
            return numberFormat?.FormatCode?.Value;
        }

        public static Type GetRuntimeType(this EnumValue<CellValues> @this)
        {
            if ((@this == null) || !@this.HasValue)
                return typeof(string);

            switch (@this.Value)
            {
                case CellValues.Boolean:
                    return typeof(bool);

                case CellValues.Date:
                    return typeof(DateTime);

                case CellValues.Number:
                    return typeof(double);

                case CellValues.Error:
                case CellValues.InlineString:
                case CellValues.SharedString:
                case CellValues.String:
                default:
                    return typeof(string);
            }
        }

        public static Type GetRuntimeType(string formatCode)
        {
            if (formatCode.IsNullOrEmpty()) return typeof(string);

            if (formatCode.Contains("d")
                || formatCode.Contains("mm")
                || formatCode.Contains("yy")
                || formatCode.Contains("hh")
                || formatCode.Contains("mm")
                || formatCode.Contains("ss"))
                return typeof(DateTime);

            if (formatCode.Contains("#")
                || formatCode.Contains("0")
                || formatCode.Contains("0.0"))
                return typeof(double);

            return typeof(string);
        }

        public static object GetValue(this Cell @this, WorkbookPart workBookPart)
        {
            if (@this == null) return null;
            var outputType = typeof(string);
            var cellValue = @this.CellValue?.Text ?? string.Empty;

            //Get From Share String Table
            if (@this.DataType != null)
                if (!string.IsNullOrEmpty(cellValue) && (@this.DataType == CellValues.SharedString))
                {
                    var child = workBookPart.SharedStringTablePart.SharedStringTable.ChildElements[int.Parse(cellValue)];
                    cellValue = child.InnerText;
                }
                else outputType = @this.DataType.GetRuntimeType();

            if (@this.StyleIndex?.HasValue ?? false)
            {
                var styleSheet = workBookPart.WorkbookStylesPart.Stylesheet;
                var style = (CellFormat) styleSheet.CellFormats.ChildElements.ElementAt((int) @this.StyleIndex.Value);
                if ((style != null) && cellValue.IsNotNullOrEmpty() && (style.ApplyNumberFormat?.Value ?? false))
                {
                    var numberFormat = styleSheet.GetNumberFormatCode((int) style.NumberFormatId.Value);
                    outputType = GetRuntimeType(numberFormat);
                }
            }

            try
            {
                if (outputType == typeof(DateTime))
                    return DateTime.FromOADate(double.Parse(cellValue));
                if (outputType == typeof(double))
                    return double.Parse(cellValue);
            }
            catch
            {
                // ignored
            }
            return cellValue;
        }

        public static void SetValue(this Cell @this, WorkbookPart workBookPart, object value)
        {
            if (@this == null) return;
            var strValue = value?.ToString() ?? string.Empty;

            if (@this.DataType != null)
                if (@this.DataType == CellValues.SharedString)
                {
                    var child =
                        workBookPart.SharedStringTablePart.SharedStringTable.ChildElements[
                            int.Parse(@this.CellValue.Text)] as SharedStringItem;

                    if (child != null)
                        child.Text = new DocumentFormat.OpenXml.Spreadsheet.Text(strValue);
                }
                else if (@this.DataType == CellValues.InlineString)
                {
                    @this.InlineString = new InlineString(new DocumentFormat.OpenXml.Spreadsheet.Text(strValue));
                }
                else @this.CellValue = new CellValue(strValue);
            else @this.CellValue = new CellValue(strValue);
        }

        internal static string[] SplitByCellReferenceFormat(this string @this)
        {
            if (@this.IsNullOrEmpty()) return new string[] {};
            var chars = @this.Where(c => (c >= 'A') && (c <= 'Z'));
            var numbers = @this.Where(c => (c >= '0') && (c <= '9'));
            return new[] {string.Join(string.Empty, chars), string.Join(string.Empty, numbers)};
        }

        #region WorksheetPart

        //public static DataTable FillSchema(this WorksheetPart @this, DataTable data)
        //    => @this.Worksheet.FillSchema(data);

        //public static DataTable FillSchema(this Worksheet @this, DataTable data)
        //{
        //    foreach (Column col in @this.Descendants<Column>())
        //        data.Columns.Add(ExcelHelper.GetColumnLabel(data.Columns.Count), typeof(object));
        //    return data;
        //}

        /// <summary>
        ///     Get SheetData from WorksheetPart.
        ///     Create new SheetData if not exsisted.
        /// </summary>
        /// <param name="@this">WorksheetPart</param>
        /// <returns>SheetData</returns>
        public static SheetData GetOrCreateSheetData(this WorksheetPart @this)
        {
            var sheetData = @this.GetSheetData();
            if (sheetData == null)
            {
                sheetData = new SheetData();
                @this.Worksheet.Append(sheetData);
            }
            return sheetData;
        }

        /// <summary>
        ///     Get SheetData from WorksheetPart.
        /// </summary>
        /// <param name="@this">WorksheetPart</param>
        /// <returns>SheetData</returns>
        public static SheetData GetSheetData(this WorksheetPart @this)
        {
            if (@this == null) return null;
            return @this.Worksheet.GetFirstChild<SheetData>();
        }

        #endregion WorksheetPart
    }
}
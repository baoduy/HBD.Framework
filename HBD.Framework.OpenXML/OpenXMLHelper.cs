using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data;
using DocumentFormat.OpenXml;

namespace HBD.Framework.OpenXML
{
    public static class OpenXMLHelper
    {
        const string _defaultWorkbookStylesPartID = "rIdStyles";
        const string _defaultFontName = "Calibri";
        const double _defaultFontSize = 11;
        const string _defaultSheetName = "Sheet {0}";

        public static SpreadsheetDocument CreateExcelFile(string fileName)
        {
            var spreadsheet = SpreadsheetDocument.Create(fileName, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);

            spreadsheet.AddWorkbookPart();
            spreadsheet.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

            //(which prevents crashes in Excel 2010)
            spreadsheet.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

            //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
            AddDefaultWorkbookStylesPart(spreadsheet);

            return spreadsheet;
        }

        public static SpreadsheetDocument OpenExcelFile(string fileName, bool isEditable = false)
        {
            return SpreadsheetDocument.Open(fileName, isEditable);
        }

        public static WorksheetPart AddSheet(SpreadsheetDocument spreadsheet, string sheetName)
        {
            if (string.IsNullOrEmpty(sheetName))
                sheetName = string.Format(_defaultSheetName, spreadsheet.WorkbookPart.WorksheetParts.Count() + 1);

            var ID = spreadsheet.WorkbookPart.Parts.Count();
            var workSheetID = "rId" + ID.ToString();
            var worksheetName = sheetName;

            var newWorksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
            newWorksheetPart.Worksheet = new Worksheet();

            // create sheet data
            newWorksheetPart.Worksheet.AppendChild(new SheetData());
            newWorksheetPart.Worksheet.AppendChild(new SheetView());

            var sheets = spreadsheet.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            if (sheets == null)
                sheets = spreadsheet.WorkbookPart.Workbook.AppendChild(new Sheets());

            sheets.AppendChild(new Sheet()
            {
                Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart),
                SheetId = (uint)ID,
                Name = sheetName
            });

            return newWorksheetPart;
        }

        private static WorkbookStylesPart AddDefaultWorkbookStylesPart(SpreadsheetDocument spreadsheet)
        {
            var workbookStylesPart = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>(_defaultWorkbookStylesPartID);
            workbookStylesPart.Stylesheet = GenerateDefaultStyleSheet();
            workbookStylesPart.Stylesheet.Save();
            return workbookStylesPart;
        }

        private static Stylesheet GenerateDefaultStyleSheet()
        {
            return new Stylesheet(
                new Fonts(
                    new Font(                                                               // Index 0 - The default font.
                        new FontSize() { Val = _defaultFontSize },
                        new Color()
                        {
                            Rgb = new HexBinaryValue()
                            {
                                Value = GetRgbColor(System.Drawing.Color.Black)
                            }
                        },
                        new FontName() { Val = _defaultFontName }),
                    new Font(                                                               // Index 1 - The bold font.
                        new Bold(),
                        new FontSize() { Val = _defaultFontSize },
                        new Color()
                        {
                            Rgb = new HexBinaryValue()
                            {
                                Value = GetRgbColor(System.Drawing.Color.Black)
                            }
                        },
                        new FontName() { Val = _defaultFontName }),
                    new Font(                                                               // Index 2 - The Italic font.
                        new Italic(),
                        new FontSize() { Val = _defaultFontSize },
                        new Color()
                        {
                            Rgb = new HexBinaryValue()
                            {
                                Value = GetRgbColor(System.Drawing.Color.Black)
                            }
                        },
                        new FontName() { Val = _defaultFontName }),
                    new Font(                                                               // Index 3 - The Times Roman font. with 16 size
                        new FontSize() { Val = 16 },
                        new Color()
                        {
                            Rgb = new HexBinaryValue()
                            {
                                Value = GetRgbColor(System.Drawing.Color.Black)
                            }
                        },
                        new FontName() { Val = "Times New Roman" })
                ),
                new Fills(
                    new Fill(                                                           // Index 0 - The default fill.
                        new PatternFill() { PatternType = PatternValues.None }),
                    new Fill(                                                           // Index 1 - The default fill of gray 125 (required)
                        new PatternFill() { PatternType = PatternValues.Gray125 }),
                    new Fill(                                                           // Index 2 - The yellow fill.
                        new PatternFill(
                            new ForegroundColor()
                            {
                                Rgb = new HexBinaryValue() { Value = GetRgbColor(System.Drawing.Color.Yellow) }
                            }
                        ) { PatternType = PatternValues.Solid }),
                    new Fill(                                                           // Index 3 - The Red fill.
                        new PatternFill(
                            new ForegroundColor()
                            {
                                Rgb = new HexBinaryValue() { Value = GetRgbColor(System.Drawing.Color.Red) }
                            }
                        ) { PatternType = PatternValues.Solid }),
                    new Fill(                                                           // Index 4 - The Green fill.
                        new PatternFill(
                            new ForegroundColor()
                            {
                                Rgb = new HexBinaryValue() { Value = GetRgbColor(System.Drawing.Color.Green) }
                            }
                        ) { PatternType = PatternValues.Solid })
                ),
                new Borders(
                    new Border(                                                         // Index 0 - The default border.
                        new LeftBorder(),
                        new RightBorder(),
                        new TopBorder(),
                        new BottomBorder(),
                        new DiagonalBorder()),
                    new Border(                                                         // Index 1 - Applies a Left, Right, Top, Bottom border to a cell
                        new LeftBorder(
                            new Color() { Auto = true }
                        ) { Style = BorderStyleValues.Thin },
                        new RightBorder(
                            new Color() { Auto = true }
                        ) { Style = BorderStyleValues.Thin },
                        new TopBorder(
                            new Color() { Auto = true }
                        ) { Style = BorderStyleValues.Thin },
                        new BottomBorder(
                            new Color() { Auto = true }
                        ) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                ),
                new CellFormats(
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 },                        // Index 0 - The default cell style.  If a cell does not have a style index applied it will use this style combination instead
                    new CellFormat() { FontId = 1, FillId = 0, BorderId = 0, ApplyFont = true },      // Index 1 - Bold 
                    new CellFormat() { FontId = 2, FillId = 0, BorderId = 0, ApplyFont = true },      // Index 2 - Italic
                    new CellFormat() { FontId = 3, FillId = 0, BorderId = 0, ApplyFont = true },      // Index 3 - Times Roman
                    new CellFormat() { FontId = 0, FillId = 2, BorderId = 0, ApplyFill = true },      // Index 4 - Yellow Fill
                    new CellFormat(                                                                   // Index 5 - Alignment
                        new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                    ) { FontId = 0, FillId = 0, BorderId = 0, ApplyAlignment = true },
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true },     // Index 6 - Border
                    new CellFormat() { FontId = 0, FillId = 3, BorderId = 0, ApplyFill = true }      // Index 4 - Red Fill
                )
            ); // return
        }

        private static HexBinaryValue GetRgbColor(System.Drawing.Color color)
        {
            return new HexBinaryValue(System.Drawing.ColorTranslator.ToHtml(
                System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B)).Replace("#", string.Empty));
        }

        public static uint AddNewFont(SpreadsheetDocument spreadsheet, double fontSize, System.Drawing.Color color, string fontName = null)
        {
            if (color == null || color.IsEmpty)
                color = System.Drawing.Color.Black;
            if (string.IsNullOrEmpty(fontName))
                fontName = _defaultFontName;

            var workbookStylesPart = spreadsheet.WorkbookPart.GetPartById(_defaultWorkbookStylesPartID) as WorkbookStylesPart;
            if (workbookStylesPart == null)
                workbookStylesPart = AddDefaultWorkbookStylesPart(spreadsheet);

            var rgb = GetRgbColor(color);
            //Try find the existing one
            var currentList = workbookStylesPart.Stylesheet.Fonts.Cast<Font>().ToList();
            var found = currentList.FirstOrDefault(f => f.FontSize != null
                && f.FontSize.Val.Value == fontSize
                && f.FontName != null
                && f.FontName.Val == fontName
                && f.Color != null
                && f.Color.Rgb.Value == rgb.Value);
            if (found != null) return Convert.ToUInt32(currentList.IndexOf(found));

            workbookStylesPart.Stylesheet.Fonts.Append(
                new Font(
                    new FontSize() { Val = fontSize },
                    new Color() { Rgb = rgb },
                    new FontName() { Val = _defaultFontName }
                    ));

            workbookStylesPart.Stylesheet.Save();
            return Convert.ToUInt32(currentList.Count);
        }

        public static uint AddNewFill(SpreadsheetDocument spreadsheet, System.Drawing.Color color)
        {
            if (color == null || color.IsEmpty || color == System.Drawing.Color.Black)
                return (int)DefaultFillStyle.Default;
            else if (color == System.Drawing.Color.Gray)
                return (int)DefaultFillStyle.FillOfGray125;
            else if (color == System.Drawing.Color.Yellow)
                return (int)DefaultFillStyle.YellowFill;
            else if (color == System.Drawing.Color.Red)
                return (int)DefaultFillStyle.RedFill;

            var workbookStylesPart = spreadsheet.WorkbookPart.GetPartById(_defaultWorkbookStylesPartID) as WorkbookStylesPart;
            if (workbookStylesPart == null)
                workbookStylesPart = AddDefaultWorkbookStylesPart(spreadsheet);

            var rgb = GetRgbColor(color);
            //Try find the existing one
            var currentList = workbookStylesPart.Stylesheet.Fills.Cast<Fill>().ToList();
            var found = currentList.FirstOrDefault(f => f.PatternFill != null
                && f.PatternFill.ForegroundColor != null
                && f.PatternFill.ForegroundColor.Rgb != null
                && f.PatternFill.ForegroundColor.Rgb.Value == rgb);
            if (found != null) return Convert.ToUInt32(currentList.IndexOf(found));

            workbookStylesPart.Stylesheet.Fills.Append(
                    new Fill(
                        new PatternFill(
                            new ForegroundColor() { Rgb = rgb }
                        ) { PatternType = PatternValues.Solid }));

            workbookStylesPart.Stylesheet.Save();
            return Convert.ToUInt32(currentList.Count);
        }

        public static uint AddNewCellFormat(SpreadsheetDocument spreadsheet, CellStyle cellStyle)
        {
            if (cellStyle == null || cellStyle.IsEmpty)
                return Convert.ToUInt32((int)DefaultCellStyle.Default);

            var fillID = AddNewFill(spreadsheet, cellStyle.BackgroundColor);
            var fontID = AddNewFont(spreadsheet, _defaultFontSize, cellStyle.ForeColor);

            var workbookStylesPart = spreadsheet.WorkbookPart.GetPartById(_defaultWorkbookStylesPartID) as WorkbookStylesPart;

            //Try find the existing one
            var currentList = workbookStylesPart.Stylesheet.CellFormats.Cast<CellFormat>().ToList();
            var found = currentList.FirstOrDefault(f => f.FillId == fillID && f.FontId == fontID);
            if (found != null) return Convert.ToUInt32(currentList.IndexOf(found));

            workbookStylesPart.Stylesheet.CellFormats.Append(new CellFormat() { FillId = fillID, FontId = fontID, ApplyFont = true, ApplyFill = true });
            workbookStylesPart.Stylesheet.Save();
            return Convert.ToUInt32(currentList.Count);
        }

        private static string GetExcelColumnName(int columnIndex)
        {
            //  Convert a zero-based column index into an Excel column reference  (A, B, C.. Y, Y, AA, AB, AC... AY, AZ, B1, B2..)
            //
            //  eg  GetExcelColumnName(0) should return "A"
            //      GetExcelColumnName(1) should return "B"
            //      GetExcelColumnName(25) should return "Z"
            //      GetExcelColumnName(26) should return "AA"
            //      GetExcelColumnName(27) should return "AB"
            //      ..etc..
            //
            if (columnIndex < 26)
                return ((char)('A' + columnIndex)).ToString();

            char firstChar = (char)('A' + (columnIndex / 26) - 1);
            char secondChar = (char)('A' + (columnIndex % 26));

            return string.Format("{0}{1}", firstChar, secondChar);
        }

        private static Cell CreateTextCell(string cellReference, string cellStringValue)
        {
            //  Add a new Excel Cell to our Row 
            var cell = new Cell() { CellReference = cellReference, DataType = CellValues.String };
            var cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);

            return cell;
        }

        private static Cell CreateNumericCell(string cellReference, string cellStringValue)
        {
            //  Add a new Excel Cell to our Row 
            var cell = new Cell() { CellReference = cellReference };
            var cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            return cell;
        }

        private static void ApplyCellStype(Cell cell, SpreadsheetDocument spreadsheet, CellStyleCollection styles, int rowIndex, int columnIndex)
        {
            if (styles == null)
                return;
            if (cell == null)
                return;

            var format = styles.FirstOrDefault(f => f.RowIndex == rowIndex && f.ColumnIndex == columnIndex);

            if (format == null
                || format.IsEmpty)
                return;

            cell.StyleIndex = AddNewCellFormat(spreadsheet, format);
        }

        private static Type GetSystemType(EnumValue<CellValues> dataType)
        {
            if (!dataType.HasValue)
                return typeof(string);

            switch (dataType.Value)
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
                default: return typeof(string);
            }
        }

        public static void FillDataToWorksheet(WorksheetPart worksheetPart, DataTable data, SpreadsheetDocument spreadsheet = null, CellStyleCollection styles = null)
        {
            if (styles != null && spreadsheet == null)
                throw new ArgumentException("CellStyleCollection is not null. Please provide SpreadsheetDocument instance");

            var worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();

            string cellValue = string.Empty;

            //  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
            //
            //  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
            //  cells of data, we'll know if to write Text values or Numeric cell values.
            int numberOfColumns = data.Columns.Count;
            bool[] IsNumericColumn = new bool[numberOfColumns];

            string[] excelColumnNames = new string[numberOfColumns];
            for (int n = 0; n < numberOfColumns; n++)
                excelColumnNames[n] = OpenXMLHelper.GetExcelColumnName(n);

            //
            //  Create the Header row in our Excel Worksheet
            //
            uint rowIndex = 1;

            var headerRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
            sheetData.Append(headerRow);

            //Add Columns to First Row of Excel file
            for (int colInx = 0; colInx < numberOfColumns; colInx++)
            {
                DataColumn col = data.Columns[colInx];
                var cell = CreateTextCell(excelColumnNames[colInx] + "1", col.ColumnName);
                cell.StyleIndex = (int)DefaultCellStyle.Bold;
                headerRow.Append(cell);
                IsNumericColumn[colInx] = (col.DataType.FullName == "System.Decimal") || (col.DataType.FullName == "System.Int32");

                ApplyCellStype(cell, spreadsheet, styles, Convert.ToInt32(rowIndex), colInx);
            }

            //
            //  Now, step through each row of data in our DataTable...
            //
            double cellNumericValue = 0;
            foreach (DataRow dr in data.Rows)
            {
                // ...create a new row, and append a set of this row's data to it.
                ++rowIndex;
                var newExcelRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
                sheetData.Append(newExcelRow);

                for (int colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    cellValue = dr.ItemArray[colInx].ToString();
                    Cell cell = null;
                    // Create cell with data
                    if (IsNumericColumn[colInx])
                    {
                        //  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                        //  If this numeric value is NULL, then don't write anything to the Excel file.
                        cellNumericValue = 0;
                        if (double.TryParse(cellValue, out cellNumericValue))
                        {
                            cellValue = cellNumericValue.ToString();
                            cell = CreateNumericCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue);

                        }
                    }
                    else
                    {
                        //  For text cells, just write the input data straight out to the Excel file.
                        cell = CreateTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue);
                    }

                    if (cell != null)
                    {
                        ApplyCellStype(cell, spreadsheet, styles, Convert.ToInt32(rowIndex), colInx);
                        newExcelRow.Append(cell);
                    }
                }
            }

            //Save worksheet
            worksheet.Save();
        }

        public static string[] GetSheetNames(SpreadsheetDocument spreadsheet)
        {
            var sheets = spreadsheet.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            return sheets.Descendants<Sheet>().Select(s => s.Name.Value).ToArray();
        }

        public static DataSet ReadAllSheets(SpreadsheetDocument spreadsheet)
        {
            var dataSet = new DataSet();
            foreach (var sheet in spreadsheet.WorkbookPart.WorksheetParts)
            {
                var data = ReadDataFromWorksheet(spreadsheet.WorkbookPart, sheet);
                if (data != null)
                    dataSet.Tables.Add(data);
            }
            return dataSet;
        }

        public static DataTable ReadDataFromWorksheet(SpreadsheetDocument spreadsheet, string sheetName)
        {
            var sheets = spreadsheet.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            var worksheetPartID = sheets.Descendants<Sheet>().FirstOrDefault(s => s.Name.Value.Equals(sheetName, StringComparison.CurrentCultureIgnoreCase)).Id;
            var worksheetPart = spreadsheet.WorkbookPart.GetPartById(worksheetPartID) as WorksheetPart;
            var data = ReadDataFromWorksheet(spreadsheet.WorkbookPart, worksheetPart);
            data.TableName = sheetName;
            return data;
        }

        private static string GetCellValue(this Cell cell, WorkbookPart workBookPart)
        {
            var cellValue = cell.CellValue != null ? cell.CellValue.Text : string.Empty;

            //Get From Share String Table
            if (!string.IsNullOrEmpty(cellValue) && cell.DataType != null && cell.DataType == CellValues.SharedString)
            {
                cellValue = workBookPart.SharedStringTablePart.SharedStringTable.ChildElements[Int32.Parse(cellValue)].InnerText;
            }

            return cellValue;
        }
        public static DataTable ReadDataFromWorksheet(WorkbookPart workBookPart, WorksheetPart worksheetPart)
        {
            if (worksheetPart == null)
                return null;

            var worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();

            var data = new DataTable();
            foreach (var row in sheetData.Elements<Row>())
            {
                int colIndex = 0;

                if (data.Columns.Count == 0)
                {
                    foreach (Cell cell in row)
                    {
                        var cellVal = cell.GetCellValue(workBookPart);
                        data.Columns.Add(!string.IsNullOrEmpty(cellVal) ? cellVal : string.Format("Column {0}", colIndex + 1), GetSystemType(cell.DataType));
                        colIndex++;
                    }
                }
                else
                {

                    var r = data.NewRow();
                    foreach (var cell in row.Descendants<Cell>())
                    {
                        if (colIndex == data.Columns.Count)
                            data.Columns.Add();

                        r[colIndex++] = cell.GetCellValue(workBookPart);
                    }

                    data.Rows.Add(r);
                }
            }

            return data;
        }
    }
}

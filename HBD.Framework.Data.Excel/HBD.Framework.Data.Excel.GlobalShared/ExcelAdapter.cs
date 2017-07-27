#region

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Validation;
using HBD.Framework.Core;
using HBD.Framework.Data.Base;
using HBD.Framework.Data.Excel.Base;
using HBD.Framework.Data.GetSetters;
using HBD.Framework.Exceptions;
using Ap = DocumentFormat.OpenXml.ExtendedProperties;
using BottomBorder = DocumentFormat.OpenXml.Spreadsheet.BottomBorder;
using Color = DocumentFormat.OpenXml.Spreadsheet.Color;
using DifferentialFormats = DocumentFormat.OpenXml.Spreadsheet.DifferentialFormats;
using Fill = DocumentFormat.OpenXml.Spreadsheet.Fill;
using Fonts = DocumentFormat.OpenXml.Spreadsheet.Fonts;
using FontScheme = DocumentFormat.OpenXml.Spreadsheet.FontScheme;
using GradientFill = DocumentFormat.OpenXml.Drawing.GradientFill;
using GradientStop = DocumentFormat.OpenXml.Drawing.GradientStop;
using Hyperlink = DocumentFormat.OpenXml.Drawing.Hyperlink;
using LeftBorder = DocumentFormat.OpenXml.Spreadsheet.LeftBorder;
using Outline = DocumentFormat.OpenXml.Drawing.Outline;
using Path = System.IO.Path;
using PatternFill = DocumentFormat.OpenXml.Spreadsheet.PatternFill;
using RightBorder = DocumentFormat.OpenXml.Spreadsheet.RightBorder;
using TopBorder = DocumentFormat.OpenXml.Spreadsheet.TopBorder;
using Vt = DocumentFormat.OpenXml.VariantTypes;
using WorkbookProperties = DocumentFormat.OpenXml.Spreadsheet.WorkbookProperties;

#endregion

// ReSharper disable VirtualMemberCallInConstructor

namespace HBD.Framework.Data.Excel
{
    /// <summary>
    ///     Excel 2007 and Newer helper class to read/write data table from/to excel file using OpenXML.
    /// </summary>
    public class ExcelAdapter : DataFileAdapterBase
    {
        private SheetInfo[] _sheetNames;

        /// <summary>
        ///     Create Excel Helper.
        /// </summary>
        /// <param name="documentFile">The document location.</param>
        /// <param name="options"></param>
        public ExcelAdapter(string documentFile, Action<ExcelAdapterOption> options = null) : base(documentFile)
        {
            Guard.ArgumentIsNotNull(documentFile, nameof(documentFile));
            if (!IsExcelFile(documentFile))
                throw new ArgumentException($"{documentFile} must be Excel Format.");

            options?.Invoke(Options);
        }

        public SpreadsheetDocument SpreadsheetDocument { get; private set; }
        public WorkbookPart WorkbookPart => SpreadsheetDocument.WorkbookPart;
        private ExcelAdapterOption Options { get; } = new ExcelAdapterOption();

        public SheetInfo[] SheetNames
        {
            get
            {
                if (_sheetNames != null) return _sheetNames;
                _sheetNames = GetSheetNames();
                return _sheetNames;
            }
        }

        public IGetSetterCollection this[int index] => this[SheetNames[index].Name];
        public IGetSetterCollection this[string sheetName] => ReadData(sheetName);

        protected internal virtual void WriteDataToSheetData(SheetData sheetData, DataTable data,
            bool ignoreHeader = false)
        {
            if ((sheetData == null) || (data == null) || (data.Rows.Count <= 0)) return;
            sheetData.RemoveAllChildren();

            if (!ignoreHeader)
            {
                //Header
                var headerRow = new Row();
                for (var i = 0; i < data.Columns.Count; i++)
                {
                    var value = data.Columns[i].ColumnName;
                    var cellReference = GetCellReference(0, i);
                    var cell = CreateCell(cellReference, value, CellValues.String);
                    headerRow.Append(cell);
                }
                sheetData.Append(headerRow);
            }

            //Data
            for (var i = 0; i < data.Rows.Count; i++)
            {
                var row = data.Rows[i];
                var exRow = WriteDataToExcelRow(row, i + 1);
                if (exRow != null)
                    sheetData.Append(exRow);
            }

            //Save worksheet
            ((Worksheet) sheetData.Parent).Save();
        }

        protected internal virtual void WriteDataToWorksheetPart(WorksheetPart worksheetPart, DataTable data,
                bool ignoreHeader = false)
            => WriteDataToSheetData(worksheetPart.GetOrCreateSheetData(), data, ignoreHeader);

        protected virtual string GetNextPartId()
        {
            if (SpreadsheetDocument == null) return null;
            var count = SpreadsheetDocument.Parts.Count();
            if (SpreadsheetDocument.WorkbookPart != null)
                count += SpreadsheetDocument.WorkbookPart.Parts.Count();

            return $"rId{count}";
        }

        protected virtual SheetInfo[] GetSheetNames()
        {
            TryOpenOrCreate();
            var sheets = SpreadsheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            if (sheets == null) return new SheetInfo[] {};
            return sheets.Descendants<Sheet>()
                .Select(s => new SheetInfo(s.Name.Value, (s.State != null) && (s.State != SheetStateValues.Visible)))
                .ToArray();
        }

        protected virtual Row WriteDataToExcelRow(DataRow row, int rowIndex)
        {
            if (row == null) return null;
            var exRow = new Row();

            for (var c = 0; c < row.ItemArray.Length; c++)
            {
                var obj = row.ItemArray[c].IsNull() ? string.Empty : row.ItemArray[c];
                var cellReference = GetCellReference(rowIndex, c);
                var cellType = obj.GetType().GetCellValues();
                var cell = CreateCell(cellReference, obj.ToString(), cellType);
                exRow.Append(cell);
            }
            return exRow;
        }

        #region Constants

        private static readonly string[] SupportedExcelExtensions = {"xlsx", "xlsm", "xltx", "xltm"};
        private const string DefaultFontName = "Calibri";
        private const string TimesNewRomanFontName = "Times New Roman";
        private const double DefaultFontSize = 11;
        private const string DefaultSheetName = "Sheet {0}";

        #endregion Constants

        #region Public Methods

        /// <summary>
        ///     Create New Excel File with AutoSave.
        /// </summary>
        /// <returns></returns>
        public virtual void Create() => Create(SpreadsheetDocumentType.Workbook);

        /// <summary>
        ///     Create New Excel File with AutoSave.
        /// </summary>
        /// <param name="documentType">SpreadsheetDocumentType</param>
        /// <returns></returns>
        protected virtual void Create(SpreadsheetDocumentType documentType)
        {
            CheckOpening();

            Options.OpenMode = OpenMode.Editable;
            SpreadsheetDocument = SpreadsheetDocument.Create(DocumentFile, documentType, true);

            #region Add Defaults Parts

            GenerateWorkbook();
            GenerateThemePart();
            GenerateWorkbookStylesPart();

            if (Options.AddDefaultSheets)
            {
                AddNewSheet();
                AddNewSheet();
                AddNewSheet();
            }

            GenerateExtendedFilePropertiesPart();
            //SetCreationPackageProperties();
            //SetUpdatetionPackageProperties();

            #endregion Add Defaults Parts
        }

        public virtual void TryOpenOrCreate()
        {
            if (SpreadsheetDocument != null) return;

            if (File.Exists(DocumentFile))
                Open();
            else if (Options.AutoCreateNewFile)
                Create();
            else throw new FileNotFoundException(DocumentFile);
        }

        protected virtual void Open()
        {
            CheckOpening();
            SpreadsheetDocument = SpreadsheetDocument.Open(DocumentFile, Options.OpenMode == OpenMode.Editable);
        }

        public override void Save()
        {
            if (Options.OpenMode == OpenMode.ReadOnly)
                throw new Exception("Document is open as read-only.");

            var errors = Validate();
            if (errors?.Count > 0)
                throw new InvalidException(errors.Select(e => e.Description).ToArray());

            WorkbookPart?.Workbook.Save();
        }

        /// <summary>
        ///     Add new blank Sheet into Spreadsheets. If sheetName is null then the new sheet anme is Sheet 1, 2, 3,..., n
        /// </summary>
        /// <returns>SheetId</returns>
        public virtual string AddNewSheet(string sheetName = null)
        {
            TryOpenOrCreate();

            _sheetNames = null; //Refresh SheetName List after added.
            var workbookPart = SpreadsheetDocument.WorkbookPart;
            var sheetId = GetNextPartId();

            #region Add WorksheetPart

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>(sheetId);

            var worksheet = new Worksheet {MCAttributes = new MarkupCompatibilityAttributes {Ignorable = "x14ac"}};
            worksheet.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            worksheet.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            worksheet.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

            var sheetDimension = new SheetDimension {Reference = "A1"};
            var sheetViews = new SheetViews(new SheetView {WorkbookViewId = 0U});
            var sheetFormatProperties = new SheetFormatProperties {DefaultRowHeight = 15D, DyDescent = 0.25D};
            var sheetData = new SheetData();
            var pageMargins = new PageMargins
            {
                Left = 0.7D,
                Right = 0.7D,
                Top = 0.75D,
                Bottom = 0.75D,
                Header = 0.3D,
                Footer = 0.3D
            };

            worksheet.Append(sheetDimension);
            worksheet.Append(sheetViews);
            worksheet.Append(sheetFormatProperties);
            worksheet.Append(sheetData);
            worksheet.Append(pageMargins);

            worksheetPart.Worksheet = worksheet;

            #endregion Add WorksheetPart

            #region Add New Sheet

            var workbook = workbookPart.Workbook;
            var sheets = workbook.Descendants<Sheets>().FirstOrDefault();
            if (sheets == null)
            {
                sheets = new Sheets();
                workbook.Append(sheets);
            }

            if (sheetName.IsNullOrEmpty())
                sheetName = string.Format(DefaultSheetName, sheets.Count() + 1);

            var sheet = new Sheet
            {
                Name = sheetName,
                SheetId = (uint) (sheets.Count() + 1),
                Id = sheetId
            };
            sheets.Append(sheet);

            #endregion Add New Sheet

            return sheet.Name;
        }

        /// <summary>
        ///     Add new Sheet into Spreadsheets with data.
        /// </summary>
        /// <returns>SheetId</returns>
        public virtual string AddNewSheet(DataTable data, bool ignoreHeader = false)
        {
            var sheetName = AddNewSheet(data.TableName);

            WriteDataToWorksheetPart(GetWorksheetByName(sheetName), data, ignoreHeader);

            return sheetName;
        }

        /// <summary>
        ///     Get Data of all sheets
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<IGetSetterCollection> GetAllData()
            => SheetNames.Select(sheetName => ReadData(sheetName.Name));

        /// <summary>
        ///     Get Data of the Sheet
        /// </summary>
        /// <param name="sheetName">If Sheet Name if empty the data of first sheet will be return</param>
        /// <returns></returns>
        public virtual IGetSetterCollection ReadData(string sheetName = null)
        {
            if (sheetName.IsNullOrEmpty())
                sheetName = SheetNames?.FirstOrDefault()?.Name;

            if (sheetName.IsNullOrEmpty()) return null;

            ValidateSheetName(sheetName);
            var sheetData = GetWorksheetByName(sheetName).GetSheetData();
            return new ExcelSheetGetSetter(this, sheetName, sheetData);
        }

        private void ValidateSheetName(string sheetName)
        {
            Guard.ArgumentIsNotNull(sheetName, nameof(sheetName));
            if (SheetNames.All(s => s.Name != sheetName))
                throw new ArgumentOutOfRangeException($"{sheetName} is not existed.");
        }

        /// <summary>
        ///     Update The Sheet with Data.
        ///     DataTable.TableName is SheetName.
        /// </summary>
        /// <param name="data">DataTable</param>
        /// <param name="ignoreHeader"></param>
        public virtual void Update(DataTable data, bool ignoreHeader = false)
        {
            Guard.ArgumentIsNotNull(data, nameof(data));
            Guard.ArgumentIsNotNull(data.TableName, "TableName");
            ValidateSheetName(data.TableName);

            var worksheetPart = GetWorksheetByName(data.TableName);
            WriteDataToWorksheetPart(worksheetPart, data, ignoreHeader);
        }

        public override DataSet ReadData(bool firstRowIsColumnName = true)
        {
            var dataSet = new DataSet();
            foreach (var sheet in GetAllData())
                dataSet.Tables.Add(sheet.ToDataTable(firstRowIsColumnName, ColumnNamingType.ExcelType));
            return dataSet;
        }

        //public virtual DataTable ToDataTable() => this.SheetNames.Length > 0 ? this.ToDataTable(this.SheetNames[0]) : null;

        //public virtual DataTable ToDataTable(int sheetIndex)
        //{
        //    if (sheetIndex < 0 || sheetIndex >= this.SheetNames.Length) return null;
        //    return ToDataTable(this.SheetNames[sheetIndex]);
        //}

        //public virtual DataTable ToDataTable(string sheetName)
        //{
        //    var worksheetPart = GetWorksheetPartByName(sheetName);
        //    if (worksheetPart == null) return null;
        //    var data = ReadDataFromWorksheetPart(worksheetPart);
        //    data.TableName = sheetName;
        //    return data;
        //}

        protected virtual WorksheetPart GetWorksheetByName(string sheetName)
        {
            ValidateSheetName(sheetName);
            var sheets = SpreadsheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            var worksheetPartId =
                sheets.Descendants<Sheet>()?.FirstOrDefault(s => s.Name.Value.EqualsIgnoreCase(sheetName))?.Id;
            return SpreadsheetDocument.WorkbookPart.GetPartById(worksheetPartId) as WorksheetPart;
        }

        protected virtual IList<ValidationErrorInfo> Validate()
        {
            if (SpreadsheetDocument == null) return null;
            var validator = new OpenXmlValidator();
            return validator.Validate(SpreadsheetDocument).ToList();
        }

        protected override void Dispose(bool isDisabling) => SpreadsheetDocument?.Dispose();

        #endregion Public Methods

        #region Static Methods

        /// <summary>
        ///     Get CellIndex from CellReference.
        ///     Input A1 => RowIndex = 0 and ColumnIndex = 0.
        /// </summary>
        /// <param name="cellReference"></param>
        /// <returns></returns>
        public static CellIndex GetCellIndex(string cellReference)
        {
            var splits = cellReference.SplitByCellReferenceFormat();

            try
            {
                var rowIndex = string.IsNullOrEmpty(splits[1]) ? -1 : int.Parse(splits[1]) - 1;
                var colIndex = string.IsNullOrEmpty(splits[0]) ? -1 : CommonFuncs.GetExcelColumnIndex(splits[0]);

                if ((rowIndex < 0) || (colIndex < 0)) return null;
                return new CellIndex(rowIndex, colIndex);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Get CellRegerence from CellIndex.
        ///     CellIndex(0,0) => "A1"
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        public static string GetCellReference(CellIndex cellIndex)
            => cellIndex == null ? string.Empty : GetCellReference(cellIndex.RowIndex, cellIndex.ColumnIndex);

        /// <summary>
        ///     Get Cell Reference-ex: A1,B1...
        /// </summary>
        /// <param name="rowIndex">Row index start from 0</param>
        /// <param name="columnIndex">Column index start from 0</param>
        /// <returns></returns>
        public static string GetCellReference(int rowIndex, int columnIndex)
            => $"{CommonFuncs.GetExcelColumnName(columnIndex)}{rowIndex + 1}";

        /// <summary>
        ///     Create Cell Value.
        /// </summary>
        /// <param name="cellReference">Cell Reference - ex: A1, B1, ...</param>
        /// <param name="value">Value of Cell</param>
        /// <param name="dataType">Data Type of Value</param>
        /// <returns></returns>
        public static Cell CreateCell(string cellReference, string value, EnumValue<CellValues> dataType)
            => new Cell(new CellValue(value)) {CellReference = cellReference, DataType = dataType};

        /// <summary>
        ///     Create Cell.
        /// </summary>
        /// <param name="rowIndex">Row index start from 0</param>
        /// <param name="columnIndex">Column index start from 0</param>
        /// <param name="value">Value of Cell</param>
        /// <param name="dataType">Data Type of Value</param>
        /// <returns></returns>
        public static Cell CreateCell(int rowIndex, int columnIndex, string value, EnumValue<CellValues> dataType)
        {
            var cellReference = GetCellReference(rowIndex, columnIndex);
            return CreateCell(cellReference, value, dataType);
        }

        public static Cell CreateCell(int rowIndex, int columnIndex, object value)
            =>
            CreateCell(rowIndex, columnIndex, value?.ToString(), value?.GetType().GetCellValues() ?? CellValues.String);

        private void GenerateExtendedFilePropertiesPart()
        {
            if (SpreadsheetDocument.ExtendedFilePropertiesPart != null) return;

            var extendedFilePropertiesPart = SpreadsheetDocument.AddNewPart<ExtendedFilePropertiesPart>(GetNextPartId());

            var properties = new Ap.Properties();
            properties.AddNamespaceDeclaration("vt",
                "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes");
            var application = new Ap.Application {Text = "Microsoft Excel"};
            var documentSecurity = new Ap.DocumentSecurity {Text = "0"};
            var scaleCrop1 = new Ap.ScaleCrop {Text = "false"};

            var company = new Ap.Company();
            var linksUpToDate = new Ap.LinksUpToDate {Text = "false"};
            var sharedDocument = new Ap.SharedDocument {Text = "false"};
            var hyperlinksChanged = new Ap.HyperlinksChanged {Text = "false"};
            var applicationVersion = new Ap.ApplicationVersion {Text = "14.0300"};

            var headingPairs = new Ap.HeadingPairs();

            var vTVector = new Vt.VTVector {BaseType = Vt.VectorBaseValues.Variant, Size = 2U};
            var variant1 = new Vt.Variant(new Vt.VTLPSTR {Text = "Worksheets"});
            var variant2 = new Vt.Variant(new Vt.VTInt32 {Text = "1"});

            vTVector.Append(variant1);
            vTVector.Append(variant2);

            headingPairs.Append(vTVector);

            var titlesOfParts = new Ap.TitlesOfParts();
            var vTVector2 = new Vt.VTVector(new Vt.VTLPSTR {Text = GetSheetNames().FirstOrDefault()?.Name})
            {
                BaseType = Vt.VectorBaseValues.Lpstr,
                Size = 1U
            };
            titlesOfParts.Append(vTVector2);

            properties.Append(application);
            properties.Append(documentSecurity);
            properties.Append(scaleCrop1);

            properties.Append(company);
            properties.Append(linksUpToDate);
            properties.Append(sharedDocument);
            properties.Append(hyperlinksChanged);
            properties.Append(applicationVersion);

            properties.Append(headingPairs);

            extendedFilePropertiesPart.Properties = properties;
            properties.Append(titlesOfParts);
        }

        private void GenerateWorkbook()
        {
            if (SpreadsheetDocument.WorkbookPart == null)
                SpreadsheetDocument.AddWorkbookPart();

            var workbookPart = SpreadsheetDocument.WorkbookPart;
            if (workbookPart.Workbook != null) return;

            var workbook = new Workbook();
            workbook.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            var fileVersion = new FileVersion
            {
                ApplicationName = "xl",
                LastEdited = "5",
                LowestEdited = "5",
                BuildVersion = "9303"
            };
            var workbookProperties = new WorkbookProperties {DefaultThemeVersion = 124226U};

            var bookViews = new BookViews();
            var workbookView = new WorkbookView
            {
                XWindow = 480,
                YWindow = 120,
                WindowWidth = 27795U,
                WindowHeight = 12585U
            };
            //var calculationProperties = new CalculationProperties() { CalculationId = 145621U };

            bookViews.Append(workbookView);
            workbook.Append(fileVersion);
            workbook.Append(workbookProperties);
            workbook.Append(bookViews);

            //workbook.Append(calculationProperties);
            workbookPart.Workbook = workbook;
        }

        private WorkbookStylesPart GenerateWorkbookStylesPart()
        {
            var workbookPart = SpreadsheetDocument.WorkbookPart;
            var workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>(GetNextPartId());

            var stylesheet = new Stylesheet {MCAttributes = new MarkupCompatibilityAttributes {Ignorable = "x14ac"}};
            stylesheet.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            stylesheet.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

            var fonts = new Fonts {Count = 1U, KnownFonts = true};

            var font = new Font();
            var fontSize = new FontSize {Val = 11D};
            var color1 = new Color {Theme = 1U};
            var fontName = new FontName {Val = DefaultFontName};
            var fontFamilyNumbering = new FontFamilyNumbering {Val = 2};
            var fontScheme = new FontScheme {Val = FontSchemeValues.Minor};

            font.Append(fontSize);
            font.Append(color1);
            font.Append(fontName);
            font.Append(fontFamilyNumbering);
            font.Append(fontScheme);

            fonts.Append(font);

            var fills = new Fills {Count = 2U};
            var fill1 = new Fill();
            var patternFill = new PatternFill {PatternType = PatternValues.None};

            fill1.Append(patternFill);

            var fill2 = new Fill();
            var patternFill2 = new PatternFill {PatternType = PatternValues.Gray125};

            fill2.Append(patternFill2);

            fills.Append(fill1);
            fills.Append(fill2);

            var borders = new Borders {Count = 1U};

            var border1 = new Border();
            var leftBorder1 = new LeftBorder();
            var rightBorder1 = new RightBorder();
            var topBorder1 = new TopBorder();
            var bottomBorder1 = new BottomBorder();
            var diagonalBorder1 = new DiagonalBorder();

            border1.Append(leftBorder1);
            border1.Append(rightBorder1);
            border1.Append(topBorder1);
            border1.Append(bottomBorder1);
            border1.Append(diagonalBorder1);

            borders.Append(border1);

            var cellStyleFormats1 = new CellStyleFormats {Count = 1U};
            var cellFormat1 = new CellFormat {NumberFormatId = 0U, FontId = 0U, FillId = 0U, BorderId = 0U};

            cellStyleFormats1.Append(cellFormat1);

            var cellFormats1 = new CellFormats {Count = 1U};
            var cellFormat2 = new CellFormat
            {
                NumberFormatId = 0U,
                FontId = 0U,
                FillId = 0U,
                BorderId = 0U,
                FormatId = 0U
            };

            cellFormats1.Append(cellFormat2);

            var cellStyles1 = new CellStyles {Count = 1U};
            var cellStyle1 = new CellStyle {Name = "Normal", FormatId = 0U, BuiltinId = 0U};

            cellStyles1.Append(cellStyle1);
            var differentialFormats = new DifferentialFormats {Count = 0U};
            var tableStyles = new TableStyles
            {
                Count = 0U,
                DefaultTableStyle = "TableStyleMedium2",
                DefaultPivotStyle = "PivotStyleLight16"
            };

            var stylesheetExtensionList = new StylesheetExtensionList();

            var stylesheetExtension = new StylesheetExtension {Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}"};
            stylesheetExtension.AddNamespaceDeclaration("x14",
                "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
            var slicerStyles1 = new SlicerStyles {DefaultSlicerStyle = "SlicerStyleLight1"};

            stylesheetExtension.Append(slicerStyles1);

            stylesheetExtensionList.Append(stylesheetExtension);

            stylesheet.Append(fonts);
            stylesheet.Append(fills);
            stylesheet.Append(borders);
            stylesheet.Append(cellStyleFormats1);
            stylesheet.Append(cellFormats1);
            stylesheet.Append(cellStyles1);
            stylesheet.Append(differentialFormats);
            stylesheet.Append(tableStyles);
            stylesheet.Append(stylesheetExtensionList);

            workbookStylesPart.Stylesheet = stylesheet;
            return workbookStylesPart;
        }

        private ThemePart GenerateThemePart()
        {
            var workbookPart = SpreadsheetDocument.WorkbookPart;
            var themePart = workbookPart.AddNewPart<ThemePart>(GetNextPartId());

            var theme = new Theme {Name = "Office Theme"};
            theme.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            var themeElements1 = new ThemeElements();

            var colorScheme1 = new ColorScheme {Name = "Office"};

            var dark1Color1 = new Dark1Color();
            var systemColor1 = new SystemColor {Val = SystemColorValues.WindowText, LastColor = "000000"};

            dark1Color1.Append(systemColor1);

            var light1Color1 = new Light1Color();
            var systemColor2 = new SystemColor {Val = SystemColorValues.Window, LastColor = "FFFFFF"};

            light1Color1.Append(systemColor2);

            var dark2Color1 = new Dark2Color();
            var rgbColorModelHex1 = new RgbColorModelHex {Val = "1F497D"};

            dark2Color1.Append(rgbColorModelHex1);

            var light2Color1 = new Light2Color();
            var rgbColorModelHex2 = new RgbColorModelHex {Val = "EEECE1"};

            light2Color1.Append(rgbColorModelHex2);

            var accent1Color1 = new Accent1Color();
            var rgbColorModelHex3 = new RgbColorModelHex {Val = "4F81BD"};

            accent1Color1.Append(rgbColorModelHex3);

            var accent2Color1 = new Accent2Color();
            var rgbColorModelHex4 = new RgbColorModelHex {Val = "C0504D"};

            accent2Color1.Append(rgbColorModelHex4);

            var accent3Color1 = new Accent3Color();
            var rgbColorModelHex5 = new RgbColorModelHex {Val = "9BBB59"};

            accent3Color1.Append(rgbColorModelHex5);

            var accent4Color1 = new Accent4Color();
            var rgbColorModelHex6 = new RgbColorModelHex {Val = "8064A2"};

            accent4Color1.Append(rgbColorModelHex6);

            var accent5Color1 = new Accent5Color();
            var rgbColorModelHex7 = new RgbColorModelHex {Val = "4BACC6"};

            accent5Color1.Append(rgbColorModelHex7);

            var accent6Color1 = new Accent6Color();
            var rgbColorModelHex8 = new RgbColorModelHex {Val = "F79646"};

            accent6Color1.Append(rgbColorModelHex8);

            var hyperlink1 = new Hyperlink();
            var rgbColorModelHex9 = new RgbColorModelHex {Val = "0000FF"};

            hyperlink1.Append(rgbColorModelHex9);

            var followedHyperlinkColor1 = new FollowedHyperlinkColor();
            var rgbColorModelHex10 = new RgbColorModelHex {Val = "800080"};

            followedHyperlinkColor1.Append(rgbColorModelHex10);

            colorScheme1.Append(dark1Color1);
            colorScheme1.Append(light1Color1);
            colorScheme1.Append(dark2Color1);
            colorScheme1.Append(light2Color1);
            colorScheme1.Append(accent1Color1);
            colorScheme1.Append(accent2Color1);
            colorScheme1.Append(accent3Color1);
            colorScheme1.Append(accent4Color1);
            colorScheme1.Append(accent5Color1);
            colorScheme1.Append(accent6Color1);
            colorScheme1.Append(hyperlink1);
            colorScheme1.Append(followedHyperlinkColor1);

            var fontScheme2 = new DocumentFormat.OpenXml.Drawing.FontScheme {Name = "Office"};

            var majorFont1 = new MajorFont();
            var latinFont1 = new LatinFont {Typeface = "Cambria"};
            var eastAsianFont1 = new EastAsianFont {Typeface = ""};
            var complexScriptFont1 = new ComplexScriptFont {Typeface = ""};
            var supplementalFont1 = new SupplementalFont {Script = "Jpan", Typeface = "ＭＳ Ｐゴシック"};
            var supplementalFont2 = new SupplementalFont {Script = "Hang", Typeface = "맑은 고딕"};
            var supplementalFont3 = new SupplementalFont {Script = "Hans", Typeface = "宋体"};
            var supplementalFont4 = new SupplementalFont {Script = "Hant", Typeface = "新細明體"};
            var supplementalFont5 = new SupplementalFont {Script = "Arab", Typeface = "Times New Roman"};
            var supplementalFont6 = new SupplementalFont {Script = "Hebr", Typeface = "Times New Roman"};
            var supplementalFont7 = new SupplementalFont {Script = "Thai", Typeface = "Tahoma"};
            var supplementalFont8 = new SupplementalFont {Script = "Ethi", Typeface = "Nyala"};
            var supplementalFont9 = new SupplementalFont {Script = "Beng", Typeface = "Vrinda"};
            var supplementalFont10 = new SupplementalFont {Script = "Gujr", Typeface = "Shruti"};
            var supplementalFont11 = new SupplementalFont {Script = "Khmr", Typeface = "MoolBoran"};
            var supplementalFont12 = new SupplementalFont {Script = "Knda", Typeface = "Tunga"};
            var supplementalFont13 = new SupplementalFont {Script = "Guru", Typeface = "Raavi"};
            var supplementalFont14 = new SupplementalFont {Script = "Cans", Typeface = "Euphemia"};
            var supplementalFont15 = new SupplementalFont {Script = "Cher", Typeface = "Plantagenet Cherokee"};
            var supplementalFont16 = new SupplementalFont {Script = "Yiii", Typeface = "Microsoft Yi Baiti"};
            var supplementalFont17 = new SupplementalFont {Script = "Tibt", Typeface = "Microsoft Himalaya"};
            var supplementalFont18 = new SupplementalFont {Script = "Thaa", Typeface = "MV Boli"};
            var supplementalFont19 = new SupplementalFont {Script = "Deva", Typeface = "Mangal"};
            var supplementalFont20 = new SupplementalFont {Script = "Telu", Typeface = "Gautami"};
            var supplementalFont21 = new SupplementalFont {Script = "Taml", Typeface = "Latha"};
            var supplementalFont22 = new SupplementalFont {Script = "Syrc", Typeface = "Estrangelo Edessa"};
            var supplementalFont23 = new SupplementalFont {Script = "Orya", Typeface = "Kalinga"};
            var supplementalFont24 = new SupplementalFont {Script = "Mlym", Typeface = "Kartika"};
            var supplementalFont25 = new SupplementalFont {Script = "Laoo", Typeface = "DokChampa"};
            var supplementalFont26 = new SupplementalFont {Script = "Sinh", Typeface = "Iskoola Pota"};
            var supplementalFont27 = new SupplementalFont {Script = "Mong", Typeface = "Mongolian Baiti"};
            var supplementalFont28 = new SupplementalFont {Script = "Viet", Typeface = "Times New Roman"};
            var supplementalFont29 = new SupplementalFont {Script = "Uigh", Typeface = "Microsoft Uighur"};
            var supplementalFont30 = new SupplementalFont {Script = "Geor", Typeface = "Sylfaen"};

            majorFont1.Append(latinFont1);
            majorFont1.Append(eastAsianFont1);
            majorFont1.Append(complexScriptFont1);
            majorFont1.Append(supplementalFont1);
            majorFont1.Append(supplementalFont2);
            majorFont1.Append(supplementalFont3);
            majorFont1.Append(supplementalFont4);
            majorFont1.Append(supplementalFont5);
            majorFont1.Append(supplementalFont6);
            majorFont1.Append(supplementalFont7);
            majorFont1.Append(supplementalFont8);
            majorFont1.Append(supplementalFont9);
            majorFont1.Append(supplementalFont10);
            majorFont1.Append(supplementalFont11);
            majorFont1.Append(supplementalFont12);
            majorFont1.Append(supplementalFont13);
            majorFont1.Append(supplementalFont14);
            majorFont1.Append(supplementalFont15);
            majorFont1.Append(supplementalFont16);
            majorFont1.Append(supplementalFont17);
            majorFont1.Append(supplementalFont18);
            majorFont1.Append(supplementalFont19);
            majorFont1.Append(supplementalFont20);
            majorFont1.Append(supplementalFont21);
            majorFont1.Append(supplementalFont22);
            majorFont1.Append(supplementalFont23);
            majorFont1.Append(supplementalFont24);
            majorFont1.Append(supplementalFont25);
            majorFont1.Append(supplementalFont26);
            majorFont1.Append(supplementalFont27);
            majorFont1.Append(supplementalFont28);
            majorFont1.Append(supplementalFont29);
            majorFont1.Append(supplementalFont30);

            var minorFont1 = new MinorFont();
            var latinFont2 = new LatinFont {Typeface = "Calibri"};
            var eastAsianFont2 = new EastAsianFont {Typeface = ""};
            var complexScriptFont2 = new ComplexScriptFont {Typeface = ""};
            var supplementalFont31 = new SupplementalFont {Script = "Jpan", Typeface = "ＭＳ Ｐゴシック"};
            var supplementalFont32 = new SupplementalFont {Script = "Hang", Typeface = "맑은 고딕"};
            var supplementalFont33 = new SupplementalFont {Script = "Hans", Typeface = "宋体"};
            var supplementalFont34 = new SupplementalFont {Script = "Hant", Typeface = "新細明體"};
            var supplementalFont35 = new SupplementalFont {Script = "Arab", Typeface = "Arial"};
            var supplementalFont36 = new SupplementalFont {Script = "Hebr", Typeface = "Arial"};
            var supplementalFont37 = new SupplementalFont {Script = "Thai", Typeface = "Tahoma"};
            var supplementalFont38 = new SupplementalFont {Script = "Ethi", Typeface = "Nyala"};
            var supplementalFont39 = new SupplementalFont {Script = "Beng", Typeface = "Vrinda"};
            var supplementalFont40 = new SupplementalFont {Script = "Gujr", Typeface = "Shruti"};
            var supplementalFont41 = new SupplementalFont {Script = "Khmr", Typeface = "DaunPenh"};
            var supplementalFont42 = new SupplementalFont {Script = "Knda", Typeface = "Tunga"};
            var supplementalFont43 = new SupplementalFont {Script = "Guru", Typeface = "Raavi"};
            var supplementalFont44 = new SupplementalFont {Script = "Cans", Typeface = "Euphemia"};
            var supplementalFont45 = new SupplementalFont {Script = "Cher", Typeface = "Plantagenet Cherokee"};
            var supplementalFont46 = new SupplementalFont {Script = "Yiii", Typeface = "Microsoft Yi Baiti"};
            var supplementalFont47 = new SupplementalFont {Script = "Tibt", Typeface = "Microsoft Himalaya"};
            var supplementalFont48 = new SupplementalFont {Script = "Thaa", Typeface = "MV Boli"};
            var supplementalFont49 = new SupplementalFont {Script = "Deva", Typeface = "Mangal"};
            var supplementalFont50 = new SupplementalFont {Script = "Telu", Typeface = "Gautami"};
            var supplementalFont51 = new SupplementalFont {Script = "Taml", Typeface = "Latha"};
            var supplementalFont52 = new SupplementalFont {Script = "Syrc", Typeface = "Estrangelo Edessa"};
            var supplementalFont53 = new SupplementalFont {Script = "Orya", Typeface = "Kalinga"};
            var supplementalFont54 = new SupplementalFont {Script = "Mlym", Typeface = "Kartika"};
            var supplementalFont55 = new SupplementalFont {Script = "Laoo", Typeface = "DokChampa"};
            var supplementalFont56 = new SupplementalFont {Script = "Sinh", Typeface = "Iskoola Pota"};
            var supplementalFont57 = new SupplementalFont {Script = "Mong", Typeface = "Mongolian Baiti"};
            var supplementalFont58 = new SupplementalFont {Script = "Viet", Typeface = "Arial"};
            var supplementalFont59 = new SupplementalFont {Script = "Uigh", Typeface = "Microsoft Uighur"};
            var supplementalFont60 = new SupplementalFont {Script = "Geor", Typeface = "Sylfaen"};

            minorFont1.Append(latinFont2);
            minorFont1.Append(eastAsianFont2);
            minorFont1.Append(complexScriptFont2);
            minorFont1.Append(supplementalFont31);
            minorFont1.Append(supplementalFont32);
            minorFont1.Append(supplementalFont33);
            minorFont1.Append(supplementalFont34);
            minorFont1.Append(supplementalFont35);
            minorFont1.Append(supplementalFont36);
            minorFont1.Append(supplementalFont37);
            minorFont1.Append(supplementalFont38);
            minorFont1.Append(supplementalFont39);
            minorFont1.Append(supplementalFont40);
            minorFont1.Append(supplementalFont41);
            minorFont1.Append(supplementalFont42);
            minorFont1.Append(supplementalFont43);
            minorFont1.Append(supplementalFont44);
            minorFont1.Append(supplementalFont45);
            minorFont1.Append(supplementalFont46);
            minorFont1.Append(supplementalFont47);
            minorFont1.Append(supplementalFont48);
            minorFont1.Append(supplementalFont49);
            minorFont1.Append(supplementalFont50);
            minorFont1.Append(supplementalFont51);
            minorFont1.Append(supplementalFont52);
            minorFont1.Append(supplementalFont53);
            minorFont1.Append(supplementalFont54);
            minorFont1.Append(supplementalFont55);
            minorFont1.Append(supplementalFont56);
            minorFont1.Append(supplementalFont57);
            minorFont1.Append(supplementalFont58);
            minorFont1.Append(supplementalFont59);
            minorFont1.Append(supplementalFont60);

            fontScheme2.Append(majorFont1);
            fontScheme2.Append(minorFont1);

            var formatScheme1 = new FormatScheme {Name = "Office"};

            var fillStyleList1 = new FillStyleList();

            var solidFill1 = new SolidFill();
            var schemeColor1 = new SchemeColor {Val = SchemeColorValues.PhColor};

            solidFill1.Append(schemeColor1);

            var gradientFill1 = new GradientFill {RotateWithShape = true};

            var gradientStopList1 = new GradientStopList();

            var gradientStop1 = new GradientStop {Position = 0};

            var schemeColor2 = new SchemeColor {Val = SchemeColorValues.PhColor};
            var tint1 = new Tint {Val = 50000};
            var saturationModulation1 = new SaturationModulation {Val = 300000};

            schemeColor2.Append(tint1);
            schemeColor2.Append(saturationModulation1);

            gradientStop1.Append(schemeColor2);

            var gradientStop2 = new GradientStop {Position = 35000};

            var schemeColor3 = new SchemeColor {Val = SchemeColorValues.PhColor};
            var tint2 = new Tint {Val = 37000};
            var saturationModulation2 = new SaturationModulation {Val = 300000};

            schemeColor3.Append(tint2);
            schemeColor3.Append(saturationModulation2);

            gradientStop2.Append(schemeColor3);

            var gradientStop3 = new GradientStop {Position = 100000};

            var schemeColor4 = new SchemeColor {Val = SchemeColorValues.PhColor};
            var tint3 = new Tint {Val = 15000};
            var saturationModulation3 = new SaturationModulation {Val = 350000};

            schemeColor4.Append(tint3);
            schemeColor4.Append(saturationModulation3);

            gradientStop3.Append(schemeColor4);

            gradientStopList1.Append(gradientStop1);
            gradientStopList1.Append(gradientStop2);
            gradientStopList1.Append(gradientStop3);
            var linearGradientFill1 = new LinearGradientFill {Angle = 16200000, Scaled = true};

            gradientFill1.Append(gradientStopList1);
            gradientFill1.Append(linearGradientFill1);

            var gradientFill2 = new GradientFill {RotateWithShape = true};

            var gradientStopList2 = new GradientStopList();

            var gradientStop4 = new GradientStop {Position = 0};

            var schemeColor5 = new SchemeColor {Val = SchemeColorValues.PhColor};
            var shade1 = new Shade {Val = 51000};
            var saturationModulation4 = new SaturationModulation {Val = 130000};

            schemeColor5.Append(shade1);
            schemeColor5.Append(saturationModulation4);

            gradientStop4.Append(schemeColor5);

            var gradientStop5 = new GradientStop {Position = 80000};

            var schemeColor6 = new SchemeColor {Val = SchemeColorValues.PhColor};
            var shade2 = new Shade {Val = 93000};
            var saturationModulation5 = new SaturationModulation {Val = 130000};

            schemeColor6.Append(shade2);
            schemeColor6.Append(saturationModulation5);

            gradientStop5.Append(schemeColor6);

            var gradientStop6 = new GradientStop {Position = 100000};

            var schemeColor7 = new SchemeColor {Val = SchemeColorValues.PhColor};
            var shade3 = new Shade {Val = 94000};
            var saturationModulation6 = new SaturationModulation {Val = 135000};

            schemeColor7.Append(shade3);
            schemeColor7.Append(saturationModulation6);

            gradientStop6.Append(schemeColor7);

            gradientStopList2.Append(gradientStop4);
            gradientStopList2.Append(gradientStop5);
            gradientStopList2.Append(gradientStop6);
            var linearGradientFill2 = new LinearGradientFill {Angle = 16200000, Scaled = false};

            gradientFill2.Append(gradientStopList2);
            gradientFill2.Append(linearGradientFill2);

            fillStyleList1.Append(solidFill1);
            fillStyleList1.Append(gradientFill1);
            fillStyleList1.Append(gradientFill2);

            var lineStyleList1 = new LineStyleList();

            var outline1 = new Outline
            {
                Width = 9525,
                CapType = LineCapValues.Flat,
                CompoundLineType = CompoundLineValues.Single,
                Alignment = PenAlignmentValues.Center
            };

            var solidFill2 = new SolidFill();

            var schemeColor8 = new SchemeColor {Val = SchemeColorValues.PhColor};
            var shade4 = new Shade {Val = 95000};
            var saturationModulation7 = new SaturationModulation {Val = 105000};

            schemeColor8.Append(shade4);
            schemeColor8.Append(saturationModulation7);

            solidFill2.Append(schemeColor8);
            var presetDash1 = new PresetDash {Val = PresetLineDashValues.Solid};

            outline1.Append(solidFill2);
            outline1.Append(presetDash1);

            var outline2 = new Outline
            {
                Width = 25400,
                CapType = LineCapValues.Flat,
                CompoundLineType = CompoundLineValues.Single,
                Alignment = PenAlignmentValues.Center
            };

            var solidFill3 = new SolidFill();
            var schemeColor9 = new SchemeColor {Val = SchemeColorValues.PhColor};

            solidFill3.Append(schemeColor9);
            var presetDash2 = new PresetDash {Val = PresetLineDashValues.Solid};

            outline2.Append(solidFill3);
            outline2.Append(presetDash2);

            var outline3 = new Outline
            {
                Width = 38100,
                CapType = LineCapValues.Flat,
                CompoundLineType = CompoundLineValues.Single,
                Alignment = PenAlignmentValues.Center
            };

            var solidFill4 = new SolidFill();
            var schemeColor10 = new SchemeColor {Val = SchemeColorValues.PhColor};

            solidFill4.Append(schemeColor10);
            var presetDash3 = new PresetDash {Val = PresetLineDashValues.Solid};

            outline3.Append(solidFill4);
            outline3.Append(presetDash3);

            lineStyleList1.Append(outline1);
            lineStyleList1.Append(outline2);
            lineStyleList1.Append(outline3);

            var effectStyleList1 = new EffectStyleList();

            var effectStyle1 = new EffectStyle();

            var effectList1 = new EffectList();

            var outerShadow1 = new OuterShadow
            {
                BlurRadius = 40000L,
                Distance = 20000L,
                Direction = 5400000,
                RotateWithShape = false
            };

            var rgbColorModelHex11 = new RgbColorModelHex {Val = "000000"};
            var alpha1 = new Alpha {Val = 38000};

            rgbColorModelHex11.Append(alpha1);

            outerShadow1.Append(rgbColorModelHex11);

            effectList1.Append(outerShadow1);

            effectStyle1.Append(effectList1);

            var effectStyle2 = new EffectStyle();

            var effectList2 = new EffectList();

            var outerShadow2 = new OuterShadow
            {
                BlurRadius = 40000L,
                Distance = 23000L,
                Direction = 5400000,
                RotateWithShape = false
            };

            var rgbColorModelHex12 = new RgbColorModelHex {Val = "000000"};
            var alpha2 = new Alpha {Val = 35000};

            rgbColorModelHex12.Append(alpha2);

            outerShadow2.Append(rgbColorModelHex12);

            effectList2.Append(outerShadow2);

            effectStyle2.Append(effectList2);

            var effectStyle3 = new EffectStyle();

            var effectList3 = new EffectList();

            var outerShadow3 = new OuterShadow
            {
                BlurRadius = 40000L,
                Distance = 23000L,
                Direction = 5400000,
                RotateWithShape = false
            };

            var rgbColorModelHex13 = new RgbColorModelHex {Val = "000000"};
            var alpha3 = new Alpha {Val = 35000};

            rgbColorModelHex13.Append(alpha3);

            outerShadow3.Append(rgbColorModelHex13);

            effectList3.Append(outerShadow3);

            var scene3DType1 = new Scene3DType();

            var camera1 = new Camera {Preset = PresetCameraValues.OrthographicFront};
            var rotation1 = new Rotation {Latitude = 0, Longitude = 0, Revolution = 0};

            camera1.Append(rotation1);

            var lightRig1 = new LightRig {Rig = LightRigValues.ThreePoints, Direction = LightRigDirectionValues.Top};
            var rotation2 = new Rotation {Latitude = 0, Longitude = 0, Revolution = 1200000};

            lightRig1.Append(rotation2);

            scene3DType1.Append(camera1);
            scene3DType1.Append(lightRig1);

            var shape3DType1 = new Shape3DType();
            var bevelTop1 = new BevelTop {Width = 63500L, Height = 25400L};

            shape3DType1.Append(bevelTop1);

            effectStyle3.Append(effectList3);
            effectStyle3.Append(scene3DType1);
            effectStyle3.Append(shape3DType1);

            effectStyleList1.Append(effectStyle1);
            effectStyleList1.Append(effectStyle2);
            effectStyleList1.Append(effectStyle3);

            var backgroundFillStyleList1 = new BackgroundFillStyleList();

            var solidFill5 = new SolidFill();
            var schemeColor11 = new SchemeColor {Val = SchemeColorValues.PhColor};

            solidFill5.Append(schemeColor11);

            var gradientFill3 = new GradientFill {RotateWithShape = true};

            var gradientStopList3 = new GradientStopList();

            var gradientStop7 = new GradientStop {Position = 0};

            var schemeColor12 = new SchemeColor {Val = SchemeColorValues.PhColor};
            var tint4 = new Tint {Val = 40000};
            var saturationModulation8 = new SaturationModulation {Val = 350000};

            schemeColor12.Append(tint4);
            schemeColor12.Append(saturationModulation8);

            gradientStop7.Append(schemeColor12);

            var gradientStop8 = new GradientStop {Position = 40000};

            var schemeColor13 = new SchemeColor {Val = SchemeColorValues.PhColor};
            var tint5 = new Tint {Val = 45000};
            var shade5 = new Shade {Val = 99000};
            var saturationModulation9 = new SaturationModulation {Val = 350000};

            schemeColor13.Append(tint5);
            schemeColor13.Append(shade5);
            schemeColor13.Append(saturationModulation9);

            gradientStop8.Append(schemeColor13);

            var gradientStop9 = new GradientStop {Position = 100000};

            var schemeColor14 = new SchemeColor {Val = SchemeColorValues.PhColor};
            var shade6 = new Shade {Val = 20000};
            var saturationModulation10 = new SaturationModulation {Val = 255000};

            schemeColor14.Append(shade6);
            schemeColor14.Append(saturationModulation10);

            gradientStop9.Append(schemeColor14);

            gradientStopList3.Append(gradientStop7);
            gradientStopList3.Append(gradientStop8);
            gradientStopList3.Append(gradientStop9);

            var pathGradientFill1 = new PathGradientFill {Path = PathShadeValues.Circle};
            var fillToRectangle1 = new FillToRectangle {Left = 50000, Top = -80000, Right = 50000, Bottom = 180000};

            pathGradientFill1.Append(fillToRectangle1);

            gradientFill3.Append(gradientStopList3);
            gradientFill3.Append(pathGradientFill1);

            var gradientFill4 = new GradientFill {RotateWithShape = true};

            var gradientStopList4 = new GradientStopList();

            var gradientStop10 = new GradientStop {Position = 0};

            var schemeColor15 = new SchemeColor {Val = SchemeColorValues.PhColor};
            var tint6 = new Tint {Val = 80000};
            var saturationModulation11 = new SaturationModulation {Val = 300000};

            schemeColor15.Append(tint6);
            schemeColor15.Append(saturationModulation11);

            gradientStop10.Append(schemeColor15);

            var gradientStop11 = new GradientStop {Position = 100000};

            var schemeColor16 = new SchemeColor {Val = SchemeColorValues.PhColor};
            var shade7 = new Shade {Val = 30000};
            var saturationModulation12 = new SaturationModulation {Val = 200000};

            schemeColor16.Append(shade7);
            schemeColor16.Append(saturationModulation12);

            gradientStop11.Append(schemeColor16);

            gradientStopList4.Append(gradientStop10);
            gradientStopList4.Append(gradientStop11);

            var pathGradientFill2 = new PathGradientFill {Path = PathShadeValues.Circle};
            var fillToRectangle2 = new FillToRectangle {Left = 50000, Top = 50000, Right = 50000, Bottom = 50000};

            pathGradientFill2.Append(fillToRectangle2);

            gradientFill4.Append(gradientStopList4);
            gradientFill4.Append(pathGradientFill2);

            backgroundFillStyleList1.Append(solidFill5);
            backgroundFillStyleList1.Append(gradientFill3);
            backgroundFillStyleList1.Append(gradientFill4);

            formatScheme1.Append(fillStyleList1);
            formatScheme1.Append(lineStyleList1);
            formatScheme1.Append(effectStyleList1);
            formatScheme1.Append(backgroundFillStyleList1);

            themeElements1.Append(colorScheme1);
            themeElements1.Append(fontScheme2);
            themeElements1.Append(formatScheme1);
            var objectDefaults1 = new ObjectDefaults();
            var extraColorSchemeList1 = new ExtraColorSchemeList();

            theme.Append(themeElements1);
            theme.Append(objectDefaults1);
            theme.Append(extraColorSchemeList1);

            themePart.Theme = theme;
            return themePart;
        }

        //private void SetCreationPackageProperties()
        //{
        //    SpreadsheetDocument.PackageProperties.Creator = UserPrincipalHelper.UserName;
        //    SpreadsheetDocument.PackageProperties.Created = DateTime.Now;
        //}

        //private void SetUpdatetionPackageProperties()
        //{
        //    SpreadsheetDocument.PackageProperties.Modified = DateTime.Now;
        //    SpreadsheetDocument.PackageProperties.LastModifiedBy = UserPrincipalHelper.UserName;
        //}

        //protected void ApplyCellStype(Cell cell, CellStyleCollection styles, int rowIndex, int columnIndex)
        //{
        //    if (cell == null || styles == null) return;

        //    var format = styles.FirstOrDefault(f => f.RowIndex == rowIndex && f.ColumnIndex == columnIndex);
        //    if (format == null || format.IsEmpty()) return;

        //    cell.StyleIndex = AddNewCellFormat(this.SpreadsheetDocument, format);
        //}

        protected static bool IsExcelFile(string documentFile)
        {
            if (documentFile.IsNullOrEmpty()) return false;
            var ext = Path.GetExtension(documentFile);
            // ReSharper disable once PossibleNullReferenceException
            return !ext.IsNullOrEmpty() && SupportedExcelExtensions.AnyIgnoreCase(ext.Replace(".", string.Empty));
        }

        protected void CheckOpening()
        {
            if (SpreadsheetDocument != null)
                throw new Exception(
                    $"SpreadsheetDocument {SpreadsheetDocument.WorkbookPart.Uri} is opening cannot create the new SpreadsheetDocument");
        }

        public override void WriteData(DataSet data, bool ignoreHeader = true)
        {
            foreach (DataTable table in data.Tables)
                WriteData(table, ignoreHeader);
        }

        /// <summary>
        ///     Add or Update Data Table to Excel File.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ignoreHeader"></param>
        public virtual void WriteData(DataTable data, bool ignoreHeader = true)
        {
            string name = null;
            var emptySheet = GetFirstEmptySheet();

            if (data.TableName.IsNullOrEmpty() || SheetNames.All(n => !n.Name.EqualsIgnoreCase(data.TableName)))
                name = emptySheet.IsNullOrEmpty() ? null : emptySheet;
            else name = SheetNames.FirstOrDefault(n => n.Name.EqualsIgnoreCase(data.TableName))?.Name;

            if (name.IsNullOrEmpty())
                AddNewSheet(data, ignoreHeader);
            else WriteDataToWorksheetPart(GetWorksheetByName(emptySheet), data, ignoreHeader);
        }

        private string GetFirstEmptySheet() => SheetNames.Where(n =>
        {
            var sheet = GetWorksheetByName(n.Name);
            var data = sheet.GetOrCreateSheetData();
            return data.InnerText.IsNullOrEmpty();
        }).Select(n => n.Name).FirstOrDefault();

        
        #endregion Static Methods
    }
}
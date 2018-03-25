#region

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml.Spreadsheet;
using HBD.Framework.Data.Csv;
using HBD.Framework.Data.Excel.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Data.Excel.Tests
{
    [TestClass]
    public class ExcelHelperTests
    {
        private DataTable _data;

        [TestInitialize]
        public void Initializer()
        {
            foreach (var f in Directory.GetFiles("TestData\\", "The*.xlsx"))
                File.Delete(f);

            var random = new Random();
            _data = new DataTable();
            _data.Columns.Add("A");
            _data.Columns.Add("B");
            _data.Columns.Add("C");

            for (var i = 0; i < 10; i++)
            {
                var row = _data.NewRow();
                row[0] = random.Next();
                row[1] = random.Next();
                row[2] = random.Next();
                _data.Rows.Add(row);
            }
        }

        [TestCleanup]
        public void CleanUp()
        {
            //foreach (var f in Directory.GetFiles("TestData\\", "The New*.xlsx"))
            //    File.Delete(f);
            _data.Dispose();
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void SheetInfo_IsHidden_Test()
        {
            using (var con = new ExcelAdapter("TestData\\DataBaseInfo.xlsx"))
            {
                Assert.IsTrue(con.SheetNames.First(s => s.Name == "HiddenSheet").IsHidden);
                Assert.IsTrue(con.SheetNames.Any(s => !s.IsHidden));
            }
        }

        #region Protected Static Methods

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void IsExcelFile_Test()
        {
            var priType = new PrivateType(typeof(ExcelAdapter));

            Assert.IsTrue((bool) priType.InvokeStatic("IsExcelFile", BindingFlags.NonPublic, "C:\\A.xlsx"));
            Assert.IsTrue((bool) priType.InvokeStatic("IsExcelFile", BindingFlags.NonPublic, "C:\\A.xlsm"));
            Assert.IsTrue((bool) priType.InvokeStatic("IsExcelFile", BindingFlags.NonPublic, "C:\\A.xltx"));
            Assert.IsTrue((bool) priType.InvokeStatic("IsExcelFile", BindingFlags.NonPublic, "C:\\A.xltm"));

            Assert.IsFalse((bool) priType.InvokeStatic("IsExcelFile", BindingFlags.NonPublic, "C:\\A"));
            Assert.IsFalse((bool) priType.InvokeStatic("IsExcelFile", BindingFlags.NonPublic, "C:\\"));
            Assert.IsFalse((bool) priType.InvokeStatic("IsExcelFile", BindingFlags.NonPublic, "C:\\A.xl"));
            Assert.IsFalse((bool) priType.InvokeStatic("IsExcelFile", BindingFlags.NonPublic, string.Empty));
            Assert.IsFalse((bool) priType.InvokeStatic("IsExcelFile", BindingFlags.NonPublic, (string) null));
        }

        //[TestMethod()]
        //[TestCategory("Fw.Data.ExcelAdapter")]
        //public void CreateTextCell_Test()
        //{
        //    var priType = new PrivateType(typeof(ExcelAdapter));

        //    var cell = priType.InvokeStatic("CreateTextCell", BindingFlags.NonPublic, "AA", "Test") as Cell;
        //    Assert.IsNotNull(cell);
        //    Assert.IsTrue(cell.CellReference == "AA");
        //    Assert.IsTrue(cell.CellValue.InnerText == "Test");
        //    Assert.IsTrue(cell.DataType == CellValues.String);
        //}

        //[TestMethod()]
        //[TestCategory("Fw.Data.ExcelAdapter")]
        //public void CreateNumericCell_Test()
        //{
        //    var priType = new PrivateType(typeof(ExcelAdapter));

        //    var cell = priType.InvokeStatic("CreateNumericCell", BindingFlags.NonPublic, "A", "123") as Cell;
        //    Assert.IsNotNull(cell);
        //    Assert.IsTrue(cell.CellReference == "A");
        //    Assert.IsTrue(cell.CellValue.InnerText == "123");
        //    Assert.IsTrue(cell.DataType != CellValues.String);
        //}

        #endregion Protected Static Methods

        #region Common Test-Cases

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        [ExpectedException(typeof(ArgumentException))]
        public void ExcelHelper_Exception_Test()
        {
            using (var con = new ExcelAdapter("TestData\\2015 Weekly Calendar.xtx"))
            {
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void ExcelHelper_AutoOpen_Test()
        {
            using (var con = new ExcelAdapter("TestData\\2015 Weekly Calendar.xlsx"))
            {
                Assert.IsTrue(con.SheetNames.Length > 0);
                Assert.IsNotNull(con.DocumentFile == "TestData\\2015 Weekly Calendar.xlsx");
                Assert.IsNotNull(con.SpreadsheetDocument);
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void ExcelHelper_NotAutoOpen_Test()
        {
            using (var con = new ExcelAdapter("TestData\\2015 Weekly Calendar.xlsx", op => op.AutoCreateNewFile = false)
            )
            {
                Assert.IsNotNull(con.DocumentFile == "TestData\\2015 Weekly Calendar.xlsx");
                Assert.IsNull(con.SpreadsheetDocument);
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void GetCellIndex_ReturnNull()
        {
            Assert.IsNull(ExcelAdapter.GetCellIndex("AA"));
            Assert.IsNull(ExcelAdapter.GetCellIndex("123"));
            Assert.IsNull(ExcelAdapter.GetCellIndex(""));
            Assert.IsNull(ExcelAdapter.GetCellIndex(null));
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void GetCellIndex_Test()
        {
            var cellIndex = ExcelAdapter.GetCellIndex("");
            Assert.IsNull(cellIndex);

            cellIndex = ExcelAdapter.GetCellIndex("A1");
            Assert.IsNotNull(cellIndex);
            Assert.AreEqual(cellIndex.RowIndex, 0);
            Assert.AreEqual(cellIndex.ColumnIndex, 0);

            cellIndex = ExcelAdapter.GetCellIndex("B2");
            Assert.IsNotNull(cellIndex);
            Assert.AreEqual(cellIndex.RowIndex, 1);
            Assert.AreEqual(cellIndex.ColumnIndex, 1);

            cellIndex = ExcelAdapter.GetCellIndex("Z100");
            Assert.IsNotNull(cellIndex);
            Assert.AreEqual(cellIndex.RowIndex, 99);
            Assert.AreEqual(cellIndex.ColumnIndex, 25);

            cellIndex = ExcelAdapter.GetCellIndex("Z90");
            Assert.IsNotNull(cellIndex);
            Assert.AreEqual(cellIndex.RowIndex, 89);
            Assert.AreEqual(cellIndex.ColumnIndex, 25);
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void GetCellReference_ReturnNull_Test()
        {
            var reference = ExcelAdapter.GetCellReference(null);
            Assert.IsTrue(reference.IsNullOrEmpty());
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void GetCellReference_Test()
        {
            var reference = ExcelAdapter.GetCellReference(new CellIndex(0, 0));
            Assert.IsNotNull(reference);
            Assert.AreEqual(reference, "A1");

            reference = ExcelAdapter.GetCellReference(new CellIndex(1, 1));
            Assert.IsNotNull(reference);
            Assert.AreEqual(reference, "B2");

            reference = ExcelAdapter.GetCellReference(new CellIndex(99, 25));
            Assert.IsNotNull(reference);
            Assert.AreEqual(reference, "Z100");
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void CreateNew_Default_Test()
        {
            const string fileName = "TestData\\The New Default File.xlsx";
            using (var con = new ExcelAdapter(fileName))
            {
                con.Create();
                Assert.IsNotNull(con.SpreadsheetDocument);
                con.Save();
            }

            Assert.IsTrue(File.Exists(fileName));

            using (var con = new ExcelAdapter(fileName))
            {
                Assert.IsTrue(con.SheetNames.Length >= 3);
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void CreateNew_NoSheet_Test()
        {
            const string fileName = "TestData\\The New Non Sheets File.xlsx";
            using (var con = new ExcelAdapter(fileName, op =>
            {
                op.AutoCreateNewFile = false;
                op.AddDefaultSheets = false;
            }))
            {
                con.Create();
                Assert.IsNotNull(con.SpreadsheetDocument);
            }

            Assert.IsTrue(File.Exists(fileName));

            using (var con = new ExcelAdapter(fileName))
            {
                Assert.AreEqual(con.SheetNames.Length, 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void AddNewSheet_WithDataTable()
        {
            const string fileName = "TestData\\The New Data File.xlsx";
            using (var con = new ExcelAdapter(fileName, op =>
            {
                op.AutoCreateNewFile = true;
                op.OpenMode = OpenMode.Editable;
            }))
            {
                var name = con.AddNewSheet(_data);
                Assert.IsTrue(con.SheetNames.Length > 3);

                var sheetData = con[name].ToDataTable(true);
                Assert.IsNotNull(sheetData);
                Assert.IsTrue(sheetData.Rows.Count == _data.Rows.Count);
                Assert.IsTrue(sheetData.Columns.Count == _data.Columns.Count);
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public string UpdateSheet_WithDataTable()
        {
            const string fileName = "TestData\\The Update Data File.xlsx";
            using (var con = new ExcelAdapter(fileName))
            {
                var sheetName = con.SheetNames.First();
                _data.TableName = sheetName.Name;
                con.Update(_data);

                var sheetData = con[sheetName.Name].ToDataTable();
                Assert.IsNotNull(sheetData);
                Assert.IsTrue(sheetData.Rows.Count == _data.Rows.Count);
                Assert.IsTrue(sheetData.Columns.Count == _data.Columns.Count);
            }
            return fileName;
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void GetNextPartId_WithoutCreate_ReturnNull_Test()
        {
            using (var con = new ExcelAdapter("TestData\\The New File.xlsx", op => op.AutoCreateNewFile = false))
            {
                var priObj = new PrivateObject(con);
                Assert.IsNull(priObj.Invoke("GetNextPartId"));
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CreateNew_AddNewSheet_WithoutCreate_Exception_Test()
        {
            using (var con = new ExcelAdapter("TestData\\The New File.xlsx", op => op.AutoCreateNewFile = false))
            {
                con.AddNewSheet();
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void CreateNew_1Sheet_Test()
        {
            const string fileName = "TestData\\The New 1 Sheets File.xlsx";
            using (var con = new ExcelAdapter(fileName, op =>
            {
                op.OpenMode = OpenMode.Editable;
                op.AutoCreateNewFile = false;
                op.AddDefaultSheets = false;
            }))
            {
                con.Create();
                con.AddNewSheet();

                Assert.IsNotNull(con.SpreadsheetDocument);
            }

            Assert.IsTrue(File.Exists(fileName));

            using (var con = new ExcelAdapter(fileName))
            {
                Assert.AreEqual(con.SheetNames.Length, 1);
            }
        }

        //[TestMethod()]
        //[TestCategory("Fw.Data.ExcelAdapter")]
        //public void ToDataTable_WithNagativeIndex_ReturnNull_Test()
        //{
        //    using (var con = new ExcelAdapter("TestData\\2015 Weekly Calendar.xlsx"))
        //    using (var data = con.ToDataTable(-1))
        //    {
        //        Assert.IsNull(data);
        //    }
        //}

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void ToDataTable_Test()
        {
            using (var con = new ExcelAdapter("TestData\\2015 Weekly Calendar.xlsx"))
            {
                using (var table = con[0].ToDataTable(false, columnNamingType: ColumnNamingType.ExcelType))
                {
                    //Validate Data
                    ValidateCalendarData(table);
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void ToDataSet_Test()
        {
            using (var con = new ExcelAdapter("TestData\\2015 Weekly Calendar.xlsx"))
            {
                using (var tb = con.ReadData(false))
                {
                    Assert.IsNotNull(tb);
                    Assert.IsTrue(tb.Tables.Count > 0);
                    //Validate Data
                    ValidateCalendarData(tb.Tables[0]);
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void ToDataTable_SheetName_Test()
        {
            using (var con = new ExcelAdapter("TestData\\2015 Weekly Calendar.xlsx"))
            {
                using (var tb = con["2015 Weekly Calendar"].ToDataTable(columnNamingType:ColumnNamingType.ExcelType))
                {
                    //Validate Data
                    ValidateCalendarData(tb);
                }
            }
        }

        private static void ValidateCalendarData(DataTable data)
        {
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Columns.Count > 0);
            Assert.IsTrue(data.Rows.Count > 0);

            Assert.AreEqual(data.Rows[0][1] as string, "2015 Weekly Calendar");
            Assert.AreEqual(data.Rows[1]["B"] as string,
                "This Excel calendar is blank & designed for easy use as a planner.  Courtesy of WinCalendar");
            Assert.AreEqual((DateTime) data.Rows[15]["K"], new DateTime(2015, 1, 29));
            Assert.AreEqual((DateTime) data.Rows[159]["O"], new DateTime(2016, 1, 2));
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void ReadDataFromEmptyHeader_Test()
        {
            using (var con = new ExcelAdapter("TestData\\The Empty SheetData.xlsx", op => op.AutoCreateNewFile = true))
            {
                for (var i = 0; i < _data.Columns.Count; i++)
                    _data.Rows[0][i] = string.Empty;

                var name = con.AddNewSheet(_data, true);
                var dt = con[name].ToDataTable(true);
                Assert.AreEqual(_data.Rows.Count - 1, dt.Rows.Count);
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void WriteDataToSheetData_WithNullData_ReturnNull_Test()
        {
            using (var con = new ExcelAdapter("TestData\\2015 Weekly Calendar.xlsx", op => op.AutoCreateNewFile = false)
            )
            {
                var priObj = new PrivateObject(con);
                Assert.IsNull(priObj.Invoke("WriteDataToSheetData", (SheetData) null, _data, true));
                Assert.IsNull(priObj.Invoke("WriteDataToSheetData", (SheetData) null, (DataTable) null, true));
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void WriteDataToExcelRow_WithNullData_ReturnNull_Test()
        {
            using (var con = new ExcelAdapter("TestData\\2015 Weekly Calendar.xlsx", op => op.AutoCreateNewFile = false)
            )
            {
                var priObj = new PrivateObject(con);
                Assert.IsNull(priObj.Invoke("WriteDataToExcelRow", (DataRow) null, 0));
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void Write_Data_To_Excel_Test()
        {
            using (var dt = new CsvAdapter("TestData\\Customers.csv").Read())
            {
                using (var con = new ExcelAdapter("TestData\\The_TestOf_WriteData_Customers.xlsx", op =>
                {
                    op.OpenMode = OpenMode.Editable;
                    op.AddDefaultSheets = false;
                }))
                {
                    con.AddNewSheet(dt);
                    con.Save();
                }

                using (var con = new ExcelAdapter("TestData\\The_TestOf_WriteData_Customers.xlsx"))
                {
                    using (var newtb = con.ReadData(dt.TableName).ToDataTable(true))
                    {
                        Assert.IsTrue(dt.Rows.Count == newtb.Rows.Count);
                        Assert.IsTrue(dt.Columns.Count == newtb.Columns.Count);
                    }
                }
            }
        }

        #endregion Common Test-Cases
    }
}
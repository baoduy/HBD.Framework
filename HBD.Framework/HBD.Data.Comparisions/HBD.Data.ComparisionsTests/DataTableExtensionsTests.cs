#region

using HBD.Data.Comparisons;
using HBD.Data.Comparisons.Base;
using HBD.Framework.IO;
using HBD.Framework.Randoms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;

#endregion

namespace HBD.Framework.Tests
{
    [TestClass]
    public class DataTableExtensionsTests
    {
        [TestInitialize]
        public void Initializer()
        {
            DirectoryEx.DeleteFiles("TestData\\", "Test*.csv");
        }

        private static DataTable CreateTable() => RandomGenerator.DataTable();

        //[TestMethod]
        //[TestCategory("Fw.DataTableExtensions")]
        //public void QuoteValue_Empty_Test()
        //{
        //    var priType = new PrivateType(typeof(DataTableExtensions));

        //    Assert.AreEqual(string.Empty, priType.InvokeStatic("QuoteValue", BindingFlags.NonPublic, (object)null));
        //    Assert.AreEqual(string.Empty, priType.InvokeStatic("QuoteValue", BindingFlags.NonPublic, string.Empty));
        //}

        //[TestMethod]
        //[TestCategory("Fw.DataTableExtensions")]
        //public void QuoteValue_WithComma_Test()
        //{
        //    var priType = new PrivateType(typeof(DataTableExtensions));

        //    var a = priType.InvokeStatic("QuoteValue", "A,B");
        //    Assert.IsTrue(a?.ToString().StartsWith("\"")==true && a?.ToString().EndsWith("\"")==true);
        //}

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void IsEquals_WithTheSameNullDataRow_ReturnTrue_Test()
        {
            Assert.IsTrue(((DataRow)null).IsEquals(null));
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void IsEquals_WithTheSameDataRow_ReturnTrue_Test()
        {
            var db = CreateTable();
            var row = db.Rows[0];

            Assert.IsTrue(row.IsEquals(row));
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void IsEquals_TheSameDataRowData_ReturnTrue_Test()
        {
            var db = CreateTable();
            var db1 = db.Copy();

            var row = db.Rows[0];
            var row1 = db1.Rows[0];

            var columns = new[]
            {
                new CompareColumnInfo(db.Columns[0].ColumnName),
                new CompareColumnInfo(db.Columns[1].ColumnName)
            };

            Assert.IsTrue(row.IsEquals(row1, columns));
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void IsEquals_DifferenceDataRowData_ReturnFalse_Test()
        {
            var db = CreateTable();
            var db1 = db.Copy();

            var row = db.Rows[0];
            var row1 = db1.Rows[0];
            row1[1] = "AAAA";

            var columns = new[]
            {
                new CompareColumnInfo(db.Columns[0].ColumnName),
                new CompareColumnInfo(db.Columns[1].ColumnName)
            };

            Assert.IsFalse(row.IsEquals(row1, columns));
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void AddAutoIncrementTest()
        {
            var tb = new DataTable();
            tb.Columns.AddAutoIncrement();
            Assert.IsTrue(tb.Columns[0].ColumnName.IsNotNullOrEmpty());
            Assert.IsTrue(tb.Columns[0].DataType == typeof(int));
            Assert.IsTrue(tb.Columns[0].AutoIncrement);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void AddMoreColumnsTest()
        {
            var tb = new DataTable();
            tb.Columns.AddMoreColumns(12);
            Assert.IsTrue(tb.Columns.Count == 12);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddMoreColumns_WithNullColumnCollection_Test()
        {
            ((DataColumnCollection)null).AddMoreColumns(12);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void LoadFromCSV_Customers_FirstRowIsHeader_Test()
        {
            var tb = new DataTable();
            tb.LoadFromCsv("TestData\\Customers.csv", op => op.FirstRowIsHeader = true);
            Assert.IsTrue(tb.Columns.Count == 11);
            Assert.IsTrue(tb.Rows.Count == 91);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void LoadFromCSV_Customers_FirstRowIsNotHeader_Test()
        {
            var tb = new DataTable();
            tb.LoadFromCsv("TestData\\Customers.csv", op => op.FirstRowIsHeader = false);
            Assert.IsTrue(tb.Columns.Count == 11);
            Assert.IsTrue(tb.Rows.Count == 92);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void LoadFromCSV_Employees_Test()
        {
            var tb = new DataTable();
            tb.LoadFromCsv("TestData\\Employees.csv", op => op.Dilimiters = new[] { "|" });
            Assert.IsTrue(tb.Columns.Count == 16);
            Assert.IsTrue(tb.Rows.Count == 9);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void LoadFromCSV_NullDataTable_Test()
        {
            var tb = ((DataTable)null).LoadFromCsv("TestData\\Employees.csv", op => op.Dilimiters = new[] { "|" });
            Assert.IsNull(tb);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void LoadFromCSV_CsvFileNotExisted_Test()
        {
            var tb = new DataTable().LoadFromCsv("TestData\\AAA.csv", op => op.Dilimiters = new[] { "|" });
            Assert.IsTrue(tb.Rows.Count == 0);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void SaveToCSV_Test()
        {
            const string file = "TestData\\TestCSV_WithHeader.csv";

            using (var tb = CreateTable())
            {
                tb.SaveToCsv(file);

                using (var newTb = new DataTable())
                {
                    newTb.LoadFromCsv(file);
                    Assert.IsTrue(tb.IsEquals(newTb));
                    Assert.AreEqual(tb.Columns.Count, newTb.Columns.Count);
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void LoadFromCSV_FixedLenghTextFile_Test()
        {
            var tb = new DataTable();
            tb.LoadFromCsv("TestData\\FixedLenghTextFile.txt", op => op.FieldWidths = new[] { 10, 17, 10 });
            Assert.IsTrue(tb.Columns.Count == 3);
            Assert.IsTrue(tb.Rows.Count == 3);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void SaveToCSV_WithHeader_Test()
        {
            var file = "TestData\\TestCSV_WithHeader.csv";

            using (var tb = CreateTable())
            {
                tb.SaveToCsv(file, op => op.IgnoreHeader = false);

                using (var newTb = new DataTable())
                {
                    newTb.LoadFromCsv(file);

                    Assert.IsTrue(tb.IsEquals(newTb));
                    Assert.AreEqual(tb.Columns.Count, newTb.Columns.Count);
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void SaveToCSV_WithFixedWidths_SingleWidth_Test()
        {
            const string file = "TestData\\TestCSV_WithFixedWidths.csv";

            using (var tb = CreateTable())
            {
                var list = new List<int>();
                for (var i = 0; i < tb.Columns.Count; i++)
                    list.Add(300);

                tb.SaveToCsv(file, op =>
                {
                    op.IgnoreHeader = false;
                    op.FieldWidths = new[] { 300 };
                });

                using (var newTb = new DataTable())
                {
                    newTb.LoadFromCsv(file, op => op.FieldWidths = list.ToArray());
                    Assert.IsTrue(tb.IsEquals(newTb));
                    Assert.AreEqual(tb.Columns.Count, newTb.Columns.Count);
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void SaveToCSV_WithFixedWidths_Test()
        {
            const string file = "TestData\\TestCSV_WithFixedWidths.csv";

            using (var tb = CreateTable())
            {
                var list = new List<int>();
                for (var i = 0; i < tb.Columns.Count; i++)
                    list.Add(300);

                tb.SaveToCsv(file, op =>
                {
                    op.IgnoreHeader = false;
                    op.FieldWidths = list.ToArray();
                });

                using (var newTb = new DataTable())
                {
                    newTb.LoadFromCsv(file, op => op.FieldWidths = list.ToArray());
                    Assert.IsTrue(tb.IsEquals(newTb));
                    Assert.AreEqual(tb.Columns.Count, newTb.Columns.Count);
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void TwoTables_IsEquals_Test()
        {
            using (var tb1 = CreateTable())
            {
                using (var tb2 = tb1.Copy())
                {
                    Assert.IsTrue(tb1.IsEquals(tb2));
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void TwoTables_IsNotEquals_Test()
        {
            using (var tb1 = CreateTable())
            {
                using (var tb2 = CreateTable())
                {
                    Assert.IsTrue(tb1.IsNotEquals(tb2));
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void CompareToTest()
        {
            using (var compare = new DataTable().CompareTo(new DataTable()))
            {
                Assert.IsTrue(compare is DataTableComparision);
            }
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void Select_Contains_EmptyString_Test()
        {
            using (var tb = new DataTable().LoadFromCsv("TestData\\Customers.csv"))
            {
                var rows = tb.Select(new ValueCondition("CompanyName", CompareOperation.Contains, ""));
                Assert.AreEqual(rows.Length, tb.Rows.Count);
            }
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void Select_StartWith_Customer_Test()
        {
            using (var tb = new DataTable().LoadFromCsv("TestData\\Customers.csv"))
            {
                var rows = tb.Select(new ValueCondition("CompanyName", CompareOperation.StartsWith, "Alfreds"));
                Assert.IsTrue(rows.Length >= 1);
            }
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void Select_EndWith_Customer_Test()
        {
            using (var tb = new DataTable().LoadFromCsv("TestData\\Customers.csv"))
            {
                var rows = tb.Select(new ValueCondition("CompanyName", CompareOperation.EndsWith, "Taquería"));
                Assert.IsTrue(rows.Length >= 1);
            }
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void Select_Contains_Customer_Test()
        {
            using (var tb = new DataTable().LoadFromCsv("TestData\\Customers.csv"))
            {
                var rows = tb.Select(new ValueCondition("CompanyName", CompareOperation.Contains, "Comidas "));
                Assert.IsTrue(rows.Length >= 1);
            }
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void Select_ContainsAndEquals_Test()
        {
            using (var tb = new DataTable().LoadFromCsv("TestData\\Customers.csv"))
            {
                var rows = tb.Select(new ValueCondition("Address", CompareOperation.Contains, "8")
                                     & new ValueCondition("[CustomerID]", CompareOperation.Equals, "THEBI"));
                Assert.IsTrue(rows.Length >= 1);
            }
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void Select_IsNotNull_Test()
        {
            using (var tb = new DataTable().LoadFromCsv("TestData\\Customers.csv"))
            {
                var rows = tb.Select(new ValueCondition("CompanyName", CompareOperation.NotNull, null));
                Assert.AreEqual(rows.Length, tb.Rows.Count);
            }
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void Select_IsNull_Test()
        {
            using (var tb = new DataTable().LoadFromCsv("TestData\\Customers.csv"))
            {
                tb.Rows[0]["CompanyName"] = DBNull.Value;
                var rows = tb.Select(new ValueCondition("CompanyName", CompareOperation.IsNull, null));
                Assert.AreEqual(1, rows.Length);
            }
        }

        //[TestMethod()]
        //[TestCategory("Fw.DataTableExtensions")]
        //public void Replace_Test()
        //{
        //    using (var tb = new DataTable().LoadFromCsv("TestData\\Customers.csv"))
        //    {
        //        tb.Replace("Anders").WithValue("Hoàng").Execute();
        //        var rows = tb.Select(new ValueCondition("[ContactName]", CompareOperation.Contains, "Hoàng"));
        //        Assert.AreEqual(1, rows.Length);
        //    }
        //}

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void TwoIdentityRows_IsEquals_ReturnTrue_Test()
        {
            var data = new DataTable();
            data.Columns.Add("ID", typeof(int));
            data.Columns.Add("Name", typeof(string));

            data.Rows.Add(1, "Hoang");
            data.Rows.Add(1, "Hoang");

            var row0 = data.Rows[0];
            var row1 = data.Rows[1];

            Assert.IsTrue(row0.IsEquals(row1));
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void TwoDifferenceRows_IsEquals_ReturnFalse_Test()
        {
            var data = new DataTable();
            data.Columns.Add("ID", typeof(int));
            data.Columns.Add("Name", typeof(string));

            data.Rows.Add(1, "Hoang");
            data.Rows.Add(2, "Hoang");

            var row0 = data.Rows[0];
            var row1 = data.Rows[1];

            Assert.IsFalse(row0.IsEquals(row1));
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void GetDiffrenceCellsTest()
        {
            var data = new DataTable();
            data.Columns.Add("ID", typeof(int));
            data.Columns.Add("Name", typeof(string));
            data.Columns.Add("Birthday", typeof(DateTime));

            data.Rows.Add(1, "Hoang", new DateTime(1990, 01, 01));
            data.Rows.Add(1, "Bao", new DateTime(1990, 01, 01));

            var row0 = data.Rows[0];
            var row1 = data.Rows[1];

            var diffcells = row0.GetDiffrenceCells(row1);

            Assert.IsTrue(diffcells.Length > 0);
            Assert.AreEqual(diffcells[0].Column, "Name");
            Assert.AreEqual(diffcells[0].CompareColumn, "Name");
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void FindEqualsRowTest()
        {
            var data = CreateTable();
            var data1 = data.Copy();

            var row0 = data.Rows[0];

            var row = row0.FindEqualsRow(data1.Select());

            Assert.IsNotNull(row);
            Assert.IsTrue(data1.Rows.IndexOf(row) >= 0);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void FindEqualsRow_DataRowNull_Test()
        {
            var data = CreateTable();
            var row = ((DataRow)null).FindEqualsRow(data.Select());
            Assert.IsNull(row);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void FindEqualsRow_CompareDataRowsNull_Test()
        {
            var data = CreateTable();
            var row = data.Rows[0].FindEqualsRow(null);
            Assert.IsNull(row);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void FindEqualsRow_CompareDataRowsEmpty_Test()
        {
            var data = CreateTable();
            var row = data.Rows[0].FindEqualsRow(new DataRow[] { });
            Assert.IsNull(row);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void FindMostEqualsRowTest()
        {
            var data = CreateTable();
            var data1 = data.Copy();

            data.Rows[0][1] = 100;
            var row0 = data.Rows[0];

            var diffRow = row0.FindMostEqualsRow(data1.Select());

            Assert.IsNotNull(diffRow);
            Assert.AreEqual(diffRow.Row, row0);
            Assert.IsTrue(diffRow.Cells.Length == 1);
            Assert.IsTrue(data1.Rows.IndexOf(diffRow.CompareRow) >= 0);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void FindMostEqualsRow_DataRowNull_Test()
        {
            var data = CreateTable();
            var row = ((DataRow)null).FindMostEqualsRow(data.Select());
            Assert.IsNull(row);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void FindMostEqualsRow_CompareDataRowsNull_Test()
        {
            var data = CreateTable();
            var row = data.Rows[0].FindMostEqualsRow(null);
            Assert.IsNull(row);
        }
    }
}
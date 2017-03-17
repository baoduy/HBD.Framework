#region

using System;
using System.Data;
using System.IO;
using HBD.Framework.Data.Csv;
using HBD.Framework.Data.GetSetters;
using HBD.Framework.IO;
using HBD.Framework.Randoms;
using HBD.Framework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        private DataTable CreateTable() => RandomGenerator.DataTable();

        //[TestMethod]
        //[TestCategory("Fw.DataTableExtensions")]
        //public void QuoteValue_Empty_Test()
        //{
        //    var priType = new PrivateType(typeof(DataTableExtensions));

        //    Assert.AreEqual(string.Empty, priType.InvokeStatic("QuoteValue", BindingFlags.NonPublic, (object)null));
        //    Assert.AreEqual(string.Empty, priType.InvokeStatic("QuoteValue", BindingFlags.NonPublic, string.Empty));
        //}

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
            ((DataColumnCollection) null).AddMoreColumns(12);
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
            tb.LoadFromCsv("TestData\\Employees_PileDilimiters.csv", op => op.Dilimiters = new[] {"|"});
            Assert.IsTrue(tb.Columns.Count >= 10);
            Assert.IsTrue(tb.Rows.Count >= 9);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void LoadFromCSV_NullDataTable_Test()
        {
            var tb = ((DataTable) null).LoadFromCsv("TestData\\Employees.csv", op => op.Dilimiters = new[] {"|"});
            Assert.IsNull(tb);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void LoadFromCSV_CsvFileNotExisted_Test()
        {
            var tb = new DataTable().LoadFromCsv("TestData\\AAA.csv", op => op.Dilimiters = new[] {"|"});
            Assert.IsTrue(tb.Rows.Count == 0);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void LoadFromCSV_FixedLenghTextFile_Test()
        {
            var tb = new DataTable();
            tb.LoadFromCsv("TestData\\FixedLenghTextFile.txt", op => op.FieldWidths = new[] {10, 17, 10});
            Assert.IsTrue(tb.Columns.Count == 3);
            Assert.IsTrue(tb.Rows.Count == 3);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void SaveTable_ToCsv_Test()
        {
            const string fileName = "TestDataTable_SaveToCsv.csv";
            var data = CreateTable();
            data.SaveToCsv(fileName);

            Assert.IsTrue(File.Exists(fileName));
            Assert.IsTrue(File.ReadAllLines(fileName).Length > 0);
        }

        [TestMethod]
        [TestCategory("Fw.DataTableExtensions")]
        public void SaveObjects_ToCsv_Test()
        {
            var fileName = "TestObjects_SaveToCsv.csv";
            var data = new[]
            {
                new TestItem {Id = 1, Name = "1", Details = "One"},
                new TestItem {Id = 2, Name = "2", Details = "Two"},
                new TestItem {Id = 3, Name = "3", Details = "Three"},
                new TestItem {Id = 4, Name = "4", Details = "Four"},
                new TestItem {Id = 5, Name = "5", Details = "Five"}
            };

            new CsvAdapter(fileName).Write(new ObjectGetSetterCollection<TestItem>(data));

            Assert.IsTrue(File.Exists(fileName));
            Assert.IsTrue(File.ReadAllLines(fileName).Length > 0);
        }
    }
}
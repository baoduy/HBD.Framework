#region using

using System;
using System.Data;
using HBD.Framework.IO;
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

        //private DataTable CreateTable() => RandomGenerator.DataTable();

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

        
    }
}
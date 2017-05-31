#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using HBD.Data.Comparisons;
using HBD.Data.Comparisons.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Data.Comparision.Tests
{
    [TestClass]
    public class DataTableComparisionExtensionsTests
    {
        [TestMethod]
        [TestCategory("Fw.Data.Comparision")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CompareTo_WithNullDataTable_Exception_Test()
        {
            using (new DataTable().CompareTo(null))
            {
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.Comparision")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CompareTo_WithEmptyColumnNames_Exception_Test()
        {
            using (var tb = new DataTable())
            {
                using (var ctb = new DataTable())
                {
                    tb.CompareTo(ctb).Column("");
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.Comparision")]
        [ExpectedException(typeof(ArgumentException))]
        public void CompareTo_WithNotExistedColumns_Exception_Test()
        {
            using (var tb = new DataTable())
            {
                using (var ctb = new DataTable())
                {
                    tb.CompareTo(ctb).Column("A");
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.Comparision")]
        public void Compare_TwoIdentityTables_Test()
        {
            var path = "TestData\\Customers.csv";
            using (var tb = new DataTable().LoadFromCsv(path))
            {
                using (var ctb = new DataTable().LoadFromCsv(path))
                {
                    var result = tb.CompareTo(ctb).Execute();
                    using (var identityRows = result.GetDataTableResult(DataTablesResultType.IdentityRows))
                    {
                        Assert.AreEqual(identityRows.Table.Rows.Count, tb.Rows.Count);
                        Assert.AreEqual(identityRows.CompareTable.Rows.Count, ctb.Rows.Count);

                        Assert.AreEqual(identityRows.CompareTable.Rows[0][0], identityRows.Table.Rows[0][0]);
                        Assert.AreEqual(identityRows.CompareTable.Rows[0][1], identityRows.Table.Rows[0][1]);
                        Assert.AreEqual(identityRows.CompareTable.Rows[0][2], identityRows.Table.Rows[0][2]);
                        Assert.AreEqual(identityRows.CompareTable.Rows[0][3], identityRows.Table.Rows[0][3]);
                    }
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.Comparision")]
        public void Compare_TwoIdentityTables_PrimaryKeys_Test()
        {
            var path = "TestData\\Customers.csv";
            using (var tb = new DataTable().LoadFromCsv(path))
            {
                using (var ctb = new DataTable().LoadFromCsv(path))
                {
                    for (var i = 1; i < tb.Columns.Count; i++)
                        tb.Rows[0][i] = DBNull.Value;

                    var compare = tb.CompareTo(ctb);
                    compare.PrimaryKey("CustomerID");

                    var result = compare.Execute();

                    //There is only 1 Difference row
                    Assert.AreEqual(1, result.Rows.Count(r => r.Cells.Length > 0));
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.Comparision")]
        public void Compare_TwoDifferenceTables_Test()
        {
            using (var tb = new DataTable().LoadFromCsv("TestData\\Customers.csv"))
            {
                using (var ctb = new DataTable().LoadFromCsv("TestData\\Employees.csv"))
                {
                    var result = tb.CompareTo(ctb).Execute();
                    using (var identityRows = result.GetDataTableResult(DataTablesResultType.IdentityRows))
                    {
                        Assert.IsTrue(identityRows.Table.Rows.Count == 0);
                        Assert.IsNotNull(identityRows.CompareTable.Rows.Count == 0);
                    }

                    using (var identityRows = result.GetDataTableResult(DataTablesResultType.DifferenceRows))
                    {
                        Assert.IsNotNull(identityRows.Table);
                        Assert.AreEqual(identityRows.Table.Rows.Count, Math.Min(tb.Rows.Count, ctb.Rows.Count));
                        Assert.AreEqual(identityRows.CompareTable.Rows.Count, Math.Min(tb.Rows.Count, ctb.Rows.Count));
                    }
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.Comparision")]
        public void Build_ExpresstionWithCustomMethod_Test()
        {
            var con = new ValueCondition("A", CompareOperation.Equals, "NewID()");
            var express = new DataTableExpressionRender().BuildCondition(con);
            Assert.IsTrue(express.Contains("NewID()"));
        }

        [TestMethod]
        [TestCategory("Fw.Data.Comparision")]
        public void Build_ExpresstionWithCustomMethod_WithParameters_Test()
        {
            var dic = new Dictionary<string, object>();
            var con = new FieldCondition("A", CompareOperation.Equals, "NewID()");
            var express = new DataTableExpressionRender().BuildCondition(con, dic);
            Assert.IsTrue(express.Contains("NewID()"));
        }
    }
}
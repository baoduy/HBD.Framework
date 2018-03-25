#region using

using System;
using HBD.Framework.Data.SqlClient.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Data.Base.Tests
{
    [TestClass]
    public class TableNameTests
    {
        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TableNameExceptionTest1()
        {
            DbName.Parse("dbo.[]");
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TableNameExceptionTest2()
        {
            DbName.Parse("[]");
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        public void TableNameTest1()
        {
            var tb = DbName.Parse("dbo.[Table]");
            Assert.IsTrue(tb.Schema == "dbo");
            Assert.IsTrue(tb.Name == "Table");
            Assert.IsTrue(tb.FullName == "[dbo].[Table]");
            Assert.IsTrue(tb.ToString() == tb.FullName);
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        public void TableNameTest2()
        {
            var tb = DbName.Parse("[Table]");
            Assert.IsTrue(tb.Schema == "dbo");
            Assert.IsTrue(tb.Name == "Table");
            Assert.IsTrue(tb.FullName == "[dbo].[Table]");
            Assert.IsTrue(tb.ToString() == tb.FullName);
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        public void TableNameTest3()
        {
            var tb = DbName.Parse("[tb].[Table]");
            Assert.IsTrue(tb.Schema == "tb");
            Assert.IsTrue(tb.Name == "Table");
            Assert.IsTrue(tb.FullName == "[tb].[Table]");
            Assert.IsTrue(tb.ToString() == tb.FullName);
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        public void TableNameTest4()
        {
            var tb = DbName.Parse("tb.Table");
            Assert.IsTrue(tb.Schema == "tb");
            Assert.IsTrue(tb.Name == "Table");
            Assert.IsTrue(tb.FullName == "[tb].[Table]");
            Assert.IsTrue(tb.ToString() == tb.FullName);
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        public void TableNameTest5()
        {
            var tb = DbName.Parse(".Table");
            Assert.IsTrue(tb.Schema == "dbo");
            Assert.IsTrue(tb.Name == "Table");
            Assert.IsTrue(tb.FullName == "[dbo].[Table]");
            Assert.IsTrue(tb.ToString() == tb.FullName);
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        public void TableName_Null_Test6()
        {
            Assert.IsNull(DbName.Parse("."));
            Assert.IsNull(DbName.Parse(""));
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        public void TableNameTest7()
        {
            var tb = new DbName("Table");
            Assert.IsTrue(tb.FullName == "[dbo].[Table]");
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        public void CompareToTest1()
        {
            Assert.IsTrue(DbName.Parse("dbo.TableName") == DbName.Parse("TableName"));
            Assert.IsTrue(DbName.Parse("[dbo].TableName") == DbName.Parse("[TableName]"));
            Assert.IsFalse(DbName.Parse("[dbo].TableName1") == DbName.Parse("[TableName]"));
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        public void CompareToTest2()
        {
            Assert.IsFalse(DbName.Parse("dbo.TableName") == null);
            Assert.IsFalse(DbName.Parse("dbo.TableName") == new DbName("ABC"));
            Assert.IsFalse(DbName.Parse("dbo.TableName") == string.Empty);
            Assert.IsFalse(DbName.Parse("[dbo].TableName") != "[TableName]");
            Assert.IsFalse(DbName.Parse("[dbo].TableName1") == "[TableName]");
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        public void CompareToTest3()
        {
            Assert.IsTrue(DbName.Parse("dbo.TableName") != null);
            Assert.IsTrue(DbName.Parse("dbo.TableName") != new DbName("ABC"));
            Assert.IsTrue(DbName.Parse("dbo.TableName") != string.Empty);
            Assert.IsTrue(DbName.Parse("[dbo].TableName") == "[TableName]");
            Assert.IsTrue(DbName.Parse("[dbo].TableName1") != "[TableName]");
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        public void GetHashCodeTest1()
        {
            Assert.IsTrue(DbName.Parse("dbo.TableName").GetHashCode() == DbName.Parse("TableName").GetHashCode());
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        public void ImplicitTest1()
        {
            DbName tb = "TableName";
            Assert.IsTrue(tb.Name == "TableName");
            Assert.IsTrue(tb.Schema == "dbo");

            string tbName = tb;
            Assert.IsTrue(tbName == tb.FullName);
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        public void ImplicitTest2()
        {
            DbName tb = "tb.TableName";
            Assert.IsTrue(tb.Name == "TableName");
            Assert.IsTrue(tb.Schema == "tb");

            string tbName = tb;
            Assert.IsTrue(tbName == tb.FullName);
        }
    }
}
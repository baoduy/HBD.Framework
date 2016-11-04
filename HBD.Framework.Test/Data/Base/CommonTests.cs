using Microsoft.VisualStudio.TestTools.UnitTesting;
using HBD.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HBD.Framework.Data.Base;

namespace HBD.Framework.Data.Tests
{
    [TestClass()]
    public class CommonTests
    {
        [TestMethod()]
        [TestCategory("Fw.Data.Base")]
        public void GetSqlNameTest()
        {
            Assert.IsTrue(Common.GetSqlName(null) == null);
            Assert.IsTrue(Common.GetSqlName("") == "");
            Assert.IsTrue(Common.GetSqlName("*") == "*");
            Assert.IsTrue(Common.GetSqlName("ABC")=="[ABC]");
            Assert.IsTrue(Common.GetSqlName("[ABC]") == "[ABC]");
            Assert.IsTrue(Common.GetSqlName("dbo.A ABC") == "[dbo].[A ABC]");
            Assert.IsTrue(Common.GetSqlName("NewID()") == "NewID()");
        }

        [TestMethod()]
        [TestCategory("Fw.Data.Base")]
        public void GetFullNameTest()
        {
            Assert.IsTrue(Common.GetFullName("dbo","ABC") == "[dbo].[ABC]");
        }

        [TestMethod()]
        [TestCategory("Fw.Data.Base")]
        public void RemoveSqlBracketsTest()
        {
            Assert.IsTrue(Common.RemoveSqlBrackets(null) == null);
            Assert.IsTrue(Common.RemoveSqlBrackets("") == "");
            Assert.IsTrue(Common.RemoveSqlBrackets("ABC") == "ABC");
            Assert.IsTrue(Common.RemoveSqlBrackets("[ABC]") == "ABC");
        }
    }
}
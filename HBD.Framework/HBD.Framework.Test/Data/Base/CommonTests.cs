#region using

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Data.Tests
{
    [TestClass]
    public class CommonTests
    {
        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        public void GetSqlNameTest()
        {
            Assert.IsTrue(Common.GetSqlName(null) == null);
            Assert.IsTrue(Common.GetSqlName("") == "");
            Assert.IsTrue(Common.GetSqlName("*") == "*");
            Assert.IsTrue(Common.GetSqlName("ABC") == "[ABC]");
            Assert.IsTrue(Common.GetSqlName("[ABC]") == "[ABC]");
            Assert.IsTrue(Common.GetSqlName("dbo.A ABC") == "[dbo].[A ABC]");
            Assert.IsTrue(Common.GetSqlName("NewID()") == "NewID()");
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        public void GetFullNameTest()
        {
            Assert.IsTrue(Common.GetFullName("dbo", "ABC") == "[dbo].[ABC]");
        }

        [TestMethod]
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
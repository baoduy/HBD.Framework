#region

using HBD.QueryBuilders.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.SqlQueryBuilder.Core.Tests
{
    [TestClass]
    public class SqlSyntaxValidationTests
    {
        [TestMethod]
        [TestCategory("Fw.SqlBuilder.Syntax")]
        public void ParseTest()
        {
            Assert.IsTrue(SqlSyntaxValidation.Parse("SELECT * FROM A").Count == 0);
            Assert.IsTrue(SqlSyntaxValidation.Parse("SELECT * FROM ").Count > 0);
            Assert.IsTrue(SqlSyntaxValidation.Parse("SELECT * FROM [A").Count > 0);
            Assert.IsTrue(SqlSyntaxValidation.Parse("SELECT * FROM [A]").Count == 0);
        }
    }
}
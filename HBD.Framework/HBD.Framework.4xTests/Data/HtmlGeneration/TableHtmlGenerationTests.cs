#region using

using System;
using HBD.Framework.Randoms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Data.HtmlGeneration.Tests
{
    [TestClass]
    public class TableHtmlGenerationTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TableHtmlGenerationTest()
        {
            new TableHtmlGeneration(null);
        }

        [TestMethod]
        public void GenerateTest()
        {
            var generation =
                new TableHtmlGeneration(
                    RandomGenerator.DataTable(numberOfColumn: 10, numberOfRows: 10).CreateGetSetter());
            var str = generation.Generate();

            Assert.IsTrue(str.StartsWith("<table"));
            Assert.IsTrue(str.CountIgnoreCase("<th") == 11);
            Assert.IsTrue(str.EndsWith("</table>"));
        }

        [TestMethod]
        public void ToClipboardFormatTest()
        {
            var generation =
                new TableHtmlGeneration(
                    RandomGenerator.DataTable(numberOfColumn: 10, numberOfRows: 10).CreateGetSetter());
            var str = generation.ToClipboardFormat();

            Assert.IsTrue(str.Contains("Version"));
            Assert.IsTrue(str.Contains("StartHTML"));
            Assert.IsTrue(str.Contains("EndHTML"));
            Assert.IsTrue(str.Contains("StartFragment"));
            Assert.IsTrue(str.Contains("EndFragment"));
            Assert.IsTrue(str.Contains("StartSelection"));
            Assert.IsTrue(str.Contains("EndSelection"));
        }

        [TestMethod]
        public void ToStringTest()
        {
            var generation =
                new TableHtmlGeneration(
                    RandomGenerator.DataTable(numberOfColumn: 10, numberOfRows: 10).CreateGetSetter());
            var str = generation.ToString();

            Assert.IsTrue(str.StartsWith("<table"));
            Assert.IsTrue(str.CountIgnoreCase("<th") == 11);
            Assert.IsTrue(str.EndsWith("</table>"));
        }
    }
}
#region using

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Test
{
    [TestClass]
    public class CommonFuncsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetColumnName_Exception()
        {
            CommonFunctions.GetColumnName(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetExcelColumnName_Exception()
        {
            CommonFunctions.GetExcelColumnName(-1);
        }

        [TestMethod]
        public void GetColumnLabel_Test()
        {
            Assert.AreEqual("F1", CommonFunctions.GetColumnName(0));
            Assert.AreEqual("F2", CommonFunctions.GetColumnName(1));
            Assert.AreEqual("F26", CommonFunctions.GetColumnName(25));
        }

        [TestMethod]
        public void GetExcelColumnLabel_Test()
        {
            Assert.AreEqual("A", CommonFunctions.GetExcelColumnName(0));
            Assert.AreEqual("B", CommonFunctions.GetExcelColumnName(1));
            Assert.AreEqual("Z", CommonFunctions.GetExcelColumnName(25));

            Assert.AreEqual("AA", CommonFunctions.GetExcelColumnName(26));
            Assert.AreEqual("BA", CommonFunctions.GetExcelColumnName(52));
            Assert.AreEqual("ZA", CommonFunctions.GetExcelColumnName(26 * 26));
            Assert.AreEqual("ZZ", CommonFunctions.GetExcelColumnName(26 * 26 + 25));

            Assert.AreEqual("AAA", CommonFunctions.GetExcelColumnName(26 * 27));
            Assert.AreEqual("AAZ", CommonFunctions.GetExcelColumnName(26 * 27 + 25));
            Assert.AreEqual("ALA", CommonFunctions.GetExcelColumnName(26 * 38));
            Assert.AreEqual("YZA", CommonFunctions.GetExcelColumnName(26 * 26 * 26));
            Assert.AreEqual("YZZ", CommonFunctions.GetExcelColumnName(26 * 26 * 26 + 25));

            Assert.AreEqual("ZAA", CommonFunctions.GetExcelColumnName(26 * 26 * 26 + 26));
            Assert.AreEqual("ZZZ", CommonFunctions.GetExcelColumnName(26 * 26 * 27 + 25));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetColumnIndex_Exception()
        {
            CommonFunctions.GetColumnIndex("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExcelColumnIndex_Exception()
        {
            CommonFunctions.GetExcelColumnIndex("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetExcelColumnIndex_ArgumentOutOfRangeException()
        {
            CommonFunctions.GetExcelColumnIndex("A123Z");
        }

        [TestMethod]
        public void GetColumnIndex_Test()
        {
            Assert.AreEqual(0, CommonFunctions.GetColumnIndex("F1"));
            Assert.AreEqual(9, CommonFunctions.GetColumnIndex("F10"));
            Assert.AreEqual(99, CommonFunctions.GetColumnIndex("F100"));
        }

        [TestMethod]
        public void GetExcelColumnIndex_Test()
        {
            Assert.AreEqual(0, CommonFunctions.GetExcelColumnIndex("A"));
            Assert.AreEqual(1, CommonFunctions.GetExcelColumnIndex("B"));
            Assert.AreEqual(25, CommonFunctions.GetExcelColumnIndex("Z"));

            Assert.AreEqual(26, CommonFunctions.GetExcelColumnIndex("AA"));
            Assert.AreEqual(52, CommonFunctions.GetExcelColumnIndex("BA"));
            Assert.AreEqual(26 * 26, CommonFunctions.GetExcelColumnIndex("ZA"));
            Assert.AreEqual(26 * 26 + 25, CommonFunctions.GetExcelColumnIndex("ZZ"));

            Assert.AreEqual(26 * 27, CommonFunctions.GetExcelColumnIndex("AAA"));
            Assert.AreEqual(26 * 27 + 25, CommonFunctions.GetExcelColumnIndex("AAZ"));
            Assert.AreEqual(26 * 38, CommonFunctions.GetExcelColumnIndex("ALA"));
            Assert.AreEqual(26 * 26 * 26, CommonFunctions.GetExcelColumnIndex("YZA"));
            Assert.AreEqual(26 * 26 * 26 + 25, CommonFunctions.GetExcelColumnIndex("YZZ"));

            Assert.AreEqual(26 * 26 * 26 + 26, CommonFunctions.GetExcelColumnIndex("ZAA"));
            Assert.AreEqual(26 * 26 * 27 + 25, CommonFunctions.GetExcelColumnIndex("ZZZ"));
        }
    }
}
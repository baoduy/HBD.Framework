using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Framework.Test
{
    [TestClass]
    public class CommonFuncsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetColumnName_Exception()
        {
            CommonFuncs.GetColumnName(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetExcelColumnName_Exception()
        {
            CommonFuncs.GetExcelColumnName(-1);
        }

        [TestMethod]
        public void GetColumnLabel_Test()
        {
            Assert.AreEqual("F1", CommonFuncs.GetColumnName(0));
            Assert.AreEqual("F2", CommonFuncs.GetColumnName(1));
            Assert.AreEqual("F26", CommonFuncs.GetColumnName(25));
        }

        [TestMethod]
        public void GetExcelColumnLabel_Test()
        {
            Assert.AreEqual("A", CommonFuncs.GetExcelColumnName(0));
            Assert.AreEqual("B", CommonFuncs.GetExcelColumnName(1));
            Assert.AreEqual("Z", CommonFuncs.GetExcelColumnName(25));

            Assert.AreEqual("AA", CommonFuncs.GetExcelColumnName(26));
            Assert.AreEqual("BA", CommonFuncs.GetExcelColumnName(52));
            Assert.AreEqual("ZA", CommonFuncs.GetExcelColumnName(26 * 26));
            Assert.AreEqual("ZZ", CommonFuncs.GetExcelColumnName(26 * 26 + 25));

            Assert.AreEqual("AAA", CommonFuncs.GetExcelColumnName(26 * 27));
            Assert.AreEqual("AAZ", CommonFuncs.GetExcelColumnName(26 * 27 + 25));
            Assert.AreEqual("ALA", CommonFuncs.GetExcelColumnName(26 * 38));
            Assert.AreEqual("YZA", CommonFuncs.GetExcelColumnName(26 * 26 * 26));
            Assert.AreEqual("YZZ",CommonFuncs.GetExcelColumnName(26 * 26 * 26 + 25));

            Assert.AreEqual("ZAA",CommonFuncs.GetExcelColumnName(26 * 26 * 26 + 26));
            Assert.AreEqual("ZZZ",CommonFuncs.GetExcelColumnName(26 * 26 * 27 + 25));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetColumnIndex_Exception()
        {
            CommonFuncs.GetColumnIndex("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExcelColumnIndex_Exception()
        {
            CommonFuncs.GetExcelColumnIndex("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetExcelColumnIndex_ArgumentOutOfRangeException()
        {
            CommonFuncs.GetExcelColumnIndex( "A123Z");
        }

        [TestMethod]
        public void GetColumnIndex_Test()
        {
            Assert.AreEqual(0, CommonFuncs.GetColumnIndex("F1"));
            Assert.AreEqual(9, CommonFuncs.GetColumnIndex("F10"));
            Assert.AreEqual(99, CommonFuncs.GetColumnIndex("F100"));
        }

        [TestMethod]
        public void GetExcelColumnIndex_Test()
        {
            Assert.AreEqual(0,  CommonFuncs.GetExcelColumnIndex(  "A"));
            Assert.AreEqual(1,  CommonFuncs.GetExcelColumnIndex(  "B"));
            Assert.AreEqual(25,  CommonFuncs.GetExcelColumnIndex(  "Z"));

            Assert.AreEqual(26,  CommonFuncs.GetExcelColumnIndex(  "AA"));
            Assert.AreEqual(52,  CommonFuncs.GetExcelColumnIndex(  "BA"));
            Assert.AreEqual(26 * 26,  CommonFuncs.GetExcelColumnIndex(  "ZA"));
            Assert.AreEqual(26 * 26 + 25,  CommonFuncs.GetExcelColumnIndex(  "ZZ"));

            Assert.AreEqual(26 * 27,  CommonFuncs.GetExcelColumnIndex(  "AAA"));
            Assert.AreEqual(26 * 27 + 25,  CommonFuncs.GetExcelColumnIndex(  "AAZ"));
            Assert.AreEqual(26 * 38,  CommonFuncs.GetExcelColumnIndex(  "ALA"));
            Assert.AreEqual(26 * 26 * 26,  CommonFuncs.GetExcelColumnIndex(  "YZA"));
            Assert.AreEqual(26 * 26 * 26 + 25,  CommonFuncs.GetExcelColumnIndex(  "YZZ"));

            Assert.AreEqual(26 * 26 * 26 + 26,  CommonFuncs.GetExcelColumnIndex(  "ZAA"));
            Assert.AreEqual(26 * 26 * 27 + 25,  CommonFuncs.GetExcelColumnIndex(  "ZZZ"));
        }
    }
}

#region

using System;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Data.Excel.Tests
{
    [TestClass]
    public class ExcelExtensionsTests
    {
        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void GetSystemType_Test()
        {
            var priType = new PrivateType(typeof(ExcelAdapter));

            Assert.AreEqual(typeof(bool), ((EnumValue<CellValues>) CellValues.Boolean).GetRuntimeType());
            Assert.AreEqual(typeof(DateTime), ((EnumValue<CellValues>) CellValues.Date).GetRuntimeType());
            Assert.AreEqual(typeof(string), ((EnumValue<CellValues>) CellValues.Error).GetRuntimeType());
            Assert.AreEqual(typeof(string), ((EnumValue<CellValues>) CellValues.InlineString).GetRuntimeType());
            Assert.AreEqual(typeof(double), ((EnumValue<CellValues>) CellValues.Number).GetRuntimeType());
            Assert.AreEqual(typeof(string), ((EnumValue<CellValues>) CellValues.SharedString).GetRuntimeType());
            Assert.AreEqual(typeof(string), ((EnumValue<CellValues>) CellValues.String).GetRuntimeType());
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void GetCellValue_ParamsNull_ReturnNull()
        {
            var priType = new PrivateType(typeof(ExcelExtensions));
            Assert.IsNull(priType.InvokeStatic("GetValue", BindingFlags.NonPublic, (Cell) null, (WorkbookPart) null));
        }

        //[TestMethod()]
        //[TestCategory("Fw.Data.ExcelAdapter")]
        //public void GetCellValue_DataTypeIsDouble_ReturnDouble()
        //{
        //    var cell = ExcelAdapter.CreateNumericCell("A1", "123");
        //    var priType = new PrivateType(typeof(ExcelExtensions));
        //    Assert.IsInstanceOfType(priType.InvokeStatic("GetCellValue", BindingFlags.NonPublic, cell, (WorkbookPart)null), typeof(double));
        //}

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void GetRuntimeType_ParamsNull_ReturnNull()
        {
            Assert.IsTrue(ExcelExtensions.GetRuntimeType(null) == typeof(string));
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void GetRuntimeType_StringFormatCode()
        {
            Assert.IsTrue(ExcelExtensions.GetRuntimeType("") == typeof(string));
            Assert.IsTrue(ExcelExtensions.GetRuntimeType("#") == typeof(double));
            Assert.IsTrue(ExcelExtensions.GetRuntimeType("0") == typeof(double));
            Assert.IsTrue(ExcelExtensions.GetRuntimeType("0.0") == typeof(double));
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void GetNumberFormatCodeTest()
        {
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 0));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 1));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 2));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 3));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 4));

            Assert.IsNull(ExcelExtensions.GetNumberFormatCode(null, 5));
            Assert.IsNull(ExcelExtensions.GetNumberFormatCode(null, 6));

            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 9));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 10));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 11));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 12));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 13));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 14));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 15));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 16));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 17));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 18));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 19));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 20));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 21));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 22));

            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 37));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 38));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 39));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 40));

            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 44));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 45));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 46));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 47));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 48));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 49));

            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 27));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 30));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 36));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 50));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 57));

            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 59));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 60));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 61));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 62));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 67));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 68));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 69));
            Assert.IsNotNull(ExcelExtensions.GetNumberFormatCode(null, 70));
        }

        [TestMethod]
        [TestCategory("Fw.Data.ExcelAdapter")]
        public void SplitByCellReferenceFormatTest()
        {
            var priType = new PrivateType(typeof(ExcelExtensions));
            var value = (string[]) priType.InvokeStatic("SplitByCellReferenceFormat", "AA1234567890");
            Assert.IsTrue(value.Length == 2);
            Assert.IsTrue(value[0] == "AA");
            Assert.IsTrue(value[1] == "1234567890");
        }
    }
}
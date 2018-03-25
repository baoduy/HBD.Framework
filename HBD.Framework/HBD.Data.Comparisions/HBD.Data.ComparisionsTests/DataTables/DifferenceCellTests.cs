#region

using System;
using HBD.Data.Comparisons.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Data.Comparision.DataTables.Tests
{
    [TestClass]
    public class DifferenceCellTests
    {
        [TestMethod]
        [TestCategory("Fw.Data.Comparision.DataTables")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DifferenceCell_NullException_Test()
        {
            new DifferenceCell(null);
        }

        [TestMethod]
        [TestCategory("Fw.Data.Comparision.DataTables")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DifferenceCell_NullException_Test2()
        {
            new DifferenceCell(null, string.Empty);
        }
    }
}
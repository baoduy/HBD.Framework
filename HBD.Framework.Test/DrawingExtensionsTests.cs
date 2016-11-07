using Microsoft.VisualStudio.TestTools.UnitTesting;
using HBD.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBD.Framework.Tests
{
    [TestClass()]
    public class DrawingExtensionsTests
    {
        [TestMethod()]
        [TestCategory("Fw.Extensions")]
        public void ToHexValue_Test()
        {
            Assert.IsTrue(Color.Black.ToHexValue() == "000000");
            Assert.IsTrue(Color.Green.ToHexValue() == "008000");
            Assert.IsTrue(Color.White.ToHexValue() == "FFFFFF");
        }

        [TestMethod()]
        [TestCategory("Fw.Extensions")]
        public void ToHexValue_WithEmpty_Test()
        {
            Assert.IsNull(Color.Empty.ToHexValue());
        }
    }
}
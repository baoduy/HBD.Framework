using Microsoft.VisualStudio.TestTools.UnitTesting;
using HBD.Framework.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBD.Framework.IO.Tests
{
    [TestClass()]
    public class PathTests
    {
        [TestMethod()]
        [TestCategory("Fw.IO")]
        public void GetFullPathTest()
        {
            Assert.IsTrue(HBD.Framework.IO.PathEx.GetFullPath("TestData").Contains("HBD.Framework.Test"));
        }

        [TestMethod()]
        [TestCategory("Fw.IO")]
        public void IsPathExistedTest()
        {
            Assert.IsFalse(HBD.Framework.IO.PathEx.IsPathExisted(""));
            Assert.IsFalse(HBD.Framework.IO.PathEx.IsPathExisted(null));

            Assert.IsTrue(HBD.Framework.IO.PathEx.IsPathExisted("TestData"));
            Assert.IsFalse(HBD.Framework.IO.PathEx.IsPathExisted("TestData\\AAA"));

            Assert.IsTrue(HBD.Framework.IO.PathEx.IsPathExisted("TestData\\DataBaseInfo.xlsx"));
            Assert.IsFalse(HBD.Framework.IO.PathEx.IsPathExisted("TestData\\AAA.txt"));
        }

        [TestMethod()]
        [TestCategory("Fw.IO")]
        public void IsDirectoryTest()
        {
            Assert.IsTrue(HBD.Framework.IO.PathEx.IsDirectory("TestData"));
            Assert.IsFalse(HBD.Framework.IO.PathEx.IsDirectory("TestData\\DataBaseInfo.xlsx"));
        }
    }
}
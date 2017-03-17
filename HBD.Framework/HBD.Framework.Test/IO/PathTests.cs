#region

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.IO.Tests
{
    [TestClass]
    public class PathTests
    {
        [TestMethod]
        [TestCategory("Fw.IO")]
        public void GetFullPathTest()
        {
            Assert.IsTrue(PathEx.GetFullPath("TestData").Contains("HBD.Framework.Test"));
        }

        [TestMethod]
        [TestCategory("Fw.IO")]
        public void IsPathExistedTest()
        {
            Assert.IsFalse(PathEx.IsPathExisted(""));
            Assert.IsFalse(PathEx.IsPathExisted(null));

            Assert.IsTrue(PathEx.IsPathExisted("TestData"));
            Assert.IsFalse(PathEx.IsPathExisted("TestData\\AAA"));

            Assert.IsTrue(PathEx.IsPathExisted("TestData\\DataBaseInfo\\DataBaseInfo.csv"));
            Assert.IsFalse(PathEx.IsPathExisted("TestData\\AAA.txt"));
        }

        [TestMethod]
        [TestCategory("Fw.IO")]
        public void IsDirectoryTest()
        {
            Assert.IsTrue(PathEx.IsDirectory("TestData"));
            Assert.IsFalse(PathEx.IsDirectory("TestData\\DataBaseInfo.xlsx"));
        }
    }
}
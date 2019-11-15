using FluentAssertions;
using HBD.Framework.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace HBD.Framework.StTests.IO
{
    [TestClass]
    public class DirectoryExTests
    {
        [TestMethod]
        public void Copy_Folder_To()
        {
            new DirectoryInfo("TestData\\TestZip")
                .CopyTo("TestData\\TestZip1");

            Directory.Exists("TestData\\TestZip1");
            Directory.GetFiles("TestData\\TestZip1").Length.Should().BeGreaterOrEqualTo(1);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using HBD.Framework.Security.Azman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HBD.Framework.Security.Azman.Tests
{
    [TestClass()]
    public class AzXmlConnectionStringBuilderTests
    {
        [TestMethod()]
        [TestCategory("Fw.AzMan")]
        public void AzXmlConnectionStringBuilder_Test()
        {
            var c = new AzXmlConnectionStringBuilder(@"msxml://TestData\XMLFile1.xml");
            Assert.IsTrue(c.ConnectionString.IsNotNullOrEmpty());
            Assert.IsTrue(c.FileName == "TestData\\XMLFile1.xml");
        }

        [TestMethod()]
        [TestCategory("Fw.AzMan")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AzXmlConnectionStringBuilder_FileEmpty_ArgumentNullException_Test()
        {
            var c = new AzXmlConnectionStringBuilder(@"msxml://");
        }

        [TestMethod()]
        [TestCategory("Fw.AzMan")]
        [ExpectedException(typeof(FileNotFoundException))]
        public void AzXmlConnectionStringBuilder_FileNotFound_ArgumentNullException_Test()
        {
            var c = new AzXmlConnectionStringBuilder(@"msxml://C:\Test.xml");
        }

        [TestMethod()]
        [TestCategory("Fw.AzMan")]
        public void AzXmlConnectionStringBuilder_Builder_Test()
        {
            var c = new AzXmlConnectionStringBuilder {FileName = "TestData\\XMLFile1.xml"};
            Assert.IsTrue(c.ConnectionString.StartsWith("msxml://"));
            Assert.IsTrue(c.ConnectionString.EndsWith("TestData\\XMLFile1.xml"));
        }
    }
}
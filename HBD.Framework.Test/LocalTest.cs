using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace HBD.Framework.Test
{
    [TestClass]
    public class LocalTest
    {
        [TestMethod]
        public void TestConnectionString()
        {
            Assert.IsTrue(ConfigurationManager.ConnectionStrings.Count > 2);
        }
    }
}

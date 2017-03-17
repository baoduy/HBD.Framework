#region

using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

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
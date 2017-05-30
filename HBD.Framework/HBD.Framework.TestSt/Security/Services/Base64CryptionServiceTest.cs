using HBD.Framework.Security.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Framework.Test.St.Security.Services
{
    [TestClass]
    public class Base64CryptionServiceTest
    {
        [TestMethod]
        public void DecryptTest()
        {
            var service = new Base64CryptionService();
            var str = service.Decrypt("SABvAGEAbgBnACAAQgBhAG8AIABEAHUAeQA=");

            Assert.IsNotNull(str);
            Assert.AreEqual("Hoang Bao Duy", str);
        }

        [TestMethod]
        public void EncryptTest()
        {
            var service = new Base64CryptionService();
            var str = service.Encrypt("Hoang Bao Duy");

            Assert.IsNotNull(str);
            Assert.AreEqual("SABvAGEAbgBnACAAQgBhAG8AIABEAHUAeQA=", str);
        }
    }
}
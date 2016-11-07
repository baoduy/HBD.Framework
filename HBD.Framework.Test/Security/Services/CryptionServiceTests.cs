using Microsoft.VisualStudio.TestTools.UnitTesting;
using HBD.Framework.Security.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBD.Framework.Security.Services.Tests
{
    [TestClass()]
    public class CryptionServiceTests
    {
        [TestMethod()]
        public void CryptionServiceTest()
        {
            var c = new CryptionService("123");
            var text = "Hoang Bao Duy";
            var encrypted = c.Encrypt(text);

            Assert.AreNotEqual(text,encrypted);
            Assert.AreEqual(text,c.Decrypt(encrypted));
        }

        [TestMethod()]
        [ExpectedException(typeof(FormatException))]
        public void Encrypt_Exception_Test()
        {
            var c = new CryptionService("123");
            var text = "Hoang Bao Duy";
           c.Decrypt(text);
        }
    }
}
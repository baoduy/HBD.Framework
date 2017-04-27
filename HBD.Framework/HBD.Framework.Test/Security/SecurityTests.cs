#region using

using System.IO;
using HBD.Framework.Security;
using HBD.Framework.Security.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Test
{
    [TestClass]
    public class SecurityTests
    {
        [TestMethod]
        [TestCategory("Fw.Security")]
        public void Test_Cryptography()
        {
            var value = File.ReadAllText("TestData\\TestCryptographyData.txt");
            var encrypted = CryptionManager.Default.Encrypt(value);
            Assert.IsTrue(value != encrypted);

            var decrypted = CryptionManager.Default.Decrypt(encrypted);
            Assert.IsNotNull(decrypted);
            Assert.IsTrue(value == decrypted);
        }

        [TestMethod]
        [TestCategory("Fw.Security")]
        public void Test_Cryptography_With_CustomPassword()
        {
            var customPassword =
                CryptionManager.Default.Encrypt(
                    "{ED469E33-E12B-4CDB-AACB-A10D89657C9C}{ED469E33-E12B-4CDB-AACB-A10D89657C9C}");
            var cryption = new CryptionService(customPassword);

            var value = File.ReadAllText("TestData\\TestCryptographyData.txt");

            var encrypted = cryption.Encrypt(value);
            Assert.IsTrue(value != encrypted);

            var decrypted = cryption.Decrypt(encrypted);
            Assert.IsNotNull(decrypted);
            Assert.IsTrue(value == decrypted);
        }

        [TestMethod]
        [TestCategory("Fw.Security")]
        public void Check_IsEncrypted()
        {
            var encrytp = CryptionManager.Default.Encrypt("Hoang Bao Duy");
            Assert.IsTrue(encrytp.IsEncrypted());

            encrytp = $"Duy2-{encrytp}";
            Assert.IsFalse(encrytp.IsEncrypted());
        }
    }
}
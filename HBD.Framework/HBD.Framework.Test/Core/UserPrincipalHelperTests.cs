using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Framework.Core.Tests
{
    [TestClass()]
    public class UserPrincipalHelperTests
    {
        [TestMethod()]
        public void GetUserNameWithoutDomain1_Test()
        {
            Assert.AreEqual(UserPrincipalHelper.GetUserNameWithoutDomain("baoduy@hbd.net"), "baoduy");
        }

        [TestMethod()]
        public void GetUserNameWithoutDomain2_Test()
        {
            Assert.AreEqual(UserPrincipalHelper.GetUserNameWithoutDomain("hbd\\baoduy"), "baoduy");
        }
    }
}
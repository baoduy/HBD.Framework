using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace HBD.Framework.Security.Azman.Base.Tests
{
    [TestClass()]
    public class AzApplicationTests
    {
        [TestMethod()]
        public void IsUserInOperationsTest()
        {
            //using (var context = new AzManContext("TestData\\AzMan.xml"))
            //{
            //    var app = context.GetApplication("App1");
            //    Assert.IsTrue(app.IsUserInOperations("schrodersad\\hoangd", "Op1"));
            //}
        }

        [TestMethod()]
        public void IsUserInOperations_Torken_Test()
        {
            //    using (var context = new AzManContext("TestData\\AzMan.xml"))
            //    {
            //        var app = context.GetApplication("App1");
            //        Assert.IsTrue(app.IsUserInOperations(UserPrincipalHelper.User, "Op1"));
            //    }
        }

        [TestMethod()]
        public void GetUserInfo_UserName_Test()
        {
            using (var context = new AzManContext("TestData\\AzMan.xml"))
            {
                var app = context.GetApplication("App1");
                var userInfo = app.GetUserInfo("steven");

                Assert.IsTrue(userInfo.Operations.Any());
                Assert.IsTrue(userInfo.Roles.Any());
                Assert.IsTrue(userInfo.Groups.Any());
            }
        }

        [TestMethod()]
        public void GetUserInfo_UserName_Scope_Test()
        {
            using (var context = new AzManContext("TestData\\AzMan.xml"))
            {
                var app = context.GetApplication("App1");
                var userInfo = app.GetUserInfo("steven","Scope2");

                Assert.IsTrue(userInfo.Operations.Any());
                Assert.IsTrue(userInfo.Roles.Any());
                Assert.IsFalse(userInfo.Groups.Any());
                Assert.IsNull(userInfo.UserScopeInfos);
            }
        }
    }
}
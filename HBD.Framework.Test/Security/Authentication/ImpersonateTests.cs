using Microsoft.VisualStudio.TestTools.UnitTesting;
using HBD.Framework.Security.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBD.Framework.Security.Authentication.Tests
{
    [TestClass()]
    public class ImpersonateTests
    {
        [TestMethod()]
        [TestCategory("Fw.Security.Auth")]
        public void Logon_Administrator_Test()
        {
            //using (var token = Impersonate.LogonUser("hoangd", "schrodersad", "Schroders5"))
            //    Assert.IsNotNull(token);
        }
    }
}
#region

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Security.Azman.Tests
{
    [TestClass]
    public class AzManContext_GetData_Tests
    {
        [TestMethod]
        [TestCategory("Fw.AzMan")]
        public void GetAll_Applications_XmlFile_Test()
        {
            using (var azMan = new AzManContext(new AzXmlConnectionStringBuilder {FileName = "TestData\\AzMan.xml"}))
            {
                var apps = azMan.Applications;
                Assert.IsTrue(apps.Count > 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.AzMan")]
        [Ignore]
        public void GetAll_Applications_Sql_Test()
        {
            var conn = @"mssql://Driver={SQL Server};Server={.};/AzManStore/Testing";
            //var conn = @"Mssql://Driver={SQL Server};Server={SIN8506,3009\bizapps};/sgAzManDB/AzmanStore";

            using (var azMan = new AzManContext(conn))
            {
                var apps = azMan.Applications;
                Assert.IsTrue(apps.Count > 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.AzMan")]
        public void GetAll_Operations_Test()
        {
            using (var azMan = new AzManContext(new AzXmlConnectionStringBuilder {FileName = "TestData\\AzMan.xml"}))
            {
                var app = azMan.Applications.FirstOrDefault();

                Assert.IsNotNull(app);
                Assert.IsTrue(app.Operations.Count > 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.AzMan")]
        public void GetAll_Roles_Test()
        {
            using (var azMan = new AzManContext(new AzXmlConnectionStringBuilder {FileName = "TestData\\AzMan.xml"}))
            {
                var app = azMan.Applications.FirstOrDefault();

                Assert.IsNotNull(app);
                Assert.IsTrue(app.Roles.Count > 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.AzMan")]
        public void Get_Operations_Roles_Test()
        {
            using (var azMan = new AzManContext(new AzXmlConnectionStringBuilder {FileName = "TestData\\AzMan.xml"}))
            {
                var app = azMan.Applications.FirstOrDefault();
                var role = app?.Roles.FirstOrDefault(r => r.Name == "Role2");

                Assert.IsNotNull(role);
                Assert.IsTrue(role.AssignedOperations.Count > 0);
                Assert.IsTrue(role.AssignedRoles.Count > 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.AzMan")]
        public void Application_Groups_Test()
        {
            var conn = new AzXmlConnectionStringBuilder {FileName = "TestData\\AzMan.xml"};

            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.Applications.FirstOrDefault();

                Assert.IsNotNull(app);
                Assert.IsTrue(app.Groups.Count > 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.AzMan")]
        public void Application_Groups_Members_Test()
        {
            var conn = new AzXmlConnectionStringBuilder {FileName = "TestData\\AzMan.xml"};

            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.Applications.FirstOrDefault();
                var group = app?.Groups.FirstOrDefault();

                Assert.IsNotNull(group);
                Assert.IsTrue(group.Members.Count > 0);
                Assert.IsTrue(group.Members.Any(m => m.AssignedRoles.Count > 0));
            }
        }

        [TestMethod]
        [TestCategory("Fw.AzMan")]
        public void Application_Scope_Test()
        {
            var conn = new AzXmlConnectionStringBuilder {FileName = "TestData\\AzMan.xml"};

            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.Applications.FirstOrDefault();

                Assert.IsNotNull(app);
                Assert.IsTrue(app.Scopes.Count > 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.AzMan")]
        public void Scope_Group_Test()
        {
            var conn = new AzXmlConnectionStringBuilder {FileName = "TestData\\AzMan.xml"};

            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.Applications.FirstOrDefault();
                Assert.IsNotNull(app);

                var scope = app.Scopes.FirstOrDefault();
                Assert.IsNotNull(scope);

                var group = scope.Groups.FirstOrDefault();
                Assert.IsNotNull(group);

                Assert.IsTrue(group.Members.Count > 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.AzMan")]
        public void Group_Member_Test()
        {
            var conn = new AzXmlConnectionStringBuilder {FileName = "TestData\\AzMan.xml"};

            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.Applications["App1"];
                Assert.IsNotNull(app);

                var scope = app.Scopes.FirstOrDefault();
                Assert.IsNotNull(scope);

                var group = scope.Groups.FirstOrDefault();
                Assert.IsNotNull(group);

                var member = group.Members.FirstOrDefault();
                Assert.IsNotNull(member);

                Assert.IsTrue(member.AssignedRoles.Count > 0);
            }
        }
    }
}
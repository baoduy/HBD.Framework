#region

using System.Linq;
using HBD.Framework.Security.Azman.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Security.Azman.Tests
{
    [TestClass]
    public class AzManContextTests
    {
        [TestMethod]
        [TestCategory("Fw.AzMan")]
        [Ignore]
        public void Create_Delete_Application_Test()
        {
            var conn = new AzXmlConnectionStringBuilder {FileName = "TestData\\AzMan.xml"};
            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.CreateApplication("AppNew");
                app.Operations.Add(new AzOperation {Id = 1, Name = "Op1"});
                app.Operations.Add(new AzOperation {Id = 2, Name = "Op2"});

                var role1 = new AzRole {Name = "Role1"};
                role1.AssignedOperations.Add(app.Operations.FirstOrDefault());
                app.Roles.Add(role1);

                app.Groups.Add(new AzGroup {Name = "Group1"});

                var scope1 = new AzScope {Name = "Scope1", Description = "Scope1"};
                app.Scopes.Add(scope1);

                var role2 = new AzRole {Name = "Role2"};
                role2.AssignedRoles.Add(role1);
                scope1.Roles.Add(role2);

                var group2 = new AzGroup {Name = "Group2"};
                scope1.Groups.Add(group2);

                var m = new AzMember {Name = "Administrator"};
                group2.Members.Add(m);
                m.AssignedRoles.Add(role2);

                app.Save();
            }

            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.Applications["AppNew"];

                Assert.IsNotNull(app);

                Assert.IsTrue(app.Operations.Count == 2);
                Assert.IsTrue(app.Operations.Any(op => op.Name == "Op1"));
                Assert.IsTrue(app.Operations.Any(op => op.Name == "Op2"));

                Assert.IsTrue(app.Roles.Count == 1);
                Assert.IsTrue(app.Roles.Any(op => op.Name == "Role1"));
                Assert.IsTrue(app.Roles["Role1"].AssignedOperations.Any());

                //Group1 wont be saved because there is no member.
                Assert.IsTrue(app.Groups.Count == 0);
                //Assert.IsTrue(app.Groups.Any(op => op.Name == "Group1"));

                Assert.IsTrue(app.Scopes.Count == 1);
                Assert.IsTrue(app.Scopes.Any(op => op.Name == "Scope1" && op.Description == "Scope1"));

                Assert.IsTrue(app.Scopes["Scope1"].Roles.Count > 0);
                Assert.IsTrue(app.Scopes["Scope1"].Roles["Role2"].AssignedRoles.Any());
                Assert.IsTrue(app.Scopes["Scope1"].Groups.Count == 1);
                Assert.IsTrue(app.Scopes["Scope1"].Groups.Any(op => op.Name == "Group2"));
                Assert.IsTrue(app.Scopes["Scope1"].Groups["Group2"].Members.Count > 0);

                Assert.IsTrue(app.Scopes["Scope1"].Groups["Group2"].Members.First().AssignedRoles.First().Name ==
                              "Role2");

                //Delete app.
                app.Delete();
                //The app has been removed out of the Application List.
                Assert.IsNull(azMan.Applications["AppNew"]);
            }

            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.Applications["AppNew"];
                Assert.IsNull(app);
            }
        }

        [TestMethod]
        [TestCategory("Fw.AzMan")]
        public void Add_Update_Delete_Operation()
        {
            var conn = new AzXmlConnectionStringBuilder {FileName = "TestData\\AzMan.xml"};
            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.Applications["AppNew1"];
                app?.Delete();
            }

            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.CreateApplication("AppNew1");

                //Create Operations
                app.Operations.Add(new AzOperation {Id = 1, Name = "Op1"});
                app.Operations.Add(new AzOperation {Id = 2, Name = "Op2"});
                app.Save();
            }

            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.Applications["AppNew1"];

                //Assert Creation
                Assert.IsTrue(app.Operations.Count == 2);
                Assert.IsTrue(app.Operations.Any(c => c.Name == "Op1"));
                Assert.IsTrue(app.Operations.Any(c => c.Name == "Op2"));

                //Delete Ops
                app.Operations.RemoveAt(0);
                //Update Ops
                app.Operations[0].Name = "Op22";
                app.Operations[0].Id = 22;
                app.Operations.Add(new AzOperation {Id = 3, Name = "Op3"});
                app.Save();
            }

            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.Applications["AppNew1"];
                //Assert Updates and Deletion.
                Assert.IsTrue(app.Operations.Count == 2);
                Assert.IsTrue(app.Operations.Any(c => c.Name == "Op22"));
                Assert.IsTrue(app.Operations.Any(c => c.Name == "Op3"));
            }
        }

        [TestMethod]
        [TestCategory("Fw.AzMan")]
        public void Add_Update_Delete_Role()
        {
            var conn = new AzXmlConnectionStringBuilder {FileName = "TestData\\AzMan.xml"};
            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.Applications["AppNew1"];
                app?.Delete();
            }

            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.CreateApplication("AppNew1");
                var op1 = new AzOperation {Id = 1, Name = "Op1"};
                app.Operations.Add(op1);
                app.Operations.Add(new AzOperation {Id = 2, Name = "Op2"});

                var role1 = new AzRole {Name = "Role1"};
                role1.AssignedOperations.Add(op1);
                app.Roles.Add(role1);

                var role2 = new AzRole {Name = "Role2"};
                role2.AssignedRoles.Add(role1);
                app.Roles.Add(role2);

                app.Save();
            }

            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.Applications["AppNew1"];

                Assert.IsTrue(app.Roles.Count == 2);
                Assert.IsTrue(app.Roles.Any(c => c.Name == "Role1"));
                Assert.IsTrue(app.Roles["Role1"].AssignedOperations.Any(c => c.Name == "Op1"));
                Assert.IsTrue(app.Roles["Role2"].AssignedRoles.Any(c => c.Name == "Role1"));

                app.Roles.RemoveAt(0);
                app.Roles[0].Name = "Role22";
                app.Roles[0].AssignedRoles.Clear();
                app.Roles[0].AssignedOperations.Add(app.Operations.First());

                var role3 = new AzRole {Name = "Role3"};
                role3.AssignedRoles.Add(app.Roles.First());
                app.Roles.Add(role3);
                app.Save();
            }

            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.Applications["AppNew1"];

                Assert.IsTrue(app.Roles.Count == 2);
                Assert.IsTrue(app.Roles.Any(c => c.Name == "Role22"));
                Assert.IsTrue(app.Roles.Any(c => c.Name == "Role3"));
            }
        }

        [TestMethod]
        [TestCategory("Fw.AzMan")]
        [Ignore]
        public void Add_Update_Delete_Group()
        {
            var conn = new AzXmlConnectionStringBuilder {FileName = "TestData\\AzMan.xml"};
            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.Applications["AppNew1"];
                app?.Delete();
            }

            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.CreateApplication("AppNew1");

                //Operations
                app.Operations.Add(new AzOperation {Id = 1, Name = "Op1"});
                app.Operations.Add(new AzOperation {Id = 2, Name = "Op2"});

                //Role
                var role1 = new AzRole {Name = "Role1"};
                role1.AssignedOperations.Add(app.Operations.First());
                app.Roles.Add(role1);

                var role2 = new AzRole {Name = "Role2"};
                role2.AssignedRoles.Add(role1);
                app.Roles.Add(role2);

                //Group
                var member1 = new AzMember {Name = "Administrator"};
                member1.AssignedRoles.Add(role1);

                var group1 = new AzGroup {Name = "Group1"};
                group1.Members.Add(member1);
                app.Groups.Add(group1);

                var member2 = new AzMember {Name = "Administrator"};
                member2.AssignedRoles.Add(role2);

                var member3 = new AzMember {Name = "Steven"};
                member3.AssignedRoles.Add(role2);

                var group2 = new AzGroup {Name = "Group2"};
                group2.Members.Add(member2);
                group2.Members.Add(member3);
                app.Groups.Add(group2);

                app.Save();
            }

            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.Applications["AppNew1"];

                //Assert Groups
                Assert.IsTrue(app.Groups.Count == 2);
                Assert.IsTrue(app.Groups.Any(g => g.Name == "Group1"));
                Assert.IsTrue(app.Groups.Any(g => g.Name == "Group2"));
                Assert.IsTrue(app.Groups["Group1"].Members.Count == 1);
                Assert.IsTrue(app.Groups["Group2"].Members.Count == 2);
                Assert.IsTrue(app.Groups.All(g => g.Members.All(m => m.AssignedRoles.Count == 1)));

                //Delete groups
                app.Groups.RemoveAt(0);

                //Update groups
                app.Groups["Group2"].Name = "Group22";
                //app.Groups["Group22"].Members.First(m => m.Name.EndsWith("Administrator", StringComparison.OrdinalIgnoreCase)).Name = "Steven";

                app.Save();
            }

            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.Applications["AppNew1"];

                Assert.IsTrue(app.Groups.Count == 1);
                Assert.IsTrue(app.Groups.Any(g => g.Name == "Group22"));
                Assert.IsTrue(app.Groups.All(g => g.Members.Count == 2));
                Assert.IsTrue(app.Groups.All(g => g.Members.Any(u => u.Name.EndsWith("Steven"))));
                Assert.IsTrue(app.Groups.All(g => g.Members.Any(u => u.Name.EndsWith("Steven"))));
                Assert.IsTrue(app.Groups.All(g => g.Members.All(m => m.AssignedRoles.Count == 1)));
            }
        }

        [TestMethod]
        [TestCategory("Fw.AzMan")]
        [Ignore]
        public void Add_Update_Delete_Scope()
        {
            var conn = new AzXmlConnectionStringBuilder {FileName = "TestData\\AzMan.xml"};
            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.Applications["AppNew1"];
                app?.Delete();
            }

            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.CreateApplication("AppNew1");

                //Operations
                app.Operations.Add(new AzOperation {Id = 1, Name = "Op1"});
                app.Operations.Add(new AzOperation {Id = 2, Name = "Op2"});

                //Role
                var role1 = new AzRole {Name = "Role1"};
                role1.AssignedOperations.Add(app.Operations.First());
                app.Roles.Add(role1);

                var scope1 = new AzScope {Name = "Scope1"};
                app.Scopes.Add(scope1);

                //Group
                var member1 = new AzMember {Name = "Administrator"};
                member1.AssignedRoles.Add(role1);

                var group1 = new AzGroup {Name = "Group1"};
                group1.Members.Add(member1);
                scope1.Groups.Add(group1);

                var member2 = new AzMember {Name = "Administrator"};
                member2.AssignedRoles.Add(role1);

                var member3 = new AzMember {Name = "Steven"};
                member3.AssignedRoles.Add(role1);

                var group2 = new AzGroup {Name = "Group2"};
                group2.Members.Add(member2);
                group2.Members.Add(member3);
                scope1.Groups.Add(group2);

                var scope2 = new AzScope {Name = "Scope2"};
                app.Scopes.Add(scope2);

                app.Save();
            }

            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.Applications["AppNew1"];

                //Assert Scope
                Assert.IsTrue(app.Scopes.Count == 2);
                Assert.IsTrue(app.Scopes.Any(s => s.Name == "Scope1"));
                Assert.IsTrue(app.Scopes.Any(s => s.Name == "Scope2"));

                Assert.IsTrue(app.Scopes["Scope1"].Groups.Count == 2);
                Assert.IsTrue(app.Scopes["Scope1"].Groups.Any(g => g.Name == "Group1"));
                Assert.IsTrue(app.Scopes["Scope1"].Groups.Any(g => g.Name == "Group2"));
                Assert.IsTrue(app.Scopes["Scope1"].Groups["Group1"].Members.Count == 1);
                Assert.IsTrue(app.Scopes["Scope1"].Groups["Group2"].Members.Count == 2);
                Assert.IsTrue(app.Scopes["Scope1"].Groups.All(g => g.Members.All(m => m.AssignedRoles.Count == 1)));

                //Delete Scope
                app.Scopes.Remove("Scope1");

                //Update Scope
                app.Scopes["Scope2"].Name = "Scope22";

                app.Save();
            }

            using (var azMan = new AzManContext(conn))
            {
                var app = azMan.Applications["AppNew1"];
                Assert.IsTrue(app.Scopes.Count == 1);
                Assert.IsTrue(app.Scopes.Any(s => s.Name == "Scope22"));
            }
        }
    }
}
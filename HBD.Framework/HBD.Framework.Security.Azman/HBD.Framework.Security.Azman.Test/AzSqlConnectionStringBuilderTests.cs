#region

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Security.Azman.Tests
{
    [TestClass]
    public class AzConnectionStringBuilderTests
    {
        [TestMethod]
        [TestCategory("Fw.AzMan")]
        public void AzSqlConnectionStringBuilder_Untrusted_Test()
        {
            var c =
                new AzSqlConnectionStringBuilder(
                    "mssql://Driver={SQL Server};Server={Server1};Uid=User;Pwd=Password;/Database/AzMan");
            Assert.AreEqual(c.ServerName, "Server1");
            Assert.AreEqual(c.UserName, "User");
            Assert.AreEqual(c.Password, "Password");
            Assert.AreEqual(c.DataBaseName, "Database");
            Assert.AreEqual(c.AzManStoreName, "AzMan");
        }

        [TestMethod]
        [TestCategory("Fw.AzMan")]
        public void AzSqlConnectionStringBuilder_Trusted_Test()
        {
            var c = new AzSqlConnectionStringBuilder("mssql://Driver={SQL Server};Server={Server1};/Database/AzMan");
            Assert.AreEqual(c.ServerName, "Server1");
            Assert.IsTrue(c.UserName.IsNull());
            Assert.IsTrue(c.Password.IsNull());
            Assert.AreEqual(c.DataBaseName, "Database");
            Assert.AreEqual(c.AzManStoreName, "AzMan");
        }

        [TestMethod]
        [TestCategory("Fw.AzMan")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AzSqlConnectionStringBuilder_ServerEmpty_Exception_Test()
        {
            var c = new AzSqlConnectionStringBuilder("mssql://Driver={SQL Server};Uid=User;Pwd=Password;/Database/AzMan");
        }

        [TestMethod]
        [TestCategory("Fw.AzMan")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AzSqlConnectionStringBuilder_DataBaseName_Exception_Test()
        {
            var c = new AzSqlConnectionStringBuilder("mssql://Driver={SQL Server};Server={Server1};//AzMan");
        }

        [TestMethod]
        [TestCategory("Fw.AzMan")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AzSqlConnectionStringBuilder_StoreNameEmpty_Exception_Test()
        {
            var c = new AzSqlConnectionStringBuilder("mssql://Driver={SQL Server};Server={Server1};/Database/");
        }

        [TestMethod]
        [TestCategory("Fw.AzMan")]
        public void AzSqlConnectionStringBuilder_BuilderTrusted_Test()
        {
            var c = new AzSqlConnectionStringBuilder
            {
                ServerName = "SV1",
                DataBaseName = "Db1",
                AzManStoreName = "Az1"
            };

            Assert.AreEqual(c.ConnectionString, "mssql://Driver={SQL Server};Server={SV1};/Db1/Az1");
        }

        [TestMethod]
        [TestCategory("Fw.AzMan")]
        public void AzSqlConnectionStringBuilder_BuilderUnTrusted_Test()
        {
            var c = new AzSqlConnectionStringBuilder
            {
                ServerName = "SV1",
                DataBaseName = "Db1",
                AzManStoreName = "Az1",
                UserName = "Us1",
                Password = "Pwd1"
            };

            Assert.AreEqual(c.ConnectionString, "mssql://Driver={SQL Server};Server={SV1};Uid=Us1;Pwd=Pwd1;/Db1/Az1");
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HBD.Framework.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace HBD.Framework.Data.Base.Tests
{
    [TestClass()]
    public class DataClientHelperTests
    {
        IDataReader reader;
        IDataParameterCollection parasCollection;
        IDbConnection conn;
        //IDataParameter param;
        DbDataAdapter adapter;
        IDbCommand command;

        [TestInitialize]
        public void Initilizer()
        {
            var pramsMock = new Mock<IDataParameterCollection>();
            pramsMock.Setup(p => p.Count).Returns(1);
            parasCollection = pramsMock.Object;

            var readerMock = new Mock<IDataReader>();
            readerMock.Setup(r => r.Dispose()).Callback(() => conn.Close());
            reader = readerMock.Object;

            var commMock = new Mock<IDbCommand>();
            commMock.Setup(c => c.Parameters).Returns(parasCollection);
            commMock.Setup(c => c.ExecuteNonQuery()).Verifiable();
            commMock.Setup(c => c.ExecuteReader()).Returns(reader).Verifiable();
            commMock.Setup(c => c.ExecuteReader(It.IsAny<CommandBehavior>())).Returns(reader).Verifiable();
            commMock.Setup(c => c.ExecuteScalar()).Verifiable();
            command = commMock.Object;

            var conMock = new Mock<IDbConnection>();
            conMock.Setup(c => c.Open()).Verifiable();
            conMock.Setup(c => c.Close()).Verifiable();
            conMock.Setup(c => c.Dispose()).Verifiable();
            conMock.Setup(c => c.CreateCommand()).Returns(command);
            conn = conMock.Object;

            var adapterMock = new Mock<DbDataAdapter>();
            adapterMock.Protected().Setup<DataTable>("FillSchema", ItExpr.IsAny<DataTable>(), ItExpr.IsAny<SchemaType>(), ItExpr.IsAny<IDbCommand>(), ItExpr.IsAny<CommandBehavior>())
                .Returns(new DataTable()).Verifiable();
            adapterMock.Protected().Setup<int>("Fill", ItExpr.IsAny<DataTable[]>(), ItExpr.IsAny<int>(), ItExpr.IsAny<int>(), ItExpr.IsAny<IDbCommand>(), ItExpr.IsAny<CommandBehavior>())
                .Returns(1).Verifiable();

            adapter = adapterMock.Object;
        }

        [TestCleanup]
        public void CleanUp()
        {
            conn?.Dispose();
            adapter?.Dispose();
            command?.Dispose();
        }

        [TestMethod()]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void Create_DataClientHelper_WithConnectionString_VerifyConnection_Test()
        {
            const string ctr = "Data Source=localhost";
            var helperMock = new Mock<DataClientAdapter>(ctr) { CallBase = true };
            helperMock.Protected().Setup<DbConnectionStringBuilder>("CreateConnectionString", ctr).Returns(new SqlConnectionStringBuilder(ctr)).Verifiable();

            var obj = helperMock.Object;
            helperMock.Protected().Verify<DbConnectionStringBuilder>("CreateConnectionString", Times.AtLeast(1),ctr);
        }

        [TestMethod()]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void Create_DataClientHelper_WitnConnectionString_VerifyBuildConnection_Test()
        {
            const string ctr = "DummyConnectionString";
            var helperMock = new Mock<DataClientAdapter>(ctr);
            helperMock.Protected().Setup<DbConnectionStringBuilder>("CreateConnectionString", ctr).Verifiable();
            helperMock.Protected().Setup<IDbConnection>("CreateConnection").Verifiable();

            var obj = helperMock.Object;

            helperMock.Protected().Verify("CreateConnectionString", Times.Once(), ctr);
            helperMock.Protected().Verify("CreateConnection", Times.Once());
        }

        [TestMethod()]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void Create_DataClientHelper_WitnIDbConnection_VerifyConnection_Test()
        {
            var helperMock = new Mock<DataClientAdapter>(conn);

            using (var h = helperMock.Object)
            {
                var priObj = new PrivateObject(h);
                Assert.AreEqual(conn, priObj.GetProperty("Connection"));
            }
        }

        [TestMethod()]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void Open_VerifyIDbConnectionOpen_Test()
        {
            Mock.Get(conn).Setup(c => c.Open()).Verifiable();

            var helperMock = new Mock<DataClientAdapter>(conn);
            using (var h = helperMock.Object) h.Open();
            Mock.Get(conn).Verify(c => c.Open(), Times.AtLeastOnce());
        }

        [TestMethod()]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void Close_VerifyIDbConnectionClose_Test()
        {
            Mock.Get(conn).Setup(c => c.State).Returns(ConnectionState.Open);
            Mock.Get(conn).Setup(c => c.Close()).Verifiable();

            var helperMock = new Mock<DataClientAdapter>(conn);
            using (var h = helperMock.Object)
            {
                Assert.AreEqual(ConnectionState.Open, h.State);
                h.Close();
            }
            Mock.Get(conn).Verify(c => c.Close(), Times.AtLeastOnce());
        }

        [TestMethod()]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void ExecuteNonQuery_With_Command_VerifyOpenCloseExecuteNonQuery_Test()
        {
            var helperMock = new Mock<DataClientAdapter>(conn) { CallBase = true };

            Mock.Get(conn).SetupGet(c => c.State).Returns(ConnectionState.Broken);

            using (var h = helperMock.Object)
                h.ExecuteNonQuery(command);

            Mock.Get(command).Verify(c => c.ExecuteNonQuery(), Times.Once());
            Mock.Get(conn).Verify(c => c.Open(), Times.Once());
            Mock.Get(conn).Verify(c => c.Close(), Times.Once());
        }

        [TestMethod()]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void ExecuteReader_With_Command_VerifyOpenCloseExecuteReader_Test()
        {
            Mock.Get(conn).SetupGet(c => c.State).Returns(ConnectionState.Broken);
            Mock.Get(command).Setup(c => c.Dispose()).Callback(() => conn.Close());

            var helperMock = new Mock<DataClientAdapter>(conn) { CallBase = true };


            using (var h = helperMock.Object)
            {
                var read = h.ExecuteReader(command);
                read.Dispose();
            }

            Mock.Get(command).Verify(c => c.ExecuteReader(It.IsAny<CommandBehavior>()), Times.Once());
            Mock.Get(conn).Verify(c => c.Open(), Times.Once());
            Mock.Get(conn).Verify(c => c.Close(), Times.Once());
        }

        [TestMethod()]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void ExecuteScalar_With_Command_VerifyOpenCloseExecuteScalar_Test()
        {
            var helperMock = new Mock<DataClientAdapter>(conn) { CallBase = true };
            Mock.Get(conn).SetupGet(c => c.State).Returns(ConnectionState.Broken);

            using (var h = helperMock.Object)
                h.ExecuteScalar(command);

            Mock.Get(command).Verify(c => c.ExecuteScalar(), Times.Once());
            Mock.Get(conn).Verify(c => c.Open(), Times.Once());
            Mock.Get(conn).Verify(c => c.Close(), Times.Once());
        }

        [TestMethod()]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void ExecuteNonQuery_With_String_Verify_Test()
        {
            var helperMock = new Mock<DataClientAdapter>(conn) { CallBase = true };
            helperMock.Setup(h => h.ExecuteNonQuery(command)).Verifiable();

            using (var h = helperMock.Object)
                h.ExecuteNonQuery("SELECT");

            helperMock.Verify(c => c.ExecuteNonQuery(command), Times.Once());
        }

        [TestMethod()]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void ExecuteReader_With_String_Verify_Test()
        {
            var helperMock = new Mock<DataClientAdapter>(conn) { CallBase = true };
            helperMock.Protected().Setup<IDataParameter>("CreateParameter", ItExpr.IsAny<string>(), ItExpr.IsAny<object>()).Returns(new SqlParameter());
            helperMock.Setup(h => h.ExecuteReader(command)).Verifiable();

            using (var h = helperMock.Object)
            {
                h.ExecuteReader("SELECT", new Dictionary<string, object> { { "123", 123 } });
                Assert.IsTrue(parasCollection.Count > 0);
            }

            helperMock.Verify(c => c.ExecuteReader(command), Times.Once());
        }

        [TestMethod()]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void ExecuteScalar_With_String_Verify_Test()
        {
            var helperMock = new Mock<DataClientAdapter>(conn) { CallBase = true };
            helperMock.Setup(h => h.ExecuteScalar(command)).Verifiable();

            using (var h = helperMock.Object)
                h.ExecuteScalar("SELECT");

            helperMock.Verify(c => c.ExecuteScalar(command), Times.Once());
        }

        //[TestMethod()]
        //[TestCategory("Fw.Data.Base.DataClientHelper")]
        //public void ExecuteNonQuery_With_Query_VerifyOpenCloseExecuteTable_Test()
        //{
        //    var helperMock = new Mock<DataClientHelper>(conn);
        //    helperMock.Protected().Setup<DbDataAdapter>("CreateAdapter", ItExpr.IsAny<string>(), ItExpr.IsAny<IDictionary<string, object>>())
        //        .Returns(adapter);
        //    helperMock.Setup(h => h.ExecuteTable(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).CallBase();

        //    using (var h = helperMock.Object)
        //        Assert.IsNotNull(h.ExecuteTable("SELECT"));

        //    helperMock.Protected().Verify("CreateAdapter", Times.Once(), ItExpr.IsAny<string>(), ItExpr.IsAny<IDictionary<string, object>>());
        //}

        [TestMethod()]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void DisposeTest()
        {
            Mock.Get(conn).Setup(c => c.Dispose()).Verifiable();

            var conMock = new Moq.Mock<DataClientAdapter>(conn) { CallBase = true };
            var h = conMock.Object;
            h.Dispose();

            var priObj = new PrivateObject(h);
            priObj.SetProperty("Connection", null);

            h.Dispose();
            Assert.AreEqual(ConnectionState.Broken, h.State);

            Mock.Get(conn).Verify(c => c.Dispose(), Times.Once());
        }
    }
}
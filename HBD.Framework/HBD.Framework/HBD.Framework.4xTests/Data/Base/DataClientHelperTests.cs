#region using

using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;

#endregion

namespace HBD.Framework.Data.Base.Tests
{
    [TestClass]
    public class DataClientHelperTests
    {
        private DbDataAdapter _adapter;
        IDbCommand _command;
        IDbConnection _conn;
        IDataParameterCollection _parasCollection;
        IDataReader _reader;

        [TestInitialize]
        public void Initilizer()
        {
            var pramsMock = new Mock<IDataParameterCollection>();
            pramsMock.Setup(p => p.Count).Returns(1);
            _parasCollection = pramsMock.Object;

            var readerMock = new Mock<IDataReader>();
            readerMock.Setup(r => r.Dispose()).Callback(() => _conn.Close());
            _reader = readerMock.Object;

            var commMock = new Mock<IDbCommand>();
            commMock.Setup(c => c.Parameters).Returns(_parasCollection);
            commMock.Setup(c => c.ExecuteNonQuery()).Verifiable();
            commMock.Setup(c => c.ExecuteReader()).Returns(_reader).Verifiable();
            commMock.Setup(c => c.ExecuteReader(It.IsAny<CommandBehavior>())).Returns(_reader).Verifiable();
            commMock.Setup(c => c.ExecuteScalar()).Verifiable();
            _command = commMock.Object;

            var conMock = new Mock<IDbConnection>();
            conMock.Setup(c => c.Open()).Verifiable();
            conMock.Setup(c => c.Close()).Verifiable();
            conMock.Setup(c => c.Dispose()).Verifiable();
            conMock.Setup(c => c.CreateCommand()).Returns(_command);
            _conn = conMock.Object;

            var adapterMock = new Mock<DbDataAdapter>();
            adapterMock.Protected()
                .Setup<DataTable>("FillSchema", ItExpr.IsAny<DataTable>(), ItExpr.IsAny<SchemaType>(),
                    ItExpr.IsAny<IDbCommand>(), ItExpr.IsAny<CommandBehavior>())
                .Returns(new DataTable()).Verifiable();
            adapterMock.Protected()
                .Setup<int>("Fill", ItExpr.IsAny<DataTable[]>(), ItExpr.IsAny<int>(), ItExpr.IsAny<int>(),
                    ItExpr.IsAny<IDbCommand>(), ItExpr.IsAny<CommandBehavior>())
                .Returns(1).Verifiable();

            _adapter = adapterMock.Object;
        }

        [TestCleanup]
        public void CleanUp()
        {
            _conn?.Dispose();
            _adapter?.Dispose();
            _command?.Dispose();
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void Create_DataClientHelper_WithConnectionString_VerifyConnection_Test()
        {
            const string ctr = "Data Source=localhost";
            var helperMock = new Mock<DataClientAdapter>(ctr) {CallBase = true};
            helperMock.Protected()
                .Setup<DbConnectionStringBuilder>("CreateConnectionString", ctr)
                .Returns(new SqlConnectionStringBuilder(ctr))
                .Verifiable();

            var obj = helperMock.Object;

            Assert.IsNotNull(obj.ConnectionString);
            helperMock.Protected().Verify<DbConnectionStringBuilder>("CreateConnectionString", Times.AtLeast(1), ctr);
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void Create_DataClientHelper_WitnConnectionString_VerifyBuildConnection_Test()
        {
            const string ctr = "DummyConnectionString";
            var helperMock = new Mock<DataClientAdapter>(ctr);
            helperMock.Protected().Setup<DbConnectionStringBuilder>("CreateConnectionString", ctr).Verifiable();
            helperMock.Protected().Setup<IDbConnection>("CreateConnection").Verifiable();

            var obj = helperMock.Object;

            Assert.IsNull(obj.ConnectionString);
            Assert.IsNull(obj.Connection);

            helperMock.Protected().Verify("CreateConnectionString", Times.Once(), ctr);
            helperMock.Protected().Verify("CreateConnection", Times.Once());
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void Create_DataClientHelper_WitnIDbConnection_VerifyConnection_Test()
        {
            var helperMock = new Mock<DataClientAdapter>(_conn);

            using (var h = helperMock.Object)
            {
                var priObj = new PrivateObject(h);
                Assert.AreEqual(_conn, priObj.GetProperty("Connection"));
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void Open_VerifyIDbConnectionOpen_Test()
        {
            Mock.Get(_conn).Setup(c => c.Open()).Verifiable();

            var helperMock = new Mock<DataClientAdapter>(_conn);
            using (var h = helperMock.Object)
            {
                h.Open();
            }
            Mock.Get(_conn).Verify(c => c.Open(), Times.AtLeastOnce());
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void Close_VerifyIDbConnectionClose_Test()
        {
            Mock.Get(_conn).Setup(c => c.State).Returns(ConnectionState.Open);
            Mock.Get(_conn).Setup(c => c.Close()).Verifiable();

            var helperMock = new Mock<DataClientAdapter>(_conn);
            using (var h = helperMock.Object)
            {
                Assert.AreEqual(ConnectionState.Open, h.State);
                h.Close();
            }
            Mock.Get(_conn).Verify(c => c.Close(), Times.AtLeastOnce());
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void ExecuteNonQuery_With_Command_VerifyOpenCloseExecuteNonQuery_Test()
        {
            var helperMock = new Mock<DataClientAdapter>(_conn) {CallBase = true};

            Mock.Get(_conn).SetupGet(c => c.State).Returns(ConnectionState.Broken);

            using (var h = helperMock.Object)
            {
                h.ExecuteNonQuery(_command);
            }

            Mock.Get(_command).Verify(c => c.ExecuteNonQuery(), Times.Once());
            Mock.Get(_conn).Verify(c => c.Open(), Times.Once());
            Mock.Get(_conn).Verify(c => c.Close(), Times.Once());
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void ExecuteReader_With_Command_VerifyOpenCloseExecuteReader_Test()
        {
            Mock.Get(_conn).SetupGet(c => c.State).Returns(ConnectionState.Broken);
            Mock.Get(_command).Setup(c => c.Dispose()).Callback(() => _conn.Close());

            var helperMock = new Mock<DataClientAdapter>(_conn) {CallBase = true};


            using (var h = helperMock.Object)
            {
                var read = h.ExecuteReader(_command);
                read.Dispose();
            }

            Mock.Get(_command).Verify(c => c.ExecuteReader(It.IsAny<CommandBehavior>()), Times.Once());
            Mock.Get(_conn).Verify(c => c.Open(), Times.Once());
            Mock.Get(_conn).Verify(c => c.Close(), Times.Once());
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void ExecuteScalar_With_Command_VerifyOpenCloseExecuteScalar_Test()
        {
            var helperMock = new Mock<DataClientAdapter>(_conn) {CallBase = true};
            Mock.Get(_conn).SetupGet(c => c.State).Returns(ConnectionState.Broken);

            using (var h = helperMock.Object)
            {
                h.ExecuteScalar(_command);
            }

            Mock.Get(_command).Verify(c => c.ExecuteScalar(), Times.Once());
            Mock.Get(_conn).Verify(c => c.Open(), Times.Once());
            Mock.Get(_conn).Verify(c => c.Close(), Times.Once());
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void ExecuteNonQuery_With_String_Verify_Test()
        {
            var helperMock = new Mock<DataClientAdapter>(_conn) {CallBase = true};
            helperMock.Setup(h => h.ExecuteNonQuery(_command)).Verifiable();

            using (var h = helperMock.Object)
            {
                h.ExecuteNonQuery("SELECT");
            }

            helperMock.Verify(c => c.ExecuteNonQuery(_command), Times.Once());
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void ExecuteReader_With_String_Verify_Test()
        {
            var helperMock = new Mock<DataClientAdapter>(_conn) {CallBase = true};
            helperMock.Protected()
                .Setup<IDataParameter>("CreateParameter", ItExpr.IsAny<string>(), ItExpr.IsAny<object>())
                .Returns(new SqlParameter());
            helperMock.Setup(h => h.ExecuteReader(_command)).Verifiable();

            using (var h = helperMock.Object)
            {
                h.ExecuteReader("SELECT", new Dictionary<string, object> {{"123", 123}});
                Assert.IsTrue(_parasCollection.Count > 0);
            }

            helperMock.Verify(c => c.ExecuteReader(_command), Times.Once());
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void ExecuteScalar_With_String_Verify_Test()
        {
            var helperMock = new Mock<DataClientAdapter>(_conn) {CallBase = true};
            helperMock.Setup(h => h.ExecuteScalar(_command)).Verifiable();

            using (var h = helperMock.Object)
            {
                h.ExecuteScalar("SELECT");
            }

            helperMock.Verify(c => c.ExecuteScalar(_command), Times.Once());
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base.DataClientHelper")]
        public void DisposeTest()
        {
            Mock.Get(_conn).Setup(c => c.Dispose()).Verifiable();

            var conMock = new Mock<DataClientAdapter>(_conn) {CallBase = true};
            var h = conMock.Object;
            h.Dispose();
           
            Assert.AreEqual(ConnectionState.Closed, h.State);

            Mock.Get(_conn).Verify(c => c.Dispose(), Times.Once());
        }
    }
}
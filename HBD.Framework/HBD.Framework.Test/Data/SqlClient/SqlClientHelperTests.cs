#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Fakes;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Linq;
using HBD.Framework.Data;
using HBD.Framework.Data.Csv;
using HBD.Framework.Data.SqlClient;
using HBD.Framework.Data.SqlClient.Base;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

#endregion

namespace HBD.Framework.Test.Data.SqlClient
{
    [TestClass]
    public class SqlClientHelperTests
    {
        private const string DummyConnection = "Data Source=localhost;Initial Catalog=Dummy;";

        private IDisposable _context;

        [TestInitialize]
        public void Initializer()
        {
            _context = ShimsContext.Create();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _context.Dispose();
        }

        //private SchemaInfo GetSchemaInfo()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        ShimDataClientHelper.AllInstances.ExecuteReaderStringIDictionaryOfStringObject
        //            = (helper, q, dic) =>
        //            {
        //                using (var excel = new ExcelAdapter("TestData\\DataBaseInfo.xlsx"))
        //                {
        //                    if (q.Contains("FROM INFORMATION_SCHEMA.COLUMNS"))
        //                        return excel["SchemaInfo"].ToDataTable(firstRowIsHeader: true).CreateDataReader();
        //                    if (q.Contains("TABLE_SCHEMA = SCHEMA_NAME(schema_id)"))
        //                        return excel["TableRowCounts"].ToDataTable(firstRowIsHeader: true).CreateDataReader();
        //                    if (q.Contains("SELECT MaxValue = CAST"))
        //                        return excel["MaxOfPrimaryKeys"].ToDataTable(firstRowIsHeader: true).CreateDataReader();
        //                }
        //                return null;
        //            };

        //        using (var conn = new SqlClientHelper(dummyConnection))
        //            return conn.GetSchemaInfo();
        //    }
        //}

        private static SchemaInfo GetSchemaInfo()
        {
            using (ShimsContext.Create())
            {
                //Verify the ExecuteNonQuery will call SqlCommand.ExecuteNonQuery
                ShimSqlCommand.AllInstances.ExecuteDbDataReaderCommandBehavior =
                    (s, c) =>
                    {
                        if (s.CommandText.ContainsIgnoreCase("FROM      Information_Schema.Columns C"))
                            return new CsvAdapter("TestData\\Northwind_SchemaInfo.csv").Read().CreateDataReader();

                        if (s.CommandText.ContainsIgnoreCase("SELECT MaxValue"))
                            return
                                new CsvAdapter("TestData\\Northwind_SchemaInfo_MaxValues.csv").Read().CreateDataReader();

                        return null;
                    };

                using (var conn = new SqlClientAdapter("Northwind"))
                {
                    return conn.GetSchemaInfo();
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.SqlClient")]
        public void SqlClientHelper_GetAllDataBaseName_Test()
        {
            using (ShimsContext.Create())
            {
                //Verify the ExecuteNonQuery will call SqlCommand.ExecuteNonQuery
                ShimSqlCommand.AllInstances.ExecuteDbDataReaderCommandBehavior =
                    (s, c) =>
                    {
                        if (!s.CommandText.ContainsIgnoreCase("SELECT [Name] as [Table_Catalog]")) return null;
                        if (!s.CommandText.ContainsIgnoreCase("FROM   Sys.Databases")) return null;

                        using (var ex = new CsvAdapter("TestData\\DataBaseInfo\\DataBaseInfo.csv"))
                        {
                            var dt = ex.ReadData();
                            return dt.CreateDataReader();
                        }
                    };

                using (var conn = new SqlClientAdapter("Northwind"))
                {
                    var dbnames = conn.GetDataBaseInfos();
                    Assert.IsTrue(dbnames.Count > 0);
                    Assert.IsTrue(dbnames.Any(n => n.Name.EqualsIgnoreCase("Northwind")));
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.SqlClient")]
        public void SqlClientHelperStringTest()
        {
            using (var conn = new SqlClientAdapter(DummyConnection))
            {
                var priObj = new PrivateObject(conn);
                Assert.IsNotNull(priObj.GetProperty("ConnectionString") as SqlConnectionStringBuilder);
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.SqlClient")]
        public void SqlClientHelperObjectTest()
        {
            var co = new SqlConnection();
            using (var conn = new SqlClientAdapter(co))
            {
                var priObj = new PrivateObject(conn);
                Assert.IsTrue(priObj.GetProperty("Connection") == co);
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.SqlClient")]
        public void GetSchemaInfoTest()
        {
            var schema = GetSchemaInfo();
            Assert.IsNotNull(schema);

            var columns = (from tb in schema.Tables
                from col in tb.Columns
                where col.IsPrimaryKey && !col.IsIdentity
                select col).ToList();

            Assert.IsTrue(columns.Any(c => c.MaxPrimaryKeyValue.IsNotNull()));
            Assert.IsTrue(schema.Tables.Any(t => t.RowCount > 0));
            Assert.AreEqual(13, schema.Tables.Count);

            Assert.AreEqual(4, schema.Tables["Categories"].Columns.Count);
            Assert.AreEqual(2, schema.Tables["CustomerCustomerDemo"].Columns.Count);
            Assert.AreEqual(2, schema.Tables["CustomerDemographics"].Columns.Count);
            Assert.AreEqual(11, schema.Tables["Customers"].Columns.Count);
            Assert.AreEqual(18, schema.Tables["Employees"].Columns.Count);
            Assert.AreEqual(2, schema.Tables["EmployeeTerritories"].Columns.Count);
            Assert.AreEqual(5, schema.Tables["Order Details"].Columns.Count);
            Assert.AreEqual(14, schema.Tables["Orders"].Columns.Count);
            Assert.AreEqual(10, schema.Tables["Products"].Columns.Count);
            Assert.AreEqual(2, schema.Tables["Region"].Columns.Count);
            Assert.AreEqual(3, schema.Tables["Shippers"].Columns.Count);
            Assert.AreEqual(12, schema.Tables["Suppliers"].Columns.Count);
            Assert.AreEqual(3, schema.Tables["Territories"].Columns.Count);

            Assert.IsTrue(schema.Views.Count > 0);
            Assert.IsTrue(schema.Views.All(v => v.Columns.Count > 0));
        }

        [TestMethod]
        [TestCategory("Fw.Data.SqlClient")]
        public void SchemaInfo_Dependence_Test()
        {
            var schema = GetSchemaInfo();
            Assert.IsNotNull(schema);

            //Check DependenceIndex
            Assert.AreEqual(0, schema.Tables["Categories"].DependenceIndex);
            Assert.AreEqual(0, schema.Tables["Region"].DependenceIndex);
            Assert.AreEqual(0, schema.Tables["Shippers"].DependenceIndex);
            Assert.AreEqual(0, schema.Tables["Suppliers"].DependenceIndex);
            Assert.AreEqual(0, schema.Tables["CustomerDemographics"].DependenceIndex);

            Assert.IsTrue(schema.Tables["CustomerCustomerDemo"].DependenceIndex >
                          schema.Tables["CustomerDemographics"].DependenceIndex
                          &&
                          schema.Tables["CustomerCustomerDemo"].DependenceIndex >
                          schema.Tables["Customers"].DependenceIndex);

            Assert.AreEqual(schema.Tables["Customers"].DependenceIndex, 0);
            Assert.AreEqual(schema.Tables["Employees"].DependenceIndex, 0);

            Assert.IsTrue(schema.Tables["EmployeeTerritories"].DependenceIndex >
                          schema.Tables["Employees"].DependenceIndex
                          &&
                          schema.Tables["EmployeeTerritories"].DependenceIndex >
                          schema.Tables["Territories"].DependenceIndex);

            Assert.IsTrue(schema.Tables["Order Details"].DependenceIndex > schema.Tables["Products"].DependenceIndex
                          && schema.Tables["Order Details"].DependenceIndex > schema.Tables["Orders"].DependenceIndex);

            Assert.IsTrue(schema.Tables["Products"].DependenceIndex > schema.Tables["Suppliers"].DependenceIndex
                          && schema.Tables["Products"].DependenceIndex > schema.Tables["Categories"].DependenceIndex);

            Assert.IsTrue(schema.Tables["Orders"].DependenceIndex > schema.Tables["Customers"].DependenceIndex
                          && schema.Tables["Orders"].DependenceIndex > schema.Tables["Employees"].DependenceIndex
                          && schema.Tables["Orders"].DependenceIndex > schema.Tables["Shippers"].DependenceIndex);

            Assert.IsTrue(schema.Tables["Territories"].DependenceIndex > schema.Tables["Region"].DependenceIndex);
        }

        [TestMethod]
        [TestCategory("Fw.Data.SqlClient")]
        public void ExecuteCommandTest()
        {
            //Fake IDbCommand and SqlConnection
            var command = new StubIDbCommand {ExecuteNonQuery = () => 1};
            var conn = new StubIDbConnection {StateGet = () => ConnectionState.Open};

            using (var s = new SqlClientAdapter(conn))
            {
                Assert.IsTrue(s.ExecuteNonQuery(command) == 1);
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.SqlClient")]
        public void ExecuteReaderTest()
        {
            //Fake IDbCommand and SqlConnection
            var command = new StubIDbCommand
                {ExecuteReaderCommandBehavior = con => new StubIDataReader {FieldCountGet = () => 10}};

            var sql = new StubIDbConnection {StateGet = () => ConnectionState.Open};

            using (var s = new SqlClientAdapter(sql))
            {
                Assert.IsTrue(s.ExecuteReader(command).FieldCount > 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.SqlClient")]
        public void ExecuteNonQueryTest()
        {
            //SqlConnection
            var sql = new StubIDbConnection
            {
                StateGet = () => ConnectionState.Open
            };
            sql.CreateCommand = () => new StubIDbCommand {ParametersGet = () => new StubIDataParameterCollection()};

            using (var s = new SqlClientAdapter(sql))
            {
                Assert.IsTrue(s.ExecuteNonQuery("Select * from A", new Dictionary<string, object> {{"@123", null}}) >= 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.SqlClient")]
        public void ExecuteScalarTest()
        {
            //SqlConnection
            var sql = new StubIDbConnection
            {
                StateGet = () => ConnectionState.Open
            };
            sql.CreateCommand = () => new StubIDbCommand {ParametersGet = () => new StubIDataParameterCollection()};

            using (var s = new SqlClientAdapter(sql))
            {
                Assert.IsNull(s.ExecuteScalar("Select * from A", new Dictionary<string, object> {{"@123", null}}));
            }
        }

        //[TestMethod()]
        //[TestCategory("Fw.Data.SqlClient")]
        //public void BuildAdapterTest()
        //{
        //    using (var conn = new SqlClientHelper(dummyConnection))
        //    {
        //        var priObj = new PrivateObject(conn);
        //        Assert.IsNotNull(priObj.Invoke("CreateAdapter", "Select * from A", null));
        //    }
        //}

        [TestMethod]
        [TestCategory("Fw.Data.SqlClient")]
        public void BuildSqlBulkCopyTest()
        {
            using (var conn = new SqlClientAdapter(DummyConnection))
            {
                var priObj = new PrivateObject(conn);
                Assert.IsNotNull(priObj.Invoke("CreateSqlBulkCopy"));
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.SqlClient")]
        public void BuildCommandTest()
        {
            using (var conn = new SqlClientAdapter(DummyConnection))
            {
                var priObj = new PrivateObject(conn);
                Assert.IsNotNull(priObj.Invoke("CreateCommand", "Select * from A",
                    new Dictionary<string, object> {{"@123", null}}));
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.SqlClient")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SchemaInfoNullTest()
        {
            using (var conn = new SqlClientAdapter(""))
            {
                var s = conn.GetSchemaInfo();
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.SqlClient")]
        public void DisposeTest()
        {
            var conn = new Mock<IDbConnection>();
            conn.Setup(c => c.Dispose()).Verifiable();

            var s = new SqlClientAdapter(conn.Object);
            s.Dispose();
            conn.Verify(c => c.Dispose(), Times.Once());
        }
    }
}
#region

using System.Collections.Generic;
using System.Data;
using HBD.Data.Comparisons.Base;
using HBD.QueryBuilders;
using HBD.QueryBuilders.Base;
using HBD.QueryBuilders.Context;
using HBD.QueryBuilders.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

#endregion

namespace HBD.SqlQueryBuilderTests.Context
{
    [TestClass]
    public class SqlBuilderContextTests
    {
        private const string dummyConnectionString = "Data Source=localhost;Initial Catalog=Dummy;";

        [TestMethod]
        [TestCategory("Fw.SqlBuilderContext")]
        public void WhereExtensionTest()
        {
            using (var sql = new SqlQueryBuilderContext(dummyConnectionString))
            {
                var select = sql.CreateSelectQuery();
                Assert.IsNotNull(select);

                select.Where(a => a.Field("A").Contains("ABC") & a.Field("B").Equals(123));

                var priObj = new PrivateObject(select);
                var cond = priObj.GetProperty("WhereConditions") as ICondition;

                Assert.IsNotNull(cond);
                Assert.IsNotNull(cond is BinaryCondition);
                Assert.IsNotNull(((BinaryCondition) cond).LeftCondition is FieldCondition);
                Assert.IsNotNull(((BinaryCondition) cond).RightCondition is FieldCondition);
                Assert.IsNotNull(((BinaryCondition) cond).Operation == BinaryOperation.And);

                select.Where(a => a.Field("AA").Equals(123) | a.Field("BB").Equals(345));
                cond = priObj.GetProperty("WhereConditions") as ICondition;

                Assert.IsNotNull(cond);
                Assert.IsNotNull(cond is BinaryCondition);
                Assert.IsNotNull(((BinaryCondition) cond).LeftCondition is BinaryCondition);
                Assert.IsNotNull(((BinaryCondition) cond).RightCondition is BinaryCondition);
                Assert.IsNotNull(((BinaryCondition) cond).Operation == BinaryOperation.And);
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlBuilderContext")]
        public void FieldsExtensionTest()
        {
            using (var sql = new SqlQueryBuilderContext(dummyConnectionString))
            {
                var select = sql.CreateSelectQuery();
                Assert.IsNotNull(select);

                select.Fields("A".As("BA"), "B".As("BB"))
                    .Fields("C", "D", "E")
                    .From("TB1");

                var priObj = new PrivateObject(select);
                var fields = priObj.GetProperty("Fields") as IList<Field>;
                Assert.IsNotNull(fields);
                Assert.IsTrue(fields.Count == 5);
                Assert.IsTrue(fields[0].Name == "A");
                Assert.IsTrue(fields[0].Alias == "BA");

                Assert.IsTrue(sql.Build(select).Validate().Count == 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlBuilderContext")]
        public void TableExtensionsTest()
        {
            using (var sql = new SqlQueryBuilderContext(dummyConnectionString))
            {
                var select = sql.CreateSelectQuery();
                select.Fields("A", "B")
                    .From(t => t.Field("ABC").As("A")
                            .LeftJoin("123").As("B").On(f => f.Field("A.A").EqualsField("B.B"))
                            .RightJoin("C").On(f => f.Field("A.C").EqualsField("C.ID"))
                    );

                var priObj = new PrivateObject(select);
                var tbs = priObj.GetProperty("Tables") as IList<Table>;

                Assert.IsNotNull(tbs);
                Assert.IsTrue(tbs.Count == 1);
                Assert.IsTrue(tbs[0].Joins.Count > 1);

                Assert.IsTrue(sql.Build(select).Validate().Count == 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlBuilderContext")]
        public void SelectQueryTest()
        {
            using (var sql = new SqlQueryBuilderContext(dummyConnectionString))
            {
                var select = sql.CreateSelectQuery();
                select.From(t => t.Field("A")
                            .LeftJoin("B").On(f => f.Field("A.ID").EqualsField("B.ID") | f.Field("A.ID").Equals(12))
                            .InnerJoin("C").On(f => f.Field("B.ID").EqualsField("C.ID") & f.Field("B.B").Equals(111))
                    )
                    .Where(c => c.Field("f1").Equals(123))
                    .GroupBy("f1")
                    .Having(c => c.Field("f2").Equals(123))
                    .OrderBy("f1")
                    .OrderByDescending("f2");

                var priObj = new PrivateObject(sql);
                var result = priObj.Invoke("Build", select) as QueryInfo;

                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Query);
                Assert.IsTrue(result.Query.Contains("SELECT"));
                Assert.IsTrue(result.Query.Contains("*"));
                Assert.IsTrue(result.Query.Contains("FROM"));
                Assert.IsTrue(result.Query.Contains("LEFT JOIN"));
                Assert.IsTrue(result.Query.Contains("ON"));
                Assert.IsTrue(result.Query.Contains("INNER JOIN"));
                Assert.IsTrue(result.Query.Contains("WHERE"));
                Assert.IsTrue(result.Query.Contains("GROUP BY"));
                Assert.IsTrue(result.Query.Contains("ORDER BY"));
                Assert.IsTrue(result.Query.Contains("HAVING"));
                Assert.IsTrue(result.Parameters.Count > 2);

                Assert.IsTrue(sql.Build(select).Validate().Count == 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlBuilderContext")]
        public void SelectQuery_Count_OrderByNewID_Test1()
        {
            using (var sql = new SqlQueryBuilderContext(dummyConnectionString))
            {
                var select = sql.CreateSelectQuery();
                select.Fields(f => f.Count()).From("A")
                    .OrderBy("NewId()");

                var result = select.Build();

                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Query);
                Assert.IsTrue(result.Query.Contains("SELECT"));
                Assert.IsTrue(result.Query.Contains("COUNT(*)"));
                Assert.IsTrue(result.Query.Contains("ORDER BY"));
                Assert.IsTrue(result.Query.Contains("NewId()"));
                Assert.IsFalse(result.Query.Contains("[NewId()]"));

                Assert.IsTrue(sql.Build(select).Validate().Count == 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlBuilderContext")]
        public void SelectQuery_Count_WhereNewID_Test()
        {
            using (var sql = new SqlQueryBuilderContext(dummyConnectionString))
            {
                var select = sql.CreateSelectQuery();
                select.Fields(f => f.Count()).From("A")
                    .Where(c => c.Field("A").EqualsField("NewId()"));

                var result = select.Build();

                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Query);
                Assert.IsTrue(result.Query.Contains("SELECT"));
                Assert.IsTrue(result.Query.Contains("COUNT(*)"));
                Assert.IsTrue(result.Query.Contains("WHERE"));
                Assert.IsTrue(result.Query.Contains("NewId()"));
                Assert.IsFalse(result.Query.Contains("[NewId()]"));

                Assert.IsTrue(sql.Build(select).Validate().Count == 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlBuilderContext")]
        public void SelectQuery_Count_GroupByNewID_Test()
        {
            using (var sql = new SqlQueryBuilderContext(dummyConnectionString))
            {
                var select = sql.CreateSelectQuery();
                select.Fields(f => f.Count()).From("A")
                    .GroupBy("NewId()");

                var result = select.Build();

                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Query);
                Assert.IsTrue(result.Query.Contains("SELECT"));
                Assert.IsTrue(result.Query.Contains("COUNT(*)"));
                Assert.IsTrue(result.Query.Contains("GROUP BY"));
                Assert.IsTrue(result.Query.Contains("NewId()"));
                Assert.IsFalse(result.Query.Contains("[NewId()]"));

                Assert.IsTrue(sql.Build(select).Validate().Count == 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlBuilderContext")]
        public void SelectQueryCustomFunctionTest()
        {
            using (var sql = new SqlQueryBuilderContext(dummyConnectionString))
            {
                var select = sql.CreateSelectQuery();
                select.Fields(f => f.Func("Function1", "ABC", 123))
                    .From("A")
                    .Where(c => c.Field("f1").Equals(123));

                var priObj = new PrivateObject(sql);
                var result = priObj.Invoke("Build", select) as QueryInfo;

                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Query);
                Assert.IsTrue(result.Query.Contains("SELECT"));
                Assert.IsTrue(result.Query.Contains("FUNCTION1(N'ABC',123)"));
                Assert.IsTrue(result.Query.Contains("FROM [dbo].[A]"));
                Assert.IsTrue(result.Query.Contains("WHERE"));
                Assert.IsTrue(result.Parameters.Count == 1);

                Assert.IsTrue(sql.Build(select).Validate().Count == 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlBuilderContext")]
        public void CreateInsertQueryTest()
        {
            using (var sql = new SqlQueryBuilderContext(dummyConnectionString))
            {
                var insert = sql.CreateInsertQuery();
                insert.Into("AA")
                    .Values("F1").By(1)
                    .Values("F2").By(2)
                    .Values("F3").By(3);

                var priObj = new PrivateObject(sql);
                var result = priObj.Invoke("Build", insert) as QueryInfo;

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Query.Contains("INSERT INTO"));
                Assert.IsTrue(result.Query.Contains("VALUES"));
                Assert.IsTrue(result.Parameters.Count == 3);

                Assert.IsTrue(sql.Build(insert).Validate().Count == 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlBuilderContext")]
        public void CreateUpdateQueryTest()
        {
            using (var sql = new SqlQueryBuilderContext(dummyConnectionString))
            {
                var update = sql.CreateUpdateQuery();
                update.Table("AA")
                    .Set("A").By(1)
                    .Set("B").By(2)
                    .Set("C").By(3)
                    .Where(f => f.Field("ID").Equals(10));

                var priObj = new PrivateObject(sql);
                var result = priObj.Invoke("Build", update) as QueryInfo;

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Query.Contains("UPDATE"));
                Assert.IsTrue(result.Query.Contains("SET"));
                Assert.IsTrue(result.Query.Contains("WHERE"));
                Assert.IsTrue(result.Parameters.Count == 4);

                Assert.IsTrue(sql.Build(update).Validate().Count == 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlBuilderContext")]
        public void CreateDeleteQueryTest()
        {
            using (var sql = new SqlQueryBuilderContext(dummyConnectionString))
            {
                var delete = sql.CreateDeleteQuery();
                delete.From("AA")
                    .Where(f => f.Field("ID").Equals(123));

                var priObj = new PrivateObject(sql);
                var result = priObj.Invoke("Build", delete) as QueryInfo;

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Query.Contains("DELETE"));
                Assert.IsTrue(result.Query.Contains("FROM"));
                Assert.IsTrue(result.Query.Contains("WHERE"));
                Assert.IsTrue(result.Parameters.Count == 1);

                Assert.IsTrue(sql.Build(delete).Validate().Count == 0);
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlBuilderContext")]
        public void Create_SqlBuilderContext_WithIDbConnection_Test()
        {
            var iDbObj = new Mock<IDbConnection>().Object;

            using (var sql = new SqlQueryBuilderContext(iDbObj))
            {
                var priObj = new PrivateObject(sql);

                Assert.AreEqual(iDbObj, priObj.GetFieldOrProperty("Connection") as IDbConnection);
                Assert.IsNotNull(priObj.GetFieldOrProperty("Provider"));
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlBuilderContext")]
        public void Create_SqlBuilderContext_WithIDbConnectionAndIBuilderProvider_Test()
        {
            var iDbObj = new Mock<IDbConnection>().Object;
            var builder = new Mock<IBuilderProvider>().Object;

            using (var sql = new SqlQueryBuilderContext(iDbObj, builder))
            {
                var priObj = new PrivateObject(sql);

                Assert.AreEqual(iDbObj, priObj.GetFieldOrProperty("Connection"));
                Assert.AreEqual(builder, priObj.GetFieldOrProperty("Provider"));
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlBuilderContext")]
        public void Create_SqlBuilderContext_WithConnectionStringAndIBuilderProvider_Test()
        {
            var builder = new Mock<IBuilderProvider>().Object;

            using (var sql = new SqlQueryBuilderContext(dummyConnectionString, builder))
            {
                var priObj = new PrivateObject(sql);
                Assert.AreEqual(builder, priObj.GetFieldOrProperty("Provider"));
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlBuilderContext")]
        public void ExecuteNonQuery_QueryBuilder_VerifyTheBaseMethod_Test()
        {
            var sqlMock = new Mock<SqlQueryBuilderContext>(dummyConnectionString) {CallBase = true};
            sqlMock.Setup(c => c.ExecuteNonQuery(It.IsNotNull<string>(), It.IsAny<IDictionary<string, object>>()))
                .Verifiable();

            using (var sql = sqlMock.Object)
            {
                var select = sql.CreateSelectQuery(); //Call Base
                select.From("A");

                sql.ExecuteNonQuery(select); //Call Base

                sqlMock.Verify(c => c.ExecuteNonQuery(It.IsNotNull<string>(), It.IsAny<IDictionary<string, object>>()));
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlBuilderContext")]
        public void ExecuteReader_QueryBuilder_VerifyTheBaseMethod_Test()
        {
            var sqlMock = new Mock<SqlQueryBuilderContext>(dummyConnectionString) {CallBase = true};
            sqlMock.Setup(c => c.ExecuteReader(It.IsNotNull<string>(), It.IsAny<IDictionary<string, object>>()))
                .Verifiable();

            using (var sql = sqlMock.Object)
            {
                var select = sql.CreateSelectQuery(); //Call Base
                select.From("A");

                sql.ExecuteReader(select); //Call Base

                sqlMock.Verify(c => c.ExecuteReader(It.IsNotNull<string>(), It.IsAny<IDictionary<string, object>>()));
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlBuilderContext")]
        public void ExecuteScalar_QueryBuilder_VerifyTheBaseMethod_Test()
        {
            var sqlMock = new Mock<SqlQueryBuilderContext>(dummyConnectionString) {CallBase = true};
            sqlMock.Setup(c => c.ExecuteScalar(It.IsNotNull<string>(), It.IsAny<IDictionary<string, object>>()))
                .Verifiable();

            using (var sql = sqlMock.Object)
            {
                var select = sql.CreateSelectQuery(); //Call Base
                select.From("A");

                sql.ExecuteScalar(select); //Call Base

                sqlMock.Verify(c => c.ExecuteScalar(It.IsNotNull<string>(), It.IsAny<IDictionary<string, object>>()));
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlBuilderContext")]
        public void ExecuteTable_QueryBuilder_VerifyTheBaseMethod_Test()
        {
            var sqlMock = new Mock<SqlQueryBuilderContext>(dummyConnectionString) {CallBase = true};
            sqlMock.Setup(c => c.ExecuteTable(It.IsNotNull<string>(), It.IsAny<IDictionary<string, object>>()))
                .Verifiable();

            using (var sql = sqlMock.Object)
            {
                var select = sql.CreateSelectQuery(); //Call Base
                select.From("A");

                sql.ExecuteTable(select); //Call Base

                sqlMock.Verify(c => c.ExecuteTable(It.IsNotNull<string>(), It.IsAny<IDictionary<string, object>>()));
            }
        }
    }
}
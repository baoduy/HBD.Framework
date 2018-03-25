#region

using HBD.Framework;
using HBD.Framework.Data.Csv;
using HBD.QueryBuilders;
using HBD.QueryBuilders.Base;
using HBD.QueryBuilders.Context;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

#endregion

namespace SqlQueryBuilder.RealDBTests
{
    [TestClass]
    public class MorthwindDbTests
    {
        private const string ConnectionName = "Northwind";

        [TestMethod]
        [TestCategory("Fw.SqlQueryBuilder.RealDb")]
        public void InsertTest()
        {
            using (ShimsContext.Create())
            {
                //Verify the ExecuteNonQuery will call SqlCommand.ExecuteNonQuery
                System.Data.SqlClient.Fakes.ShimSqlCommand.AllInstances.ExecuteNonQuery =
                    c =>
                    {
                        if (!c.CommandText.StartsWithIgnoreCase("INSERT INTO")) return 0;
                        if (!c.CommandText.ContainsIgnoreCase("[dbo].[Categories]([CategoryName])")) return 0;
                        if (!c.CommandText.ContainsIgnoreCase("VALUES (@CategoryName)")) return 0;
                        if (c.Parameters.Count != 1) return 0;
                        return 1;
                    };

                using (var sql = new SqlQueryBuilderContext(ConnectionName))
                {
                    var insert = sql.CreateInsertQuery();
                    insert.Into("Categories")
                        .Values("CategoryName").By("Category 1");

                    var v = sql.ExecuteNonQuery(insert);
                    Assert.IsTrue(v > 0);
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlQueryBuilder.RealDb")]
        public void UpdateTest()
        {
            //Insert first
            InsertTest();

            using (ShimsContext.Create())
            {
                //Verify the ExecuteNonQuery will call SqlCommand.ExecuteNonQuery
                System.Data.SqlClient.Fakes.ShimSqlCommand.AllInstances.ExecuteNonQuery =
                    c =>
                    {
                        if (!c.CommandText.StartsWithIgnoreCase("UPDATE [dbo].[Categories]")) return 0;
                        if (!c.CommandText.ContainsIgnoreCase("SET [CategoryName] = @CategoryName")) return 0;
                        if (!c.CommandText.ContainsIgnoreCase("WHERE ([CategoryName] LIKE @CategoryName1)")) return 0;
                        if (c.Parameters.Count != 2) return 0;
                        return 1;
                    };

                using (var sql = new SqlQueryBuilderContext(ConnectionName))
                {
                    var update = sql.CreateUpdateQuery();
                    update.Table("Categories")
                        .Set("CategoryName").By("Category U")
                        .Where(c => c.Field("CategoryName").Contains("Category"));

                    var v = sql.ExecuteNonQuery(update);
                    Assert.IsTrue(v > 0);
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlQueryBuilder.RealDb")]
        public void DeleteTest()
        {
            //Insert first
            InsertTest();

            using (ShimsContext.Create())
            {
                //Verify the ExecuteNonQuery will call SqlCommand.ExecuteNonQuery
                System.Data.SqlClient.Fakes.ShimSqlCommand.AllInstances.ExecuteNonQuery =
                    c =>
                    {
                        if (!c.CommandText.StartsWithIgnoreCase("DELETE FROM [dbo].[Categories]")) return 0;
                        if (!c.CommandText.ContainsIgnoreCase("WHERE ([CategoryName] LIKE @CategoryName)")) return 0;
                        if (c.Parameters.Count != 1) return 0;
                        return 1;
                    };

                using (var sql = new SqlQueryBuilderContext(ConnectionName))
                {
                    var delete = sql.CreateDeleteQuery();
                    delete.From("Categories")
                        .Where(c => c.Field("CategoryName").Contains("Category"));

                    var v = sql.ExecuteNonQuery(delete);
                    Assert.IsTrue(v > 0);
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlQueryBuilder.RealDb")]
        public void SelectTest()
        {
            using (ShimsContext.Create())
            {
                //Verify the ExecuteNonQuery will call SqlCommand.ExecuteNonQuery
                System.Data.SqlClient.Fakes.ShimSqlCommand.AllInstances.ExecuteDbDataReaderCommandBehavior =
                    (s, c) =>
                    {
                        if (!s.CommandText.StartsWithIgnoreCase("SELECT")) return null;
                        if (!s.CommandText.ContainsIgnoreCase("*")) return null;
                        if (!s.CommandText.ContainsIgnoreCase("FROM [dbo].[Categories]")) return null;
                        if (!s.CommandText.ContainsIgnoreCase("WHERE ([CategoryID] >= @CategoryID)")) return null;
                        if (s.Parameters.Count != 1) return null;

                        return new CsvAdapter("TestData\\Categories.csv").Read().CreateDataReader();
                    };

                using (var sql = new SqlQueryBuilderContext(ConnectionName))
                {
                    var select = sql.CreateSelectQuery();
                    select.From("Categories")
                        .Where(c => c.Field("CategoryID").GreaterThanOrEquals(1));

                    var data = sql.ExecuteTable(select);
                    Assert.IsNotNull(data);
                    Assert.IsTrue(data.Rows.Count > 0);
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlQueryBuilder.RealDb")]
        public void ComplexSelectTest()
        {
            using (ShimsContext.Create())
            {
                //Verify the ExecuteNonQuery will call SqlCommand.ExecuteNonQuery
                System.Data.SqlClient.Fakes.ShimSqlCommand.AllInstances.ExecuteDbDataReaderCommandBehavior =
                    (s, c) =>
                    {
                        if (!s.CommandText.StartsWithIgnoreCase("SELECT")) return null;
                        if (!s.CommandText.ContainsIgnoreCase("[E].[EmployeeID],[O].[OrderID],[O].[Freight]"))
                            return null;
                        if (!s.CommandText.ContainsIgnoreCase("FROM [dbo].[Employees][E]")) return null;
                        if (
                            !s.CommandText.ContainsIgnoreCase(
                                "INNER JOIN [dbo].[Orders][O] ON ([E].[EmployeeID] = [O].[EmployeeID])")) return null;
                        if (s.Parameters.Count <= 1) return null;

                        return new DataTable().CreateDataReader();
                    };

                using (var sql = new SqlQueryBuilderContext(ConnectionName))
                {
                    var select = sql.CreateSelectQuery();
                    select.From(t => t.Field("Employees").As("E")
                                .InnerJoin("Orders")
                                .As("O")
                                .On(j => j.Field("E.EmployeeID").EqualsField("O.EmployeeID"))
                                .InnerJoin("Order Details")
                                .As("D")
                                .On(j => j.Field("D.OrderID").EqualsField("O.OrderID"))
                        )
                        .Fields("E.EmployeeID", "O.OrderID", "O.Freight")
                        .Where(f => f.Field("E.TitleOfCourtesy").In("Ms.", "Dr.")
                                    & f.Field("D.OrderID").In(10258, 10270, 10275)
                        )
                        .GroupBy("E.EmployeeID", "O.OrderID", "O.Freight")
                        .Having(f => f.Field("E.EmployeeID").LessThan(5))
                        .OrderBy("E.EmployeeID");

                    var data = sql.ExecuteTable(select);
                    Assert.IsNotNull(data);
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.SqlQueryBuilder.RealDb")]
        public void SelectMaxTest()
        {
            using (ShimsContext.Create())
            {
                //Verify the ExecuteNonQuery will call SqlCommand.ExecuteNonQuery
                System.Data.SqlClient.Fakes.ShimSqlCommand.AllInstances.ExecuteDbDataReaderCommandBehavior =
                    (s, c) =>
                    {
                        if (!s.CommandText.StartsWithIgnoreCase("SELECT")) return null;
                        if (
                            !s.CommandText.ContainsIgnoreCase(
                                "MAX([O].[Freight]) [Max],MIN([O].[Freight]) [Min],SUM([O].[Freight]) [Sum],AVG([O].[Freight]) [Avg],[E].[EmployeeID],[O].[OrderID]"))
                            return null;
                        if (s.Parameters.Count <= 1) return null;

                        return new CsvAdapter("TestData\\Employees.csv").Read().CreateDataReader();
                    };

                using (var sql = new SqlQueryBuilderContext(ConnectionName))
                {
                    var select = sql.CreateSelectQuery();
                    select.Fields(f =>
                                new Field[]
                                {
                                    f.Max("O.Freight").As("Max"),
                                    f.Min("O.Freight").As("Min"),
                                    f.Sum("O.Freight").As("Sum"),
                                    f.Avg("O.Freight").As("Avg")
                                }
                        )
                        .Fields("E.EmployeeID", "O.OrderID")
                        .From(t => t.Field("Employees").As("E")
                                .InnerJoin("Orders")
                                .As("O")
                                .On(j => j.Field("E.EmployeeID").EqualsField("O.EmployeeID"))
                                .InnerJoin("Order Details")
                                .As("D")
                                .On(j => j.Field("D.OrderID").EqualsField("O.OrderID"))
                        )
                        .Where(f => f.Field("E.TitleOfCourtesy").In("Ms.", "Dr.")
                                    & f.Field("D.OrderID").In(10258, 10270, 10275)
                        )
                        .GroupBy("E.EmployeeID", "O.OrderID")
                        .Having(f => f.Field("E.EmployeeID").LessThan(5))
                        .OrderBy("E.EmployeeID");

                    var data = sql.ExecuteTable(select);
                    Assert.IsNotNull(data);
                    Assert.IsTrue(data.Rows.Count >= 3);
                }
            }
        }
    }
}
#region using

using System.Data;
using System.Data.SqlClient.Fakes;
using System.IO;
using HBD.Framework.Data.Csv;
using HBD.Framework.Data.SSDT;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HBD.Framework.Data;

#endregion

namespace HBD.Framework.Test.Data.SSDT
{
    [TestClass]
    public class MergeScriptGenerationTests
    {
        private const string ConnectionStringName =
            "Data Source=DESKTOP-PK6P1F4\\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True";

        [TestMethod]
        [TestCategory("Fw.Data.SSDT")]
        public void GenerateTest()
        {
            using (ShimsContext.Create())
            {
                //Verify the ExecuteNonQuery will call SqlCommand.ExecuteNonQuery
                ShimSqlCommand.AllInstances.ExecuteDbDataReaderCommandBehavior =
                    (s, c) =>
                    {
                        if (s.CommandText.ContainsIgnoreCase("Information_Schema.Columns C"))
                            return new CsvAdapter("TestData\\DataBaseInfo\\SchemaInfo.csv").Read().ToDataTable().CreateDataReader();

                        if (s.CommandText.ContainsIgnoreCase("SELECT MaxValue"))
                            return
                                new CsvAdapter("TestData\\DataBaseInfo\\MaxOfPrimaryKeys.csv").Read().ToDataTable().CreateDataReader();

                        if (s.CommandText.ContainsIgnoreCase("Categories"))
                            return new CsvAdapter("TestData\\Northwind\\Categories.csv").Read().ToDataTable().CreateDataReader();

                        if (s.CommandText.ContainsIgnoreCase("Customers"))
                            return new CsvAdapter("TestData\\Northwind\\Customers.csv").Read().ToDataTable().CreateDataReader();

                        if (s.CommandText.ContainsIgnoreCase("Employees"))
                            return new CsvAdapter("TestData\\Northwind\\Employees.csv").Read().ToDataTable().CreateDataReader();

                        return null;
                    };

                using (var merge = new SqlMergeScriptGeneration(ConnectionStringName))
                {
                    merge.Generate(MergeScriptOption.All, "dbo.Categories", "Customers", "dbo.[Employees]");
                    Assert.IsTrue(Directory.GetFiles(merge.OutputFolder).Length > 1);
                }
            }
        }

        [TestMethod]
        [TestCategory("Fw.Data.SSDT")]
        public void GenerateAllTest()
        {
            using (ShimsContext.Create())
            {
                //Verify the ExecuteNonQuery will call SqlCommand.ExecuteNonQuery
                ShimSqlCommand.AllInstances.ExecuteDbDataReaderCommandBehavior =
                    (s, c) =>
                    {
                        if (s.CommandText.ContainsIgnoreCase("Information_Schema.Columns C"))
                            return new CsvAdapter("TestData\\DataBaseInfo\\SchemaInfo.csv").Read().ToDataTable().CreateDataReader();

                        if (s.CommandText.ContainsIgnoreCase("SELECT MaxValue"))
                            return
                                new CsvAdapter("TestData\\DataBaseInfo\\MaxOfPrimaryKeys.csv").Read().ToDataTable().CreateDataReader();

                        if (s.CommandText.ContainsIgnoreCase("Categories"))
                            return new CsvAdapter("TestData\\Northwind\\Categories.csv").Read().ToDataTable().CreateDataReader();

                        if (s.CommandText.ContainsIgnoreCase("Customers"))
                            return new CsvAdapter("TestData\\Northwind\\Customers.csv").Read().ToDataTable().CreateDataReader();

                        if (s.CommandText.ContainsIgnoreCase("Employees"))
                            return new CsvAdapter("TestData\\Northwind\\Employees.csv").Read().ToDataTable().CreateDataReader();

                        if (s.CommandText.ContainsIgnoreCase("EmployeeTerritories"))
                            return
                                new CsvAdapter("TestData\\Northwind\\EmployeeTerritories.csv").Read().ToDataTable().CreateDataReader();

                        if (s.CommandText.ContainsIgnoreCase("Order Details"))
                            return new CsvAdapter("TestData\\Northwind\\Order Details.csv").Read().ToDataTable().CreateDataReader();

                        if (s.CommandText.ContainsIgnoreCase("Orders"))
                            return new CsvAdapter("TestData\\Northwind\\Orders.csv").Read().ToDataTable().CreateDataReader();

                        if (s.CommandText.ContainsIgnoreCase("Products"))
                            return new CsvAdapter("TestData\\Northwind\\Products.csv").Read().ToDataTable().CreateDataReader();

                        if (s.CommandText.ContainsIgnoreCase("Region"))
                            return new CsvAdapter("TestData\\Northwind\\Region.csv").Read().ToDataTable().CreateDataReader();

                        if (s.CommandText.ContainsIgnoreCase("Shippers"))
                            return new CsvAdapter("TestData\\Northwind\\Shippers.csv").Read().ToDataTable().CreateDataReader();

                        if (s.CommandText.ContainsIgnoreCase("Suppliers"))
                            return new CsvAdapter("TestData\\Northwind\\Suppliers.csv").Read().ToDataTable().CreateDataReader();

                        if (s.CommandText.ContainsIgnoreCase("Territories"))
                            return new CsvAdapter("TestData\\Northwind\\Territories.csv").Read().ToDataTable().CreateDataReader();

                        return new DataTable().CreateDataReader();
                    };

                using (var merge = new SqlMergeScriptGeneration(ConnectionStringName))
                {
                    //All
                    merge.OutputFolder = "Output/AllOption";
                    merge.GenerateAll(MergeScriptOption.All);
                    Assert.IsTrue(Directory.GetFiles(merge.OutputFolder).Length > 1);

                    //Insert Only
                    merge.OutputFolder = "Output/Insert";
                    merge.GenerateAll(MergeScriptOption.Insert);
                    Assert.IsTrue(Directory.GetFiles(merge.OutputFolder).Length > 1);

                    //Update Only
                    merge.OutputFolder = "Output/Update";
                    merge.GenerateAll(MergeScriptOption.Update);
                    Assert.IsTrue(Directory.GetFiles(merge.OutputFolder).Length > 1);

                    //Update Only
                    merge.OutputFolder = "Output/Delete";
                    merge.GenerateAll(MergeScriptOption.Delete);
                    Assert.IsTrue(Directory.GetFiles(merge.OutputFolder).Length > 1);

                    //Update Only
                    merge.OutputFolder = "Output/Default";
                    merge.GenerateAll();
                    Assert.IsTrue(Directory.GetFiles(merge.OutputFolder).Length > 1);
                }
            }
        }
    }
}
#region using

using System.Data;
using System.Linq;
using HBD.Framework.Data.SqlClient.Base;
using HBD.Framework.Data.SqlClient.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Data.Base.Tests
{
    [TestClass]
    public class SchemaInfoTests
    {
        private SchemaInfo CreateSchemaInfo()
        {
            var schema = new TestSchemaInfo("TEST");
            var tb1 = new TableInfo("Table1");
            schema.Tables.Add(tb1);
            tb1.Columns.AddRange(
                new[]
                {
                    new ColumnInfo {Name = "Id", IsPrimaryKey = true, DataType = SqlDbType.Int},
                    new ColumnInfo {Name = "Name"}
                });

            var tb5 = new TableInfo("Table5");
            schema.Tables.Add(tb5);
            tb5.Columns.AddRange(
                new[]
                {
                    new ColumnInfo {Name = "Id", IsPrimaryKey = true, DataType = SqlDbType.Int},
                    new ColumnInfo {Name = "Name"},
                    new ColumnInfo {Name = "Tb3Id"}
                });
            tb5.ForeignKeys.Add(new ForeignKeyInfo("1", tb5.Columns[2], new ReferencedColumnInfo("Table3", "Id")));

            var tb3 = new TableInfo("Table3");
            schema.Tables.Add(tb3);
            tb3.Columns.AddRange(
                new[]
                {
                    new ColumnInfo {Name = "Id", IsPrimaryKey = true, DataType = SqlDbType.Int},
                    new ColumnInfo {Name = "Name"},
                    new ColumnInfo {Name = "Tb2Id"}
                });
            tb3.ForeignKeys.Add(new ForeignKeyInfo("1", tb3.Columns[2], new ReferencedColumnInfo("Table2", "Id")));

            var tb4 = new TableInfo("Table4");
            schema.Tables.Add(tb4);
            tb4.Columns.AddRange(
                new[]
                {
                    new ColumnInfo {Name = "Id", IsPrimaryKey = true, DataType = SqlDbType.Int},
                    new ColumnInfo {Name = "Name"},
                    new ColumnInfo {Name = "Tb2Id"},
                    new ColumnInfo {Name = "Tb3Id"}
                });
            tb4.ForeignKeys.Add(new ForeignKeyInfo("1", tb4.Columns[2], new ReferencedColumnInfo("Table2", "Id")));
            tb4.ForeignKeys.Add(new ForeignKeyInfo("2", tb4.Columns[3], new ReferencedColumnInfo(tb3.Name, "Id")));

            var tb2 = new TableInfo("Table2");
            schema.Tables.Add(tb2);
            tb2.Columns.AddRange(
                new[]
                {
                    new ColumnInfo {Name = "Id", IsPrimaryKey = true, DataType = SqlDbType.Int},
                    new ColumnInfo {Name = "Name"},
                    new ColumnInfo {Name = "Tb1Id"}
                });
            tb2.ForeignKeys.Add(new ForeignKeyInfo("2", tb2.Columns[2], new ReferencedColumnInfo(tb1.Name, "Id")));

            return schema;
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        public void DependenceIndexTest()
        {
            var schema = CreateSchemaInfo();

            Assert.IsTrue(schema.Name == "TEST");
            Assert.IsTrue(schema.Tables.Count == 5);
            Assert.IsTrue(schema.Tables[0]?.Name == "Table1");
            Assert.IsTrue(schema.Tables[1]?.Name == "Table5");
            Assert.IsTrue(schema.Tables[2]?.Name == "Table3");
            Assert.IsTrue(schema.Tables[3]?.Name == "Table4");
            Assert.IsTrue(schema.Tables[4]?.Name == "Table2");

            Assert.IsTrue(schema.Tables["Table1"]?.Name == "Table1");
            Assert.IsTrue(schema.Tables["Table5"]?.Name == "Table5");
            Assert.IsTrue(schema.Tables["Table3"]?.Name == "Table3");
            Assert.IsTrue(schema.Tables["Table4"]?.Name == "Table4");
            Assert.IsTrue(schema.Tables["Table2"]?.Name == "Table2");

            Assert.IsTrue(schema.Tables["Table1"].ForeignKeys.Count == 0);
            Assert.IsTrue(schema.Tables["Table2"].ForeignKeys.Count == 1);
            Assert.IsTrue(schema.Tables["Table3"].ForeignKeys.Count == 1);
            Assert.IsTrue(schema.Tables["Table4"].ForeignKeys.Count == 2);
            Assert.IsTrue(schema.Tables["Table5"].ForeignKeys.Count == 1);

            var depenOnTb1 = schema.Tables["Table1"].DependenceTables;
            Assert.IsTrue(depenOnTb1.Count == 1);
            Assert.IsTrue(depenOnTb1[0].Name == "Table2");

            var depenOnTb2 = schema.Tables["Table2"].DependenceTables;
            Assert.IsTrue(depenOnTb2.Count == 2);
            Assert.IsTrue(depenOnTb2[0].Name == "Table3");
            Assert.IsTrue(depenOnTb2[1].Name == "Table4");

            //Check DependenceIndex
            Assert.IsTrue(schema.Tables["Table2"].DependenceIndex > schema.Tables["Table1"].DependenceIndex);
            Assert.IsTrue(schema.Tables["Table3"].DependenceIndex > schema.Tables["Table2"].DependenceIndex);
            Assert.IsTrue(schema.Tables["Table4"].DependenceIndex > schema.Tables["Table2"].DependenceIndex
                          && schema.Tables["Table4"].DependenceIndex > schema.Tables["Table3"].DependenceIndex);

            Assert.IsTrue(schema.Tables["Table5"].DependenceIndex > schema.Tables["Table2"].DependenceIndex);
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        public void SortByDependencesTest1()
        {
            var list = CreateSchemaInfo().Tables.SortByDependences();

            Assert.IsTrue(list[0].Name == "Table1");
            Assert.IsTrue(list[1].Name == "Table2");
            Assert.IsTrue(list[2].Name == "Table3");
            //Table4 and Table5 are the same DependenceIndex.
            Assert.IsTrue(list[3].Name == "Table4" || list[3].Name == "Table5");
            Assert.IsTrue(list[4].Name == "Table4" || list[4].Name == "Table5");
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        public void SortByDependencesTest2()
        {
            var list = CreateSchemaInfo().Tables.SortByDependences("Table0", "Table4", "Table5", "Table3", "Table1");

            Assert.IsTrue(list[0] == "Table0" || list[0] == "Table1");
            Assert.IsTrue(list[1] == "Table0" || list[1] == "Table1");
            Assert.IsTrue(list[2] == "Table3");
            //Table4 and Table5 are the same DependenceIndex.
            Assert.IsTrue(list[3] == "Table4" || list[3] == "Table5");
            Assert.IsTrue(list[4] == "Table4" || list[4] == "Table5");
        }

        [TestMethod]
        [TestCategory("Fw.Data.Base")]
        public void CreateDataTableTest()
        {
            var schema = CreateSchemaInfo();
            var tb = schema.Tables["Table4"];
            var data = tb.CreateDataTable();

            Assert.IsNull(((TableInfo) null).CreateDataTable());
            Assert.IsNotNull(data);
            Assert.IsTrue(data.TableName == "[dbo].[Table4]");
            Assert.IsTrue(data.Columns.Count == tb.Columns.Count);
            Assert.IsTrue(data.PrimaryKey.Length == tb.Columns.Count(c => c.IsPrimaryKey));
            Assert.IsTrue(data.PrimaryKey[0].ColumnName == tb.Columns.First(c => c.IsPrimaryKey).Name);
            Assert.IsTrue(data.PrimaryKey[0].DataType == typeof(int));
        }

        private class TestSchemaInfo : SchemaInfo
        {
            public TestSchemaInfo(string name) : base(name)
            {
            }
        }
    }
}
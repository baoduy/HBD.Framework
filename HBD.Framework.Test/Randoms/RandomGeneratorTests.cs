using HBD.Framework.Data.SqlClient;
using HBD.Framework.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Linq;

namespace HBD.Framework.Test
{
    [TestClass()]
    public class RandomGeneratorTests
    {
        [TestMethod()]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void StringTest()
        {
            Assert.AreEqual(12, RandomGenerator.String(12).Length);
            Assert.AreEqual(byte.MaxValue, RandomGenerator.String().Length);
            Assert.AreEqual(RandomGenerator.LimitLength, RandomGenerator.String(int.MaxValue).Length);
        }

        [TestMethod()]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void BooleanTest()
        {
            Assert.IsInstanceOfType(RandomGenerator.Boolean(), typeof(bool));
        }

        [TestMethod()]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void IntTest()
        {
            Assert.IsTrue(RandomGenerator.Int() >= int.MinValue + 1);
            Assert.IsTrue(RandomGenerator.Int() <= int.MaxValue - 1);
            Assert.IsTrue(RandomGenerator.Int(0, 5) <= 5);
            Assert.IsTrue(RandomGenerator.Int(0, 5) >= 0);
            Assert.IsTrue(RandomGenerator.Int(-1) <= -1);
            Assert.IsTrue(RandomGenerator.Int(10, 1) == 10);
        }

        [TestMethod()]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void DecimalTest()
        {
            Assert.IsTrue(RandomGenerator.Decimal() >= double.MinValue);
            Assert.IsTrue(RandomGenerator.Decimal() <= double.MaxValue);
            Assert.IsTrue(RandomGenerator.Decimal(0, 500) <= 500);
            Assert.IsTrue(RandomGenerator.Decimal(0, 500) >= 0);
            Assert.IsTrue(RandomGenerator.Decimal(-1) <= -1);
        }

        [TestMethod()]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void DateTimeTest()
        {
            Assert.IsTrue(RandomGenerator.DateTime() >= DateTime.MinValue);
            Assert.IsTrue(RandomGenerator.DateTime() <= DateTime.MaxValue);
            Assert.IsTrue(RandomGenerator.DateTime(DateTime.Today.AddDays(1)) <= DateTime.Today.AddDays(1));
            Assert.IsTrue(RandomGenerator.DateTime(DateTime.Today.AddDays(-10), DateTime.Today.AddDays(10)) >= DateTime.Today.AddDays(-10));
            Assert.IsTrue(RandomGenerator.DateTime(DateTime.Today.AddDays(-10), DateTime.Today.AddDays(10)) <= DateTime.Today.AddDays(10));
            Assert.IsTrue(RandomGenerator.DateTime(DateTime.Today.AddDays(10), DateTime.Today.AddDays(-10)) == DateTime.Today.AddDays(10));
        }

        [TestMethod()]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void ByteArrayTest()
        {
            Assert.IsTrue(RandomGenerator.ByteArray().Length <= 128);
            Assert.IsTrue(RandomGenerator.ByteArray(12).Length <= 12);
        }

        [TestMethod()]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void DataTableTest()
        {
            using (var data = RandomGenerator.DataTable())
            {
                Assert.IsTrue(data.Columns.Count > 1);
                Assert.IsTrue(data.Rows.Count > 1);
            }
        }

        [TestMethod()]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void DataTable_Null_Test()
        {
            using (var data = RandomGenerator.DataTable(null, -1))
            {
                Assert.IsTrue(data.Columns.Count > 1);
                Assert.AreEqual(1, data.Rows.Count);
            }
        }

        [TestMethod()]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void DataTable_NotNull_Test()
        {
            using (var data = new DataTable())
            {
                data.Columns.AddAutoIncrement("ID");
                data.Columns.Add("Name", typeof(string));

                RandomGenerator.DataTable(data, 100);
                Assert.AreEqual(100, data.Rows.Count);
            }
        }

        [TestMethod()]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void DataTable_100Rows_Test()
        {
            using (var data = RandomGenerator.DataTable(new System.Data.DataTable(), 100))
            {
                Assert.IsTrue(data.Columns.Count > 1);
                Assert.IsTrue(data.Rows.Count == 100);
            }
        }

        [TestMethod()]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void TableInfo_IntPrimary_Test()
        {
            var info = new Framework.Data.SqlClient.Base.TableInfo("A", "B");
            info.Columns.Add(new Framework.Data.SqlClient.Base.ColumnInfo { IsPrimaryKey = true, Name = "Col1", DataType = typeof(int).ToSqlDbType() });
            info.Columns.Add(new Framework.Data.SqlClient.Base.ColumnInfo { Name = "Col2", DataType = typeof(string).ToSqlDbType() });
            info.Columns.Add(new Framework.Data.SqlClient.Base.ColumnInfo { Name = "Col3", DataType = typeof(int).ToSqlDbType(), IsComputed = true, ComputedExpression = "Col1" });
            info.Columns.Add(new Framework.Data.SqlClient.Base.ColumnInfo { Name = "Col4", DataType = typeof(int).ToSqlDbType(), IsIdentity = true });

            using (var data = RandomGenerator.TableInfo(info, 100))
            {
                Assert.AreEqual(4, data.Columns.Count);
                Assert.AreEqual(100, data.Rows.Count);

                Assert.AreEqual(data.Rows[0][0], data.Rows[0][2]);
                Assert.IsTrue((int)data.Rows[0][3] >= 0);
            }
        }

        [TestMethod()]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void TableInfo_StringPrimary_Test()
        {
            var info = new Framework.Data.SqlClient.Base.TableInfo("A", "B");
            info.Columns.Add(new Framework.Data.SqlClient.Base.ColumnInfo { IsPrimaryKey = true, Name = "Col1", DataType = typeof(string).ToSqlDbType() });
            info.Columns.Add(new Framework.Data.SqlClient.Base.ColumnInfo { Name = "Col2", DataType = typeof(string).ToSqlDbType() });
            info.Columns.Add(new Framework.Data.SqlClient.Base.ColumnInfo { Name = "Col3", DataType = typeof(int).ToSqlDbType(), IsIdentity = true });
            info.Columns.Add(new Framework.Data.SqlClient.Base.ColumnInfo { Name = "Col4", DataType = typeof(int).ToSqlDbType(), IsComputed = true, ComputedExpression = "Col3" });

            using (var data = RandomGenerator.TableInfo(info, 100))
            {
                Assert.AreEqual(4, data.Columns.Count);
                Assert.AreEqual(100, data.Rows.Count);

                Assert.AreEqual(data.Rows[0][3], data.Rows[0][2]);
                Assert.IsTrue((int)data.Rows[0][3] >= 0);
            }
        }

        [TestMethod()]
        [TestCategory("Fw.Testing.RandomGenerator")]
        public void TableInfo_StringPrimaryMaxLengh1000_Test()
        {
            var info = new Framework.Data.SqlClient.Base.TableInfo("A", "B");
            info.Columns.Add(new Framework.Data.SqlClient.Base.ColumnInfo { IsPrimaryKey = true, Name = "Col1", DataType = typeof(string).ToSqlDbType(), MaxLengh = 1000 });
            info.Columns.Add(new Framework.Data.SqlClient.Base.ColumnInfo { Name = "Col2", DataType = typeof(string).ToSqlDbType() });

            using (var data = RandomGenerator.TableInfo(info, 100))
            {
                Assert.AreEqual(2, data.Columns.Count);
                Assert.AreEqual(100, data.Rows.Count);

                Assert.IsTrue(data.Rows.Cast<DataRow>().All(r => r.ItemArray.All(b => b.IsNotNullOrEmpty())));
            }
        }

        [TestMethod()]
        [TestCategory("Fw.Testing.RandomGenerator")]
        [ExpectedException(typeof(OutOfCapacityException))]
        public void TableInfo_IntPrimaryMax_Test()
        {
            var info = new Framework.Data.SqlClient.Base.TableInfo("A", "B");
            info.Columns.Add(new Framework.Data.SqlClient.Base.ColumnInfo { IsPrimaryKey = true, Name = "Col1", DataType = typeof(int).ToSqlDbType(), MaxPrimaryKeyValue = int.MaxValue });

            using (var data = RandomGenerator.TableInfo(info, 100)) { }
        }

        [TestMethod()]
        [TestCategory("Fw.Testing.RandomGenerator")]
        [ExpectedException(typeof(OutOfCapacityException))]
        public void TableInfo_StringPrimaryMax_Test()
        {
            var info = new Framework.Data.SqlClient.Base.TableInfo("A", "B");
            info.Columns.Add(new Framework.Data.SqlClient.Base.ColumnInfo { IsPrimaryKey = true, Name = "Col1", DataType = typeof(string).ToSqlDbType(), MaxPrimaryKeyValue = "ZZZZ", MaxLengh = 4 });

            using (var data = RandomGenerator.TableInfo(info, 100)) { }
        }
    }
}
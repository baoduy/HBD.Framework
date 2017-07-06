#region using

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using HBD.Framework.Data;
using HBD.Framework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.AutoMapping.Tests
{
    [TestClass]
    public class MapperExtensionsTests
    {
        private DataTable data;
        private IList<TestItem3> results;

        [TestInitialize]
        public void CreateData()
        {
            data = new DataTable();
            data.Columns.Add("Name");
            data.Columns.Add("Id");
            data.Columns.Add("Type");
            data.Columns.Add("Level");
            data.Columns.Add("Summ");

            data.Rows.Add("Hoang", 1, "Enum2", null, "AA");
            data.Rows.Add("Duy", 2, TestEnum.Enum2, 123, "BB");
            data.Rows.Add();
        }

        [TestCleanup]
        public void CleanUp()
        {
            data?.Dispose();
            data = null;
            results?.Clear();
            results = null;
        }

        private void AssertResults(bool ignoreEmptyRows = true)
        {
            if (ignoreEmptyRows)
            {
                Assert.AreEqual(2, results.Count);
            }
            else
            {
                Assert.AreEqual(3, results.Count);
                Assert.IsTrue(results.Any(e => e.Name.IsNull() && e.Id == 0 && e.Type == TestEnum.Enum1));
            }

            Assert.IsTrue(
                results.Any(e => e.Name == "Hoang" && e.Id == 1 && e.Type == TestEnum.Enum2 && e.Summary == "AA"));
            Assert.IsTrue(results.Any(e => e.Name == "Duy" && e.Id == 2 && e.Type == TestEnum.Enum2 && e.Summary == "BB"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataReader_MappingTo_Exception_Test()
        {
            results = ((IDataReader) null).MappingTo<TestItem3>().ToList();
        }

        [TestMethod]
        public void DataReader_MappingTo_Entity_IgnoreEmptyRow_Test()
        {
            results = data.CreateDataReader().MappingTo<TestItem3>(true).ToList();
            AssertResults(true);
        }

        [TestMethod]
        public async Task DataReaderAsync_MappingTo_Entity_Test()
        {
            var r = await data.CreateDataReader().MappingToAsync<TestItem3>(true);
            results = r.ToList();
            AssertResults(true);
        }

        [TestMethod]
        public void DataReader_MappingTo_Entity_NotIgnoreEmptyRow_Test()
        {
            results = data.CreateDataReader().MappingTo<TestItem3>(false).ToList();
            AssertResults(false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataTable_MappingTo_Exception_Test()
        {
            var list = ((DataTable) null).MappingTo<TestItem3>().ToList();
        }

        [TestMethod]
        public void DataTable_MappingTo_Entity_IgnoreEmptyRow_Test()
        {
            results = data.MappingTo<TestItem3>(true).ToList();
            AssertResults(true);
        }

        [TestMethod]
        public void DataTable_MappingTo_Entity_NotIgnoreEmptyRow_Test()
        {
            results = data.MappingTo<TestItem3>(false).ToList();
            AssertResults(false);
        }

        [TestMethod]
        public async Task DataTableAsync_MappingTo_Entity_Test()
        {
            var r = await data.MappingToAsync<TestItem3>();
            results = r.ToList();
            AssertResults();
        }
    }
}
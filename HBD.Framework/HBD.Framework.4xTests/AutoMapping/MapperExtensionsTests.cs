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
        private DataTable _data;
        private IList<TestItem3> _results;

        [TestInitialize]
        public void CreateData()
        {
            _data = new DataTable();
            _data.Columns.Add("Name");
            _data.Columns.Add("Id");
            _data.Columns.Add("Type");
            _data.Columns.Add("Level");
            _data.Columns.Add("Summ");

            _data.Rows.Add("Hoang", 1, "Enum2", null, "AA");
            _data.Rows.Add("Duy", 2, TestEnum.Enum2, 123, "BB");
            _data.Rows.Add();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _data?.Dispose();
            _data = null;
            _results?.Clear();
            _results = null;
        }

        private void AssertResults(bool ignoreEmptyRows = true)
        {
            if (ignoreEmptyRows)
            {
                Assert.AreEqual(2, _results.Count);
            }
            else
            {
                Assert.AreEqual(3, _results.Count);
                Assert.IsTrue(_results.Any(e => e.Name.IsNull() && e.Id == 0 && e.Type == TestEnum.Enum1));
            }

            Assert.IsTrue(
                _results.Any(e => e.Name == "Hoang" && e.Id == 1 && e.Type == TestEnum.Enum2 && e.Summary == "AA"));
            Assert.IsTrue(_results.Any(e => e.Name == "Duy" && e.Id == 2 && e.Type == TestEnum.Enum2 && e.Summary == "BB"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataReader_MappingTo_Exception_Test()
        {
            _results = ((IDataReader) null).MappingTo<TestItem3>().ToList();
        }

        [TestMethod]
        public void DataReader_MappingTo_Entity_IgnoreEmptyRow_Test()
        {
            _results = _data.CreateDataReader().MappingTo<TestItem3>(true).ToList();
            AssertResults(true);
        }

        [TestMethod]
        public async Task DataReaderAsync_MappingTo_Entity_Test()
        {
            var r = await _data.CreateDataReader().MappingToAsync<TestItem3>(true);
            _results = r.ToList();
            AssertResults(true);
        }

        [TestMethod]
        public void DataReader_MappingTo_Entity_NotIgnoreEmptyRow_Test()
        {
            _results = _data.CreateDataReader().MappingTo<TestItem3>(false).ToList();
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
            _results = _data.MappingTo<TestItem3>(true).ToList();
            AssertResults(true);
        }

        [TestMethod]
        public void DataTable_MappingTo_Entity_NotIgnoreEmptyRow_Test()
        {
            _results = _data.MappingTo<TestItem3>(false).ToList();
            AssertResults(false);
        }

        [TestMethod]
        public async Task DataTableAsync_MappingTo_Entity_Test()
        {
            var r = await _data.MappingToAsync<TestItem3>();
            _results = r.ToList();
            AssertResults();
        }
    }
}
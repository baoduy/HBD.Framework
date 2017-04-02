#region using

using System.Linq;
using HBD.Framework.Exceptions;
using HBD.Framework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Collections.Tests
{
    [TestClass]
    public class DistinctCollectionTests
    {
        [TestMethod]
        [ExpectedException(typeof(DuplicatedException))]
        public void AddTest_Exception_WhenAddDuplicateKey()
        {
            var coll = new DistinctCollection<int, TestItem>(i => i.Id);
            coll.Add(new TestItem {Id = 1});
            coll.Add(new TestItem {Id = 1});
        }

        [TestMethod]
        public void ClearTest()
        {
            var coll = new DistinctCollection<int, TestItem>(i => i.Id);
            coll.Add(new TestItem {Id = 1});
            coll.Add(new TestItem {Id = 2});

            Assert.IsTrue(coll.Count == 2);
            coll.Clear();
            Assert.IsTrue(coll.Count == 0);
        }

        [TestMethod]
        public void ContainsTest()
        {
            var coll = new DistinctCollection<int, TestItem>(i => i.Id);
            var i1 = new TestItem {Id = 1};
            coll.Add(i1);
            coll.Add(new TestItem {Id = 2});

            Assert.IsTrue(coll.Contains(i1));
        }

        [TestMethod]
        public void ContainsKeyTest()
        {
            var coll = new DistinctCollection<int, TestItem>(i => i.Id);
            coll.Add(new TestItem {Id = 1});
            coll.Add(new TestItem {Id = 2});

            Assert.IsTrue(coll.ContainsKey(1));
        }

        [TestMethod]
        public void CopyToTest()
        {
            var coll = new DistinctCollection<int, TestItem>(i => i.Id);
            coll.Add(new TestItem {Id = 1});
            coll.Add(new TestItem {Id = 2});

            var arrary = new TestItem[2];
            coll.CopyTo(arrary, 0);

            Assert.IsFalse(arrary.Any(a => a.IsNull()));
        }

        [TestMethod]
        public void GetEnumeratorTest()
        {
            var coll = new DistinctCollection<int, TestItem>(i => i.Id);
            coll.Add(new TestItem {Id = 1});
            coll.Add(new TestItem {Id = 2});

            Assert.IsFalse(coll.Any(a => a.IsNull()));
        }

        [TestMethod]
        public void RemoveTest()
        {
            var coll = new DistinctCollection<int, TestItem>(i => i.Id);
            var i1 = new TestItem {Id = 1};
            coll.Add(i1);
            coll.Add(new TestItem {Id = 2});
            coll.Remove(i1);

            Assert.IsFalse(coll.ContainsKey(1));
        }

        [TestMethod]
        public void RemoveAtTest()
        {
            var coll = new DistinctCollection<int, TestItem>(i => i.Id);
            coll.Add(new TestItem {Id = 1});
            coll.Add(new TestItem {Id = 2});
            coll.RemoveAt(1);

            Assert.IsFalse(coll.ContainsKey(2));
        }
    }
}
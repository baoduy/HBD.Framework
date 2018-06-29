#region using

using System.Collections.Generic;
using System.Linq;
using HBD.Framework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Collections.Tests
{
    [TestClass]
    public class LazyListTests
    {
        private int _callCount;
        private bool _canLoad = true;

        [TestCleanup]
        public void TestCleanUp()
            => _callCount = 0;

        private IEnumerable<TestItem> GetItems()
        {
            _callCount++;
            for (var i = 0; i < 100; i++)
                yield return new TestItem {Id = i, Name = i.ToString()};
        }

        private bool CanLoad() => _canLoad;

        [TestMethod]
        public void LazyListTest()
        {
            var list = new LazyList<TestItem>(GetItems);
            Assert.IsTrue(list.Count == 100);
            Assert.IsTrue(list.Count == 100);
            Assert.IsTrue(_callCount == 1);
        }

        [TestMethod]
        public void GetEnumeratorTest()
        {
            var list = new LazyList<TestItem>(GetItems);

            Assert.IsFalse(list.Any(i => i.IsNull()));
            Assert.IsFalse(list.Any(i => i.IsNull()));
            Assert.IsTrue(_callCount == 1);
        }

        [TestMethod]
        public void AddTest()
        {
            var list = new LazyList<TestItem>(GetItems) {new TestItem {Id = 101}};

            Assert.IsTrue(list.Count == 101);
            Assert.IsTrue(list.Count == 101);
            Assert.IsTrue(_callCount == 1);
        }

        [TestMethod]
        public void ClearTest()
        {
            var list = new LazyList<TestItem>(GetItems);
            Assert.IsTrue(list.Count >= 100);
            list.Clear();

            Assert.IsTrue(_callCount == 1);
            Assert.IsTrue(list.Count == 0);
        }

        [TestMethod]
        public void ClearAndResetTest()
        {
            var list = new LazyList<TestItem>(GetItems);
            Assert.IsTrue(list.Count >= 100);

            list.ClearAndReset();

            Assert.IsTrue(list.Count >= 100);
            Assert.IsTrue(_callCount == 2);
        }

        [TestMethod]
        public void ContainsTest()
        {
            var list = new LazyList<TestItem>(GetItems);
            Assert.IsTrue(list.Contains(list[0]));
            Assert.IsTrue(_callCount == 1);
        }

        [TestMethod]
        public void CopyToTest()
        {
            var array = new TestItem[100];
            var list = new LazyList<TestItem>(GetItems);
            list.CopyTo(array, 0);

            Assert.IsTrue(array.All(i => !i.IsNull()));
            Assert.IsTrue(_callCount == 1);
        }

        [TestMethod]
        public void RemoveTest()
        {
            var list = new LazyList<TestItem>(GetItems);
            var item = list.First();
            list.Remove(item);

            Assert.IsTrue(list.Count == 99);
            Assert.IsTrue(_callCount == 1);
        }

        [TestMethod]
        public void IndexOfTest()
        {
            var list = new LazyList<TestItem>(GetItems);
            var item = list.Last();

            Assert.IsTrue(list.IndexOf(item) == 99);
            Assert.IsTrue(_callCount == 1);
        }

        [TestMethod]
        public void InsertTest()
        {
            var list = new LazyList<TestItem>(GetItems);
            var item = new TestItem();
            list.Insert(10, item);

            Assert.IsTrue(list.IndexOf(item) == 10);
            Assert.IsTrue(_callCount == 1);
        }

        [TestMethod]
        public void RemoveAtTest()
        {
            var list = new LazyList<TestItem>(GetItems);
            list.RemoveAt(10);

            Assert.IsTrue(list.Count == 99);
            Assert.IsTrue(_callCount == 1);
        }

        [TestMethod]
        public void CanLoadTest()
        {
            var list = new LazyList<TestItem>(GetItems, CanLoad);

            _canLoad = false;
            Assert.IsTrue(list.Count == 0);

            _canLoad = true;
            Assert.IsTrue(list.Count == 100);
            Assert.IsTrue(_callCount == 1);
        }
    }
}
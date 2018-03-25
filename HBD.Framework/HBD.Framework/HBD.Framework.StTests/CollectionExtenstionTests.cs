#region using

using System;
using System.Collections.Generic;
using System.Linq;
using HBD.Framework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Tests
{
    [TestClass]
    public class CollectionExtenstionTests
    {
        [TestMethod]
        [TestCategory("Fw.Extensions")]
        [ExpectedException(typeof(NotSupportedException))]
        public void AddRange_WithReadOnlyArray_Test()
        {
            new object[] { }.AddRange(new object[] { });
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void AddRange_WithNullArray_Test()
        {
            IList<object> list = new List<object>();
            list.AddRange(null);
            Assert.IsTrue(list.Count == 0);
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void HasDuplicatedItemTest()
        {
            IList<TestItem> list = null;
            Assert.IsFalse(list.DuplicatedItems(t => t.Name));
            Assert.IsFalse(list.DuplicatedItems(t => t.Id));

            list = new List<TestItem> {new TestItem {Name = "A"}, new TestItem {Name = "A"}};
            Assert.IsTrue(list.DuplicatedItems(t => t.Name));
            Assert.IsFalse(list.DuplicatedItems(null));

            list = new List<TestItem> {new TestItem {Name = "A"}};
            Assert.IsFalse(list.DuplicatedItems(t => t.Name));

            list = new List<TestItem> {new TestItem {Id = 1}, new TestItem {Id = 1}};
            Assert.IsTrue(list.DuplicatedItems(t => t.Id));
            Assert.IsFalse(list.DuplicatedItems(null));

            list = new List<TestItem> {new TestItem {Id = 1}};
            Assert.IsFalse(list.DuplicatedItems(t => t.Id));
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void MergeFrom_ListToList1_Test()
        {
            var list1 = new List<TestItem> {new TestItem {Name = "A"}};
            var list2 = new List<TestItem> {new TestItem {Name = "A"}, new TestItem {Name = "B"}};

            list1.MergeFrom(null, null);
            Assert.IsTrue(list1.Count == 1);

            list1.MergeFrom(list2, null);
            Assert.IsTrue(list1.Count == 1);

            list1.MergeFrom(null, t => t.Name);
            Assert.IsTrue(list1.Count == 1);

            list1.MergeFrom(list2, t => t.Id);
            Assert.IsTrue(list1.Count == 1);

            list1.MergeFrom(list2, t => t.Name);
            Assert.IsTrue(list1.Count == 2);
            Assert.IsFalse(list1.DuplicatedItems(t => t.Name));
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void MergeFrom_ListToList2_Test()
        {
            var list1 = new List<TestItem> {new TestItem {Id = 1, Name = "A"}};
            var list2 = new List<TestItem>
            {
                new TestItem {Id = 2, Name = "A"},
                new TestItem {Id = 3, Name = "B"},
                new TestItem {Id = 3, Name = "C"}
            };

            list1.MergeFrom(null, null);
            Assert.IsTrue(list1.Count == 1);

            list1.MergeFrom(list2, null);
            Assert.IsTrue(list1.Count == 1);

            list1.MergeFrom(list2, t => t.Id);
            Assert.IsTrue(list1.Count == 3);
            Assert.IsFalse(list1.DuplicatedItems(t => t.Id));
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void MergeFrom_DicToList_Test()
        {
            var list1 = new List<TestItem> {new TestItem {Name = "A"}};
            var list2 = new Dictionary<string, TestItem>
            {
                {"A", new TestItem {Name = "A"}},
                {"B", new TestItem {Name = "B"}}
            };

            list1.MergeFrom(list2, null);
            Assert.IsTrue(list1.Count == 1);

            list1.MergeFrom(list2, t => t.Name);
            Assert.IsTrue(list1.Count == 2);
            Assert.IsFalse(list1.DuplicatedItems(t => t.Name));
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void MergeFrom_ListToDic_Test()
        {
            var list1 = new List<TestItem> {new TestItem {Name = "A"}, new TestItem {Name = "C"}};
            var list2 = new Dictionary<string, TestItem>
            {
                {"A", new TestItem {Name = "A"}},
                {"B", new TestItem {Name = "B"}}
            };

            list2.MergeFrom(list1, null);
            Assert.IsTrue(list2.Count == 2);

            list2.MergeFrom(list1, t => t.Name);
            Assert.IsTrue(list2.Count == 3);
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void MergeFrom_DicToDic1_Test()
        {
            var list1 = new Dictionary<string, TestItem>
            {
                {"A", new TestItem {Name = "A"}},
                {"C", new TestItem {Name = "C"}}
            };
            var list2 = new Dictionary<string, TestItem>
            {
                {"A", new TestItem {Name = "A"}},
                {"B", new TestItem {Name = "B"}}
            };

            list1.MergeFrom(null);
            Assert.IsTrue(list1.Count == 2);

            list1.MergeFrom(list2);
            Assert.IsTrue(list1.Count == 3);
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void MergeFrom_DicToDic2_Test()
        {
            var list1 = new Dictionary<int, TestItem>
            {
                {1, new TestItem {Name = "A"}},
                {3, new TestItem {Name = "C"}}
            };
            var list2 = new Dictionary<int, TestItem>
            {
                {1, new TestItem {Name = "A"}},
                {2, new TestItem {Name = "B"}}
            };

            list1.MergeFrom(null);
            Assert.IsTrue(list1.Count == 2);

            list1.MergeFrom(list2);
            Assert.IsTrue(list1.Count == 3);
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void ToDictionnary_Test()
        {
            IList<TestItem> list = null;
            Assert.IsTrue(list.ToDictionary(t => t.Id, t => t).Count == 0);

            list = new List<TestItem>
            {
                new TestItem {Id = 1, Name = "A"},
                new TestItem {Id = 2, Name = "B"},
                new TestItem {Id = 3, Name = "C"},
                new TestItem {Id = 3, Name = "C"},
                new TestItem {Id = 2, Name = null}
            };

            var dic1 = list.ToDictionary(t => t.Name, t => t);
            Assert.IsTrue(dic1.Count == 3);

            var dic2 = list.ToDictionary(t => t.Id, t => t);
            Assert.IsTrue(dic2.Count == 3);
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void Dictionary_AddRange_Dictionary_Test()
        {
            var dic1 = new Dictionary<string, string>
            {
                {"1", "1"},
                {"2", "2"}
            };
            var dic2 = new Dictionary<string, string>
            {
                {"2", "1"},
                {"3", "2"}
            };

            dic1.AddRange(dic2);
            Assert.IsTrue(dic1.Count == 3);
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void Dictionary_AddRange_NullDictionary_Test()
        {
            var dic1 = new Dictionary<string, string>
            {
                {"1", "1"},
                {"2", "2"}
            };

            dic1.AddRange((Dictionary<string, string>) null);
            Assert.IsTrue(dic1.Count == 2);
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void Dictionary_AddRange_IEnumrable_Test()
        {
            var dic1 = new Dictionary<string, string>
            {
                {"1", "1"},
                {"2", "2"}
            };

            IEnumerable<KeyValuePair<string, string>> dic2 = new[]
            {
                new KeyValuePair<string, string>("2", "1"),
                new KeyValuePair<string, string>("3", "2")
            };

            dic1.AddRange(dic2);
            Assert.IsTrue(dic1.Count == 3);
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void Dictionary_AddRange_Arrary_Test()
        {
            var dic1 = new Dictionary<string, string>
            {
                {"1", "1"},
                {"2", "2"}
            };

            var dic2 = new[]
            {
                new KeyValuePair<string, string>("2", "1"),
                new KeyValuePair<string, string>("3", "2")
            };

            dic1.AddRange(dic2);
            Assert.IsTrue(dic1.Count == 3);
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void EnqueueArrange_Test()
        {
            var dic1 = new[] {new object(), new object(), new object()};
            var queue = new Queue<object>();

            queue.EnqueueArrange(dic1);
            Assert.IsTrue(queue.Count == 3);
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void EnqueueArrange_Null_Test()
        {
            var queue = new Queue<object>();

            queue.EnqueueArrange(null);
            Assert.IsTrue(queue.Count == 0);
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void RemoveRange_Test()
        {
            var list = Enumerable.Range(1, 100).ToList();
            list.RemoveRange(Enumerable.Range(10, 10));

            Assert.IsTrue(list.Count == 90);
        }
    }
}
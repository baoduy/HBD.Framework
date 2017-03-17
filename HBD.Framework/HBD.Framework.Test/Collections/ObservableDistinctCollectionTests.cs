#region

using System.Collections.Specialized;
using System.Linq;
using HBD.Framework.Exceptions;
using HBD.Framework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Collections.Tests
{
    [TestClass]
    public class ObservableDistinctCollectionTests
    {
        [TestMethod]
        [ExpectedException(typeof(DuplicatedException))]
        public void ObservableDistinctCollection_ExceptionWhenAddDuplicatekey_Test()
        {
            var propertyEvent = 0;
            var changedEvent = 0;

            var coll = new ObservableDistinctCollection<int, TestItem>(i => i.Id);
            coll.PropertyChanged += (s, e) => propertyEvent++;
            coll.CollectionChanged += (s, e) =>
            {
                if (e.NewItems?.Cast<TestItem>().Count() > 0)
                    changedEvent++;
            };

            coll.Add(new TestItem {Id = 1});
            coll.Add(new TestItem {Id = 1});
        }

        [TestMethod]
        public void AddTest()
        {
            var propertyEvent = 0;
            var changedEvent = 0;

            var coll = new ObservableDistinctCollection<int, TestItem>(i => i.Id);
            coll.PropertyChanged += (s, e) => propertyEvent++;
            coll.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add
                    && e.NewItems?.Cast<TestItem>().Count() > 0)
                    changedEvent++;
            };

            coll.Add(new TestItem {Id = 1});
            coll.Add(new TestItem {Id = 2});

            Assert.IsTrue(propertyEvent == 4);
            Assert.IsTrue(changedEvent == 2);
        }

        [TestMethod]
        public void ClearTest()
        {
            var propertyEvent = 0;
            var changedEvent = 0;

            var coll = new ObservableDistinctCollection<int, TestItem>(i => i.Id);
            coll.PropertyChanged += (s, e) => propertyEvent++;
            coll.CollectionChanged += (s, e) =>
            {
                if (e.NewItems?.Cast<TestItem>().Count() > 0 ||
                    e.Action == NotifyCollectionChangedAction.Reset)
                    changedEvent++;
            };

            coll.Add(new TestItem {Id = 1});
            coll.Add(new TestItem {Id = 2});
            coll.Clear();

            Assert.IsTrue(propertyEvent == 2*3);
            Assert.IsTrue(changedEvent == 3);
        }
    }
}
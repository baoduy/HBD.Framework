#region using

using System.Linq;
using HBD.Framework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Collections.Tests
{
    [TestClass]
    public class ObservableSortedListTests
    {
        [TestMethod]
        public void ObservableSortedListTest()
        {
            var list = new ObservableSortedCollection<int, NotifyPropertyChangedObject>(i => i.Id);
            //Assert.AreEqual(new PrivateObject(list).GetFieldOrProperty("_keyPropertyName"), "Id");
        }

        [TestMethod]
        public void AddTest()
        {
            var collCount = 0;
            var propCount = 0;
            var list = new ObservableSortedCollection<int, NotifyPropertyChangedObject>(i => i.Id);
            list.CollectionChanged += (s, e) => collCount += 1;
            list.PropertyChanged += (s, e) => propCount += 1;


            list.Add(new NotifyPropertyChangedObject {Id = 3});
            list.Add(new NotifyPropertyChangedObject {Id = 2});
            list.Add(new NotifyPropertyChangedObject {Id = 1});

            Assert.AreEqual(list.Count, 3);

            Assert.IsTrue(collCount == 3);
            Assert.IsTrue(propCount == 6);

            list[0].Id = 4;

            Assert.AreEqual(list[0].Id, 2);
            Assert.AreEqual(list[1].Id, 3);
            Assert.AreEqual(list[2].Id, 4);

            Assert.IsTrue(collCount == 3);
            Assert.IsTrue(propCount == 6);
        }

        [TestMethod]
        public void ClearTest()
        {
            var list = new ObservableSortedCollection<int, NotifyPropertyChangedObject>(i => i.Id)
            {
                new NotifyPropertyChangedObject {Id = 3},
                new NotifyPropertyChangedObject {Id = 2},
                new NotifyPropertyChangedObject {Id = 1}
            };

            list.Clear();

            Assert.IsTrue(list.Count == 0);
        }

        [TestMethod]
        public void ContainsTest()
        {
            var item1 = new NotifyPropertyChangedObject {Id = 3};
            var item2 = new NotifyPropertyChangedObject {Id = 2};
            var item3 = new NotifyPropertyChangedObject {Id = 1};

            var list = new ObservableSortedCollection<int, NotifyPropertyChangedObject>(i => i.Id)
            {
                item1,
                item2,
                item3
            };

            Assert.IsTrue(list.Contains(item1));
            Assert.IsTrue(list.Contains(item2));
            Assert.IsTrue(list.Contains(item3));
        }

        [TestMethod]
        public void CopyToTest()
        {
            var item1 = new NotifyPropertyChangedObject {Id = 3};
            var item2 = new NotifyPropertyChangedObject {Id = 2};
            var item3 = new NotifyPropertyChangedObject {Id = 1};

            var list = new ObservableSortedCollection<int, NotifyPropertyChangedObject>(i => i.Id)
            {
                item1,
                item2,
                item3
            };

            var array = new NotifyPropertyChangedObject[3];
            list.CopyTo(array, 0);

            Assert.IsTrue(array.All(i => i != null));
        }

        [TestMethod]
        public void RemoveTest()
        {
            var item1 = new NotifyPropertyChangedObject {Id = 3};
            var item2 = new NotifyPropertyChangedObject {Id = 2};
            var item3 = new NotifyPropertyChangedObject {Id = 1};

            var list = new ObservableSortedCollection<int, NotifyPropertyChangedObject>(i => i.Id)
            {
                item1,
                item2,
                item3
            };

            list.Remove(item3);

            Assert.IsTrue(list.Contains(item1));
            Assert.IsTrue(list.Contains(item2));
            Assert.IsFalse(list.Contains(item3));
        }
    }
}
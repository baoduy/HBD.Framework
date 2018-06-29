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
            var list = new ObservableSortedCollection<NotifyPropertyChangedObject>(i => i.Id);
            //Assert.AreEqual(new PrivateObject(list).GetFieldOrProperty("_keyPropertyName"), "Id");
        }

        [TestMethod]
        public void AddTest()
        {
            var collCount = 0;
            var list = new ObservableSortedCollection<NotifyPropertyChangedObject>(i => i.Id);
            list.CollectionChanged += (s, e) => collCount += 1;

            list.Add(new NotifyPropertyChangedObject { Id = 3 });
            list.Add(new NotifyPropertyChangedObject { Id = 2 });
            list.Add(new NotifyPropertyChangedObject { Id = 1 });

            Assert.AreEqual(list.Count, 3);
            Assert.IsTrue(collCount == 3);
            Assert.IsTrue(list[0].Id == 1);

            list[0].Id = 4;

            Assert.AreEqual(list[0].Id, 2);
            Assert.AreEqual(list[1].Id, 3);
            Assert.AreEqual(list[2].Id, 4);

            Assert.IsTrue(collCount >= 3);
        }

        [TestMethod]
        public void Add_2_Test()
        {
            var list = new ObservableSortedCollection<NotifyPropertyChangedObject>(i => i.Id)
            {
                new NotifyPropertyChangedObject {Id = 12},
                new NotifyPropertyChangedObject {Id = 13},
                new NotifyPropertyChangedObject {Id = 11},
                new NotifyPropertyChangedObject {Id = 1},
                new NotifyPropertyChangedObject {Id = 6},
                new NotifyPropertyChangedObject {Id = 5},
                new NotifyPropertyChangedObject {Id = 9},
                new NotifyPropertyChangedObject {Id = 0},
                new NotifyPropertyChangedObject {Id = 14}
            };


            Assert.AreEqual(list[0].Id, 0);
            Assert.AreEqual(list[1].Id, 1);
            Assert.AreEqual(list[2].Id, 5);
            Assert.AreEqual(list[3].Id, 6);
            Assert.AreEqual(list[4].Id, 9);
            Assert.AreEqual(list[5].Id, 11);
            Assert.AreEqual(list[6].Id, 12);
            Assert.AreEqual(list[7].Id, 13);
            Assert.AreEqual(list[8].Id, 14);

            var old = list[4];
            list[4] = new NotifyPropertyChangedObject {Id = 15};

            Assert.AreEqual(list[0].Id, 0);
            Assert.AreEqual(list[1].Id, 1);
            Assert.AreEqual(list[2].Id, 5);
            Assert.AreEqual(list[3].Id, 6);
            Assert.AreEqual(list[4].Id, 11);
            Assert.AreEqual(list[5].Id, 12);
            Assert.AreEqual(list[6].Id, 13);
            Assert.AreEqual(list[7].Id, 14);
            Assert.AreEqual(list[8].Id, 15);

            var count = 0;
            list.CollectionChanged += (s,e) => count++;

            list[0].Name = "Hoang";
            list[8].Name = "Duy";

            Assert.IsTrue(count == 0);
        }

        [TestMethod]
        public void Changed_Property_To_TheSame_Value()
        {
            var list = new ObservableSortedCollection<NotifyPropertyChangedObject>(i => i.Id)
            {
                new NotifyPropertyChangedObject {Id = 12},
                new NotifyPropertyChangedObject {Id = 13},
                new NotifyPropertyChangedObject {Id = 11},
                new NotifyPropertyChangedObject {Id = 1},
                new NotifyPropertyChangedObject {Id = 6},
                new NotifyPropertyChangedObject {Id = 5},
                new NotifyPropertyChangedObject {Id = 9},
                new NotifyPropertyChangedObject {Id = 0},
                new NotifyPropertyChangedObject {Id = 14}
            };


            Assert.AreEqual(list[0].Id, 0);
            Assert.AreEqual(list[1].Id, 1);
            Assert.AreEqual(list[2].Id, 5);
            Assert.AreEqual(list[3].Id, 6);
            Assert.AreEqual(list[4].Id, 9);
            Assert.AreEqual(list[5].Id, 11);
            Assert.AreEqual(list[6].Id, 12);
            Assert.AreEqual(list[7].Id, 13);
            Assert.AreEqual(list[8].Id, 14);

            list[3].Id = 6;

            Assert.AreEqual(list[0].Id, 0);
            Assert.AreEqual(list[1].Id, 1);
            Assert.AreEqual(list[2].Id, 5);
            Assert.AreEqual(list[3].Id, 6);
            Assert.AreEqual(list[4].Id, 9);
            Assert.AreEqual(list[5].Id, 11);
            Assert.AreEqual(list[6].Id, 12);
            Assert.AreEqual(list[7].Id, 13);
            Assert.AreEqual(list[8].Id, 14);
        }

        [TestMethod]
        public void Changed_Property_To_Different_Value_But_Still_TheSame_Position()
        {
            var list = new ObservableSortedCollection<NotifyPropertyChangedObject>(i => i.Id)
            {
                new NotifyPropertyChangedObject {Id = 12},
                new NotifyPropertyChangedObject {Id = 13},
                new NotifyPropertyChangedObject {Id = 11},
                new NotifyPropertyChangedObject {Id = 1},
                new NotifyPropertyChangedObject {Id = 6},
                new NotifyPropertyChangedObject {Id = 5},
                new NotifyPropertyChangedObject {Id = 9},
                new NotifyPropertyChangedObject {Id = 0},
                new NotifyPropertyChangedObject {Id = 14}
            };


            Assert.AreEqual(list[0].Id, 0);
            Assert.AreEqual(list[1].Id, 1);
            Assert.AreEqual(list[2].Id, 5);
            Assert.AreEqual(list[3].Id, 6);
            Assert.AreEqual(list[4].Id, 9);
            Assert.AreEqual(list[5].Id, 11);
            Assert.AreEqual(list[6].Id, 12);
            Assert.AreEqual(list[7].Id, 13);
            Assert.AreEqual(list[8].Id, 14);

            list[3].Id = 7;

            Assert.AreEqual(list[0].Id, 0);
            Assert.AreEqual(list[1].Id, 1);
            Assert.AreEqual(list[2].Id, 5);
            Assert.AreEqual(list[3].Id, 7);
            Assert.AreEqual(list[4].Id, 9);
            Assert.AreEqual(list[5].Id, 11);
            Assert.AreEqual(list[6].Id, 12);
            Assert.AreEqual(list[7].Id, 13);
            Assert.AreEqual(list[8].Id, 14);
        }

        [TestMethod]
        public void Changed_Property_To_Different_Value_And_Different_Position()
        {
            var list = new ObservableSortedCollection<NotifyPropertyChangedObject>(i => i.Id)
            {
                new NotifyPropertyChangedObject {Id = 12},
                new NotifyPropertyChangedObject {Id = 13},
                new NotifyPropertyChangedObject {Id = 11},
                new NotifyPropertyChangedObject {Id = 1},
                new NotifyPropertyChangedObject {Id = 6},
                new NotifyPropertyChangedObject {Id = 5},
                new NotifyPropertyChangedObject {Id = 9},
                new NotifyPropertyChangedObject {Id = 0},
                new NotifyPropertyChangedObject {Id = 14}
            };


            Assert.AreEqual(list[0].Id, 0);
            Assert.AreEqual(list[1].Id, 1);
            Assert.AreEqual(list[2].Id, 5);
            Assert.AreEqual(list[3].Id, 6);
            Assert.AreEqual(list[4].Id, 9);
            Assert.AreEqual(list[5].Id, 11);
            Assert.AreEqual(list[6].Id, 12);
            Assert.AreEqual(list[7].Id, 13);
            Assert.AreEqual(list[8].Id, 14);

            list[8].Id = -1;

            Assert.AreEqual(list[0].Id, -1);
            Assert.AreEqual(list[1].Id, 0);
            Assert.AreEqual(list[2].Id, 1);
            Assert.AreEqual(list[3].Id, 5);
            Assert.AreEqual(list[4].Id, 6);
            Assert.AreEqual(list[5].Id, 9);
            Assert.AreEqual(list[6].Id, 11);
            Assert.AreEqual(list[7].Id, 12);
            Assert.AreEqual(list[8].Id, 13);
        }

        [TestMethod]
        public void PropertyChangedTest()
        {
            var list = new ObservableSortedCollection<NotifyPropertyChangedObject>(i => i.Id)
            {
                new NotifyPropertyChangedObject {Id = 3,Name="3"},
                new NotifyPropertyChangedObject {Id = 2,Name="2"},
                new NotifyPropertyChangedObject {Id = 1,Name="1"}
            };

            list.First(i => i.Name == "3").Id = 100;
            list.First(i => i.Name == "2").Id = 50;
            list.First(i => i.Name == "1").Id = 200;

            Assert.IsTrue(list[0].Name == "2");
            Assert.IsTrue(list[1].Name == "3");
            Assert.IsTrue(list[2].Name == "1");
        }

        [TestMethod]
        public void ClearTest()
        {
            var list = new ObservableSortedCollection<NotifyPropertyChangedObject>(i => i.Id)
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
            var item1 = new NotifyPropertyChangedObject { Id = 3 };
            var item2 = new NotifyPropertyChangedObject { Id = 2 };
            var item3 = new NotifyPropertyChangedObject { Id = 1 };

            var list = new ObservableSortedCollection<NotifyPropertyChangedObject>(i => i.Id)
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
            var item1 = new NotifyPropertyChangedObject { Id = 3 };
            var item2 = new NotifyPropertyChangedObject { Id = 2 };
            var item3 = new NotifyPropertyChangedObject { Id = 1 };

            var list = new ObservableSortedCollection<NotifyPropertyChangedObject>(i => i.Id)
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
            var item1 = new NotifyPropertyChangedObject { Id = 3 };
            var item2 = new NotifyPropertyChangedObject { Id = 2 };
            var item3 = new NotifyPropertyChangedObject { Id = 1 };

            var list = new ObservableSortedCollection<NotifyPropertyChangedObject>(i => i.Id)
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

        //[TestMethod]
        //public void NotHandle_AnyChanghedEventAfterRemoved_Test()
        //{
        //    var item1 = new NotifyPropertyChangedObject { Id = 3 };
        //    var item2 = new NotifyPropertyChangedObject { Id = 2 };
        //    var item3 = new NotifyPropertyChangedObject { Id = 1 };

        //    var list = new ObservableSortedCollection<int, NotifyPropertyChangedObject>(i => i.Id)
        //    {
        //        item1,
        //        item2,
        //        item3
        //    };

        //    list.Clear();
        //}
    }
}
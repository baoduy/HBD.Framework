#region using

using System.Collections.Specialized;
using HBD.Framework.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Test.Collections
{
    [TestClass]
    public class ObservableCollectionTests
    {
        [TestMethod]
        [TestCategory("Fw.Collection")]
        public void ObservableCollection_Add_Test()
        {
            var called = false;

            var col = new ChangingObservableCollection<object>();
            col.CollectionChanging += (s, e) => called = e.Action == NotifyCollectionChangedAction.Add
                                                         && e.NewItems.Count > 0;
            col.Add(new object());

            Assert.IsTrue(called);
        }

        [TestMethod]
        [TestCategory("Fw.Collection")]
        public void ObservableCollection_Remove_Test()
        {
            var called = false;

            var col = new ChangingObservableCollection<object>();
            col.CollectionChanging += (s, e) => called = e.Action == NotifyCollectionChangedAction.Remove
                                                         && e.OldItems.Count > 0;

            var obj = new object();
            col.Add(obj);
            col.Remove(obj);

            Assert.IsTrue(called);
        }


        [TestMethod]
        [TestCategory("Fw.Collection")]
        public void ObservableCollection_Replace_Test()
        {
            var called = false;

            var col = new ChangingObservableCollection<object>();
            col.CollectionChanging += (s, e) => called = e.Action == NotifyCollectionChangedAction.Replace
                                                         && e.OldItems.Count > 0 && e.NewItems.Count > 0;

            var obj1 = new object();
            var obj2 = new object();
            col.Add(obj1);
            col[0] = obj2;

            Assert.IsTrue(called);
        }

        [TestMethod]
        [TestCategory("Fw.Collection")]
        public void ObservableCollection_Move_Test()
        {
            var called = false;

            var col = new ChangingObservableCollection<object>();
            col.CollectionChanging += (s, e) => called = e.Action == NotifyCollectionChangedAction.Move
                                                         && e.OldItems.Count > 0 && e.NewItems.Count > 0;

            var obj1 = new object();
            var obj2 = new object();
            col.Add(obj1);
            col.Add(obj2);

            col.Move(1, 0);

            Assert.IsTrue(called);
        }

        [TestMethod]
        [TestCategory("Fw.Collection")]
        public void ObservableCollection_Cancel_Test()
        {
            var col = new ChangingObservableCollection<object>();
            col.CollectionChanging += (s, e) => e.Cancel = true;

            col.Add(new object());
            Assert.IsTrue(col.Count == 0);
        }
    }
}
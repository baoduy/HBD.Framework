#region

using System;
using HBD.Framework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Core.Tests
{
    [TestClass]
    public class ChangeTrackingEntryTests
    {
        [TestMethod]
        [TestCategory("HBD.Framework.Core")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChangeTrackingEntry_NullException_Test()
        {
            var obj = new ChangeTrackingEntry<NotifyPropertyChangedObject>(null);
        }

        [TestMethod]
        [TestCategory("HBD.Framework.Core")]
        public void ChangeTrackingEntryTest()
        {
            const string originalName = "Duy";
            var originalItem = new TestItem();
            var obj =
                new ChangeTrackingEntry<NotifyPropertyChangedObject>(new NotifyPropertyChangedObject
                {
                    Name = originalName,
                    Item = originalItem
                })
                {
                    Entity =
                    {
                        Name = "Hoang",
                        Item = new TestItem()
                    }
                };


            Assert.AreNotEqual(obj.Entity.Name, originalName);
            Assert.AreNotEqual(obj.Entity.Item, originalItem);
            Assert.IsTrue(obj.IsChanged);
        }

        [TestMethod]
        [TestCategory("HBD.Framework.Core")]
        public void AcceptChangesTest()
        {
            const string originalName = "Duy";
            var originalItem = new TestItem();
            var obj =
                new ChangeTrackingEntry<NotifyPropertyChangedObject>(new NotifyPropertyChangedObject
                {
                    Name = originalName,
                    Item = originalItem
                });

            var newName = "Hoang";
            var newItem = new TestItem();

            obj.Entity.Name = newName;
            obj.Entity.Item = newItem;

            obj.AcceptChanges();

            Assert.AreEqual(obj.Entity.Name, newName);
            Assert.AreEqual(obj.Entity.Item, newItem);
            Assert.IsFalse(obj.IsChanged);

            obj.UndoChanges();
            Assert.AreEqual(obj.Entity.Name, newName);
            Assert.AreEqual(obj.Entity.Item, newItem);
            Assert.IsFalse(obj.IsChanged);
        }

        [TestMethod]
        [TestCategory("HBD.Framework.Core")]
        public void UndoChangesTest()
        {
            const string originalName = "Duy";
            var originalItem = new TestItem();
            var obj =
                new ChangeTrackingEntry<NotifyPropertyChangedObject>(new NotifyPropertyChangedObject
                {
                    Name = originalName,
                    Item = originalItem
                });

            var newName = "Hoang";
            var newItem = new TestItem();

            obj.Entity.Name = newName;
            obj.Entity.Item = newItem;

            obj.UndoChanges();

            Assert.AreEqual(obj.Entity.Name, originalName);
            Assert.AreEqual(obj.Entity.Item, originalItem);
            Assert.IsFalse(obj.IsChanged);

            obj.AcceptChanges();
            Assert.AreEqual(obj.Entity.Name, originalName);
            Assert.AreEqual(obj.Entity.Item, originalItem);
            Assert.IsFalse(obj.IsChanged);
        }

        [TestMethod]
        [TestCategory("HBD.Framework.Core")]
        public void TrackChanges_Even_DisabledEvents_Test()
        {
            const string originalName = "Duy";
            var originalItem = new TestItem();
            var obj = new NotifyPropertyChangedObject {Name = originalName, Item = originalItem};
            var objEntry = new ChangeTrackingEntry<NotifyPropertyChangedObject>(obj);

            using (obj.DisablePropertyChangeNotifies())
            {
                var newName = "Hoang";
                var newItem = new TestItem();

                obj.Name = newName;
                obj.Item = newItem;
            }

            Assert.IsTrue(objEntry.IsChanged);
            Assert.IsTrue(objEntry.OriginalValues.Count == 2);
            Assert.AreEqual(objEntry.OriginalValues["Name"], originalName);
            Assert.AreEqual(objEntry.OriginalValues["Item"], originalItem);
        }
    }
}
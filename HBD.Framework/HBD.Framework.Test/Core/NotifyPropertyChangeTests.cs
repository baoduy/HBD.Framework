#region using

using HBD.Framework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Core.Tests
{
    [TestClass]
    public class NotifyPropertyChangeTests
    {
        [TestMethod]
        public void DisablePropertyChangedEventTest()
        {
            var changedCount = 0;
            var changingCount = 0;

            var obj = new NotifyPropertyChangedObject();
            obj.PropertyChanged += (s, e) => changedCount += 1;
            obj.PropertyChanging += (s, e) => changingCount += 1;

            using (obj.DisablePropertyChangedNotify())
            {
                obj.Name = "AAAAA";
                Assert.IsTrue(obj.Name == "AAAAA");
                Assert.IsTrue(changedCount == 0);
                Assert.IsTrue(changingCount == 1);
            }

            obj.Name = "BBBBBB";
            Assert.IsTrue(obj.Name == "BBBBBB");
            Assert.IsTrue(changedCount == 1);
            Assert.IsTrue(changingCount == 2);
        }

        [TestMethod]
        public void DisablePropertyChangingEventTest()
        {
            var changedCount = 0;
            var changingCount = 0;

            var obj = new NotifyPropertyChangedObject();
            obj.PropertyChanged += (s, e) => changedCount += 1;
            obj.PropertyChanging += (s, e) => changingCount += 1;

            using (obj.DisablePropertyChangingNotify())
            {
                obj.Name = "AAAAA";
                Assert.IsTrue(obj.Name == "AAAAA");
                Assert.IsTrue(changedCount == 1);
                Assert.IsTrue(changingCount == 0);
            }

            obj.Name = "BBBBBB";
            Assert.IsTrue(obj.Name == "BBBBBB");
            Assert.IsTrue(changedCount == 2);
            Assert.IsTrue(changingCount == 1);
        }

        [TestMethod]
        public void DisablePropertyChangeEventsTest()
        {
            var changedCount = 0;
            var changingCount = 0;

            var obj = new NotifyPropertyChangedObject();
            obj.PropertyChanged += (s, e) => changedCount += 1;
            obj.PropertyChanging += (s, e) => changingCount += 1;

            using (obj.DisablePropertyChangeNotifies())
            {
                obj.Name = "AAAAA";
                Assert.IsTrue(obj.Name == "AAAAA");
                Assert.IsTrue(changedCount == 0);
                Assert.IsTrue(changingCount == 0);
            }

            obj.Name = "BBBBBB";
            Assert.IsTrue(obj.Name == "BBBBBB");
            Assert.IsTrue(changedCount == 1);
            Assert.IsTrue(changingCount == 1);
        }
    }
}
using HBD.Framework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Framework.Test.Core
{
    [TestClass]
    public class NotifyPropertyChangedTests
    {
        [TestMethod]
        public void NotifyPropertyChangedObjectTest()
        {
            var changingCount = 0;
            var changedCount = 0;

            var obj = new NotifyPropertyChangedObject();
            obj.PropertyChanging += (s, e) => changingCount++;
            obj.PropertyChanged += (s, e) => changedCount++;

            obj.Name = "Duy";
            obj.Name = "Duy";
            obj.Name = "Duy";

            Assert.AreEqual(changedCount, 1);
            Assert.AreEqual(changingCount, 1);

            obj.Name = "Hoang";

            Assert.AreEqual(changedCount, 2);
            Assert.AreEqual(changingCount, 2);

            changingCount = 0;
            changedCount = 0;

            var testItem = new TestItem();

            obj.Item = testItem;
            obj.Item = testItem;
            obj.Item = testItem;

            Assert.AreEqual(changedCount, 1);
            Assert.AreEqual(changingCount, 1);

            obj.Item = new TestItem();

            Assert.AreEqual(changedCount, 2);
            Assert.AreEqual(changingCount, 2);
        }
    }
}
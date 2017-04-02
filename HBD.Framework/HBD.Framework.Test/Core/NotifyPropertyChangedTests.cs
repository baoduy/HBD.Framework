#region using

using HBD.Framework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

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

        [TestMethod]
        public void NotifyPropertyChangedObject_Cancelation_Test()
        {
            var changingCount = 0;
            var changedCount = 0;

            var obj = new NotifyPropertyChangedObject {Name = "123"};
            obj.PropertyChanging += (s, e) =>
            {
                changingCount++;
                e.Cancel = true;

                Assert.IsTrue(e.OldValue.Equals("123"));
                Assert.IsTrue(e.NewValue.Equals("Duy"));
            };

            obj.PropertyChanged += (s, e) => changedCount++;

            obj.Name = "Duy";

            Assert.IsTrue(changingCount == 1);
            Assert.IsTrue(changedCount == 0);
            Assert.IsTrue(obj.Name == "123");
        }
    }
}
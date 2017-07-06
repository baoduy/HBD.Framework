#region using

using HBD.Framework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Tests
{
    [TestClass]
    public class SingletonManagerTests
    {
        private int _count;
        private TestItem _item;

        public TestItem Item1 => SingletonManager.GetOrLoad(ref _item, () =>
        {
            _count++;
            return new TestItem();
        });

        public TestItem3 Item2 => SingletonManager.GetOrLoadOne<TestItem3>(() =>
        {
            _count++;
            return null;
        });

        public TestItem3 Item3 => SingletonManager.GetOrLoadOne(() =>
        {
            _count++;
            return new TestItem3();
        });

        [TestInitialize]
        public void Initialize() => _count = 0;

        [TestCleanup]
        public void Cleaup()
        {
            SingletonManager.Clear();
        }

        [TestMethod]
        public void GetOrLoadTest()
        {
            var t1 = Item1;
            var t2 = Item1;

            Assert.IsNotNull(t1);
            Assert.AreEqual(t1, t2);
            Assert.IsTrue(_count == 1);

            _item = null;

            t1 = Item1;
            t2 = Item1;

            Assert.IsNotNull(t1);
            Assert.AreEqual(t1, t2);
            Assert.IsTrue(_count == 2);
        }

        [TestMethod]
        public void GetOrLoadOneTest()
        {
            var t1 = Item2;
            var t2 = Item2;

            Assert.IsNull(t1);
            Assert.AreEqual(t1, t2);
            Assert.IsTrue(_count == 1);

            _item = null;

            t1 = Item2;
            t2 = Item2;

            Assert.IsNull(t1);
            Assert.AreEqual(t1, t2);
            Assert.IsTrue(_count == 1);
        }

        [TestMethod]
        public void ResetGenericTest()
        {
            var t1 = Item3;
            SingletonManager.Reset<TestItem3>();
            var t2 = Item3;

            Assert.IsTrue(_count == 2);
        }

        [TestMethod]
        public void ResetTest()
        {
            var t1 = Item3;
            SingletonManager.Reset(t1);
            var t2 = Item3;

            Assert.IsTrue(_count == 2);
        }

        [TestMethod]
        public void ClearTest()
        {
            var t1 = Item3;
            SingletonManager.Clear();
            Assert.IsTrue(t1.IsDisposed);
        }
    }
}
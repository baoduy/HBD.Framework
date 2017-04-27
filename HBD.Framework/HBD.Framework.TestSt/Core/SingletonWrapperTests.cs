#region using

using HBD.Framework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Core.Tests
{
    [TestClass]
    public class SingletonWrapperTests
    {
        [TestMethod]
        public void SingleInstanceWrapperTest()
        {
            var count = 0;
            var a = new SingletonWrapper<TestItem>(() =>
            {
                count++;
                return new TestItem();
            });

            var i = a.Instance;

            Assert.IsNotNull(a.Instance);
            Assert.AreEqual(i, a.Instance);
            Assert.IsTrue(count == 1);

            a.Reset();

            Assert.IsNotNull(a.Instance);
            Assert.AreNotEqual(i, a.Instance);
            Assert.IsTrue(count == 2);
        }
    }
}
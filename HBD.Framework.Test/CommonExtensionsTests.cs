using HBD.Framework.Data.Base;
using HBD.Framework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace HBD.Framework.Tests
{
    [TestClass()]
    public class CommonExtensionsTests
    {
        [TestMethod()]
        public void IsNullTest()
        {
            Assert.IsTrue(((object)null).IsNull());
            Assert.IsTrue("".IsNull());
            Assert.IsTrue(DBNull.Value.IsNull());
            Assert.IsTrue(string.Empty.IsNull());

            Assert.IsFalse(" ".IsNull());
            Assert.IsFalse(new object().IsNull());
            Assert.IsFalse(123.IsNull());
        }

        [TestMethod()]
        public void IsNotNullTest()
        {
            Assert.IsFalse(((object)null).IsNotNull());
            Assert.IsFalse("".IsNotNull());
            Assert.IsFalse(DBNull.Value.IsNotNull());
            Assert.IsFalse(string.Empty.IsNotNull());

            Assert.IsTrue(" ".IsNotNull());
            Assert.IsTrue(new object().IsNotNull());
            Assert.IsTrue(123d.IsNotNull());
        }

        [TestMethod()]
        public void IsNullOrEmptyTest()
        {
            Assert.IsTrue(((object)null).IsNullOrEmpty());
            Assert.IsTrue("".IsNullOrEmpty());
            Assert.IsTrue(DBNull.Value.IsNullOrEmpty());
            Assert.IsTrue(string.Empty.IsNullOrEmpty());
            Assert.IsTrue(" ".IsNullOrEmpty());
            Assert.IsFalse(new object().IsNullOrEmpty());
            Assert.IsFalse(123.IsNullOrEmpty());

            Assert.IsTrue(new List<object>().IsNullOrEmpty());
        }

        [TestMethod()]
        public void IsNotNullOrEmptyTest()
        {
            Assert.IsFalse(((object)null).IsNotNullOrEmpty());
            Assert.IsFalse("".IsNotNullOrEmpty());
            Assert.IsFalse(DBNull.Value.IsNotNullOrEmpty());
            Assert.IsFalse(string.Empty.IsNotNullOrEmpty());
            Assert.IsFalse(" ".IsNotNullOrEmpty());
            Assert.IsTrue(new object().IsNotNullOrEmpty());
            Assert.IsTrue(123.IsNotNullOrEmpty());

            Assert.IsFalse(new List<object>().IsNotNullOrEmpty());
        }

        [TestMethod()]
        [TestCategory("Fw.Expressions")]
        public void IsNullOrEmptyTest1()
        {
            Assert.IsFalse(((object)"AA").IsNullOrEmpty());
            Assert.IsFalse(new object().IsNullOrEmpty());
            Assert.IsTrue(((object)null).IsNullOrEmpty());
            Assert.IsTrue((DBNull.Value).IsNullOrEmpty());
            Assert.IsTrue(((object)"").IsNullOrEmpty());
        }

        [TestMethod()]
        [TestCategory("Fw.Expressions")]
        public void IsNotNullOrEmptyTest2()
        {
            Assert.IsTrue(((object)"AA").IsNotNullOrEmpty());
            Assert.IsTrue(new object().IsNotNullOrEmpty());
            Assert.IsFalse(((object)null).IsNotNullOrEmpty());
            Assert.IsFalse((DBNull.Value).IsNotNullOrEmpty());
            Assert.IsFalse(((object)"").IsNotNullOrEmpty());
        }

        [TestMethod()]
        [TestCategory("Fw.Expressions")]
        public void CreateInstanceTest()
        {
            Assert.IsNotNull(typeof(TestItem).CreateInstance());
            Assert.IsNotNull(typeof(TestItem3).CreateInstance("AA"));
            Assert.IsNull(typeof(DataClientAdapter).CreateInstance("AA"));
            Assert.IsNull(typeof(IDataClientAdapter).CreateInstance());
        }

        [TestMethod()]
        [TestCategory("Fw.Expressions")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateInstance_WithNullType_Test()
        {
            CommonExtensions.CreateInstance(null);
        }

        [TestMethod()]
        [TestCategory("Fw.Expressions")]
        public void IsDefaultTest()
        {
            var item = default(KeyValuePair<string, object>);
            Assert.IsTrue(item.IsDefault());
            Assert.IsFalse(new KeyValuePair<string, object>("A", new object()).IsDefault());
        }

        [TestMethod()]
        [TestCategory("Fw.Expressions")]
        public void IsNotDefaultTest()
        {
            var item = default(KeyValuePair<string, object>);
            Assert.IsFalse(item.IsNotDefault());
            Assert.IsTrue(new KeyValuePair<string, object>("A", new object()).IsNotDefault());
        }

        [TestMethod()]
        public void GetNameTest()
        {
            var a = TestEnum.Enum1;
            Assert.IsTrue(a.GetName() == "Enum 1");
        }
    }
}
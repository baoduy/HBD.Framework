#region using

using System;
using System.Collections.Generic;
using HBD.Framework.Extensions.Tests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Extensions.Tests
{
    [TestClass]
    public class CommonExtensionsTests
    {
        [TestMethod]
        public void IsNullTest()
        {
            Assert.IsTrue(((object) null).IsNull());
            Assert.IsTrue("".IsNull());
            Assert.IsTrue(DBNull.Value.IsNull());
            Assert.IsTrue(string.Empty.IsNull());

            Assert.IsFalse(" ".IsNull());
            Assert.IsFalse(new object().IsNull());
            Assert.IsFalse(123.IsNull());
        }

        [TestMethod]
        public void IsNotNullTest()
        {
            Assert.IsFalse(((object) null).IsNotNull());
            Assert.IsFalse("".IsNotNull());
            Assert.IsFalse(DBNull.Value.IsNotNull());
            Assert.IsFalse(string.Empty.IsNotNull());

            Assert.IsTrue(" ".IsNotNull());
            Assert.IsTrue(new object().IsNotNull());
            Assert.IsTrue(123d.IsNotNull());
        }

        [TestMethod]
        public void IsNullTest1()
        {
            Assert.IsTrue(((object) null).IsNull());
            Assert.IsTrue("".IsNullOrEmpty());
            Assert.IsTrue(DBNull.Value.IsNull());
            Assert.IsTrue(string.Empty.IsNull());
            Assert.IsTrue(" ".IsNullOrEmpty());
            Assert.IsFalse(new object().IsNull());
            Assert.IsFalse(123.IsNull());
        }

        [TestMethod]
        public void IsNotNullTest1()
        {
            Assert.IsFalse(((object) null).IsNotNull());
            Assert.IsFalse("".IsNotNullOrEmpty());
            Assert.IsFalse(DBNull.Value.IsNotNull());
            Assert.IsFalse(string.Empty.IsNotNull());
            Assert.IsFalse(" ".IsNotNullOrEmpty());
            Assert.IsTrue(new object().IsNotNull());
            Assert.IsTrue(123.IsNotNull());
        }

        [TestMethod]
        [TestCategory("Fw.Expressions")]
        public void IsNullTest2()
        {
            Assert.IsFalse("AA".IsNull());
            Assert.IsFalse(new object().IsNull());
            Assert.IsTrue(((object) null).IsNull());
            Assert.IsTrue(DBNull.Value.IsNull());
            Assert.IsTrue("".IsNull());
        }

        [TestMethod]
        [TestCategory("Fw.Expressions")]
        public void IsNotNullOrEmptyTest2()
        {
            Assert.IsTrue("AA".IsNotNull());
            Assert.IsTrue(new object().IsNotNull());
            Assert.IsFalse(((object) null).IsNotNull());
            Assert.IsFalse(DBNull.Value.IsNotNull());
            Assert.IsFalse("".IsNotNull());
        }

        [TestMethod]
        [TestCategory("Fw.Expressions")]
        public void CreateInstanceTest()
        {
            Assert.IsNotNull(typeof(TestItem).CreateInstance());
            Assert.IsNotNull(typeof(TestItem3).CreateInstance("AA"));
        }

        [TestMethod]
        [TestCategory("Fw.Expressions")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateInstance_WithNullType_Test()
        {
            ShareExtensions.CreateInstance(null);
        }

        [TestMethod]
        [TestCategory("Fw.Expressions")]
        public void IsDefaultTest()
        {
            var item = default(KeyValuePair<string, object>);
            Assert.IsTrue(item.IsDefault());
            Assert.IsFalse(new KeyValuePair<string, object>("A", new object()).IsDefault());
        }

        [TestMethod]
        [TestCategory("Fw.Expressions")]
        public void IsNotDefaultTest()
        {
            var item = default(KeyValuePair<string, object>);
            Assert.IsFalse(item.IsNotDefault());
            Assert.IsTrue(new KeyValuePair<string, object>("A", new object()).IsNotDefault());
        }
    }
}
#region

using System;
using HBD.Framework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Tests
{
    [TestClass]
    public class TypeExtenstionsTests
    {
        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void IsAssignableFrom_Test()
        {
            Assert.IsTrue(typeof(Attribute).IsAssignableFrom<TestAttribute>());
            Assert.IsFalse(typeof(TypeExtenstionsTests).IsAssignableFrom<TestAttribute>());
            Assert.IsFalse(((Type) null).IsAssignableFrom<TestAttribute>());
            Assert.IsFalse(typeof(TypeExtenstionsTests).IsAssignableFrom(null));
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void IsNotAssignableFrom_Test()
        {
            Assert.IsFalse(typeof(Attribute).IsNotAssignableFrom<TestAttribute>());
            Assert.IsTrue(typeof(TypeExtenstionsTests).IsNotAssignableFrom<TestAttribute>());
            Assert.IsTrue(((Type) null).IsNotAssignableFrom<TestAttribute>());
            Assert.IsFalse(typeof(TypeExtenstionsTests).IsNotAssignableFrom(null));
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void ChangeType_WithNull_Test()
        {
            Assert.IsNull("123".ChangeType(null));
            Assert.IsNull(((object) null).ChangeType(typeof(object)));
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void IsNumericType_WithType_Test()
        {
            Assert.IsFalse(((Type) null).IsNumericType());
            Assert.IsTrue(typeof(float).IsNumericType());
            Assert.IsFalse(typeof(string).IsNumericType());
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void IsNotNumericType_WithObject5_Test()
        {
            Assert.IsTrue(new object().IsNotNumericType());
            Assert.IsFalse(float.MaxValue.IsNotNumericType());
            Assert.IsTrue(string.Empty.IsNotNumericType());
        }

        [TestMethod]
        public void ChangeTypeTest()
        {
            Assert.AreEqual("12".ChangeType<decimal>(), 12);
            Assert.AreEqual(123.ChangeType<string>(), "123");
            Assert.AreEqual(1.ChangeType<bool>(), true);
            Assert.AreEqual(0.ChangeType<bool>(), false);
            Assert.AreEqual(true.ChangeType<bool>(), true);
            Assert.AreEqual(false.ChangeType<bool>(), false);
        }

        [TestMethod]
        public void IsNumericTypeTest()
        {
            Assert.IsTrue("123".IsNumber());
            Assert.IsTrue("123.12".IsNumber());
            Assert.IsTrue("-123.99".IsNumber());
            Assert.IsTrue("123,123,123.99".IsNumber());
        }

        [TestMethod]
        public void IsNotNumericTypeTest()
        {
            Assert.IsTrue("AAA".IsNotNumber());
            Assert.IsTrue("123.12.".IsNotNumber());
            Assert.IsTrue("-123.99-".IsNotNumber());
            Assert.IsTrue("123,,123,123.99".IsNotNumber());
        }
    }
}
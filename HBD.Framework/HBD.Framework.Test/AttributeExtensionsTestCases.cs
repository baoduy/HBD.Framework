#region

using System;
using System.Linq;
using System.Reflection;
using HBD.Framework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Test
{
    [TestClass]
    public class AttributeExtensionsTestCases
    {
        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void HasAttribute_Test()
        {
            var obj1 = new HasAttributeTestClass1();
            Assert.IsTrue(obj1.HasAttribute<TestAttribute>());
            Assert.IsTrue(obj1.HasAttributeOnProperty<TestAttribute>("Prop1"));

            var obj2 = new HasAttributeTestClass2();
            Assert.IsTrue(obj2.HasAttribute<TestAttribute>());
            Assert.IsTrue(obj2.HasAttributeOnProperty<TestAttribute>("Prop1"));

            var obj3 = new HasAttributeTestClass3();
            Assert.IsFalse(obj3.HasAttribute<TestAttribute>());
            Assert.IsFalse(obj3.HasAttributeOnProperty<TestAttribute>("Prop3"));

            Assert.IsFalse(((object) null).HasAttribute<TestAttribute>());
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void NullBoj_HasAttribute_ReturnsFalse_Test()
        {
            Assert.IsFalse(((object) null).HasAttribute<TestAttribute>());
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void NullPropertyInfo_HasAttribute_ReturnsFalse_Test()
        {
            Assert.IsFalse(((PropertyInfo) null).HasAttribute<TestAttribute>());
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void GetProperties_WithAttribute_Test()
        {
            var obj = new HasAttributeTestClass2();
            Assert.IsTrue(obj.GetProperties<TestAttribute>().Count() == 1);
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void GetProperties_WithNullObj_Return0_Test()
        {
            Assert.IsFalse(((object) null).GetProperties<TestAttribute>().Any());
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void GetAttribute_WithType_ReturnExpectedAttribute_Test()
        {
            var type = typeof(HasAttributeTestClass1);
            Assert.IsNotNull(type.GetAttribute(typeof(TestAttribute)));
            Assert.IsNotNull(type.GetAttribute(typeof(TestAttribute)) is TestAttribute);
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void GetAttribute_WithNullType_ReturnNull_Test()
        {
            Type type = null;
            Assert.IsNull(type.GetAttribute(typeof(TestAttribute)));
            Assert.IsNull(((PropertyInfo) null).GetAttribute<TestAttribute>());
            Assert.IsNull(((object) null).GetAttribute<TestAttribute>());
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void GetAttributeGeneric_WithType_ReturnExpectedAttribute_Test()
        {
            var type = typeof(HasAttributeTestClass1);
            Assert.IsNotNull(type.GetAttribute<TestAttribute>());
        }
    }
}
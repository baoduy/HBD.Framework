using HBD.Framework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace HBD.Framework.Extensions.Tests
{
    [TestClass()]
    public class ComparisionExtensionsTests
    {
        [TestMethod()]
        [TestCategory("Fw.Expression")]
        public void IsEquals_NullObj_Test()
        {
            Assert.IsTrue(((object)null).IsEquals(null));
            Assert.IsFalse(((object)null).IsEquals(new object()));
        }

        [TestMethod()]
        [TestCategory("Fw.Expression")]
        public void IsEquals_Number_Test()
        {
            Assert.IsTrue(((object)11).IsEquals(11));
            Assert.IsFalse(((object)11).IsEquals(12));
            Assert.IsTrue(((object)11).IsEquals("11"));
        }

        [TestMethod()]
        [TestCategory("Fw.Expression")]
        public void IsEquals_String_Test()
        {
            Assert.IsTrue("12".IsEquals("12"));
            Assert.IsFalse("12".IsEquals("13"));

            Assert.IsTrue(12.IsEquals("12"));
            Assert.IsTrue(13.IsEquals("13"));
            Assert.IsFalse(12.IsEquals("13"));
        }

        [TestMethod()]
        [TestCategory("Fw.Expression")]
        public void IsNotEquals_Test()
        {
            Assert.IsTrue(("12").IsNotEquals("13"));
            Assert.IsTrue(("12").IsNotEquals(null));
        }

        [TestMethod()]
        [TestCategory("Fw.Expression")]
        public void IsStringOrValueTypeTest()
        {
            Assert.IsFalse(((Type)null).IsStringOrValueType());
            Assert.IsFalse(((PropertyInfo)null).IsStringOrValueType());
            Assert.IsFalse(((object)null).IsStringOrValueType());

            Assert.IsTrue(123.IsStringOrValueType());
            Assert.IsTrue("123".IsStringOrValueType());
            Assert.IsFalse(new object().IsStringOrValueType());

            Assert.IsTrue(new HasAttributeTestClass3().GetProperty("Prop3").IsStringOrValueType());
            Assert.IsFalse(new HasAttributeTestClass3().GetProperty("Prop4").IsStringOrValueType());
            Assert.IsTrue(new HasAttributeTestClass3().GetProperty("Prop5").IsStringOrValueType());

            Assert.IsTrue(((object)new HasAttributeTestClass3().GetProperty("Prop3")).IsStringOrValueType());
            Assert.IsTrue(((object)typeof(string)).IsStringOrValueType());

            Assert.IsTrue(typeof(string).IsStringOrValueType());
            Assert.IsTrue(typeof(int).IsStringOrValueType());
            Assert.IsFalse(typeof(object).IsStringOrValueType());
            Assert.IsFalse(typeof(IList<object>).IsStringOrValueType());
        }

        [TestMethod()]
        [TestCategory("Fw.Expression")]
        public void CompareTest()
        {
            Assert.IsTrue(123.CompareTo(CompareOperation.Contains, "123"));
            Assert.IsTrue(123.CompareTo(CompareOperation.NotContains, "5"));
            Assert.IsTrue(123.CompareTo(CompareOperation.EndsWith, "3"));
            Assert.IsTrue(123.CompareTo(CompareOperation.Equals, 123));
            Assert.IsTrue(123.CompareTo(CompareOperation.NotEquals, "5"));
            Assert.IsTrue(123.CompareTo(CompareOperation.GreaterThan, 111));
            Assert.IsTrue(123.CompareTo(CompareOperation.GreaterThanOrEquals, 122));
            Assert.IsTrue(123.CompareTo(CompareOperation.In, new object[] { 123, 456, 789 }));
            Assert.IsTrue(123.CompareTo(CompareOperation.NotIn, new object[] { 111, 456, 789 }));
            Assert.IsTrue(((object)null).CompareTo(CompareOperation.IsNull, null));
            Assert.IsTrue(123.CompareTo(CompareOperation.NotNull, null));
            Assert.IsTrue(123.CompareTo(CompareOperation.LessThan, "124"));
            Assert.IsTrue(123.CompareTo(CompareOperation.LessThanOrEquals, "124"));
            Assert.IsTrue(123.CompareTo(CompareOperation.StartsWith, "1"));

            Assert.IsTrue("A".CompareTo(CompareOperation.LessThan, "B"));
            Assert.IsTrue("a".CompareTo(CompareOperation.LessThan, "b"));
            Assert.IsTrue("A".CompareTo(CompareOperation.LessThanOrEquals, "a"));

            Assert.IsFalse("100".CompareTo(CompareOperation.Contains, "123"));
            Assert.IsFalse("100".CompareTo(CompareOperation.NotContains, "0"));
            Assert.IsFalse("100".CompareTo(CompareOperation.EndsWith, "3"));
            Assert.IsFalse("100".CompareTo(CompareOperation.Equals, 123));
            Assert.IsFalse("100".CompareTo(CompareOperation.NotEquals, "100"));
            Assert.IsFalse("100".CompareTo(CompareOperation.GreaterThan, 111));
            Assert.IsFalse("100".CompareTo(CompareOperation.GreaterThanOrEquals, 122));
            Assert.IsFalse("100".CompareTo(CompareOperation.In, new object[] { 123, 456, 789 }));
            Assert.IsFalse("100".CompareTo(CompareOperation.NotIn, new object[] { 100, 456, 789 }));
            Assert.IsFalse("100".CompareTo(CompareOperation.IsNull, null));
            Assert.IsFalse(((object)null).CompareTo(CompareOperation.NotNull, null));
            Assert.IsFalse("100".CompareTo(CompareOperation.LessThan, 90));
            Assert.IsFalse("100".CompareTo(CompareOperation.LessThanOrEquals, 80));
            Assert.IsFalse("100".CompareTo(CompareOperation.StartsWith, "0"));
        }
    }
}
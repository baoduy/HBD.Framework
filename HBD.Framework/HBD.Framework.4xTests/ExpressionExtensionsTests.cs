#region using

using System;
using System.Linq;
using System.Linq.Expressions;
using HBD.Framework.Extensions;
using HBD.Framework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Tests
{
    [TestClass]
    public class ExpressionExtensionsTests
    {
        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void ExtractProperties_Test()
        {
            Expression<Func<TestItem, bool>> ex = i => i.Name != null && i.Id > 0;
            var properties = ex.ExtractProperties().ToArray();
            Assert.IsTrue(properties.Length == 2);
            Assert.IsTrue(properties[0].Name == "Name");
            Assert.IsTrue(properties[1].Name == "Id");
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void GetProperty_Test()
        {
            Expression<Func<TestItem, object>> ex = i => i.Name;
            Assert.IsTrue(ex.ExtractProperties().First().Name == "Name");
        }

        [TestMethod]
        [TestCategory("Fw.Extensions")]
        public void GetProperty_NullExpress_Test()
        {
            Expression<Func<TestItem, object>> ex = null;
            Assert.IsFalse(ex.ExtractProperties().Any());
        }

        //[TestMethod]
        //[TestCategory("Fw.Extensions")]
        //public void GetPropertyNameTest()
        //{
        //    Expression<Func<TestItem, object>> ex = i => i.Name;
        //    Assert.IsTrue(ex.ExtractPropertyNames().FirstOrDefault() == "Name");
        //}

        //[TestMethod]
        //[TestCategory("Fw.Extensions")]
        //public void GetPropertyName_NullExpress_Test()
        //{
        //    Expression<Func<TestItem, object>> ex = null;
        //    Assert.IsNull(ex.ExtractPropertyNames().FirstOrDefault());
        //}

        //[TestMethod]
        //[TestCategory("Fw.Extensions")]
        //public void ToEqualsExpress_WithObject_Test()
        //{
        //    var i = new TestItem();
        //    var ex = i.ToEqualsExpress("Name", "Details");
        //    var props = ex.ExtractProperties().ToArray();

        //    Assert.IsTrue(props.Length == 2);
        //    Assert.IsTrue(props[0].Name == "Name");
        //    Assert.IsTrue(props[1].Name == "Details");
        //}

        //[TestMethod]
        //[TestCategory("Fw.Extensions")]
        //public void ToEqualsExpress_WithNullObject_Test()
        //{
        //    Assert.IsNull(((TestItem) null).ToEqualsExpress("Name"));
        //}

        //[TestMethod]
        //[TestCategory("Fw.Extensions")]
        //public void ToEqualsExpress_WithNullParams_Test()
        //{
        //    Assert.IsNull(new TestItem().ToEqualsExpress(null));
        //}

        //[TestMethod]
        //[TestCategory("Fw.Extensions")]
        //public void ToEqualsExpress_WithDictionary_Test()
        //{
        //    var dic = new Dictionary<string, object> {{"Name", "1"}, {"Id", 2}};
        //    var ex = dic.ToEqualsExpress<TestItem>();
        //    var props = ex.ExtractProperties().ToArray();

        //    Assert.IsTrue(props.Length == 2);
        //    Assert.IsTrue(props[0].Name == "Name");
        //    Assert.IsTrue(props[1].Name == "Id");
        //}

        //[TestMethod]
        //[TestCategory("Fw.Extensions")]
        //public void ToEqualsExpress_WithNullDictionary_Test()
        //{
        //    var ex = ((Dictionary<string, object>) null).ToEqualsExpress<TestItem>();

        //    Assert.IsNull(ex);
        //}

        //[TestMethod]
        //[TestCategory("Fw.Extensions")]
        //public void ToContainsExpress_Test()
        //{
        //    var ex = ExpressionExtensions.ToContainsExpress<TestItem>("A", "Name", "Details");
        //    var props = ex.ExtractProperties().ToArray();

        //    Assert.IsTrue(props.Length == 2);
        //    Assert.IsTrue(props[0].Name == "Name");
        //    Assert.IsTrue(props[1].Name == "Details");
        //}

        //[TestMethod]
        //[TestCategory("Fw.Extensions")]
        //public void ToContainsExpress_NullExpress_Test()
        //{
        //    var ex = ExpressionExtensions.ToContainsExpress<TestItem>("A", null);
        //    Assert.IsNull(ex);
        //}

        //[TestMethod]
        //public void ExtractPropertyNameTest()
        //{
        //    var obj = new TestItem();
        //    Expression<Func<int>> ex = () => obj.Id;

        //    Assert.AreEqual("Id", ex.ExtractPropertyName());
        //}
    }
}
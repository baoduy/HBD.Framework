#region

using HBD.Framework.Data.HtmlGeneration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Tests
{
    [TestClass]
    public class EnumExtensionsTests
    {
        [TestMethod]
        public void ToStringTest()
        {
            var a = StyleNames.BackgroundColor.ToEnumString();
            Assert.AreEqual(a, "background-color");

            a = StyleNames.TextOverflow.ToEnumString();
            Assert.AreEqual(a, "TextOverflow");
        }

        [TestMethod]
        public void GetEnumFromTest()
        {
            var a = "background-color".ToEnum<StyleNames>();
            Assert.AreEqual(a, StyleNames.BackgroundColor);

            a = "BorderCollapse".ToEnum<StyleNames>();
            Assert.AreEqual(a, StyleNames.BorderCollapse);
        }
    }
}
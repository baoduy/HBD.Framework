#region

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Data.HtmlGeneration.Tests
{
    [TestClass]
    public class StyleCollectionTests
    {
        [TestMethod]
        public void AddTest()
        {
            var style = new StyleCollection {{StyleNames.BackgroundColor, "123"}, {StyleNames.TextOverflow, "123"}};
            Assert.AreEqual(style["background-color"], "123");
            Assert.AreEqual(style["text-overflow"], "123");
        }

        [TestMethod]
        public void RemoveTest()
        {
            var style = new StyleCollection {{StyleNames.BackgroundColor, "123"}, {StyleNames.TextOverflow, "123"}};
            style.Remove(StyleNames.BackgroundColor);

            Assert.IsFalse(style.ContainsKey("background-color"));
        }

        [TestMethod]
        public void TryGetValueTest()
        {
            var style = new StyleCollection {{StyleNames.BackgroundColor, "123"}, {StyleNames.TextOverflow, "123"}};
            string str;

            Assert.IsTrue(style.TryGetValue(StyleNames.BackgroundColor, out str));
            Assert.AreEqual(str, "123");
        }

        [TestMethod]
        public void ToStringTest()
        {
            var style = new StyleCollection {{StyleNames.BackgroundColor, "123"}, {StyleNames.TextOverflow, "123"}};
            Assert.AreEqual(style.ToString(), "background-color:123;text-overflow:123;");
        }
    }
}
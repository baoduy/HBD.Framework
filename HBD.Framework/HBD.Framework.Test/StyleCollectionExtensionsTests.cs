using HBD.Framework.Data;
using HBD.Framework.Data.HtmlGeneration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace HBD.Framework.Tests
{
    [TestClass()]
    public class StyleCollectionExtensionsTests
    {
        [TestMethod()]
        public void ToStyleNameTest()
        {
            Assert.IsTrue(StyleNames.TextOverflow.ToStyleName() == "text-overflow");
            Assert.IsTrue(DirectionValues.LeftToRight.ToStyleName() == "ltr");
        }

        [TestMethod()]
        public void AddBackgroundColorTest()
        {
            var coll = new StyleCollection();
            coll.AddBackgroundColor("Red");
            Assert.IsTrue(coll.ContainsKey(StyleNames.BackgroundColor));
        }

        [TestMethod()]
        public void AddBackgroundColorTest1()
        {
            var coll = new StyleCollection();
            coll.AddBackgroundColor(Color.Aqua);
            Assert.IsTrue(coll.ContainsKey(StyleNames.BackgroundColor));
        }

        [TestMethod()]
        public void AddBorderTest()
        {
            var coll = new StyleCollection();
            coll.AddBorder(1, BorderStyleValues.Solid, Color.Aqua);
            Assert.IsTrue(coll.ContainsKey(StyleNames.Border));
        }

        [TestMethod()]
        public void AddBorderTest1()
        {
            var coll = new StyleCollection();
            coll.AddBorder("1", BorderStyleValues.Solid, Color.Aqua);
            Assert.IsTrue(coll.ContainsKey(StyleNames.Border));
        }

        [TestMethod()]
        public void AddBorderCollapseTest()
        {
            var coll = new StyleCollection();
            coll.AddBorderCollapse(BorderCollapseValue.Collapse);
            Assert.IsTrue(coll.ContainsKey(StyleNames.BorderCollapse));
        }

        [TestMethod()]
        public void AddBorderColorTest()
        {
            var coll = new StyleCollection();
            coll.AddBorderColor(Color.Black);
            Assert.IsTrue(coll.ContainsKey(StyleNames.BorderColor));
        }

        [TestMethod()]
        public void AddBorderColorTest1()
        {
            var coll = new StyleCollection();
            coll.AddBorderColor("Red");
            Assert.IsTrue(coll.ContainsKey(StyleNames.BorderColor));
        }

        [TestMethod()]
        public void AddBorderStyleTest()
        {
            var coll = new StyleCollection();
            coll.AddBorderStyle(BorderStyleValues.Solid);
            Assert.IsTrue(coll.ContainsKey(StyleNames.BorderStyle));
        }

        [TestMethod()]
        public void AddBorderSpacingTest()
        {
            var coll = new StyleCollection();
            coll.AddBorderSpacing(1);
            Assert.IsTrue(coll.ContainsKey(StyleNames.BorderSpacing));
        }

        [TestMethod()]
        public void AddBorderSpacingTest1()
        {
            var coll = new StyleCollection();
            coll.AddBorderSpacing("1");
            Assert.IsTrue(coll.ContainsKey(StyleNames.BorderSpacing));
        }

        [TestMethod()]
        public void AddBorderWidthTest()
        {
            var coll = new StyleCollection();
            coll.AddBorderWidth("1");
            Assert.IsTrue(coll.ContainsKey(StyleNames.BorderWidth));
        }

        [TestMethod()]
        public void AddBorderWidthTest1()
        {
            var coll = new StyleCollection();
            coll.AddBorderWidth(1);
            Assert.IsTrue(coll.ContainsKey(StyleNames.BorderWidth));
        }

        [TestMethod()]
        public void AddColorTest()
        {
            var coll = new StyleCollection();
            coll.AddColor(Color.Black);
            Assert.IsTrue(coll.ContainsKey(StyleNames.Color));
        }

        [TestMethod()]
        public void AddColorTest1()
        {
            var coll = new StyleCollection();
            coll.AddColor("Red");
            Assert.IsTrue(coll.ContainsKey(StyleNames.Color));
        }

        [TestMethod()]
        public void AddFontFamilyTest()
        {
            var coll = new StyleCollection();
            coll.AddFontFamily("Red");
            Assert.IsTrue(coll.ContainsKey(StyleNames.FontFamily));
        }

        [TestMethod()]
        public void AddFontSizeTest()
        {
            var coll = new StyleCollection();
            coll.AddFontSize("1");
            Assert.IsTrue(coll.ContainsKey(StyleNames.FontSize));
        }

        [TestMethod()]
        public void AddFontSizeTest1()
        {
            var coll = new StyleCollection();
            coll.AddFontSize(11);
            Assert.IsTrue(coll.ContainsKey(StyleNames.FontSize));
        }

        [TestMethod()]
        public void AddFontStyleTest()
        {
            var coll = new StyleCollection();
            coll.AddFontStyle(FontStyleValues.Italic);
            Assert.IsTrue(coll.ContainsKey(StyleNames.FontStyle));
        }

        [TestMethod()]
        public void AddFontWeightTest()
        {
            var coll = new StyleCollection();
            coll.AddFontWeight(FontWeightValues.Bold);
            Assert.IsTrue(coll.ContainsKey(StyleNames.FontWeight));
        }

        [TestMethod()]
        public void AddHeightTest()
        {
            var coll = new StyleCollection();
            coll.AddHeight(11);
            Assert.IsTrue(coll.ContainsKey(StyleNames.Height));
        }

        [TestMethod()]
        public void AddHeightTest1()
        {
            var coll = new StyleCollection();
            coll.AddHeight("11");
            Assert.IsTrue(coll.ContainsKey(StyleNames.Height));
        }

        [TestMethod()]
        public void AddWidthTest()
        {
            var coll = new StyleCollection();
            coll.AddWidth("11");
            Assert.IsTrue(coll.ContainsKey(StyleNames.Width));
        }

        [TestMethod()]
        public void AddWidthTest1()
        {
            var coll = new StyleCollection();
            coll.AddWidth(11);
            Assert.IsTrue(coll.ContainsKey(StyleNames.Width));
        }

        [TestMethod()]
        public void AddTextDecorationTest()
        {
            var coll = new StyleCollection();
            coll.AddTextDecoration(TextDecorationValues.Overline);
            Assert.IsTrue(coll.ContainsKey(StyleNames.TextDecoration));
        }

        [TestMethod()]
        public void AddDirectionTest()
        {
            var coll = new StyleCollection();
            coll.AddDirection(DirectionValues.LeftToRight);
            Assert.IsTrue(coll.ContainsKey(StyleNames.Direction));
        }

        [TestMethod()]
        public void AddLeftTest()
        {
            var coll = new StyleCollection();
            coll.AddLeft(1);
            Assert.IsTrue(coll.ContainsKey(StyleNames.Left));
        }

        [TestMethod()]
        public void AddLeftTest1()
        {
            var coll = new StyleCollection();
            coll.AddLeft("11");
            Assert.IsTrue(coll.ContainsKey(StyleNames.Left));
        }

        [TestMethod()]
        public void AddMarginTest()
        {
            var coll = new StyleCollection();
            coll.AddMargin(11);
            Assert.IsTrue(coll.ContainsKey(StyleNames.Margin));
        }

        [TestMethod()]
        public void AddMarginTest1()
        {
            var coll = new StyleCollection();
            coll.AddMargin(11,12,12);
            Assert.IsTrue(coll.ContainsKey(StyleNames.Margin));
        }

        [TestMethod()]
        public void AddMarginTest2()
        {
            var coll = new StyleCollection();
            coll.AddMargin(11, 12, 12,13);
            Assert.IsTrue(coll.ContainsKey(StyleNames.Margin));
        }

        [TestMethod()]
        public void AddMarginTest3()
        {
            var coll = new StyleCollection();
            coll.AddMargin(11, 12);
            Assert.IsTrue(coll.ContainsKey(StyleNames.Margin));
        }

        [TestMethod()]
        public void AddMarginBottomTest()
        {
            var coll = new StyleCollection();
            coll.AddMarginBottom(11);
            Assert.IsTrue(coll.ContainsKey(StyleNames.MarginBottom));
        }

        [TestMethod()]
        public void AddMarginLeftTest()
        {
            var coll = new StyleCollection();
            coll.AddMarginLeft(11);
            Assert.IsTrue(coll.ContainsKey(StyleNames.MarginLeft));
        }

        [TestMethod()]
        public void AddMarginRightTest()
        {
            var coll = new StyleCollection();
            coll.AddMarginRight(11);
            Assert.IsTrue(coll.ContainsKey(StyleNames.MarginRight));
        }

        [TestMethod()]
        public void AddMarginTopTest()
        {
            var coll = new StyleCollection();
            coll.AddMarginTop(11);
            Assert.IsTrue(coll.ContainsKey(StyleNames.MarginTop));
        }

        [TestMethod()]
        public void AddOverflowTest()
        {
            var coll = new StyleCollection();
            coll.AddOverflow(OverflowValues.Auto);
            Assert.IsTrue(coll.ContainsKey(StyleNames.Overflow));
        }

        [TestMethod()]
        public void AddOverflowXTest()
        {
            var coll = new StyleCollection();
            coll.AddOverflowX(OverflowValues.Auto);
            Assert.IsTrue(coll.ContainsKey(StyleNames.OverflowX));
        }

        [TestMethod()]
        public void AddOverflowYTest()
        {
            var coll = new StyleCollection();
            coll.AddOverflowY(OverflowValues.Auto);
            Assert.IsTrue(coll.ContainsKey(StyleNames.OverflowY));
        }

        [TestMethod()]
        public void AddPaddingTest()
        {
            var coll = new StyleCollection();
            coll.AddPadding(11);
            Assert.IsTrue(coll.ContainsKey(StyleNames.Padding));
        }

        [TestMethod()]
        public void AddPaddingTest1()
        {
            var coll = new StyleCollection();
            coll.AddPadding(11,12);
            Assert.IsTrue(coll.ContainsKey(StyleNames.Padding));
        }

        [TestMethod()]
        public void AddPaddingTest2()
        {
            var coll = new StyleCollection();
            coll.AddPadding(11,12,13);
            Assert.IsTrue(coll.ContainsKey(StyleNames.Padding));
        }

        [TestMethod()]
        public void AddPaddingTest3()
        {
            var coll = new StyleCollection();
            coll.AddPadding(11,12,13,14);
            Assert.IsTrue(coll.ContainsKey(StyleNames.Padding));
        }

        [TestMethod()]
        public void AddPaddingBottomTest()
        {
            var coll = new StyleCollection();
            coll.AddPaddingBottom(11);
            Assert.IsTrue(coll.ContainsKey(StyleNames.PaddingBottom));
        }

        [TestMethod()]
        public void AddPaddingLeftTest()
        {
            var coll = new StyleCollection();
            coll.AddPaddingLeft(11);
            Assert.IsTrue(coll.ContainsKey(StyleNames.PaddingLeft));
        }

        [TestMethod()]
        public void AddPaddingRightTest()
        {
            var coll = new StyleCollection();
            coll.AddPaddingRight(11);
            Assert.IsTrue(coll.ContainsKey(StyleNames.PaddingRight));
        }

        [TestMethod()]
        public void AddPaddingTopTest()
        {
            var coll = new StyleCollection();
            coll.AddPaddingTop(11);
            Assert.IsTrue(coll.ContainsKey(StyleNames.PaddingTop));
        }

        [TestMethod()]
        public void AddTextAlignTest()
        {
            var coll = new StyleCollection();
            coll.AddTextAlign(TextAlignValues.Center);
            Assert.IsTrue(coll.ContainsKey(StyleNames.TextAlign));
        }

        [TestMethod()]
        public void AddVerticalAlignTest()
        {
            var coll = new StyleCollection();
            coll.AddVerticalAlign(VerticalAlignValues.Bottom);
            Assert.IsTrue(coll.ContainsKey(StyleNames.VerticalAlign));
        }

        [TestMethod()]
        public void AddTextOverflowTest()
        {
            var coll = new StyleCollection();
            coll.AddTextOverflow(TextOverflowValues.Ellipsis);
            Assert.IsTrue(coll.ContainsKey(StyleNames.TextOverflow));
        }

        [TestMethod()]
        public void AddTopTest()
        {
            var coll = new StyleCollection();
            coll.AddTop(11);
            Assert.IsTrue(coll.ContainsKey(StyleNames.Top));
        }

        [TestMethod()]
        public void AddTopTest1()
        {
            var coll = new StyleCollection();
            coll.AddTop("11");
            Assert.IsTrue(coll.ContainsKey(StyleNames.Top));
        }

        [TestMethod()]
        public void AddVisibilityTest()
        {
            var coll = new StyleCollection();
            coll.AddVisibility(VisibilityValues.Hidden);
            Assert.IsTrue(coll.ContainsKey(StyleNames.Visibility));
        }

        [TestMethod()]
        public void AddWhiteSpaceTest()
        {
            var coll = new StyleCollection();
            coll.AddWhiteSpace(WhiteSpaceValues.Nowrap);
            Assert.IsTrue(coll.ContainsKey(StyleNames.WhiteSpace));
        }
    }
}
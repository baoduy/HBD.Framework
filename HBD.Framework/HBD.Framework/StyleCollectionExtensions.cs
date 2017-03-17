using HBD.Framework.Data;
using HBD.Framework.Data.HtmlGeneration;
using System;
using System.Drawing;
using System.Linq;

namespace HBD.Framework
{
    public static class StyleCollectionExtensions
    {
        public static string ToStyleName(this Enum @this)
        {
            string str;
            return @this.TryToEnumString(out str) ? str : string.Join("-", str.SplitWords().Select(s => s.ToLower()));
        }

        public static StyleCollection AddBackgroundColor(this StyleCollection @this, string colorCode)
        {
            @this.Add(StyleNames.BackgroundColor, colorCode);
            return @this;
        }

        public static StyleCollection AddBackgroundColor(this StyleCollection @this, Color color)
        {
            @this.Add(StyleNames.BackgroundColor, color.ToHtmlCode());
            return @this;
        }

        public static StyleCollection AddBorder(this StyleCollection @this, string borderWidth, BorderStyleValues value, Color color)
        {
            @this.Add(StyleNames.Border, $"{borderWidth} {value.ToStyleName()} {color.ToHtmlCode()}");
            return @this;
        }

        public static StyleCollection AddBorder(this StyleCollection @this, int borderWidth, BorderStyleValues value, Color color)
            => @this.AddBorder($"{borderWidth}px", value, color);

        public static StyleCollection AddBorderCollapse(this StyleCollection @this, BorderCollapseValue value)
        {
            @this.Add(StyleNames.BorderCollapse, value.ToStyleName());
            return @this;
        }

        public static StyleCollection AddBorderColor(this StyleCollection @this, string colorCode)
        {
            @this.Add(StyleNames.BorderColor, colorCode);
            return @this;
        }

        public static StyleCollection AddBorderColor(this StyleCollection @this, Color color)
            => @this.AddBorderColor(color.ToHtmlCode());

        public static StyleCollection AddBorderStyle(this StyleCollection @this, BorderStyleValues value)
        {
            @this.Add(StyleNames.BorderStyle, value.ToStyleName());
            return @this;
        }

        public static StyleCollection AddBorderSpacing(this StyleCollection @this, string length)
        {
            @this.Add(StyleNames.BorderSpacing, length);
            return @this;
        }

        public static StyleCollection AddBorderSpacing(this StyleCollection @this, int length)
            => @this.AddBorderSpacing($"{length}px");

        public static StyleCollection AddBorderWidth(this StyleCollection @this, string width)
        {
            @this.Add(StyleNames.BorderWidth, width);
            return @this;
        }

        public static StyleCollection AddBorderWidth(this StyleCollection @this, int width)
            => @this.AddBorderWidth($"{width}px");

        public static StyleCollection AddColor(this StyleCollection @this, string colorCode)
        {
            @this.Add(StyleNames.Color, colorCode);
            return @this;
        }

        public static StyleCollection AddColor(this StyleCollection @this, Color color)
            => @this.AddColor(color.ToHtmlCode());

        public static StyleCollection AddFontFamily(this StyleCollection @this, string fontName)
        {
            @this.Add(StyleNames.FontFamily, fontName);
            return @this;
        }

        public static StyleCollection AddFontSize(this StyleCollection @this, string fontSize)
        {
            @this.Add(StyleNames.FontSize, fontSize);
            return @this;
        }

        public static StyleCollection AddFontSize(this StyleCollection @this, int fontSize)
            => @this.AddFontSize($"{fontSize}px");

        public static StyleCollection AddFontStyle(this StyleCollection @this, FontStyleValues value)
        {
            @this.Add(StyleNames.FontStyle, value.ToStyleName());
            return @this;
        }

        public static StyleCollection AddFontWeight(this StyleCollection @this, FontWeightValues value)
        {
            @this.Add(StyleNames.FontWeight, value.ToStyleName());
            return @this;
        }

        public static StyleCollection AddHeight(this StyleCollection @this, string height)
        {
            @this.Add(StyleNames.Height, height);
            return @this;
        }

        public static StyleCollection AddHeight(this StyleCollection @this, int height)
            => @this.AddHeight($"{height}px");

        public static StyleCollection AddWidth(this StyleCollection @this, string width)
        {
            @this.Add(StyleNames.Width, width);
            return @this;
        }

        public static StyleCollection AddWidth(this StyleCollection @this, int width)
            => @this.AddWidth($"{width}px");

        public static StyleCollection AddTextDecoration(this StyleCollection @this, TextDecorationValues value)
        {
            @this.Add(StyleNames.TextDecoration, value.ToStyleName());
            return @this;
        }

        public static StyleCollection AddDirection(this StyleCollection @this, DirectionValues value)
        {
            @this.Add(StyleNames.Direction, value.ToStyleName());
            return @this;
        }

        public static StyleCollection AddLeft(this StyleCollection @this, string value)
        {
            @this.Add(StyleNames.Left, value);
            return @this;
        }

        public static StyleCollection AddLeft(this StyleCollection @this, int value)
            => @this.AddLeft($"{value}px");

        public static StyleCollection AddMargin(this StyleCollection @this, int top, int right, int bottom, int left)
        {
            @this.Add(StyleNames.Margin, $"{top}px {right}px {bottom}px {left}px");
            return @this;
        }

        public static StyleCollection AddMargin(this StyleCollection @this, int top, int leftAndRight, int bottom)
        {
            @this.Add(StyleNames.Margin, $"{top}px {leftAndRight}px {bottom}px");
            return @this;
        }

        public static StyleCollection AddMargin(this StyleCollection @this, int topAndBottom, int leftAndRight)
        {
            @this.Add(StyleNames.Margin, $"{topAndBottom}px {leftAndRight}px");
            return @this;
        }

        public static StyleCollection AddMargin(this StyleCollection @this, int all)
        {
            @this.Add(StyleNames.Margin, $"{all}px");
            return @this;
        }

        public static StyleCollection AddMarginBottom(this StyleCollection @this, int value)
        {
            @this.Add(StyleNames.MarginBottom, $"{value}px");
            return @this;
        }

        public static StyleCollection AddMarginLeft(this StyleCollection @this, int value)
        {
            @this.Add(StyleNames.MarginLeft, $"{value}px");
            return @this;
        }

        public static StyleCollection AddMarginRight(this StyleCollection @this, int value)
        {
            @this.Add(StyleNames.MarginRight, $"{value}px");
            return @this;
        }

        public static StyleCollection AddMarginTop(this StyleCollection @this, int value)
        {
            @this.Add(StyleNames.MarginTop, $"{value}px");
            return @this;
        }

        public static StyleCollection AddOverflow(this StyleCollection @this, OverflowValues value)
        {
            @this.Add(StyleNames.Overflow, value.ToStyleName());
            return @this;
        }

        public static StyleCollection AddOverflowX(this StyleCollection @this, OverflowValues value)
        {
            @this.Add(StyleNames.OverflowX, value.ToStyleName());
            return @this;
        }

        public static StyleCollection AddOverflowY(this StyleCollection @this, OverflowValues value)
        {
            @this.Add(StyleNames.OverflowY, value.ToStyleName());
            return @this;
        }

        public static StyleCollection AddPadding(this StyleCollection @this, int top, int right, int bottom, int left)
        {
            @this.Add(StyleNames.Padding, $"{top}px {right}px {bottom}px {left}px");
            return @this;
        }

        public static StyleCollection AddPadding(this StyleCollection @this, int top, int leftAndRight, int bottom)
        {
            @this.Add(StyleNames.Padding, $"{top}px {leftAndRight}px {bottom}px");
            return @this;
        }

        public static StyleCollection AddPadding(this StyleCollection @this, int topAndBottom, int leftAndRight)
        {
            @this.Add(StyleNames.Padding, $"{topAndBottom}px {leftAndRight}px");
            return @this;
        }

        public static StyleCollection AddPadding(this StyleCollection @this, int all)
        {
            @this.Add(StyleNames.Padding, $"{all}px");
            return @this;
        }

        public static StyleCollection AddPaddingBottom(this StyleCollection @this, int value)
        {
            @this.Add(StyleNames.PaddingBottom, $"{value}px");
            return @this;
        }

        public static StyleCollection AddPaddingLeft(this StyleCollection @this, int value)
        {
            @this.Add(StyleNames.PaddingLeft, $"{value}px");
            return @this;
        }

        public static StyleCollection AddPaddingRight(this StyleCollection @this, int value)
        {
            @this.Add(StyleNames.PaddingRight, $"{value}px");
            return @this;
        }

        public static StyleCollection AddPaddingTop(this StyleCollection @this, int value)
        {
            @this.Add(StyleNames.PaddingTop, $"{value}px");
            return @this;
        }

        public static StyleCollection AddTextAlign(this StyleCollection @this, TextAlignValues value)
        {
            @this.Add(StyleNames.TextAlign, value.ToStyleName());
            return @this;
        }

        public static StyleCollection AddVerticalAlign(this StyleCollection @this, VerticalAlignValues value)
        {
            @this.Add(StyleNames.VerticalAlign, value.ToStyleName());
            return @this;
        }

        public static StyleCollection AddTextOverflow(this StyleCollection @this, TextOverflowValues value)
        {
            @this.Add(StyleNames.TextOverflow, value.ToStyleName());
            return @this;
        }

        public static StyleCollection AddTop(this StyleCollection @this, string value)
        {
            @this.Add(StyleNames.Top, value);
            return @this;
        }

        public static StyleCollection AddTop(this StyleCollection @this, int value)
            => @this.AddTop($"{value}px");

        public static StyleCollection AddVisibility(this StyleCollection @this, VisibilityValues value)
        {
            @this.Add(StyleNames.Visibility, value.ToStyleName());
            return @this;
        }

        public static StyleCollection AddWhiteSpace(this StyleCollection @this, WhiteSpaceValues value)
        {
            @this.Add(StyleNames.WhiteSpace, value.ToStyleName());
            return @this;
        }
    }
}
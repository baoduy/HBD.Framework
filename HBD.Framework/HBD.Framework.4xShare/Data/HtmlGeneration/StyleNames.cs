#region using

using HBD.Framework.Attributes;

#endregion

namespace HBD.Framework.Data.HtmlGeneration
{
    public enum StyleNames
    {
        [EnumString("background-color")] BackgroundColor,

        [EnumString("border")] Border,

        [EnumString("border-collapse")] BorderCollapse,

        [EnumString("border-color")] BorderColor,

        [EnumString("border-style")] BorderStyle,

        [EnumString("border-spacing")] BorderSpacing,

        [EnumString("border-width")] BorderWidth,

        [EnumString("color")] Color,

        [EnumString("font-family")] FontFamily,

        [EnumString("font-size")] FontSize,

        [EnumString("font-style")] FontStyle,

        [EnumString("font-weight")] FontWeight,

        [EnumString("height")] Height,

        [EnumString("text-decoration")] TextDecoration,

        [EnumString("width")] Width,

        [EnumString("direction")] Direction,

        [EnumString("left")] Left,

        [EnumString("margin")] Margin,

        [EnumString("margin-bottom")] MarginBottom,

        [EnumString("margin-left")] MarginLeft,

        [EnumString("margin-right")] MarginRight,

        [EnumString("margin-top")] MarginTop,

        [EnumString("overflow")] Overflow,

        [EnumString("overflow-x")] OverflowX,

        [EnumString("overflow-y")] OverflowY,

        [EnumString("padding")] Padding,

        [EnumString("padding-bottom")] PaddingBottom,

        [EnumString("padding-left")] PaddingLeft,

        [EnumString("padding-right")] PaddingRight,

        [EnumString("padding-top")] PaddingTop,

        [EnumString("text-align")] TextAlign,

        [EnumString("vertical-align")] VerticalAlign,

        //Didn't put DisplayValue for testing purpose.
        TextOverflow,

        [EnumString("top")] Top,

        [EnumString("visibility")] Visibility,

        [EnumString("white-space")] WhiteSpace
    }
}
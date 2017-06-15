#region using

using HBD.Framework.Attributes;

#endregion

namespace HBD.Framework.Data.HtmlGeneration
{
    public enum DirectionValues
    {
        [EnumString("rtl")] RightToLeft,

        [EnumString("ltr")] LeftToRight
    }
}
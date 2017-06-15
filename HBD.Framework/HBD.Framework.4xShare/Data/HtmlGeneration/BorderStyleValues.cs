#region using

using HBD.Framework.Attributes;

#endregion

namespace HBD.Framework.Data.HtmlGeneration
{
    public enum BorderStyleValues
    {
        [EnumString("dotted")] Dotted,

        [EnumString("solid")] Solid,

        [EnumString("double")] Double,

        [EnumString("dashed")] Dashed
    }
}
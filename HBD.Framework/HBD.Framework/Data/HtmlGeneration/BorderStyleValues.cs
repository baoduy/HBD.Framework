using HBD.Framework.Attributes;

namespace HBD.Framework.Data.HtmlGeneration
{
    public enum BorderStyleValues
    {
        [EnumString("dotted")]
        Dotted,

        [EnumString("solid")]
        Solid,

        [EnumString("double")]
        Double,

        [EnumString("dashed")]
        Dashed
    }
}
using DocumentFormat.OpenXml;
using System.Drawing;

namespace HBD.Framework
{
    public static class DrawingExtensions
    {
        public static HexBinaryValue ToHexValue(this Color @this)
            => @this == Color.Empty
                ? null
                : new HexBinaryValue(ColorTranslator.ToHtml(
                    Color.FromArgb(@this.A, @this.R, @this.G, @this.B)).Replace("#", string.Empty));
    }
}
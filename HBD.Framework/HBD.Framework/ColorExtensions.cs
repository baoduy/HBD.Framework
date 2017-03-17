#region

using System.Drawing;

#endregion

namespace HBD.Framework
{
    public static class ColorExtensions
    {
        public static string ToHtmlCode(this Color @this)
            => ColorTranslator.ToHtml(@this);
    }
}
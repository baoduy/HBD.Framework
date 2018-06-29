#region using

using System.Drawing;

#endregion

namespace HBD.Framework
{
#if !NETSTANDARD2_0
    public static class ColorExtensions
    {
        public static string ToHtmlCode(this Color @this)
            => ColorTranslator.ToHtml(@this);
    }
#endif
}
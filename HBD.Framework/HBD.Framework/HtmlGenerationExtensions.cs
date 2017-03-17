#region

using HBD.Framework.Data.HtmlGeneration;
using System;

#endregion

namespace HBD.Framework
{
    public static class HtmlGenerationExtensions
    {
        public static T WithTableStyle<T>(this T @this, Action<StyleCollection> options) where T : HtmlGenerationBase
        {
            options?.Invoke(@this.TableStyle);
            return @this;
        }

        public static T WithRowStyle<T>(this T @this, Action<StyleCollection> options) where T : HtmlGenerationBase
        {
            options?.Invoke(@this.RowStyle);
            return @this;
        }

        public static T WithAlternativeRowStyle<T>(this T @this, Action<StyleCollection> options)
            where T : HtmlGenerationBase
        {
            options?.Invoke(@this.AlternativeRowStyle);
            return @this;
        }

        public static T WithFooterStyle<T>(this T @this, Action<StyleCollection> options) where T : HtmlGenerationBase
        {
            options?.Invoke(@this.FooterStyle);
            return @this;
        }

        public static T WithHeaderStyle<T>(this T @this, Action<StyleCollection> options) where T : HtmlGenerationBase
        {
            options?.Invoke(@this.HeaderStyle);
            return @this;
        }
    }
}
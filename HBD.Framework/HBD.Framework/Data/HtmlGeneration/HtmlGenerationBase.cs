#region using

using System.Text;

#endregion

namespace HBD.Framework.Data.HtmlGeneration
{
    public abstract class HtmlGenerationBase : IHtmlGeneration
    {
        public virtual StyleCollection TableStyle { get; } = new StyleCollection();
        public virtual StyleCollection HeaderStyle { get; } = new StyleCollection();
        public virtual StyleCollection RowStyle { get; } = new StyleCollection();
        public virtual StyleCollection AlternativeRowStyle { get; } = new StyleCollection();
        public virtual StyleCollection FooterStyle { get; } = new StyleCollection();

        public abstract string Generate();

        public virtual string ToClipboardFormat()
        {
            var htmlBody = Generate();
            var build = new StringBuilder(@"Format: HTML  Format
Version: 1.0
StartHTML:[1]
EndHTML:[2]
StartFragment:[3]
EndFragment:[4]
StartSelection:[3]
EndSelection:[3]
");
            var startHtml = build.Length;

            build.Append(@"<!DOCTYPE HTML PUBLIC  ""-//W3C//DTD HTML 5  Transitional//EN""><!--StartFragment-->");
            var fragmentStart = build.Length;

            build.Append(htmlBody);

            var fragmentEnd = build.Length;

            build.Append(@"<!--EndFragment-->");
            var endHtml = build.Length;

            build.Replace("[1]", $"{startHtml,8}");
            build.Replace("[2]", $"{endHtml,8}");
            build.Replace("[3]", $"{fragmentStart,8}");
            build.Replace("[4]", $"{fragmentEnd,8}");

            return build.ToString();
        }

        public override string ToString() => Generate();
    }
}
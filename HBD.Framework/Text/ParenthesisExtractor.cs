using System.Text.RegularExpressions;

namespace HBD.Framework.Text
{
    /// <summary>
    ///  Extract Text from Patterns "(Text)"
    /// </summary>
    public class ParenthesisExtractor : PatternExtractor
    {
        protected override Regex Regex => new Regex(@"\(([^\)]+)\)", RegexOptions.IgnoreCase);

        public ParenthesisExtractor(string originalString)
            : base(originalString) { }
    }
}
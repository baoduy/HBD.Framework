using System.Text.RegularExpressions;

namespace HBD.Framework.Text
{
    /// <summary>
    /// Extract Text from Patterns "'<Text>'"
    /// </summary>
    public class AngledBracketsExtractor : PatternExtractor
    {
        protected override Regex Regex => new Regex("<([^>]+)>", RegexOptions.IgnoreCase);

        public AngledBracketsExtractor(string originalString)
            : base(originalString) { }
    }
}
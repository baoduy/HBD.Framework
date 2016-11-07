using System.Text.RegularExpressions;

namespace HBD.Framework.Text
{
    /// <summary>
    /// Extract Text from Patterns "[Text]"
    /// </summary>
    public class BracketsExtractor : PatternExtractor
    {
        protected override Regex Regex => new Regex(@"\[([^\]]+)\]", RegexOptions.IgnoreCase);

        public BracketsExtractor(string originalString)
            : base(originalString) { }
    }
}
using System.Text.RegularExpressions;

namespace HBD.Framework.Text
{
    /// <summary>
    ///  Extract Text from Patterns "{Text}"
    /// </summary>
    public class BracesExtractor : PatternExtractor
    {
        protected override Regex Regex => new Regex("{([^>}]+)}", RegexOptions.IgnoreCase);

        public BracesExtractor(string originalString)
            : base(originalString) { }
    }
}
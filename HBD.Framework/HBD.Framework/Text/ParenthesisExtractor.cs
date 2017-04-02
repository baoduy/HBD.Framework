#region using

using System.Text.RegularExpressions;

#endregion

namespace HBD.Framework.Text
{
    /// <summary>
    ///     Extract Text from Patterns "(Text)"
    /// </summary>
    public class ParenthesisExtractor : PatternExtractor
    {
        public ParenthesisExtractor(string originalString)
            : base(originalString)
        {
        }

        protected override Regex Regex => new Regex(@"\(([^\)]+)\)", RegexOptions.IgnoreCase);
    }
}
#region using

using System.Text.RegularExpressions;

#endregion

namespace HBD.Framework.Text
{
    /// <summary>
    ///     Extract Text from Patterns "[Text]"
    /// </summary>
    public class BracketsExtractor : PatternExtractor
    {
        public BracketsExtractor(string originalString)
            : base(originalString)
        {
        }

        protected override Regex Regex => new Regex(@"\[([^\]]+)\]", RegexOptions.IgnoreCase);
    }
}
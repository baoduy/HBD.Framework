using System.Text.RegularExpressions;

namespace HBD.Framework.Text
{
    public abstract class TagPatternExtractor : PatternExtractor<Pattern>
    {
        protected internal abstract char BeginCharacter { get; }
        protected internal abstract char EndCharacter { get; }

        protected TagPatternExtractor(string originalString) : base(originalString)
        {
        }

        protected virtual string GetFormat(char c)
        {
            switch (c)
            {
                case '[': return "\\[";
                case ']': return "\\]";
                case '(': return "\\(";
                case ')': return "\\)";
                default: return c.ToString();
            }
        }

        protected override Regex Regex => new Regex($"{GetFormat(BeginCharacter)}([^{GetFormat(EndCharacter)}]+){GetFormat(EndCharacter)}", RegexOptions.IgnoreCase);
        protected override Pattern CreatePattern(string value) => new Pattern(this, value);
    }
}

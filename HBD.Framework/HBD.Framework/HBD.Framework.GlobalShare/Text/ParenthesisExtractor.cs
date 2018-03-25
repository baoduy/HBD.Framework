#region using


#endregion

namespace HBD.Framework.Text
{
    /// <summary>
    ///     Extract Text from Patterns "(Text)"
    /// </summary>
    public class ParenthesisExtractor : TagPatternExtractor
    {
        public ParenthesisExtractor(string originalString)
            : base(originalString)
        {
        }

        protected internal override char BeginCharacter => '(';

        protected internal override char EndCharacter => ')';
    }
}
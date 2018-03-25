#region using

using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Text
{
    public interface IPattern
    {
        string Value { get; }
        string PatternValue { get; }
    }

    public class Pattern : IPattern
    {
        internal Pattern(TagPatternExtractor owner, string formatedValue)
        {
            Guard.ArgumentIsNotNull(owner, nameof(owner));
            Guard.ArgumentIsNotNull(formatedValue, nameof(formatedValue));

            Owner = owner;
            PatternValue = formatedValue;
            Value = PatternValue.Remove(owner.BeginCharacter.ToString(), owner.EndCharacter.ToString());
        }

        public string Value { get; }
        public TagPatternExtractor Owner { get; }
        public string PatternValue { get; }
    }
}
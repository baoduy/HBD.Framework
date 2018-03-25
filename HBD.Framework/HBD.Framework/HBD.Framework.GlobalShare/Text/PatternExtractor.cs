#region using

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Text
{
    public abstract class PatternExtractor<TPattern> : IPatternExtractor<TPattern> where TPattern : IPattern
    {
        protected PatternExtractor(string originalString)
        {
            Guard.ArgumentIsNotNull(originalString, nameof(originalString));
            OriginalString = originalString;
        }

        public string OriginalString { get; }

        protected abstract Regex Regex { get; }

        public virtual IEnumerator<TPattern> GetEnumerator()
        {
            var p = Regex.Matches(OriginalString);
            return (from a in p.OfType<Match>() select CreatePattern(a.Groups[0].Value)).GetEnumerator();
        }

        protected abstract TPattern CreatePattern(string value);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
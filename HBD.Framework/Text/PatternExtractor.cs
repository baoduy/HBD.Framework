﻿using HBD.Framework.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HBD.Framework.Text
{
    public abstract class PatternExtractor : IPatternExtractor
    {
        public string OriginalString { get; }

        protected abstract Regex Regex { get; }

        protected PatternExtractor(string originalString)
        {
            Guard.ArgumentIsNotNull(originalString, nameof(originalString));
            this.OriginalString = originalString;
        }

        public virtual IEnumerator<IPattern> GetEnumerator()
        {
            var p = Regex.Matches(OriginalString);
            return (from a in p.OfType<Match>() select new Pattern(a.Groups[0].Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
#region using

using System.Text.RegularExpressions;

#endregion

namespace HBD.Framework.Core
{
    /// <summary>
    ///     The helper class allows to compare string with wildcard.
    /// </summary>
    public class Wildcard
    {
        private readonly bool _ignoreCase;
        private readonly string _wildcardPattern;
        private Regex _regex;

        public Wildcard(string wildcardPattern, bool ignoreCase = true)
        {
            _wildcardPattern = wildcardPattern;
            _ignoreCase = ignoreCase;
        }

        public bool IsMatch(string value)
        {
            if (_regex == null)
                Initialize();

            return _regex.IsMatch(value);
        }

        private void Initialize()
        {
            var pattern = "^" + Regex.Escape(_wildcardPattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";
            _regex = _ignoreCase ? new Regex(pattern, RegexOptions.IgnoreCase) : new Regex(pattern);
        }
    }
}
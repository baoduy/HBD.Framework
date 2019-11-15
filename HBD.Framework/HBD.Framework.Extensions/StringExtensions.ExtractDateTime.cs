using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace HBD.Framework.Extensions
{
    public static partial class StringExtensions
    {
        #region Public Methods

        public static IEnumerable<DateTime> ExtractDates(this string @this, params string[] dateFormatStrings)
        {
            if (@this.IsNullOrEmpty()) yield break;

            foreach (var f in dateFormatStrings.Select(f => f.ToDateExtractRegex()).Where(f => f != null))
                foreach (Match match in f.Item2.Matches(@this))
                {
                    if (!match.Success) continue;
                    DateTime dt;
                    if (DateTime.TryParseExact(match.Value, f.Item1, null, DateTimeStyles.None, out dt))
                        yield return dt;
                }
        }

        #endregion Public Methods

        #region Private Methods

        private static Tuple<string, Regex> ToDateExtractRegex(this string dateFormat)
        {
            if (dateFormat.IsNullOrEmpty()) return null;

            //var dIndex = dateFormat.IndexOf("d", StringComparison.Ordinal);
            //var MIndex = dateFormat.IndexOf("M", StringComparison.Ordinal);
            //var yIndex = dateFormat.IndexOf("y", StringComparison.Ordinal);

            var dCount = dateFormat.Count("d");
            var MCount = dateFormat.Count("M");
            var yCount = dateFormat.Count("y");

            if (dCount == 0 && MCount == 0 && yCount == 0) return null;

            var hCount = dateFormat.Count("h");
            var HCount = dateFormat.Count("H");
            var mCount = dateFormat.Count("m");
            var sCount = dateFormat.Count("s");

            var regex = dateFormat.Replace("-", @"[-]")
                .Replace(".", @"[.]")
                .Replace(@"\", @"[\]")
                .Replace("/", "[/]")
                .Replace(":", "[:]")
                .Replace("_", "[_]");

            if (dCount > 0)
                regex = regex.Replace(new string('d', dCount), $"(\\d{{{dCount}}})");
            if (yCount > 0)
                regex = regex.Replace(new string('y', yCount), $"(\\d{{{yCount}}})");
            if (MCount > 0)
                regex = regex.Replace(new string('M', MCount), MCount <= 2 ? $"(\\d{{{MCount}}})" : "([a-zA-Z]+)");

            if (hCount > 0)
                regex = regex.Replace(new string('h', hCount), $"(\\d{{{hCount}}})");
            if (HCount > 0)
                regex = regex.Replace(new string('H', HCount), $"(\\d{{{HCount}}})");
            if (mCount > 0)
                regex = regex.Replace(new string('m', mCount), $"(\\d{{{mCount}}})");
            if (sCount > 0)
                regex = regex.Replace(new string('s', sCount), $"(\\d{{{sCount}}})");

            return new Tuple<string, Regex>(dateFormat, new Regex(regex));
        }

        #endregion Private Methods
    }
}
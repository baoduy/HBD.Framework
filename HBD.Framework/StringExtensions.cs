using HBD.Framework.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace HBD.Framework
{
    public static partial class StringExtensions
    {
        /// <summary> Extract Text from Patterns "'<Text>'" </summary> <param name="this"></param> <returns>Patterns</returns>
        public static IEnumerable<IPattern> ExtractAngledBrackets(this string @this)
            => new AngledBracketsExtractor(@this);

        /// <summary>
        /// Extract Text from Patterns "[Text]"
        /// </summary>
        /// <param name="this"></param>
        /// <returns>Patterns</returns>
        public static IEnumerable<IPattern> ExtractBrackets(this string @this)
            => new BracketsExtractor(@this);

        /// <summary>
        /// Extract Text from Patterns "(Text)"
        /// </summary>
        /// <param name="this"></param>
        /// <returns>Patterns</returns>
        public static IEnumerable<IPattern> ExtractParenthesis(this string @this)
            => new ParenthesisExtractor(@this);

        /// <summary>
        /// Extract Text from Patterns "{Text}"
        /// </summary>
        /// <param name="this"></param>
        /// <returns>Patterns</returns>
        public static IEnumerable<IPattern> ExtractBraces(this string @this)
            => new BracesExtractor(@this);

        /// <summary> Extract Text from Patterns "(Text)", "[Text]", "'<Text>'" and "{Text}"
        /// </summary> <param name="this"></param> <returns>Pattern</returns>
        public static IEnumerable<IPattern> ExtractPatterns(this string @this)
            => @this.ExtractAngledBrackets()
                .Union(@this.ExtractBrackets())
                .Union(@this.ExtractParenthesis())
                .Union(@this.ExtractBraces());

        public static string ExtractNumber(this string @this)
            => new string(@this.Where(c => char.IsDigit(c) || c == '.' || c == ',' || c == '-').ToArray());

        public static string GetMd5HashCode(this string @this)
        {
            if (@this.IsNullOrEmpty()) return string.Empty;

            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(@this));
            var sBuilder = new StringBuilder();
            foreach (var t in data)
                sBuilder.Append(t.ToString("x2"));
            return sBuilder.ToString();
        }

        internal static bool IsBase64String(this string @this)
        {
            var regex = new Regex("^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{4}|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)$");
            return regex.IsMatch(@this);
        }

        /// <summary>
        /// Check whether the string is encrypted with Base64 Encoding algorithms or not.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsEncrypted(this string @this) => @this.IsBase64String();

        public static bool IsNumber(this object @this)
        {
            if (@this == null) return false;
            if (@this is string) return @this.ToString().IsNumber();
            return @this.IsNumericType();
        }

        public static bool IsNotNumber(this object @this) => !@this.IsNumber();

        public static bool IsNumber(this string @this)
        {
            if (@this.IsNullOrEmpty()) return false;
            if (@this.Count(c => c == '.') > 1) return false;
            if (@this.Contains(",,")) return false;
            if (@this.LastIndexOf("-", StringComparison.Ordinal) > 0) return false;
            return @this.Where(c => c != '.' && c != ',' && c != '-').All(c => c >= '0' && c <= '9');
        }

        public static bool IsNotNumber(this string @this) => !@this.IsNumber();

        public static bool IsNullOrEmpty(this string @this) => string.IsNullOrWhiteSpace(@this);

        public static bool IsNotNullOrEmpty(this string @this) => !string.IsNullOrWhiteSpace(@this);

        public static bool IsLessThan(this string @this, string value)
            => string.Compare(@this, value, StringComparison.Ordinal) < 0;

        public static bool IsLessThanIgnoreCase(this string @this, string value)
            => string.Compare(@this, value, StringComparison.OrdinalIgnoreCase) < 0;

        public static bool IsLessThanOrEquals(this string @this, string value)
            => string.Compare(@this, value, StringComparison.Ordinal) <= 0;

        public static bool IsLessThanOrEqualsIgnoreCase(this string @this, string value)
          => string.Compare(@this, value, StringComparison.OrdinalIgnoreCase) <= 0;

        public static bool IsGreaterThan(this string @this, string value)
           => string.Compare(@this, value, StringComparison.Ordinal) > 0;

        public static bool IsGreaterThanIgnoreCase(this string @this, string value)
         => string.Compare(@this, value, StringComparison.OrdinalIgnoreCase) > 0;

        public static bool IsGreaterThanOrEquals(this string @this, string value)
            => string.Compare(@this, value, StringComparison.Ordinal) >= 0;

        public static bool IsGreaterThanOrEqualsIgnoreCase(this string @this, string value)
          => string.Compare(@this, value, StringComparison.OrdinalIgnoreCase) >= 0;

        public static bool EqualsIgnoreCase(this string @this, string value)
            => @this?.Equals(value, StringComparison.OrdinalIgnoreCase) == true;

        public static bool ContainsIgnoreCase(this string @this, string value)
            => @this?.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;

        public static bool ContainsIgnoreCase(this ICollection<string> @this, string value)
            => @this?.Any(s => s.EqualsIgnoreCase(value)) == true;

        public static bool ContainsAny(this string @this, params string[] values)
            => values.Any(@this.Contains);

        public static bool ContainsAll(this string @this, params string[] values)
            => values.All(@this.Contains);

        public static bool ContainsAnyIgnoreCase(this string @this, params string[] values)
            => values.Any(@this.ContainsIgnoreCase);

        public static bool ContainsAllIgnoreCase(this string @this, params string[] values)
            => values.All(@this.ContainsIgnoreCase);

        public static bool StartsWithIgnoreCase(this string @this, string value)
            => @this?.StartsWith(value, StringComparison.OrdinalIgnoreCase) == true;

        public static bool IsAnyOf(this string @this, params string[] values)
            => values.Contains(@this);

        public static bool IsAnyOfIgnoreCase(this string @this, params string[] values)
            => values.ContainsIgnoreCase(@this);

        public static string Remove(this string @this, params string[] values)
        {
            if (@this.IsNullOrEmpty() || values.IsEmpty()) return @this;
            return values.Where(va => !va.IsNullOrEmpty()).Aggregate(@this, (current, va) => current.Replace(va, string.Empty));
        }

        /// <summary>
        /// Count the occurrences of substring in a string.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="value"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static int Count(this string @this, string value, StringComparison comparison = StringComparison.Ordinal)
        {
            if (@this.IsNullOrEmpty() || value.IsNullOrEmpty()) return 0;

            var lastIndex = 0;
            var count = 0;

            while (lastIndex != -1)
            {
                lastIndex = @this.IndexOf(value, lastIndex, comparison);

                if (lastIndex == -1) continue;
                count++;
                lastIndex += value.Length;
            }

            return count;
        }

        /// <summary>
        /// Count the occurrences of substring in a string with OrdinalIgnoreCase
        /// </summary>
        /// <param name="this"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int CountIgnoreCase(this string @this, string value)
            => @this.Count(value, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Replace the string Ignore the sensitive Case.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string ReplaceIgnoreCase(this string @this, string oldValue, string newValue)
        {
            if (@this.IsNullOrEmpty()) return @this;

            return Regex.Replace(
                 @this,
                 Regex.Escape(oldValue),
                 newValue.Replace("$", "$$"),
                 RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Insert the space to between 2 words.
        /// Ex: input: HelloWorld output: Hello World.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string SplitWords(this string @this)
        {
            //Func<char, bool> isLowChar = (c) => c >= 'a' && c <= 'z';
            Func<char, bool> isUpChar = c => c >= 'A' && c <= 'Z';
            Func<char, bool> isNumChar = c => c >= '0' && c <= '9';
            Func<char, char, bool> isChange = (c, l)
                => isUpChar(c) && (!isUpChar(l) || isNumChar(l))
                   || isNumChar(c) && !isNumChar(l);

            if (@this.IsNullOrEmpty()) return @this;

            var builder = new StringBuilder();
            var lastCharater = char.MinValue;

            for (var i = 0; i < @this.Length; i++)
            {
                var c = @this[i];
                if (lastCharater == char.MinValue) lastCharater = c;

                if (isChange(c, lastCharater)
                    && (i - 1 > 0 && @this[i - 1] != ' '))
                    builder.Append(' ');

                builder.Append(c);
                lastCharater = c;
            }
            return builder.ToString();
        }

        /// <summary>
        /// Swap the html string to clipboard format that can me paste in to office application.
        /// </summary>
        /// <param name="html"></param>
        public static string ClipboardHtmlFormat(this string html)
        {
            if (html.IsClipboardHtmlFormat()) return html;

            var build = new StringBuilder(@"Format: HTML  Format
Version: 1.0
StartHTML:<<<<<<<1
EndHTML:<<<<<<<2
StartFragment:<<<<<<<3
EndFragment:<<<<<<<4
StartSelection:<<<<<<<3
EndSelection:<<<<<<<3
");
            var startHtml = build.Length;

            build.Append(@"<!DOCTYPE HTML PUBLIC  ""-//W3C//DTD HTML 4.0  Transitional//EN""><!--StartFragment-->");
            var fragmentStart = build.Length;

            build.Append(html);

            var fragmentEnd = build.Length;

            build.Append(@"<!--EndFragment-->");
            var endHtml = build.Length;

            build.Replace("<<<<<<<1", $"{startHtml,8}");
            build.Replace("<<<<<<<2", $"{endHtml,8}");
            build.Replace("<<<<<<<3", $"{fragmentStart,8}");
            build.Replace("<<<<<<<4", $"{fragmentEnd,8}");

            return build.ToString();
        }

        private static bool IsClipboardHtmlFormat(this string html)
            => html.ContainsAllIgnoreCase("Version", "StartHTML", "EndHTML", "StartFragment", "EndFragment", "StartSelection", "EndSelection");
    }
}
#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using HBD.Framework.Text;

#endregion

namespace HBD.Framework
{
    public static partial class StringExtensions
    {
        /// <summary> Extract Text from Patterns "'<Text>'" </summary>
        /// <param name="this"></param>
        /// <returns>Patterns</returns>
        public static IEnumerable<IPattern> ExtractAngledBrackets(this string @this)
            => new AngledBracketsExtractor(@this);

        /// <summary>
        ///     Extract Text from Patterns "[Text]"
        /// </summary>
        /// <param name="this"></param>
        /// <returns>Patterns</returns>
        public static IEnumerable<IPattern> ExtractBrackets(this string @this)
            => new BracketsExtractor(@this);

        /// <summary>
        ///     Extract Text from Patterns "(Text)"
        /// </summary>
        /// <param name="this"></param>
        /// <returns>Patterns</returns>
        public static IEnumerable<IPattern> ExtractParenthesis(this string @this)
            => new ParenthesisExtractor(@this);

        /// <summary>
        ///     Extract Text from Patterns "{Text}"
        /// </summary>
        /// <param name="this"></param>
        /// <returns>Patterns</returns>
        public static IEnumerable<IPattern> ExtractBraces(this string @this)
            => new BracesExtractor(@this);

        /// <summary>
        ///     Extract Text from Patterns "(Text)", "[Text]", "'<Text>'" and "{Text}"
        /// </summary>
        /// <param name="this"></param>
        /// <returns>Pattern</returns>
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
        ///     Check whether the string is encrypted with Base64 Encoding algorithms or not.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsEncrypted(this string @this) => @this.IsBase64String();

        public static bool IsStringOrValueType(this object @this)
        {
            if (@this == null) return false;
            return (@this as Type)?.IsStringOrValueType()
                   ?? (@this as PropertyInfo)?.IsStringOrValueType()
                   ?? @this.GetType().IsStringOrValueType();
        }

        public static bool IsStringOrValueType(this PropertyInfo @this)
            => @this?.PropertyType.IsStringOrValueType() == true;

        public static bool IsStringOrValueType(this Type @this)
        {
            if (@this == null) return false;
            var result = Type.GetTypeCode(@this) == TypeCode.String || (!@this.IsGenericType && @this.IsValueType);
            //Null able is IsValueType and IsGenericType
            return result;
        }

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

        public static bool EqualsIgnoreCase(this string @this, string value)
            => @this?.Equals(value, StringComparison.OrdinalIgnoreCase) == true;

        public static bool ContainsIgnoreCase(this string @this, string value)
            => @this?.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;

        /// <summary>
        /// Check whether the string collection is contains item.
        /// Ignore Case when do the comparison.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool AnyIgnoreCase(this IEnumerable<string> @this, string item)
            => @this?.Any(s => s.EqualsIgnoreCase(item)) == true;

        /// <summary>
        /// Check whether the string is contains any item in the list of values.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool AnyOf(this string @this, params string[] values)
            => values.Any(@this.Contains);

        /// <summary>
        /// Check whether the string is contains any item in the list of values.
        /// Ignore Case when do the comparison.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool AnyOfIgnoreCase(this string @this, params string[] values)
            => values.Any(@this.ContainsIgnoreCase);

        /// <summary>
        /// Check whether the string is contains all items in the list of values.
        /// Ignore Case when do the comparison.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool AllIgnoreCase(this string @this, params string[] values)
            => values.All(@this.ContainsIgnoreCase);

        public static bool StartsWithIgnoreCase(this string @this, string value)
            => @this?.StartsWith(value, StringComparison.OrdinalIgnoreCase) == true;

        public static string Remove(this string @this, params string[] values)
        {
            if (@this.IsNullOrEmpty() || values.NotAnyItem()) return @this;
            return values.Where(va => !va.IsNullOrEmpty())
                .Aggregate(@this, (current, va) => current.Replace(va, string.Empty));
        }

        /// <summary>
        ///     Count the occurrences of substring in a string.
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
        ///     Count the occurrences of substring in a string with OrdinalIgnoreCase
        /// </summary>
        /// <param name="this"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int CountIgnoreCase(this string @this, string value)
            => @this.Count(value, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        ///     Replace the string Ignore the sensitive Case.
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

        public static string[] SplitWords(this string @this)
            => @this.ConsolidateWords().Split(' ');

        /// <summary>
        ///     Insert the space to between 2 words.
        ///     Ex: input: HelloWorld output: Hello World.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string ConsolidateWords(this string @this)
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

                if (isChange(c, lastCharater) && i - 1 > 0 && @this[i - 1] != ' ')
                    builder.Append(' ');

                builder.Append(c);
                lastCharater = c;
            }
            return builder.ToString();
        }
    }
}
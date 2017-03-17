#region

using System;
using System.Collections.Generic;
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
        private static readonly IList<SupportedFormat> SupportedFormats;

        static Pattern()
        {
            SupportedFormats = new List<SupportedFormat>
            {
                new SupportedFormat {Start = "<", End = ">", Type = PatternType.AngledBrackets},
                new SupportedFormat {Start = "[", End = "]", Type = PatternType.Brackets},
                new SupportedFormat {Start = "(", End = ")", Type = PatternType.Parenthesis},
                new SupportedFormat {Start = "{", End = "}", Type = PatternType.Braces}
            };
        }

        internal Pattern(string formatedValue)
        {
            Guard.ArgumentIsNotNull(formatedValue, "Pattern Value");
            PatternValue = formatedValue;

            var found = false;
            foreach (var item in SupportedFormats)
            {
                if (!PatternValue.StartsWith(item.Start) || !PatternValue.EndsWith(item.End)) continue;

                Value = PatternValue.Remove(item.Start, item.End);
                Type = item.Type;

                found = true;
                break;
            }

            if (!found) throw new NotSupportedException($"The Value '{formatedValue}' is not supported.");
        }

        public PatternType Type { get; private set; }

        public string Value { get; }
        public string PatternValue { get; }

        internal class SupportedFormat
        {
            public string Start { get; set; }
            public string End { get; set; }
            public PatternType Type { get; set; }
        }
    }

    [Flags]
    public enum PatternType
    {
        /// <summary>
        ///     Open and Close Bracket "[Text]"
        /// </summary>
        Brackets = 1,

        /// <summary>
        ///     Open and Close Parenthesis "(Text)"
        /// </summary>
        Parenthesis = 2,

        /// <summary>
        ///     Open and Close AngledBracket "<Text>"
        /// </summary>
        AngledBrackets = 4,

        /// <summary>
        ///     Open and Close AngledBracket "{Text}"
        /// </summary>
        Braces = 8
    }
}
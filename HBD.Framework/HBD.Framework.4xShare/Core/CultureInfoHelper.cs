#region using

using System;
using System.Globalization;
using HBD.Framework.Properties;

#endregion

namespace HBD.Framework.Core
{
    public static class CultureInfoHelper
    {
        public static string[] GetCurrecySymbols()
        {
            const string key = "CultureInfoHelper_CurrencySymbols";

            return Resources.ResourceManager.GetString(key)
                ?.Split(new[] {',', ';'}, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] GetCurrecyCharacters()
        {
            const string key = "CultureInfoHelper_CurrencyCharacters";

            return Resources.ResourceManager.GetString(key)
                ?.Split(new[] {',', ';'}, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] GetDefaultDateFormats()
        {
            var dateFormate = CultureInfo.CurrentCulture.DateTimeFormat;
            return new[]
                {dateFormate.LongDatePattern, dateFormate.ShortDatePattern, dateFormate.SortableDateTimePattern};
        }
    }
}
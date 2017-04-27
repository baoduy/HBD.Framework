#region using

using System;
using System.Globalization;
using HBD.Framework.Cache;
using HBD.Framework.Properties;

#endregion

namespace HBD.Framework.Core
{
    public static class CultureInfoHelper
    {
        public static string[] GetCurrecySymbols()
        {
            const string key = "CultureInfoHelper_CurrencySymbols";
            var values = CacheManager.Default.Get<string[]>(key);

            if (values?.NotAny() == true) return values;
            values = Resources.ResourceManager.GetString(key)
                ?.Split(new[] {',', ';'}, StringSplitOptions.RemoveEmptyEntries);
            CacheManager.Default.SetValueToProperty(key, values);
            return values;
        }

        public static string[] GetCurrecyCharacters()
        {
            const string key = "CultureInfoHelper_CurrencyCharacters";
            var values = CacheManager.Default.Get<string[]>(key);

            if (values?.NotAny() == true) return values;
            values = Resources.ResourceManager.GetString(key)
                ?.Split(new[] {',', ';'}, StringSplitOptions.RemoveEmptyEntries);
            CacheManager.Default.SetValueToProperty(key, values);
            return values;
        }

        public static string[] GetDefaultDateFormats()
        {
            var dateFormate = CultureInfo.CurrentCulture.DateTimeFormat;
            return new[]
                {dateFormate.LongDatePattern, dateFormate.ShortDatePattern, dateFormate.SortableDateTimePattern};
        }
    }
}
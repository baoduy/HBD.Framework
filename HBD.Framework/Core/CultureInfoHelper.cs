using HBD.Framework.Cache;
using HBD.Framework.Properties;
using System;
using System.Globalization;

namespace HBD.Framework.Core
{
    public static class CultureInfoHelper
    {
        public static string[] GetCurrecySymbols()
        {
            //var method = MethodBase.GetCurrentMethod();
            //var key = $"{method.DeclaringType.FullName}.{method.Name}";
            var key = "CultureInfoHelper_CurrencySymbols";
            var values = CacheManager.Default.Get<string[]>(key);

            if (!values.IsEmpty()) return values;
            values = Resource.ResourceManager.GetString(key)?
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            CacheManager.Default.SetValueToProperty(key, values);
            return values;
        }

        public static string[] GetCurrecyCharacters()
        {
            var key = "CultureInfoHelper_CurrencyCharacters";
            var values = CacheManager.Default.Get<string[]>(key);

            if (!values.IsEmpty()) return values;
            values = Resource.ResourceManager.GetString(key)?
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            CacheManager.Default.SetValueToProperty(key,values);
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
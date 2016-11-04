using HBD.Framework.Core;
using System;

namespace HBD.Framework
{
    public static class DateTimeExtensions
    {
        public static int Quarter(this DateTime @this)
        {
            Guard.ArgumentIsNotNull(@this, "DateTime");

            if (@this.Month <= 3)
                return 1;

            if (@this.Month <= 6)
                return 2;

            return @this.Month <= 9 ? 3 : 4;
        }

        public static DateTime? LastDayOfMoth(this DateTime? @this)
        {
            if (@this == null) return null;
            return @this.Value.LastDayOfMoth();
        }

        public static DateTime LastDayOfMoth(this DateTime @this)
        {
            var lastday = DateTime.DaysInMonth(@this.Year, @this.Month);
            return new DateTime(@this.Year, @this.Month, lastday, @this.Hour, @this.Minute, @this.Second, @this.Millisecond);
        }
    }
}
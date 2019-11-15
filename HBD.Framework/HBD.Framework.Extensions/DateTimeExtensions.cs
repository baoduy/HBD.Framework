using System;

namespace HBD.Framework.Extensions
{
    public static class DateTimeExtensions
    {
        #region Public Methods

        public static DateTime? LastDayOfMoth(this DateTime? @this) => @this?.LastDayOfMoth();

        public static DateTime LastDayOfMoth(this DateTime @this)
        {
            var lastday = DateTime.DaysInMonth(@this.Year, @this.Month);
            return new DateTime(@this.Year, @this.Month, lastday, @this.Hour, @this.Minute, @this.Second,
                @this.Millisecond);
        }

        public static int Quarter(this DateTime @this)
        {
            if (@this.Month <= 3)
                return 1;

            if (@this.Month <= 6)
                return 2;

            return @this.Month <= 9 ? 3 : 4;
        }

        #endregion Public Methods
    }
}
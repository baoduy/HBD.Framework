using System;
using System.Collections;
using System.Linq;

namespace HBD.Framework.Extensions
{
    public static class ComparisionExtensions
    {
        public static bool IsEquals(this object @this, object obj) => @this.CompareTo(obj) == 0;

        public static bool IsNotEquals(this object @this, object obj) => !@this.IsEquals(obj);

        #region Object comparision methods

        public static int CompareTo(this object @this, object obj)
        {
            if (@this != null && obj != null)
            {
                if (@this.Equals(obj)) return 0;

                if (
                    (!(@this is string) && !(obj is string)) &&
                    (@this is IComparable && obj is IComparable) &&
                    @this.GetType() == obj.GetType()
                    ) // If not are string and are IComparable
                {
                    return ((IComparable)@this).CompareTo((IComparable)obj);
                }

                var strThis = @this.ToString().Trim();
                var strObj = obj.ToString().Trim();

                if (strThis.IsNumber() && strObj.IsNumber())
                    return strThis.ChangeType<decimal>().CompareTo(strObj.ChangeType<decimal>());

                return string.Compare(@this.ToString().Trim(), obj.ToString().Trim(), StringComparison.OrdinalIgnoreCase);
                // Compare as string.
            }

            if (@this == null && obj == null) return 0;
            return @this != null ? 1 : -1;
        }

        public static bool CompareTo(this object @this, CompareOperation operation, object obj)
        {
            switch (operation)
            {
                case CompareOperation.GreaterThan:
                    return @this.CompareTo(obj) > 0;

                case CompareOperation.GreaterThanOrEquals:
                    return @this.CompareTo(obj) >= 0;

                case CompareOperation.LessThan:
                    return @this.CompareTo(obj) < 0;

                case CompareOperation.LessThanOrEquals:
                    return @this.CompareTo(obj) <= 0;

                case CompareOperation.Contains:
                    return @this.IsContains(obj);

                case CompareOperation.NotContains:
                    return !@this.IsContains(obj);

                case CompareOperation.StartsWith:
                    return @this.IsStartsWith(obj);

                case CompareOperation.EndsWith:
                    return @this.IsEndsWith(obj);

                case CompareOperation.In:
                    return @this.IsIn(obj);

                case CompareOperation.NotIn:
                    return !@this.IsIn(obj);

                case CompareOperation.IsNull:
                    return @this == null && obj == null;

                case CompareOperation.NotNull:
                    return @this != null || obj != null;

                case CompareOperation.NotEquals:
                    return @this.IsNotEquals(obj);

                default:
                case CompareOperation.Equals:
                    return @this.IsEquals(obj);
            }
        }

        private static bool IsContains(this object @this, object obj)
        {
            if (@this == null || obj == null) return false;
            var strA = @this.ToString();
            var strB = obj.ToString();
            return strA.Trim().ToLower().Contains(strB.Trim().ToLower());
        }

        private static bool IsStartsWith(this object @this, object obj)
        {
            if (@this == null || obj == null) return false;
            var strA = @this.ToString();
            var strB = obj.ToString();
            return strA.Trim().ToLower().StartsWith(strB.Trim().ToLower());
        }

        private static bool IsEndsWith(this object @this, object obj)
        {
            if (@this == null || obj == null) return false;
            var strA = @this.ToString();
            var strB = obj.ToString();
            return strA.Trim().ToLower().EndsWith(strB.Trim().ToLower());
        }

        private static bool IsIn(this object @this, object obj)
        {
            if (@this == null || obj == null) return false;
            if (!(obj is IEnumerable)) return false;

            return ((IEnumerable)obj).Cast<object>().Any(@this.IsEquals);
        }

        #endregion Object comparision methods
    }

    public enum CompareOperation
    {
        Equals,
        NotEquals,
        GreaterThan,
        LessThan,
        GreaterThanOrEquals,
        LessThanOrEquals,
        Contains,
        NotContains,
        StartsWith,
        EndsWith,
        In,
        NotIn,
        IsNull,
        NotNull
    }
}
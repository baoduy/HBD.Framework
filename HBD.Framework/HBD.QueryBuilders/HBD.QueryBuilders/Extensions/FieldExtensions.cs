#region

using HBD.Data.Comparisons;
using HBD.Data.Comparisons.Base;
using HBD.QueryBuilders.Base;
using HBD.QueryBuilders.Providers;

#endregion

namespace HBD.QueryBuilders
{
    public static class FieldExtensions
    {
        //private static readonly SqlConditionRender SqlRender = new SqlConditionRender();

        public static Field As(this string @this, string alias) => new Field(@this).As(alias);

        public static T As<T>(this T @this, string alisaName) where T : Aliasable
        {
            if (@this == null) return null;
            @this.Alias = alisaName;
            return @this;
        }

        //#endregion

        //public static FieldSet By(this string @this, object value) => new FieldSet(@this, value);
        //public static FieldSet By(this Field @this, object value) => (object)@this == null ? null : new FieldSet(@this.Name, value);

        //#region FieldSet

        #region Field with Values

        public static ValueCondition StartWith(this Field @this, object value)
            => new ValueCondition(@this.Name, CompareOperation.StartsWith, value);

        public static ValueCondition EndWith(this Field @this, object value)
            => new ValueCondition(@this.Name, CompareOperation.EndsWith, value);

        public static ValueCondition Contains(this Field @this, object value)
            => new ValueCondition(@this.Name, CompareOperation.Contains, value);

        public static ValueCondition NotContains(this Field @this, object value)
            => new ValueCondition(@this.Name, CompareOperation.NotContains, value);

        public static ValueCondition In(this Field @this, params object[] values)
            => new ValueCondition(@this.Name, CompareOperation.In, values);

        public static ValueCondition NotIn(this Field @this, params object[] values)
            => new ValueCondition(@this.Name, CompareOperation.NotIn, values);

        public static ValueCondition IsNull(this Field @this)
            => new ValueCondition(@this.Name, CompareOperation.IsNull, null);

        public static ValueCondition NotNull(this Field @this)
            => new ValueCondition(@this.Name, CompareOperation.NotNull, null);

        #endregion Field with Values
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBD.Framework.Data;
using System.Collections;
using HBD.Framework.Core;

namespace HBD.Framework.Data.Utilities
{
    public static class FilterManager
    {
        public static IFilterClause CreateFilterClause(string fieldName, CompareOperation operation, object value)
        {
            return new FilterClause() { FieldName = fieldName, Operation = operation, Value = value };
        }

        private static IFilterClause CreateBinaryFilter(IFilterClause leftFilter, BinaryOperation operation, IFilterClause rightFilter)
        {
            return new BinaryFilterItem() { LeftClause = leftFilter, Operation = operation, RightClause = rightFilter };
        }

        public static IFilterClause IsEquals(string fieldName, object value)
        {
            Guard.ArgumentNotNull(fieldName, "FieldName");
            Guard.MustBeValueType(value, "Value");
            return CreateFilterClause(fieldName, CompareOperation.Equals, value);
        }

        public static IFilterClause EndsWith(string fieldName, object value)
        {
            Guard.ArgumentNotNull(fieldName, "FieldName");
            Guard.MustBeValueType(value, "Value");
            return CreateFilterClause(fieldName, CompareOperation.EndsWith, value);
        }

        public static IFilterClause StartsWith(string fieldName, object value)
        {
            Guard.ArgumentNotNull(fieldName, "FieldName");
            Guard.MustBeValueType(value, "Value");
            return CreateFilterClause(fieldName, CompareOperation.StartsWith, value);
        }

        public static IFilterClause GreaterThan(string fieldName, object value)
        {
            Guard.ArgumentNotNull(fieldName, "FieldName");
            Guard.MustBeValueType(value, "Value");
            return CreateFilterClause(fieldName, CompareOperation.GreaterThan, value);
        }

        public static IFilterClause GreaterThanOrEquals(string fieldName, object value)
        {
            Guard.ArgumentNotNull(fieldName, "FieldName");
            Guard.MustBeValueType(value, "Value");
            return CreateFilterClause(fieldName, CompareOperation.GreaterThanOrEquals, value);
        }

        public static IFilterClause LessThan(string fieldName, object value)
        {
            Guard.ArgumentNotNull(fieldName, "FieldName");
            Guard.MustBeValueType(value, "Value");
            return CreateFilterClause(fieldName, CompareOperation.LessThan, value);
        }

        public static IFilterClause LessThanOrEquals(string fieldName, object value)
        {
            Guard.ArgumentNotNull(fieldName, "FieldName");
            Guard.MustBeValueType(value, "Value");
            return CreateFilterClause(fieldName, CompareOperation.LessThanOrEquals, value);
        }

        public static IFilterClause Contains(string fieldName, object value)
        {
            Guard.ArgumentNotNull(fieldName, "FieldName");
            Guard.MustBeValueType(value, "Value");
            return CreateFilterClause(fieldName, CompareOperation.Contains, value);
        }

        public static IFilterClause NotContains(string fieldName, object value)
        {
            Guard.ArgumentNotNull(fieldName, "FieldName");
            Guard.MustBeValueType(value, "Value");
            return CreateFilterClause(fieldName, CompareOperation.NotContains, value);
        }

        public static IFilterClause In(string fieldName, IEnumerable<object> collection)
        {
            Guard.ArgumentNotNull(fieldName, "FieldName");
            Guard.MustBeValuesType(collection);
            return CreateFilterClause(fieldName, CompareOperation.In, collection);
        }

        public static IFilterClause NotIn(string fieldName, IEnumerable<object> collection)
        {
            Guard.ArgumentNotNull(fieldName, "FieldName");
            Guard.MustBeValuesType(collection);
            return CreateFilterClause(fieldName, CompareOperation.NotIn, collection);
        }

        public static IFilterClause AndsWith(this IFilterClause leftClause, IFilterClause rightClause)
        {
            Guard.ArgumentNotNull(leftClause, "Left Filter Clause");
            Guard.ArgumentNotNull(rightClause, "Right Filter Clause");

            var b = CreateBinaryFilter(leftClause, BinaryOperation.AND, rightClause);
            leftClause = b;
            return b;
        }

        public static IFilterClause OrWith(this IFilterClause leftClause, IFilterClause rightClause)
        {
            Guard.ArgumentNotNull(leftClause, "Left Filter Clause");
            Guard.ArgumentNotNull(rightClause, "Right Filter Clause");

            var b = CreateBinaryFilter(leftClause, BinaryOperation.OR, rightClause);
            leftClause = b;
            return b;
        }
    }
}

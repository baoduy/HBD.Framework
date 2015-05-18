using HBD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace HBD.Framework.Extension
{
    public static class ExpressionExtensions
    {
        private static readonly MethodInfo ContainsMethod = typeof(string).GetMethod("Contains");
        private static readonly MethodInfo StartsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        private static readonly MethodInfo EndsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });

        public static Expression<Func<T, bool>> ToEquals<T>(T item, params string[] propertyNames)
        {
            Guard.ArgumentNotNull(item, "Entity");
            Guard.ArgumentNotNull(propertyNames, "propertyNames");

            var pe = Expression.Parameter(typeof(T));

            dynamic lambda = (from p in propertyNames
                              let value = item.GetValue(p)
                              let left = Expression.Property(pe, p)
                              let right = Expression.Constant(value)
                              select Expression.Equal(left, right))
                              .Aggregate<Expression, dynamic>(null, (current, compare) => current == null ? compare : Expression.And(current, compare));

            return Expression.Lambda(lambda, pe);
        }

        public static Expression<Func<T, bool>> ToEquals<T>(Dictionary<string, object> keyValues)
        {
            Guard.ArgumentNotNull(keyValues, "keyValues");

            var pe = Expression.Parameter(typeof(T));

            dynamic lambda = (from k in keyValues
                              let value = k.Value
                              let left = Expression.Property(pe, k.Key)
                              let right = Expression.Constant(value)
                              select Expression.Equal(left, right))
                              .Aggregate<BinaryExpression, dynamic>(null, (current, compare) => current == null ? compare : Expression.And(current, compare));

            return Expression.Lambda(lambda, pe);
        }

        public static Expression<Func<T, bool>> ToContains<T>(string value, params string[] propertyNames)
        {
            Guard.ArgumentNotNull(propertyNames, "propertyNames");

            var pe = Expression.Parameter(typeof(T));

            dynamic lambda = (from p in propertyNames
                              let left = Expression.Property(pe, p)
                              let right = Expression.Constant(value)
                              select Expression.Call(left, ContainsMethod, right))
                             .Aggregate<Expression, dynamic>(null, (current, compare) => current == null ? compare : Expression.Or(current, compare));

            return Expression.Lambda(lambda, pe);
        }

    }
}

#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework
{
    public static class ExpressionExtensions
    {
        private static readonly MethodInfo ContainsMethod = typeof(string).GetTypeInfo().GetMethod("Contains");

        //private static readonly MethodInfo StartsWithMethod = typeof(string).GetMethod("StartsWith",
        //    new[] { typeof(string) });

        //private static readonly MethodInfo EndsWithMethod = typeof(string).GetMethod("EndsWith",
        //    new[] { typeof(string) });

        public static IEnumerable<PropertyInfo> ExtractProperties<T, TKey>(this Expression<Func<T, TKey>>[] @this)
            where T : class
        {
            if (@this == null) yield break;

            foreach (var lamdar in @this)
                foreach (var p in lamdar.ExtractProperties())
                    yield return p;
        }

        public static IEnumerable<PropertyInfo> ExtractProperties<T, TKey>(this Expression<Func<T, TKey>> @this)
            where T : class
        {
            if (@this == null) yield break;

            var queue = new Queue<Expression>();
            queue.Enqueue(@this.Body);

            while (queue.Count > 0)
            {
                var ex = queue.Dequeue();

                var expression = ex as MemberExpression;
                if (expression != null)
                {
                    dynamic tmp = expression;
                    yield return tmp.Member;
                }
                else if (ex is UnaryExpression)
                {
                    dynamic tmp = ((UnaryExpression)ex).Operand as MemberExpression;
                    yield return tmp?.Member;
                }
                else if (ex is BinaryExpression)
                {
                    var tmp = ex as BinaryExpression;
                    queue.Enqueue(tmp.Left);
                    queue.Enqueue(tmp.Right);
                }
                else if (ex is MethodCallExpression)
                {
                    dynamic tmp = ex as MethodCallExpression;
                    yield return tmp.Object.Member;
                }
            }
        }

        public static IEnumerable<string> ExtractPropertyNames<T, TKey>(this Expression<Func<T, TKey>> @this)
            where T : class
            => @this.ExtractProperties().Select(p => p.Name);

        public static string ExtractPropertyName<T>(this Expression<Func<T>> propertyExpression)
            => propertyExpression.ExtractProperty().Name;

        public static PropertyInfo ExtractProperty<T>(this Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            if (!(propertyExpression.Body is MemberExpression memberExpression))
                throw new NotSupportedException(propertyExpression.Body.GetType().FullName);

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
                throw new NotSupportedException(memberExpression.Member.GetType().FullName);

            var getMethod = property.GetMethod;
            if (getMethod.IsStatic)
                throw new NotSupportedException("Static Method");

            return property;
        }

        public static PropertyInfo ExtractProperty<TSource, TField>(this Expression<Func<TSource, TField>> propertyExpression)
        {
            Guard.ArgumentIsNotNull(propertyExpression, nameof(propertyExpression));

            MemberExpression expr = null;

            switch (propertyExpression.Body)
            {
                case MemberExpression _:
                    expr = (MemberExpression)propertyExpression.Body;
                    break;
                case UnaryExpression _:
                    expr = (MemberExpression)((UnaryExpression)propertyExpression.Body).Operand;
                    break;
                default:
                    var message = $"Expression '{propertyExpression}' not supported.";
                    throw new ArgumentException(message, nameof(propertyExpression));
            }

            return expr.Member as PropertyInfo;
        }

        public static Expression<Func<T, bool>> ToEqualsExpress<T>(this T @this, params string[] propertyNames)
            where T : class
        {
            if (@this == null) return null;
            if (propertyNames.NotAny()) return null;

            var pe = Expression.Parameter(typeof(T));

            dynamic expression = null;
            foreach (var p in propertyNames.Where(a => a.IsNotNullOrEmpty()))
            {
                var value = @this.PropertyValue(p);
                var left = Expression.Property(pe, p);
                var right = Expression.Constant(value);

                expression = expression == null
                    ? Expression.Equal(left, right)
                    : Expression.And(expression, Expression.Equal(left, right));
            }

            return Expression.Lambda(expression, pe);
        }

        public static Expression<Func<T, bool>> ToEqualsExpress<T>(this Dictionary<string, object> @this)
        {
            if (@this.NotAny()) return null;

            var pe = Expression.Parameter(typeof(T));

            dynamic expression = null;
            foreach (var k in @this.Where(a => a.Key.IsNotNullOrEmpty()))
            {
                var value = k.Value;
                var left = Expression.Property(pe, k.Key);
                var right = Expression.Constant(value);

                expression = expression == null
                    ? Expression.Equal(left, right)
                    : Expression.And(expression, Expression.Equal(left, right));
            }

            return Expression.Lambda(expression, pe);
        }

        public static Expression<Func<T, bool>> ToContainsExpress<T>(string value, params string[] propertyNames)
        {
            if (propertyNames.NotAny() == true) return null;

            var pe = Expression.Parameter(typeof(T));

            dynamic expression = null;
            foreach (var p in propertyNames.Where(a => a.IsNotNullOrEmpty()))
            {
                var left = Expression.Property(pe, p);
                var right = Expression.Constant(value);

                expression = expression == null
                    ? Expression.Call(left, ContainsMethod, right)
                    : Expression.Or(expression, Expression.Call(left, ContainsMethod, right));
            }

            return Expression.Lambda(expression, pe);
        }
    }
}
using HBD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HBD.Framework.ThreeLayers
{
    public static class DynamicOrderExtention
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string[] properties)
        {
            IOrderedQueryable<T> orderedItems = null;

            foreach (var p in properties)
            {
                orderedItems = orderedItems == null ? source.OrderBy(p) : orderedItems.ThenBy(p);
            }
            return orderedItems;
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string[] properties)
        {
            IOrderedQueryable<T> orderedItems = null;

            foreach (var p in properties)
            {
                orderedItems = orderedItems == null ? source.OrderByDescending(p) : orderedItems.ThenByDescending(p);
            }
            return orderedItems;
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            Guard.ArgumentNotNull(property, "Property");
            return ApplyOrder<T>(source, property, "OrderBy");
        }
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            Guard.ArgumentNotNull(property, "Property");
            return ApplyOrder<T>(source, property, "OrderByDescending");
        }
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenBy");
        }
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenByDescending");
        }
        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            var props = property.Split('.');
            var type = typeof(T);
            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            //Get Property of Child Object and create expresstion OrderMethod(t=>t.Property)
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                var pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            //Create generic order method
            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            var lambda = Expression.Lambda(delegateType, expr, arg);

            var result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });

            return (IOrderedQueryable<T>)result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.CompilerServices;
using HBD.Framework.Core;
using HBD.Framework.Extension;

namespace HBD.Framework.ThreeLayers
{
    public static class DbContextExtention
    {
        public static Type GetUnProxyType(Type type)
        {
            return type.Name.Contains("_") && type.Name.StartsWith(type.BaseType.Name) ? type.BaseType : type;
        }

        public static Type GetUnProxyType(this object obj)
        {
            Guard.ArgumentNotNull(obj, "object");
            if (obj is Type)
                return GetUnProxyType((Type)obj);
            return GetUnProxyType(obj.GetType());
        }

        public static void ApplyDefaultValues(this IEntity item, bool isModifiedState)
        {
            if (string.IsNullOrEmpty(item.CreatedBy))
                item.CreatedBy = IdentityExtension.Name;
            if (item.CreatedDate == DateTime.MinValue)
                item.CreatedDate = DateTime.Now;

            if (!isModifiedState) return;

            if (string.IsNullOrEmpty(item.UpdatedBy))
                item.UpdatedBy = IdentityExtension.Name;
            if (item.UpdatedDate == null
                || item.UpdatedDate == DateTime.MinValue)
                item.UpdatedDate = DateTime.Now;
        }

        public static string[] GetKeyNames<TEntity>(this DbContext dbContext) where TEntity : class
        {
            return dbContext.GetKeyNames(typeof(TEntity));
        }

        /// <summary>
        /// Get the Primary Key Names of Entity
        /// </summary>
        /// <param name="dbContext">DbContext</param>
        /// <param name="entityType">The type of entity</param>
        /// <returns>Primary Keys</returns>
        public static string[] GetKeyNames(this DbContext dbContext, Type entityType)
        {
            Guard.ArgumentNotNull(entityType, "entityType");
            var realType = entityType.GetUnProxyType();

            //var objectSet = ((IObjectContextAdapter)dbContext).ObjectContext.CreateObjectSet<TEntity>();

            //These code create the objectSet above.
            var objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
            dynamic objectSet = typeof(ObjectContext).GetMethod("CreateObjectSet", Type.EmptyTypes)
                .MakeGenericMethod(realType).Invoke(objectContext, null);

            IEnumerable<dynamic> keyMembers = objectSet.EntitySet.ElementType.KeyMembers;
            return keyMembers.Select(k => (string)k.Name).ToArray();
        }

        public static IEntity GetById(this DbContext dbContext, IEntity item)
        {
            var keyValues = dbContext.GetKeyValues(item).Select(k => k.Value).ToArray();
            return dbContext.GetById(item.GetType(), keyValues) as IEntity;
        }

        public static T GetById<T>(this DbContext dbContext, T item) where T : class
        {
            var keyValues = dbContext.GetKeyValues(item).Select(k => k.Value).ToArray();
            return dbContext.GetById<T>(keyValues);
        }

        public static T GetById<T>(this DbContext dbContext, params object[] keyValues) where T : class
        {
            return dbContext.Set<T>().Find(keyValues);
        }

        public static object GetById(this DbContext dbContext, Type itemType, params object[] keyValues)
        {
            if (itemType.IsInterface) throw new ArgumentException("They Entity Type cannot an Interface.");
            return dbContext.Set(itemType.GetUnProxyType()).Find(keyValues);
        }

        public static void Add(this DbContext dbContext, IEntity item)
        {
            dbContext.Set(item.GetUnProxyType()).Add(item);
        }

        public static void AddRange(this DbContext dbContext, IEnumerable<IEntity> items)
        {
            dbContext.Set(items.GetType().GenericTypeArguments[0]).AddRange(items);
        }

        public static void Remove(this DbContext dbContext, IEntity item)
        {
            dbContext.Set(item.GetUnProxyType()).Remove(item);
        }

        public static void RemoveRange(this DbContext dbContext, IEnumerable<IEntity> items)
        {
            dbContext.Set(items.GetType().GenericTypeArguments[0]).RemoveRange(items);
        }

        public static bool IsNew(this DbContext dbContext, IEntity item)
        {
            //Get Primary Key Values on Item
            var keyValues = dbContext.GetKeyValues(item);

            //For Entity with Primary is auto generation.
            if (keyValues.Any(k => k.Value.IsNullOrLessThanZeo()))
                return true;

            //For Entity with Primary key is not auto generation.
            var finded = dbContext.GetById(item);
            return finded == null;
        }

        /// <summary>
        /// Get the primary key values of Entity
        /// </summary>
        /// <param name="dbContext">DbContext</param>
        /// <param name="entity">Entity instance</param>
        /// <returns>The values of primary keys</returns>
        public static IDictionary<string, object> GetKeyValues<TEntity>(this DbContext dbContext, TEntity entity) where TEntity : class
        {
            var type = entity.GetType();
            var keyNames = GetKeyNames(dbContext, type);

            return keyNames.ToDictionary(k => k, entity.GetValue);
        }

        /// <summary>
        /// Paging Queryable. PageIndex must >=0 and pageSize > 0
        /// </summary>
        /// <typeparam name="TEntity">Entity must be a class</typeparam>
        public static IPagableEntity<TEntity> ToPagable<TEntity>(this IQueryable<TEntity> query, int pageIndex, int pageSize) where TEntity : class
        {
            if (query.Expression.Type == typeof(IOrderedQueryable<TEntity>))
                return new Pagable<TEntity>((IOrderedQueryable<TEntity>)query, pageIndex, pageSize);
            else
            {
                //Order by the first Property of Entity
                var name = Extension.Extensions.GetProperties<TEntity>().First().Name;
                return new Pagable<TEntity>(query.OrderBy(name), pageIndex, pageSize);
            }
        }
    }
}

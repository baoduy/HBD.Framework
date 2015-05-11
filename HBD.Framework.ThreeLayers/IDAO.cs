using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
namespace HBD.Framework.ThreeLayers
{
    public interface IDao<TEntity> : IDisposable where TEntity : class, IEntity
    {
        DbContext DbContext { get; }
        DbSet<TEntity> DbSet { get; }
        bool Save(params TEntity[] items);
        bool Delete(params TEntity[] items);
        bool DeleteById(params object[] keyValues);

        /// <summary>
        /// Check item property is already existed in DB or not
        /// </summary>
        /// <typeparam name="TKey">The column (field) meed to be check</typeparam>
        /// <param name="item">The instace of TEntity</param>
        /// <param name="keySelectors">The column (field) meed to be check</param>
        /// <returns>Indicate existed or not</returns>
        bool IsExisted(TEntity item, params string[] propertyNames);
        IQueryable<TEntity> GetAll();
        TEntity GetById(params object[] keyValues);
    }
}

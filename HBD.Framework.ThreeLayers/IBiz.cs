using HBD.Framework.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace HBD.Framework.ThreeLayers
{
    public interface IBiz<TEntity> where TEntity : class, IEntity
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, object>> orderby, ListSortDirection orderDirection = ListSortDirection.Ascending);
        IQueryable<TEntity> Get<TKey>(Expression<Func<TEntity, bool>> filterBy, Expression<Func<TEntity, TKey>> orderby, ListSortDirection orderDirection = ListSortDirection.Ascending);
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filterBy = null, string orderBy = null, ListSortDirection orderDirection = ListSortDirection.Ascending);

        IPagableEntity<TEntity> GetPage(Expression<Func<TEntity, bool>> filterBy, string orderBy, ListSortDirection orderDirection, int pageIndex, int pageSize);
        IPagableEntity<TEntity> GetPage(string orderBy, ListSortDirection orderDirection, int pageIndex, int pageSize);
        IPagableEntity<TEntity> GetPage<TKey>(Expression<Func<TEntity, bool>> filterBy, Expression<Func<TEntity, TKey>> orderby, ListSortDirection orderDirection, int pageIndex, int pageSize);
        IPagableEntity<TEntity> GetPage<TKey>(Expression<Func<TEntity, TKey>> orderby, ListSortDirection orderDirection, int pageIndex, int pageSize);
        IPagableEntity<TEntity> GetPage(Expression<Func<TEntity, bool>> filterBy, int pageIndex, int pageSize);
        IPagableEntity<TEntity> GetPage(int pageIndex = 0, int pageSize = 0);
        IEnumerable<HBDKeyValuePair<TKey, TValue>> GetSelectionList<TKey, TValue>( Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, TValue>> valueSelector );

        /// <summary>
        /// Check item property is already existed in DB or not
        /// </summary>
        /// <typeparam name="TKey">The column (field) meed to be check</typeparam>
        /// <param name="item">The instace of TEntity</param>
        /// <param name="keySelector">The column (field) meed to be check</param>
        /// <returns>Indicate existed or not</returns>
        bool IsExisted<TKey>(TEntity item, Expression<Func<TEntity, TKey>> keySelector);
        TEntity GetById(params object[] id);
        IDictionary<string, object> GetPrimaryKeyValues(TEntity item);
        bool Save(params TEntity[] items);
        bool Delete(params TEntity[] items);
        bool DeleteById(params object[] keyValues);
    }
}

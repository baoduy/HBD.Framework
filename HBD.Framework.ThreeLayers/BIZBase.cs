using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using HBD.Framework.Extension;
using HBD.Framework.Core;
using HBD.Libraries.Unity;
using System.Data.Entity;
using Microsoft.Practices.Unity;
using System.Collections;
using System.Data.Entity.Validation;
using HBD.Framework.Log;

namespace HBD.Framework.ThreeLayers
{
    public abstract class BizBase<TEntity, TDbContext> : IBiz<TEntity>
        where TEntity : class, IEntity
        where TDbContext : DbContext
    {
        static TDbContext _dbContext;
        /// <summary>
        /// Singletone instace of DAL.
        /// </summary>
        public static TDbContext DbContext
        {
            get
            {
                if ( _dbContext != null ) return _dbContext;

                //If nto registered then Register the type to Unity Manager
                if ( !UnityManager.Container.IsRegistered<TDbContext>() )
                    UnityManager.Container.RegisterType<TDbContext>();

                //Ensure SqlProviderServices is loaded. The error may happen when running on WindowForms
                var sqlInstance= System.Data.Entity.SqlServer.SqlProviderServices.Instance;

                //3. Get Item from Unity Manager.
                _dbContext = UnityManager.Container.Resolve<TDbContext>();

                //Disable Proxy Creation of DbContext;
                //_dbContext.Configuration.ProxyCreationEnabled = false;
                return _dbContext;
            }
        }

        protected virtual Expression<Func<TEntity, object>>[] GetIncludeProperties()
        {
            return new Expression<Func<TEntity, object>>[] { };
        }

        public DbSet<TEntity> DbSet
        {
            get { return DbContext.Set<TEntity>(); }
        }

        public IQueryable<TEntity> GetAll()
        {
            IQueryable<TEntity> set = this.DbSet;
            var includeds = this.GetIncludeProperties();
            if ( includeds == null || includeds.Length <= 0 ) return set;

            return includeds.Aggregate( set, ( current, expression ) => current.Include( expression ) );
        }

        /// <summary>
        /// Get All Existing Entities.
        /// </summary>
        /// <returns>List of Entities</returns>
        public IQueryable<TEntity> GetAll( Expression<Func<TEntity, object>> orderby, ListSortDirection orderDirection = ListSortDirection.Ascending )
        {
            return this.Get( null, orderby, orderDirection );
        }

        public IQueryable<TEntity> Get<TKey>( Expression<Func<TEntity, bool>> filterBy, Expression<Func<TEntity, TKey>> orderBy, ListSortDirection orderDirection = ListSortDirection.Ascending )
        {
            var items = this.GetAll();

            if ( filterBy != null )
                items = items.Where( filterBy );

            if ( orderBy != null )
                items = orderDirection == ListSortDirection.Ascending ? items.OrderBy( orderBy ) : items.OrderByDescending( orderBy );
            return items;
        }

        public IQueryable<TEntity> Get( Expression<Func<TEntity, bool>> filterBy, string orderBy, ListSortDirection orderDirection = ListSortDirection.Ascending )
        {
            var items = this.GetAll();

            if ( filterBy != null )
                items = items.Where( filterBy );

            if ( !string.IsNullOrEmpty( orderBy ) )
                items = orderDirection == ListSortDirection.Ascending ? items.OrderBy( orderBy ) : items.OrderByDescending( orderBy );

            return items;
        }

        public virtual IQueryable<TEntity> Get( Expression<Func<TEntity, bool>> filterBy = null )
        {
            return filterBy == null ? this.GetAll() : this.GetAll().Where( filterBy );
        }

        public IPagableEntity<TEntity> GetPage<TKey>( Expression<Func<TEntity, bool>> filterBy, Expression<Func<TEntity, TKey>> orderBy, ListSortDirection orderDirection, int pageIndex, int pageSize )
        {
            return this.Get( filterBy, orderBy, orderDirection ).ToPagable( pageIndex, pageSize );
        }

        public IPagableEntity<TEntity> GetPage( Expression<Func<TEntity, bool>> filterBy, string orderBy, ListSortDirection orderDirection, int pageIndex, int pageSize )
        {
            return this.Get( filterBy, orderBy, orderDirection ).ToPagable( pageIndex, pageSize );
        }

        public IPagableEntity<TEntity> GetPage( string orderBy, ListSortDirection orderDirection, int pageIndex, int pageSize )
        {
            return this.Get( null, orderBy, orderDirection ).ToPagable( pageIndex, pageSize );
        }

        public IPagableEntity<TEntity> GetPage<TKey>( Expression<Func<TEntity, TKey>> orderby, ListSortDirection orderDirection, int pageIndex, int pageSize )
        {
            var items = this.Get( null, orderby, orderDirection );
            return items.ToPagable( pageIndex, pageSize );
        }

        public IPagableEntity<TEntity> GetPage( Expression<Func<TEntity, bool>> filterBy, int pageIndex, int pageSize )
        {
            var items = this.Get( filterBy );
            return items.ToPagable( pageIndex, pageSize );
        }

        public IPagableEntity<TEntity> GetPage( int pageIndex, int pageSize )
        {
            return this.Get( null, ( Expression<Func<TEntity, object>> )null ).ToPagable( pageIndex, pageSize );
        }

        public IEnumerable<HBDKeyValuePair<TKey, TValue>> GetSelectionList<TKey, TValue>( Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, TValue>> valueSelector )
        {
            var keyName = Extensions.GetProperty( keySelector ).Name;
            var valName = Extensions.GetProperty( valueSelector ).Name;

            var kFunc = keySelector.Compile();
            var vFunc = valueSelector.Compile();

            var selected = this.GetAll().SelectPartially<TEntity>( keyName, valName );

            foreach ( var i in selected )
                yield return new HBDKeyValuePair<TKey, TValue>( kFunc( i ), vFunc( i ) );
        }

        /// <summary>
        /// Get Entity by Primary Key.
        /// </summary>
        /// <param name="keyValues">The Primary Key</param>
        /// <returns>Entity</returns>
        public virtual TEntity GetById( params object[] keyValues )
        {
            return DbContext.GetById<TEntity>( keyValues );
        }

        /// <summary>
        /// Get Primary key values of entity
        /// </summary>
        /// <param name="item">Entity</param>
        /// <returns>Dictionary of pair Key value</returns>
        public virtual IDictionary<string, object> GetPrimaryKeyValues( TEntity item )
        {
            var keys = DbContext.GetKeyNames<TEntity>();
            var dic = new Dictionary<string, object>();
            if ( keys == null ) return dic;

            foreach ( var k in keys )
                dic.Add( k, item.GetValue( k ) );
            return dic;
        }

        /// <summary>
        /// Check item property is already existed in DB or not
        /// </summary>
        /// <typeparam name="TKey">The column (field) meed to be check</typeparam>
        /// <param name="item">The instace of TEntity</param>
        /// <param name="keySelector">The column (field) meed to be check</param>
        /// <returns>Indicate existed or not</returns>
        public bool IsExisted<TKey>( TEntity item, Expression<Func<TEntity, TKey>> keySelectors )
        {
            var properties = Extensions.GetPropertiesName( keySelectors );
            return this.IsExisted( item, properties );
        }

        public bool IsExisted( TEntity item, params string[] propertyNames )
        {
            var compareFunc = ExpressionExtensions.ToEquals( item, propertyNames );
            return this.DbSet.Any( compareFunc );
        }

        protected virtual bool IsNew( TEntity item )
        {
            //Get Primary Key Values on Item
            var keyValues = DbContext.GetKeyValues( item );

            //For Entity with Primary is auto generation.
            if ( keyValues.Any( k => k.Value.IsNullOrLessThanZeo() ) )
                return true;

            //For Entity with Primary key is not auto generation.
            var keys = keyValues.Select( k => k.Key ).ToArray();
            if ( !this.IsExisted( item, keys ) )
                return true;
            return false;
        }

        /// <summary>
        /// Consolidate values when saving entity
        /// </summary>
        /// <param name="item">Saving Entity</param>
        protected virtual void OnSaving( IEntity item )
        {
            Guard.ArgumentNotNull( item, "Entity" );

            if ( DbContext.IsNew( item ) )
            {
                //Apply Default values for item.
                //The child item default value will ahndle by ValidateEntity
                item.ApplyDefaultValues( false );

                //Add new item to DbSet. This item will be inserted to DB.
                DbContext.Add( item );
            }
            //Item to be update to DB.
            else
            {
                //Check Detached State
                var newItemEntry = DbContext.Entry( item );
                if ( newItemEntry.State != EntityState.Detached && newItemEntry.State != EntityState.Modified ) return;

                //Check if item is detached from DbSet.
                //Try to find the orginal item in Local by primarykey.
                //If found attachedEntity in Local.
                //Try to attach attachedEntity to DbSet again.
                //Update the new values from item in UI to attachedEntity.
                //If not found that mean item is new one then add that item to DbContext.

                //Apply Default values for item.
                //The child item default value will ahndle by ValidateEntity
                item.ApplyDefaultValues( true );

                var originalEntity = DbContext.GetById( item );
                if ( originalEntity != null )
                {
                    var originalEntry = DbContext.Entry( originalEntity );
                    //Set new Value to Original Entity
                    originalEntry.CurrentValues.SetValues( item );
                }
                else
                {
                    DbContext.Add( item );
                }
            }
        }

        /// <summary>
        /// Save relationship Properties for TEntity
        /// </summary>
        /// <param name="item">TEntity item</param>
        private void OnSavingRelationship( TEntity item )
        {
            var keyValues = DbContext.GetKeyValues( item ).Select( k => k.Value ).ToArray();
            var originalEntity = this.GetById( keyValues );
            var entry = DbContext.Entry( originalEntity );

            foreach ( var property in this.GetIncludeProperties() )
            {
                var val = property.Compile()( item );
                if ( !( val is IEnumerable ) ) continue;

                var hasChanged = ( val as IEnumerable ).Cast<IEntity>().Any( i => DbContext.Entry( i ).State != EntityState.Unchanged );
                if ( !hasChanged ) continue;

                foreach ( var t in val as IEnumerable )
                    this.OnSaving( t as IEntity );

                var origVal = property.Compile()( entry.Entity );
                if ( !( origVal is IEnumerable ) ) continue;

                //Delete the unchanges items as the latest items were marked Added or Modified already.
                foreach ( IEntity t in ( from object t in origVal as IEnumerable where DbContext.Entry( t ).State == EntityState.Unchanged select t ).ToArray() )
                {
                    DbContext.Remove( t );
                }
            }
        }

        /// <summary>
        /// Insert or Update entity to DB
        /// </summary>
        /// <param name="items"></param>
        public bool Save( params TEntity[] items )
        {
            Guard.ArgumentNotNull( items, "Entity" );
            try
            {
                foreach ( var t in items )
                {
                    //Set Lastes value to DbContext
                    this.OnSaving( t );
                    //Set Relationship values for Entity
                    this.OnSavingRelationship( t );
                }
                //Save changes to DB.
                DbContext.SaveChanges();
                return true;
            }
            catch ( DbEntityValidationException ex )
            {
                LogManager.Write( ex );
                throw;
            }
            catch ( Exception ex )
            {
                LogManager.Write( ex );
            }
            return false;
        }

        protected virtual void OnDeleting( IEntity item )
        {
            var entry = DbContext.Entry( item );

            if ( entry.State == EntityState.Detached )
            {
                var originalEntity = DbContext.GetById( item );
                if ( originalEntity != null )
                    DbContext.Remove( originalEntity );
                else entry.State = EntityState.Deleted;
            }
            else DbContext.Remove( item );
        }

        private void OnDeletingRelationship( TEntity item )
        {
            foreach ( var property in this.GetIncludeProperties() )
            {
                var val = property.Compile()( item );
                if ( !( val is IEnumerable ) ) continue;

                foreach ( var t in ( val as IEnumerable ).Cast<IEntity>().ToArray() )
                    this.OnDeleting( t );
            }
        }

        /// <summary>
        /// Delete Entity in DB
        /// </summary>
        /// <param name="items"></param>
        public bool Delete( params TEntity[] items )
        {
            try
            {
                foreach ( var t in items )
                {
                    this.OnDeletingRelationship( t );
                    this.OnDeleting( t );
                }

                //Save changes to DB.
                DbContext.SaveChanges();
            }
            catch ( Exception ex )
            {
                LogManager.Write( ex );
                return false;
            }

            return true;
        }

        public bool DeleteById( params object[] keyValues )
        {
            var item = this.GetById( keyValues );
            if ( item == null ) return false;

            this.OnDeletingRelationship( item );
            this.DbSet.Remove( item );

            try
            {
                //Save changes to DB.
                DbContext.SaveChanges();
            }
            catch ( Exception ex )
            {
                LogManager.Write( ex );
                return false;
            }

            return true;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using HBD.Framework.Core;
using HBD.Framework.Extension;
using HBD.Framework.Log;
using HBD.Libraries.Unity;
using Microsoft.Practices.Unity;

namespace HBD.Framework.ThreeLayers
{
    public abstract class DaoBase<TEntity> : IDao<TEntity>
        where TEntity : class, IEntity
    {
        DbContext _dbContext;
        public DbContext DbContext
        {
            get
            {
                if (_dbContext != null) return _dbContext;

                //Ensure SqlProviderServices is loaded. This error happen when running on WindowForms
                var _ = System.Data.Entity.SqlServer.SqlProviderServices.Instance;

                var type = typeof(DbContext);
                //If nto registered then Register the type to Unity Manager
                if (!UnityManager.Container.IsRegistered(type))
                    UnityManager.Container.RegisterType( type, new InjectionConstructor( this.NameOrConnectionString ) );

                //3. Get Item from Unity Manager.
                _dbContext = (DbContext)UnityManager.Container.Resolve(type);

                //Disable Proxy Creation of DbContext;
                //_dbContext.Configuration.ProxyCreationEnabled = false;
                return _dbContext;
            }
        }

        protected abstract string NameOrConnectionString { get; }

        /// <summary>
        /// Include required properties sso_RolePermission. This properties will be take care when update, delete.
        /// </summary>
        /// <returns>Expression</returns>
        protected virtual Expression<Func<TEntity, object>>[] GetIncludeProperties()
        {
            return new Expression<Func<TEntity, object>>[] { };
        }

        public DbSet<TEntity> DbSet
        {
            get { return this.DbContext.Set<TEntity>(); }
        }

        /// <summary>
        /// Get All Item include Relationship Properties by GetInclude()
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetAll()
        {
            IQueryable<TEntity> set = this.DbSet;
            var includeds = this.GetIncludeProperties();
            if (includeds == null || includeds.Length <= 0) return set;

            return includeds.Aggregate(set, (current, expression) => current.Include(expression));
        }

        public TEntity GetById(params object[] keyValues)
        {
            return this.DbContext.GetById<TEntity>(keyValues);
        }

        protected virtual bool IsNew(TEntity item)
        {
            //Get Primary Key Values on Item
            var keyValues = this.DbContext.GetKeyValues(item);

            //For Entity with Primary is auto generation.
            if (keyValues.Any(k => k.Value.IsNullOrLessThanZeo()))
                return true;

            //For Entity with Primary key is not auto generation.
            var keys = keyValues.Select(k => k.Key).ToArray();
            if (!this.IsExisted(item, keys))
                return true;
            return false;
        }

        public bool IsExisted(TEntity item, params string[] propertyNames)
        {
            var compareFunc = ExpressionExtensions.ToEquals(item, propertyNames);
            return this.DbSet.Any(compareFunc);
        }

        /// <summary>
        /// Consolidate values when saving entity
        /// </summary>
        /// <param name="item">Saving Entity</param>
        protected virtual void OnSaving(IEntity item)
        {
            Guard.ArgumentNotNull(item, "Entity");

            if (this.DbContext.IsNew(item))
            {
                //Apply Default values for item.
                //The child item default value will ahndle by ValidateEntity
                item.ApplyDefaultValues(false);

                //Add new item to DbSet. This item will be inserted to DB.
                this.DbContext.Add(item);
            }
            //Item to be update to DB.
            else
            {
                //Check Detached State
                var newItemEntry = this.DbContext.Entry(item);
                if (newItemEntry.State != EntityState.Detached && newItemEntry.State != EntityState.Modified) return;

                //Check if item is detached from DbSet.
                //Try to find the orginal item in Local by primarykey.
                //If found attachedEntity in Local.
                //Try to attach attachedEntity to DbSet again.
                //Update the new values from item in UI to attachedEntity.
                //If not found that mean item is new one then add that item to DbContext.

                //Apply Default values for item.
                //The child item default value will ahndle by ValidateEntity
                item.ApplyDefaultValues(true);

                var originalEntity = this.DbContext.GetById(item);
                if (originalEntity != null)
                {
                    var originalEntry = this.DbContext.Entry(originalEntity);
                    //Set new Value to Original Entity
                    originalEntry.CurrentValues.SetValues(item);
                }
                else
                {
                    this.DbContext.Add(item);
                }
            }
        }

        /// <summary>
        /// Save relationship Properties for TEntity
        /// </summary>
        /// <param name="item">TEntity item</param>
        private void OnSavingRelationship(TEntity item)
        {
            var keyValues = this.DbContext.GetKeyValues(item).Select(k => k.Value).ToArray();
            var originalEntity = this.GetById(keyValues);
            var entry = this.DbContext.Entry(originalEntity);

            foreach (var property in this.GetIncludeProperties())
            {
                var val = property.Compile()(item);
                if (!(val is IEnumerable)) continue;

                var hasChanged = (val as IEnumerable).Cast<IEntity>().Any(i => this.DbContext.Entry(i).State != EntityState.Unchanged);
                if (!hasChanged) continue;

                foreach (var t in val as IEnumerable)
                    this.OnSaving(t as IEntity);

                var origVal = property.Compile()(entry.Entity);
                if (!(origVal is IEnumerable)) continue;

                //Delete the unchanges items as the latest items were marked Added or Modified already.
                foreach (IEntity t in (from object t in origVal as IEnumerable where this.DbContext.Entry(t).State == EntityState.Unchanged select t).ToArray())
                {
                    this.DbContext.Remove(t);
                }
            }
        }

        /// <summary>
        /// Insert or Update entity to DB
        /// </summary>
        /// <param name="items"></param>
        public bool Save(params TEntity[] items)
        {
            Guard.ArgumentNotNull(items, "Entity");
            try
            {
                foreach (var t in items)
                {
                    //Set Lastes value to DbContext
                    this.OnSaving(t);
                    //Set Relationship values for Entity
                    this.OnSavingRelationship(t);
                }
                //Save changes to DB.
                this.DbContext.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                LogManager.Write(ex);
                throw;
            }
            catch (Exception ex)
            {
                LogManager.Write(ex);
            }
            return false;
        }

        protected virtual void OnDeleting(IEntity item)
        {
            var entry = this.DbContext.Entry(item);

            if (entry.State == EntityState.Detached)
            {
                var originalEntity = this.DbContext.GetById(item);
                if (originalEntity != null)
                    this.DbContext.Remove(originalEntity);
                else entry.State = EntityState.Deleted;
            }
            else this.DbContext.Remove(item);
        }

        private void OnDeletingRelationship(TEntity item)
        {
            foreach (var property in this.GetIncludeProperties())
            {
                var val = property.Compile()(item);
                if (!(val is IEnumerable)) continue;

                foreach (var t in (val as IEnumerable).Cast<IEntity>().ToArray())
                    this.OnDeleting(t);
            }
        }

        /// <summary>
        /// Delete Entity in DB
        /// </summary>
        /// <param name="items"></param>
        public bool Delete(params TEntity[] items)
        {
            try
            {
                foreach (var t in items)
                {
                    this.OnDeletingRelationship(t);
                    this.OnDeleting(t);
                }

                //Save changes to DB.
                this.DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogManager.Write(ex);
                return false;
            }

            return true;
        }

        public bool DeleteById(params object[] keyValues)
        {
            var item = this.GetById(keyValues);
            if (item == null) return false;

            this.OnDeletingRelationship(item);
            this.DbSet.Remove(item);

            try
            {
                //Save changes to DB.
                this.DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogManager.Write(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Dispose the DbContext object
        /// </summary>
        public void Dispose()
        {
            if (this._dbContext == null) return;

            this._dbContext.Dispose();
            this._dbContext = null;
        }
    }
}

using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace HBD.Framework.ThreeLayers
{
    /// <summary>
    /// The ExtraDbContext is inheritted from DbContext and override the ValidationEntity method to set default value: CreatedDate, CreatedBy, Updated Date, UpdatedBy if empty.
    /// </summary>
    public class ExtraDbContext : DbContext
    {
        public ExtraDbContext() { }
        public ExtraDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            var entity = entityEntry.Entity as IEntity;
            if (entity != null)
                entity.ApplyDefaultValues(entityEntry.State == EntityState.Modified);
            return base.ValidateEntity(entityEntry, items);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBD.Framework.ThreeLayers
{
    internal class DbContextInterceptor : Castle.DynamicProxy.IInterceptor
    {
        public void Intercept(Castle.DynamicProxy.IInvocation invocation)
        {
            if (invocation.Method.Name == "ValidateEntity")
            {
                foreach (var a in invocation.Arguments)
                {
                    if (!(a is DbEntityEntry)) continue;
                    var entityEntry = a as DbEntityEntry;
                    if (!(entityEntry.Entity is IEntity)) continue;
                    
                    var entiy = entityEntry.Entity as IEntity;
                    entiy.ApplyDefaultValues(entityEntry.State == System.Data.Entity.EntityState.Modified);
                }
            }
            invocation.Proceed();
        }
    }
}

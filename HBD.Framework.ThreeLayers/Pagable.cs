using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBD.Framework.ThreeLayers
{
    /// <summary>
    /// Paging Queryable. PageIndex must >=0 and pageSize > 0
    /// </summary>
    /// <typeparam name="TEntity">Entity must be a class</typeparam>
    public class Pagable<TEntity> : List<TEntity>, IPagableEntity<TEntity> where TEntity : class
    {
        public Pagable(IOrderedQueryable<TEntity> items, int pageIndex, int pageSize)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalItems = items.Count();
            if (TotalItems == 0) return;

            var itemIndex = (pageIndex - 1) * pageSize;
            if (itemIndex < 0) itemIndex = 0;//Get first Page
            if (itemIndex >= TotalItems) itemIndex = TotalItems - pageSize;//Get last page.

            if (pageIndex >= 0 && pageSize > 0)
                this.AddRange(items.Skip(itemIndex).Take(pageSize));
            else this.AddRange(items);
        }

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public int TotalItems { get; private set; }
    }
}

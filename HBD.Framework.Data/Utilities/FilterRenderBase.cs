using HBD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HBD.Framework.Data.Utilities
{
    /// <summary>
    /// Render Item using FilterClause and BinaryFilterItem
    /// </summary>
    public abstract class FilterRenderBase : IFilterRender
    {
        protected abstract string RenderFilter(FilterClause filter);
        protected abstract string RenderFilter(BinaryFilterItem filter);

        public virtual string RenderFilter(IFilterClause filter)
        {
            Guard.ArgumentNotNull(filter, "IFilterClause");

            if (filter is FilterClause)
            {
                return this.RenderFilter(filter as FilterClause);
            }
            else if (filter is BinaryFilterItem)
            {
                return RenderFilter(filter as BinaryFilterItem);
            }
            else throw new ArgumentException(string.Format("This render is not cover the FilterClause type: {0}", filter.GetType().FullName));
        }
    }
}

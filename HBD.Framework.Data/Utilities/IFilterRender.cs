using System;

namespace HBD.Framework.Data.Utilities
{
    public interface IFilterRender
    {
        string RenderFilter(IFilterClause filter);
    }
}

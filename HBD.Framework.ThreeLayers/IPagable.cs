using System;
namespace HBD.Framework.ThreeLayers
{
    public interface IPagable
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalItems { get; }
    }
}

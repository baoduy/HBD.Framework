#region using

using HBD.Framework.Caching.Services;

#endregion

namespace HBD.Framework.Caching
{
    public static class CacheManager
    {
        public static ICacheService Default { get; } = new MemoryCacheService();
    }
}
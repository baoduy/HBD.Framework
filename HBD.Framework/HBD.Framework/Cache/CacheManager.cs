#region using

using HBD.Framework.Cache.Services;

#endregion

namespace HBD.Framework.Cache
{
    public static class CacheManager
    {
        public static ICacheService Default { get; } = new MemoryCacheService();
    }
}
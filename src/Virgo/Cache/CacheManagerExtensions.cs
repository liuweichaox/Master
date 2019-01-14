using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Cache
{
    /// <summary>
    /// <see cref ="ICacheManager"/>的扩展方法
    /// </summary>
    public static class CacheManagerExtensions
    {
        public static ITypedCache<TKey, TValue> GetCache<TKey, TValue>(this ICacheManager cacheManager, string name)
        {
            return cacheManager.GetCache(name).AsTyped<TKey, TValue>();
        }
    }
}

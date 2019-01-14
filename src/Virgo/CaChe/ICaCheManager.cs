using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Cache
{
    /// <summary>
    /// <see cref="ICache"/> 对象的上层容器
    /// 缓存管理器应该作为Singleton工作，并跟踪和管理<see cref ="ICache"/>对象
    /// </summary>
    public interface ICacheManager : IDisposable
    {
        /// <summary>
        /// 获取所有缓存
        /// </summary>
        /// <returns>缓存列表</returns>
        IReadOnlyList<ICache> GetAllCaches();

        /// <summary>
        /// 获取<see cref ="ICache"/>实例
        /// 如果缓存尚不存在，它可能会创建缓存
        /// </summary>
        /// <param name="name">
        /// 缓存的唯一且区分大小写的名称
        /// </param>
        /// <returns>缓存引用</returns>
        [NotNull] ICache GetCache([NotNull] string name);
    }
}

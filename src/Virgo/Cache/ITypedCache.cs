using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Virgo.Cache
{
    /// <summary>
    /// 以类型化方式使用缓存的接口
    /// 使用<see cref ="CacheExtensions.AsTyped {TKey，TValue}"/>方法将<see cref ="ICache"/>转换为此接口
    /// </summary>
    /// <typeparam name="TKey">缓存项的键类型</typeparam>
    /// <typeparam name="TValue">缓存项的值类型</typeparam>
    public interface ITypedCache<TKey, TValue> : IDisposable
    {
        /// <summary>
        /// 缓存的唯一名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 缓存项的默认滑动到期时间
        /// </summary>
        TimeSpan DefaultSlidingExpireTime { get; set; }

        /// <summary>
        /// 获取内部缓存
        /// </summary>
        ICache InternalCache { get; }

        /// <summary>
        /// 从缓存中获取一个项目
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="factory">如果不存在，则创建缓存项的工厂方法</param>
        /// <returns>缓存的项目</returns>
        TValue Get(TKey key, Func<TKey, TValue> factory);

        /// <summary>
        /// 从缓存中获取多个项目
        /// </summary>
        /// <param name="keys">键</param>
        /// <param name="factory">如果不存在，则创建缓存项的工厂方法</param>
        /// <returns>缓存的项目</returns>
        TValue[] Get(TKey[] keys, Func<TKey, TValue> factory);

        /// <summary>
        /// 从缓存中获取一个项目
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="factory">如果不存在，则创建缓存项的工厂方法</param>
        /// <returns>缓存的项目</returns>
        Task<TValue> GetAsync(TKey key, Func<TKey, Task<TValue>> factory);

        /// <summary>
        /// 从缓存中获取多个项目
        /// </summary>
        /// <param name="keys">键</param>
        /// <param name="factory">如果不存在，则创建缓存项的工厂方法</param>
        /// <returns>缓存的项目</returns>
        Task<TValue[]> GetAsync(TKey[] keys, Func<TKey, Task<TValue>> factory);

        /// <summary>
        /// 从缓存中获取项目，如果未找到，则返回null
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>缓存项目，如果未找到则为null</returns>
        TValue GetOrDefault(TKey key);

        /// <summary>
        /// 从缓存中获取项目。 对于未找到的每个键，返回空值。
        /// </summary>
        /// <param name="keys">键</param>
        /// <returns>缓存的项目</returns>
        TValue[] GetOrDefault(TKey[] keys);

        /// <summary>
        /// 从缓存中获取项目，如果未找到，则返回null
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>缓存项目，如果未找到则为null</returns>
        Task<TValue> GetOrDefaultAsync(TKey key);

        /// <summary>
        /// 从缓存中获取项目。 对于未找到的每个键，返回空值。
        /// </summary>
        /// <param name="keys">键</param>
        /// <returns>缓存的项目</returns>
        Task<TValue[]> GetOrDefaultAsync(TKey[] keys);

        /// <summary>
        /// 通过键保存/覆盖缓存中的项目。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="slidingExpireTime">滑动到期时间</param>
        /// <param name="absoluteExpireTime">绝对到期时间</param>
        void Set(TKey key, TValue value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        /// <summary>
        /// 按对保存/覆盖缓存中的项目
        /// </summary>
        /// <param name="pairs">键值对</param>
        /// <param name="slidingExpireTime">滑动到期时间</param>
        /// <param name="absoluteExpireTime">绝对到期时间</param>
        void Set(KeyValuePair<TKey, TValue>[] pairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        /// <summary>
        /// 通过键保存/覆盖缓存中的项目。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="slidingExpireTime">滑动到期时间</param>
        /// <param name="absoluteExpireTime">绝对到期时间</param>
        Task SetAsync(TKey key, TValue value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        /// <summary>
        /// 按对保存/覆盖缓存中的项目
        /// </summary>
        /// <param name="pairs">键值对</param>
        /// <param name="slidingExpireTime">滑动到期时间</param>
        /// <param name="absoluteExpireTime">绝对到期时间</param>
        Task SetAsync(KeyValuePair<TKey, TValue>[] pairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        /// <summary>
        /// 通过它的键删除缓存项（如果缓存中不存在给定密钥则不执行任何操作）
        /// </summary>
        /// <param name="key">键</param>
        void Remove(TKey key);

        /// <summary>
        /// 按键删除缓存项
        /// </summary>
        /// <param name="keys">键</param>
        void Remove(TKey[] keys);

        /// <summary>
        /// 通过它的键删除缓存项（如果缓存中不存在给定密钥则不执行任何操作）
        /// </summary>
        /// <param name="key">键</param>
        Task RemoveAsync(TKey key);

        /// <summary>
        /// 按键删除缓存项
        /// </summary>
        /// <param name="keys">键</param>
        Task RemoveAsync(TKey[] keys);

        /// <summary>
        /// 清除此缓存中的所有项目
        /// </summary>
        void Clear();

        /// <summary>
        /// 清除此缓存中的所有项目
        /// </summary>
        Task ClearAsync();
    }
}
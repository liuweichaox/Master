using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Virgo.Redis
{
    /// <summary>
    /// 分布式锁
    /// </summary>
    public interface ILock
    {
        /// <summary>
        /// 获取redis分布式锁
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="value">当前占用值</param>
        /// <param name="second">过期时间(秒)</param>
        /// <returns></returns>
        bool LockTake(string key, string value, int second);
        /// <summary>
        /// 释放redis分布式锁
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="value">当前占用值</param>
        /// <returns></returns>
        bool LockRelease(string key, string value);
        /// <summary>
        /// 使用锁执行一个方法
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="value">当前占用值</param>
        /// <param name="second">过期时间(秒)</param>
        /// <param name="executeAction">要执行的方法</param>
        void ExecuteWithLock(string key, string value, int second, Action executeAction);
        /// <summary>
        /// 异步获取redis分布式锁
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="value">当前占用值</param>
        /// <param name="second">过期时间(秒)</param>
        /// <returns></returns>
        Task<bool> LockTakeAsync(string key, string value, int second);
        /// <summary>
        /// 异步释放redis分布式锁
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="value">当前占用值</param>
        /// <returns></returns>
        Task<bool> LockReleaseAsync(string key, string value);
        /// <summary>
        /// 异步使用锁执行一个方法
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="value">当前占用值</param>
        /// <param name="second">过期时间(秒)</param>
        /// <param name="executeAction">要执行的方法</param>
        Task ExecuteWithLockAsync(string key, string value, int second, Func<Task> executeAction);
        /// <summary>
        /// Redis分布式锁续期问题解决方案
        /// 设置redis分布式锁情况下，客户端获取锁成功执行redis操作时，同时开启一个后台线程（看门狗）
        /// 每隔过期时间的1/3时检查是否还持有锁，如果持有则自动续期
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="value">当前占用值</param>
        /// <param name="second">过期时间(秒)</param>
        void LockWatchDogStart(string key, string value, int second);
        /// <summary>
        /// 设置redis分布式锁情况下，客户端获取锁并执行结束redis操作时，释放后台线程（看门狗）
        /// </summary>
        void LockWatchDogStop();
    }
}

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
        /// <param name="key">要操作的Key</param>
        /// <param name="value">Guid</param>
        /// <param name="second">过期时间(秒)</param>
        /// <returns></returns>
        Task<bool> LockTakeAsync(string key, string value, int second);
        /// <summary>
        /// 释放redis分布式锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> LockReleaseAsync(string key, string value);
        /// <summary>
        /// Redis分布式锁续期问题解决方案
        /// 设置redis分布式锁情况下，客户端获取锁成功执行redis操作时，同时开启一个后台线程（看门狗）
        /// 每隔过期时间的1/3时检查是否还持有锁，如果持有则自动续期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="second"></param>
        void LockWatchDogStart(string key, string value, int second);
        /// <summary>
        /// 设置redis分布式锁情况下，客户端获取锁并执行结束redis操作时，释放后台线程（开门狗）
        /// </summary>
        void LockWatchDogStop();
    }
}

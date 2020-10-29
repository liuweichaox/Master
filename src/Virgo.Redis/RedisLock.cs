using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Virgo.Redis
{
    /// <summary>
    /// Redis分布式锁实现类
    /// </summary>
    public class RedisLock : ILock
    {
        private Timer _timer;
        private readonly IDatabase _redis;
        public RedisLock(IRedisCacheProvider redis)
        {
            _redis = redis.GetDatabase();
        }
        /// <summary>
        /// 获取锁
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="value">当前占用值</param>
        /// <param name="second">过期时间(秒)</param>
        /// <returns></returns>
        public bool LockTake(string key, string value, int second)
        {
            return _redis.LockTake(key, value, TimeSpan.FromSeconds(second));
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="value">当前占用值</param>
        /// <returns></returns>
        public bool LockRelease(string key, string value)
        {
            return _redis.LockRelease(key, value);
        }

        /// <summary>
        /// 使用锁执行一个方法
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="value">当前占用值</param>
        /// <param name="second">过期时间(秒)</param>
        /// <param name="executeAction">要执行的方法</param>
        public void ExecuteWithLock(string key, string value, int second, Action executeAction)
        {
            if (executeAction == null) return;
            if (LockTake(key, value, second))
            {
                try
                {
                    LockWatchDogStart(key, value, second);
                    executeAction();
                }
                finally
                {
                    LockWatchDogStop();
                    LockRelease(key, value);
                }
            }
        }

        /// <summary>
        /// 获取锁
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="value">当前占用值</param>
        /// <param name="second">过期时间(秒)</param>
        /// <returns></returns>
        public async Task<bool> LockTakeAsync(string key, string value, int second)
        {
            return await _redis.LockTakeAsync(key, value, TimeSpan.FromSeconds(second));
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="value">当前占用值</param>
        /// <returns></returns>
        public async Task<bool> LockReleaseAsync(string key, string value)
        {
            return await _redis.LockReleaseAsync(key, value);
        }

        /// <summary>
        /// Redis分布式锁续期问题解决方案
        /// 设置redis分布式锁情况下，客户端获取锁成功执行redis操作时，同时开启一个后台线程（看门狗），
        /// 每隔过期时间的1/3时检查是否还持有锁，如果持有则自动续期
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="value">当前占用值</param>
        /// <param name="second">过期时间(秒)</param>
        public void LockWatchDogStart(string key, string value, int second)
        {
            _timer = new Timer(second * 1000 / 3.0);
            _timer.Elapsed += (obj, evt) =>
            {
                LockRenew(key, value, second);
            };
            _timer.Start();
        }

        /// <summary>
        /// 续期锁
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="value">当前占用值</param>
        /// <param name="second">过期时间(秒)</param>
        private void LockRenew(string key, string value, int second)
        {
            //如果是当前key对应的value，则进行守护，否则释放
            var current = _redis.StringGet(key, CommandFlags.PreferSlave);
            if (current == value)
            {
                Console.WriteLine($"--设置前剩余过期时间为{_redis.KeyTimeToLive(key)}");                
                _redis.KeyExpire(key, DateTime.Now.AddSeconds(second));// 设置锁的过期时间
            }
            else
            {
                LockWatchDogStop();
                Console.WriteLine($"--设置过期时间失败，当前value:{current},redisvalue:{value}");
            }
        }

        /// <summary>
        /// 设置redis分布式锁情况下，客户端获取锁并执行结束redis操作时，释放后台线程（看门狗）
        /// </summary>
        public void LockWatchDogStop()
        {
            Console.WriteLine($"Stop——{System.Threading.Thread.CurrentThread.ManagedThreadId}关闭看门狗，时间为:{DateTime.Now}");
            _timer.Stop();
        }

        /// <summary>
        /// 异步使用锁执行一个方法
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="value">当前占用值</param>
        /// <param name="second">过期时间(秒)</param>
        /// <param name="executeAction">要执行的方法</param>
        public async Task ExecuteWithLockAsync(string key, string value, int second, Func<Task> executeAction)
        {
            if (executeAction == null) return;
            if (await LockTakeAsync(key, value, second))
            {
                try
                {
                    LockWatchDogStart(key, value, second);
                    await executeAction();
                }
                finally
                {
                    LockWatchDogStop();
                    await LockReleaseAsync(key, value);
                }
            }
        }
    }
}

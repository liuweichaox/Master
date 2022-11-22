using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virgo.Cache
{
    public abstract class CacheBase : ICache
    {
        public TimeSpan DefaultSlidingExpireTime { get; set; }
        public TimeSpan? DefaultAbsoluteExpireTime { get; set; }

        protected readonly object SyncObj = new object();

        private readonly AsyncLock _asyncLock = new AsyncLock();

        public CacheBase()
        {
            DefaultSlidingExpireTime = TimeSpan.FromMinutes(60);
        }

        public abstract void Clear();

        public Task ClearAsync()
        {
            Clear();
            return Task.FromResult(0);
        }

        public TValue Get<TValue>(string key, Func<string, TValue> factory) where TValue : class
        {
            TValue item = null;
            try
            {
                item = GetOrDefault<TValue>(key);
            }
            catch (Exception ex)
            {
                //
            }

            if (item != null) return item;

            lock (SyncObj)
            {
                try
                {
                    item = GetOrDefault<TValue>(key);
                }
                catch (Exception ex)
                {
                    //
                }

                if (item != null) return item;
                {
                    item = factory(key);

                    if (item == null)
                    {
                        return null;
                    }

                    try
                    {
                        Set(key, item);
                    }
                    catch (Exception ex)
                    {
                        //
                    }
                }
            }

            return item;
        }


        public async Task<TValue> GetAsync<TValue>(string key, Func<string, Task<TValue>> factory) where TValue : class
        {
            TValue item = null;
            try
            {
                item = await GetOrDefaultAsync<TValue>(key);
            }
            catch (Exception ex)
            {
                //
            }

            if (item != null) return item;
            {
                using (await _asyncLock.LockAsync())
                {
                    try
                    {
                        item = await GetOrDefaultAsync<TValue>(key);
                    }
                    catch (Exception ex)
                    {
                        //
                    }

                    if (item != null) return item;
                    {
                        item = await factory(key);

                        if (item == null)
                        {
                            return null;
                        }

                        try
                        {
                            await SetAsync(key, item);
                        }
                        catch (Exception ex)
                        {
                            //
                        }
                    }
                }
            }

            return item;
        }


        public abstract TValue GetOrDefault<TValue>(string key) where TValue : class;

        public Task<TValue> GetOrDefaultAsync<TValue>(string key) where TValue : class
        {
            return Task.FromResult(GetOrDefault<TValue>(key));
        }

        public abstract void Remove(string key);

        public void Remove(string[] keys)
        {
            foreach (var key in keys)
            {
                Remove(key);
            }
        }

        public Task RemoveAsync(string key)
        {
            Remove(key);
            return Task.FromResult(0);
        }

        public Task RemoveAsync(string[] keys)
        {
            return Task.WhenAll(keys.Select(RemoveAsync));
        }

        public abstract void Set<TValue>(string key, TValue value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null) where TValue : class;

        public Task SetAsync<TValue>(string key, TValue value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null) where TValue : class
        {
            Set(key, value, slidingExpireTime, absoluteExpireTime);
            return Task.FromResult(0);
        }
        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~CacheBase()
        // {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}

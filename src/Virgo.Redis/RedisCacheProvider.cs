using StackExchange.Redis;
using System;
using Virgo.DependencyInjection;

namespace Virgo.Redis
{
    /// <summary>
    /// 实现 <see cref="IRedisCacheProvider"/>.
    /// </summary>
    public class RedisCacheProvider : IRedisCacheProvider, ISingletonDependency
    {
        /// <summary>
        /// 缓存配置
        /// </summary>
        private readonly RedisCaCheConfiguration _configuration;
        /// <summary>
        /// 连接器
        /// </summary>
        private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;

        /// <summary>
        /// 初始化<see cref="ConnectionMultiplexer"/>的实例
        /// </summary>
        public RedisCacheProvider()
        {
            _configuration = new RedisCaCheConfiguration()
            {
                DatabaseId = 0,
                HostAndPort = "localhost:6379",
                ConnectionString = "localhost:6379,Password=123456,ConnectTimeout=1000,ConnectRetry=1,SyncTimeout=10000"
            }; ;
            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
        }
        private ConnectionMultiplexer CreateConnectionMultiplexer()
        {
            return ConnectionMultiplexer.Connect(_configuration.ConnectionString);
        }
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.Value.GetDatabase(_configuration.DatabaseId);
        }

        /// <summary>
        /// 获取指定服务器的发布/订阅者连接
        /// </summary>
        /// <returns></returns>
        public ISubscriber GetSubscriber()
        {
            return _connectionMultiplexer.Value.GetSubscriber();
        }

        /// <summary>
        /// 获取单个服务器的配置API
        /// </summary>
        /// <returns></returns>
        public IServer GetServer()
        {
            return _connectionMultiplexer.Value.GetServer(_configuration.HostAndPort);
        }
    }
}

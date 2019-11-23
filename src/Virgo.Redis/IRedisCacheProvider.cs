using StackExchange.Redis;

namespace Virgo.Redis
{
    /// <summary>
    /// 用于获取Redis缓存提供程序信息
    /// </summary>
    public interface IRedisCacheProvider
    {
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        IDatabase GetDatabase();
        /// <summary>
        /// 获取指定服务器的发布/订阅者连接
        /// </summary>
        /// <returns></returns>
        ISubscriber GetSubscriber();
        /// <summary>
        /// 获取单个服务器的配置API
        /// </summary>
        /// <returns></returns>
        IServer GetServer();
    }
}

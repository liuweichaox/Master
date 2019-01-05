using Autofac.Extras.IocManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Redis
{
    /// <summary>
    /// Redis缓存配置
    /// </summary>
    public class RedisCaCheConfiguration:ISingletonDependency
    {
        /// <summary>
        /// 数据库Id
        /// </summary>
        public int DatabaseId { get; set; } = -1;
        /// <summary>
        /// 主机和端口
        /// </summary>
        public string HostAndPort { get; set; } = "localhost:6379";
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = "123456";
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString => $"{HostAndPort},Password={Password},ConnectTimeout=1000,ConnectRetry=1,SyncTimeout=10000";
    }
}

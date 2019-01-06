using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Redis
{
    /// <summary>
    /// Redis缓存配置
    /// </summary>
    public interface IRedisCaCheConfiguration
    {
        /// <summary>
        /// 数据库Id
        /// </summary>
        int DatabaseId { get; set; }
        /// <summary>
        /// 主机和端口
        /// </summary>
        string HostAndPort { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        string ConnectionString { get; set; }
    }
}

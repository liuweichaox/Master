namespace Virgo.RabbitMQ
{
    /// <summary>
    /// RabbitMQ配置
    /// </summary>
    public interface IRabbitMqConfiguration
    {
        /// <summary>
        /// 连接地址
        /// </summary>
        string HostName { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        string Password { get; set; }
    }
}

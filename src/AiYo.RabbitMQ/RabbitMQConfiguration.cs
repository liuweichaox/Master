namespace Virgo.RabbitMQ
{
    
    /// <summary>
    /// RabbitMQ配置类
    /// </summary>
    public class RabbitMqConfiguration : IRabbitMqConfiguration
    {

        /// <summary>
        /// 主机地址
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}

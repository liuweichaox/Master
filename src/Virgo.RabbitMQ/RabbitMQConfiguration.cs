namespace Virgo.RabbitMQ
{
    
    /// <summary>
    /// RabbmitMQ配置类
    /// </summary>
    public class RabbitMQConfiguration : IRabbitMQConfiguration
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }
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

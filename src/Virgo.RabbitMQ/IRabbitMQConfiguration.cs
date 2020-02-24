namespace Virgo.RabbitMQ
{
    /// <summary>
    /// RabbitMQ配置
    /// </summary>
    public interface IRabbitMQConfiguration
    {
        /// <summary>
        /// 连接地址
        /// </summary>
        string Url { get; set; }
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

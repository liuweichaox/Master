using System;

namespace Virgo.RabbitMQ
{
    /// <summary>
    /// 队列消息基类
    /// </summary>
    [Serializable]
    public class QueueMessage
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="body"></param>
        public QueueMessage(string body)
        {
            this.CreateTime = DateTime.Now;
            this.Body = body;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public QueueMessage()
        {
            this.CreateTime = DateTime.Now;
        }
        /// <summary>
        /// 主体
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.RabbitMQ
{
    /// <summary>
    /// 队列消息基类
    /// </summary>
    [Serializable]
    public class QueueMessage
    {
        public QueueMessage(string body)
        {
            this.CreateTime = DateTime.Now;
            this.Body = body;
        }
        public QueueMessage()
        {
            this.CreateTime = DateTime.Now;
        }
        public string Body { get; set; }

        public DateTime CreateTime { get; set; }
    }
}

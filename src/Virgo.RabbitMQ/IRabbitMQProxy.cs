using System;
using System.Collections.Generic;

namespace Virgo.RabbitMQ
{
    /// <summary>
    /// RabbitMQ代理接口
    /// </summary>
    public interface IRabbitMQProxy
    {
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="message">消息</param>
        /// <typeparam name="T">消息类型</typeparam>
        /// <returns>是否发布成功</returns>
        bool Publish<T>(string queueName, T message)
            where T : QueueMessage;
        
        /// <summary>
        /// 批量发布消息
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="list">消息集合</param>
        /// <typeparam name="T">消息类型</typeparam>
        /// <returns>是否发布成功</returns>
        bool BatchPublish<T>(string queueName, IList<T> list)
            where T : QueueMessage;
        
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="queueName">队列</param>
        /// <param name="func">委托</param>
        /// <typeparam name="T">消息类型</typeparam>
        void Subscribe<T>(string queueName, Func<T, bool> func)
            where T : QueueMessage;
    }
}

using System;
using System.Collections.Generic;

namespace Virgo.RabbitMQ
{
    public interface IRabbitMQProxy
    {
        bool Publish<T>(string queueName, T message)
            where T : QueueMessage;
        bool BatchPublish<T>(string queueName, IList<T> list)
            where T : QueueMessage;
        void Subscribe<T>(string queueName, Func<T, bool> func)
            where T : QueueMessage;
    }
}

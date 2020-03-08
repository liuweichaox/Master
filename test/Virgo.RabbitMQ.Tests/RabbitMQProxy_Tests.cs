using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Virgo.RabbitMQ.Tests
{
    /// <summary>
    /// <see cref="RabbitMQProxy"/>测试
    /// </summary>
    public class RabbitMQProxy_Tests
    {
        private readonly RabbitMQProxy _rabbitMQProxy;
        public RabbitMQProxy_Tests()
        {
            _rabbitMQProxy = new RabbitMQProxy(new RabbitMQConfiguration
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            });
        }

        [Fact]
        public void Publish_Test()
        {
            var message = new QueueMessage()
            {
                Name = "Jon"
            };
            _rabbitMQProxy.Publish("topic.virgo.user", message, "exchangeJon", ExchangeType.Topic, "routingKeyByJon");
        }
        [Fact]
        public void Subscribe_Test()
        {
            _rabbitMQProxy.Subscribe<QueueMessage>("topic.virgo.user", "exchangeJon", ExchangeType.Topic, "routingKeyByJon", (item) =>
             {

                 return true;
             });
        }

        public class QueueMessage
        {
            public string Name { get; set; }

            public DateTime SendTime { get; set; } = DateTime.Now;
        }
    }
}

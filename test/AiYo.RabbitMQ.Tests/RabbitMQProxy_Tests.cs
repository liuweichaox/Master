using RabbitMQ.Client;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Virgo.RabbitMQ.Tests
{
    /// <summary>
    /// <see cref="RabbitMqProxy"/>测试
    /// </summary>
    public class RabbitMQProxy_Tests
    {
        /// <summary>
        /// RabbitMQ
        /// </summary>
        private readonly RabbitMqProxy _rabbitMQProxy;
        /// <summary>
        /// 测试输出
        /// </summary>
        protected readonly ITestOutputHelper Output;
        public RabbitMQProxy_Tests(ITestOutputHelper testOutputHelper)
        {
            Output = testOutputHelper;
            _rabbitMQProxy = new RabbitMqProxy(new RabbitMqConfiguration
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "root",
                Password = "123456"
            });
        }

        /// <summary>
        /// 发布消息-工作者模式测试
        /// </summary>
        [Fact]
        public void Publish_Worker_Test()
        {
            var message = GetMessage();
            _rabbitMQProxy.Publish("work_queue", message);
        }

        /// <summary>
        /// 消息订阅-工作者模式测试
        /// </summary>
        [Fact]
        public void Subscribe_Worker_Test()
        {
            _rabbitMQProxy.Subscribe<QueueMessage>("work_queue", (item) =>
             {
                 Output.WriteLine($"来自 work_queue 队列消息 Name：{item.Name}");
                 Output.WriteLine($"来自 work_queue 队列消息 SendTime：{item.SendTime}");
                 return true;
             });
        }

        /// <summary>
        /// Direct发布消息-完全匹配模式测试
        /// </summary>
        [Fact]
        public void Publish_DirectExchange_Test()
        {
            var message = GetMessage();
            _rabbitMQProxy.Publish("", message, "direct_exchange_test", ExchangeType.Direct, "direct_routingKey_test");
        }

        /// <summary>
        /// Direct订阅消息-完全匹配模式测试
        /// </summary>
        [Fact]
        public void Subscribe_DirectExchange_Test()
        {
            _rabbitMQProxy.Subscribe<QueueMessage>("", "direct_exchange_test", ExchangeType.Direct, "direct_routingKey_test", (item) =>
             {
                 Output.WriteLine($"来自 direct_exchange_test 队列消息 Name：{item.Name}");
                 Output.WriteLine($"来自 direct_exchange_test 队列消息 SendTime：{item.SendTime}");
                 return true;
             });
        }

        /// <summary>
        /// Fanout发布消息-广播模式
        /// </summary>
        [Fact]
        public void Publish_FanoutExchange_Test()
        {
            var message = GetMessage();
            _rabbitMQProxy.Publish("", message, "fanout_exchange_test", ExchangeType.Fanout, "");
        }

        /// <summary>
        /// Fanout订阅消息-广播模式
        /// </summary>
        [Fact]
        public void Subscribe_FanoutExchange_Test()
        {
            _rabbitMQProxy.Subscribe<QueueMessage>("", "fanout_exchange_test", ExchangeType.Fanout, "", (item) =>
            {
                Output.WriteLine($"来自 fanout_exchange_test 队列消息 Name：{item.Name}");
                Output.WriteLine($"来自 fanout_exchange_test 队列消息 SendTime：{item.SendTime}");
                return true;
            });
        }


        /// <summary>
        /// Topic发布消息-通配符模式
        /// </summary>
        [Fact]
        public void Publish_TopicExchange_Test()
        {
            var message = GetMessage();
            _rabbitMQProxy.Publish("", message, "topic_exchange_test", ExchangeType.Topic, "topic.virgo.mq");
        }

        /// <summary>
        /// Topic订阅消息-通配符模式
        /// </summary>
        [Theory]
        [InlineData("topic.#")]
        [InlineData("topic.*.*")]
        public void Subscribe_TopicExchange_Test(string routingKey)
        {
            _rabbitMQProxy.Subscribe<QueueMessage>("", "topic_exchange_test", ExchangeType.Topic, routingKey, (item) =>
            {
                Output.WriteLine($"来自 topic_exchange_test 队列消息：{item.Name}");
                Output.WriteLine($"来自 topic_exchange_test 队列消息：{item.SendTime}");
                return true;
            });
        }


        QueueMessage GetMessage()
        {
            var message = new QueueMessage()
            {
                Name = "Hello"
            };
            return message;
        }


        public class QueueMessage
        {
            public string Name { get; set; }

            public DateTime SendTime { get; set; } = DateTime.Now;
        }
    }
}

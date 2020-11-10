using RabbitMQ.Client;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Virgo.RabbitMQ.Tests
{
    /// <summary>
    /// <see cref="RabbitMQProxy"/>测试
    /// </summary>
    public class RabbitMQProxy_Tests
    {
        private readonly RabbitMQProxy _rabbitMQProxy;
        protected readonly ITestOutputHelper Output;
        public RabbitMQProxy_Tests(ITestOutputHelper testOutputHelper)
        {
            Output = testOutputHelper;
            _rabbitMQProxy = new RabbitMQProxy(new RabbitMQConfiguration
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "root",
                Password = "123456"
            });
        }

        [Fact]
        public void Publish_Worker_Test()
        {
            var message = new QueueMessage()
            {
                Name = "Jon"
            };
            _rabbitMQProxy.Publish("work_queue", message);
        }
        [Fact]
        public void Subscribe_Worker_Test()
        {
            _rabbitMQProxy.Subscribe<QueueMessage>("work_queue", (item) =>
             {
                 Output.WriteLine($"来自work_queue队列消息：{item.Name}");
                 Output.WriteLine($"来自work_queue队列消息：{item.SendTime}");
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

using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Virgo.RabbitMQ
{
    /// <summary>
    /// RabbitMQ代理实现
    /// </summary>
    public class RabbitMqProxy
    {
        private readonly IRabbitMqConfiguration _mqConfiguration;
        public RabbitMqProxy(IRabbitMqConfiguration mQConfiguration)
        {
            _mqConfiguration = mQConfiguration;
        }

        /// <summary>
        /// 创建连接工厂
        /// </summary>
        /// <returns></returns>
        private ConnectionFactory CreateConnectionFactory()
        {
            var factory = new ConnectionFactory
            {
                VirtualHost = "/",
                HostName = _mqConfiguration.HostName,
                Port = _mqConfiguration.Port,
                UserName = _mqConfiguration.UserName,
                Password = _mqConfiguration.Password
            };
            return factory;
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="model">发送数据</param>
        /// <returns>是否成功</returns>
        public bool Publish(string queueName, object model)
        {
            try
            {
                // 1.实例化连接工厂。
                var factory = CreateConnectionFactory();
                // 2.创建连接
                var conn = factory.CreateConnection();
                // 3.创建频道
                var channel = conn.CreateModel();
                // 4. 申明队列(指定durable:true,告知rabbitmq对消息进行持久化)
                channel.QueueDeclare(queueName, true, false, false, null);
                //将消息标记为持久性 - 将IBasicProperties.SetPersistent设置为true
                var props = channel.CreateBasicProperties();
                props.Persistent = true;
                // 5. 构建byte消息数据包
                var msg = JsonConvert.SerializeObject(model);
                var messageBodyBytes = Encoding.UTF8.GetBytes(msg);
                // 6. 发送数据包(指定basicProperties)
                channel.BasicPublish("", queueName, props, messageBodyBytes);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 消息订阅
        /// </summary>
        /// <typeparam name="T">消息模型</typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="func">处理消息</param>
        public void Subscribe<T>(string queueName, Func<T, bool> func) where T : class
        {
            // 1.实例化连接工厂。
            var factory = CreateConnectionFactory();
            // 2.建立连接
            var conn = factory.CreateConnection();
            // 3.创建频道 
            var channel = conn.CreateModel();
            // 4.申明队列(指定durable:true,告知rabbitmq对消息进行持久化)
            channel.QueueDeclare(queueName, true, false, false, null);
            //设置prefetchCount : 1来告知RabbitMQ，在未收到消费端的消息确认时，不再分发消息，也就确保了当消费端处于忙碌状态时，不再分配任务。
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            // 5.构造消费者实例
            var consumer = new EventingBasicConsumer(channel);
            // 6.绑定消息接收后的事件委托
            consumer.Received += (model, ea) =>
            {
                var msg = "";
                var bytes = ea.Body;
                msg = Encoding.UTF8.GetString(bytes);
                var instance = JsonConvert.DeserializeObject<T>(msg);
                //7. 模拟耗时
                var success = func(instance);
                //应答确认
                if (success)
                {
                    // 8. 发送消息确认信号（手动消息确认）
                    channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            // 9.启动消费者
            //autoAck:true；自动进行消息确认，当消费端接收到消息后，就自动发送ack信号，不管消息是否正确处理完毕
            //autoAck:false；关闭自动消息确认，通过调用BasicAck方法手动进行消息确认
            channel.BasicConsume(queueName, false, consumer);
        }

        /// <summary>
        /// 异步消息订阅
        /// </summary>
        /// <typeparam name="T">消息模型</typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="func">处理消息</param>
        public void SubscribeAsync<T>(string queueName, Func<T, Task<bool>> func) where T : class
        {
            // 1.实例化连接工厂。
            var factory = CreateConnectionFactory();
            // 2.建立连接
            var conn = factory.CreateConnection();
            // 3.创建频道 
            var channel = conn.CreateModel();
            // 4.申明队列(指定durable:true,告知rabbitmq对消息进行持久化)
            channel.QueueDeclare(queueName, true, false, false, null);
            //设置prefetchCount : 1来告知RabbitMQ，在未收到消费端的消息确认时，不再分发消息，也就确保了当消费端处于忙碌状态时，不再分配任务。
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            // 5.构造消费者实例
            var consumer = new AsyncEventingBasicConsumer(channel);
            // 6.绑定消息接收后的事件委托
            consumer.Received += async (model, ea) =>
            {
                var msg = "";
                var bytes = ea.Body;
                msg = Encoding.UTF8.GetString(bytes);
                var instance = JsonConvert.DeserializeObject<T>(msg);
                // 7.模拟耗时
                var success = await func(instance);
                //应答确认
                if (success)
                {
                    // 8.发送消息确认信号（手动消息确认）
                    channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            // 9.启动消费者
            //autoAck:true；自动进行消息确认，当消费端接收到消息后，就自动发送ack信号，不管消息是否正确处理完毕
            //autoAck:false；关闭自动消息确认，通过调用BasicAck方法手动进行消息确认
            channel.BasicConsume(queueName, false, consumer);
        }

        /// <summary>
        /// 发布消息-Exchange模式
        /// </summary>
        /// <param name="queueName">队列名称（空则不定义队列）</param>
        /// <param name="model">发送数据</param>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="exchangeType">交换机类型(direct,fanout,topic)</param>
        /// <param name="routingKey">路由键</param>
        /// <returns>是否成功</returns>
        public bool Publish(string queueName, object model, string exchangeName, string exchangeType, string routingKey)
        {

            try
            {
                // 1.实例化连接工厂。
                var factory = CreateConnectionFactory();
                // 2.建立连接
                var conn = factory.CreateConnection();
                // 3.创建频道 
                var channel = conn.CreateModel();
                // 4.定义交换机
                channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);
                //交换器模式，队列不是必须的，也可以根据指定队列获取消息
                if (string.IsNullOrEmpty(queueName) == false)
                {
                    // 定义队列
                    channel.QueueDeclare(queueName, true, false, false, null);
                    //绑定交换机-队列
                    channel.QueueBind(queueName, exchangeName, routingKey, null);
                }
                // 5. 构建byte消息数据包
                var msg = JsonConvert.SerializeObject(model);
                var messageBodyBytes = Encoding.UTF8.GetBytes(msg);
                // 将消息标记为持久性。
                var props = channel.CreateBasicProperties();
                props.Persistent = true;
                // 6. 发送数据包(指定basicProperties)
                channel.BasicPublish(exchangeName, routingKey, props, messageBodyBytes);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 消息订阅-Exchange模式
        /// </summary>
        /// <typeparam name="T">消息模型</typeparam>
        /// <param name="queueName">队列名称（空则为临时队列）</param>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="exchangeType">交换机类型(direct,fanout,topic)</param>
        /// <param name="routingKey">路由键</param>
        /// <param name="func">处理消息</param>
        public void Subscribe<T>(string queueName, string exchangeName, string exchangeType, string routingKey, Func<T, bool> func) where T : class
        {
            // 1.实例化连接工厂。
            var factory = CreateConnectionFactory();
            // 2.建立连接
            var conn = factory.CreateConnection();
            // 3.创建频道 
            var channel = conn.CreateModel();
            // 4.定义交换机
            channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);
            // 声明队列
            if (string.IsNullOrEmpty(queueName))
                queueName = channel.QueueDeclare().QueueName;
            else
                channel.QueueDeclare(queueName, true, false, false, null);
            //绑定交换机-队列
            channel.QueueBind(queueName, exchangeName, routingKey, null);
            // 5.构造消费者实例
            var consumer = new EventingBasicConsumer(channel);
            // 6.绑定消息接收后的事件委托
            consumer.Received += (model, ea) =>
            {
                var msg = "";
                var bytes = ea.Body;
                msg = Encoding.UTF8.GetString(bytes);
                var instance = JsonConvert.DeserializeObject<T>(msg);
                // 7. 模拟耗时
                var success = func(instance);
                //应答确认
                if (success)
                {
                    // 8.发送消息确认信号（手动消息确认）
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                // 9.启动消费者
            };
            //autoAck:true；自动进行消息确认，当消费端接收到消息后，就自动发送ack信号，不管消息是否正确处理完毕
            //autoAck:false；关闭自动消息确认，通过调用BasicAck方法手动进行消息确认
            channel.BasicConsume(queueName, false, consumer);
        }
        /// <summary>
        /// 异步消息订阅-Exchange模式
        /// </summary>
        /// <typeparam name="T">消息模型</typeparam>
        /// <param name="queueName">队列名称（空则为临时队列）</param>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="exchangeType">交换机类型(direct,fanout,topic)</param>
        /// <param name="routingKey">路由键</param>
        /// <param name="func">处理消息</param>
        public void SubscribeAsync<T>(string queueName, string exchangeName, string exchangeType, string routingKey, Func<T, Task<bool>> func) where T : class
        {
            // 1.实例化连接工厂。
            var factory = CreateConnectionFactory();
            // 2.建立连接
            var conn = factory.CreateConnection();
            // 3.创建频道 
            var channel = conn.CreateModel();
            // 4.定义交换机
            channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);
            // 声明队列
            if (string.IsNullOrEmpty(queueName))
                queueName = channel.QueueDeclare().QueueName;
            else
                channel.QueueDeclare(queueName, true, false, false, null);
            //绑定交换机-队列
            channel.QueueBind(queueName, exchangeName, routingKey, null);
            // 5.构造消费者实例
            var consumer = new AsyncEventingBasicConsumer(channel);
            // 6.绑定消息接收后的事件委托
            consumer.Received += async (model, ea) =>
             {
                 var msg = "";
                 var bytes = ea.Body;
                 msg = Encoding.UTF8.GetString(bytes);
                 var instance = JsonConvert.DeserializeObject<T>(msg);
                 // 7. 模拟耗时
                 var success = await func(instance);
                 //应答确认
                 if (success)
                 {
                     // 8.发送消息确认信号（手动消息确认）
                     channel.BasicAck(ea.DeliveryTag, false);
                 }
             };
            // 9.启动消费者
            //autoAck:true；自动进行消息确认，当消费端接收到消息后，就自动发送ack信号，不管消息是否正确处理完毕
            //autoAck:false；关闭自动消息确认，通过调用BasicAck方法手动进行消息确认
            channel.BasicConsume(queueName, false, consumer);
        }
    }
}

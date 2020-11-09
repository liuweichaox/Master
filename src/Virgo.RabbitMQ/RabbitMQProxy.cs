using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Virgo.RabbitMQ
{
    /// <summary>
    /// RabbmitMQ代理实现
    /// </summary>
    public class RabbitMQProxy
    {
        private readonly IRabbitMQConfiguration _mQConfiguration;
        public RabbitMQProxy(IRabbitMQConfiguration mQConfiguration)
        {
            _mQConfiguration = mQConfiguration;
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="model">发送数据</param>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="exchangeType">交换机类型</param>
        /// <param name="routingKey">路由键</param>
        /// <returns>是否成功</returns>
        public bool Publish(string queueName, object model, string exchangeName, string exchangeType, string routingKey)
        {
            if (string.IsNullOrEmpty(_mQConfiguration.HostName))
            {
                return false;
            }
            if (model == null)
            {
                return false;
            }
            try
            {
                // 实例化连接工厂。
                var factory = new ConnectionFactory
                {
                    VirtualHost = "/",
                    HostName = _mQConfiguration.HostName,
                    Port = _mQConfiguration.Port,
                    UserName = _mQConfiguration.UserName ?? "",
                    Password = _mQConfiguration.Password ?? ""
                };
                // 创建连接、信道。
                using var conn = factory.CreateConnection();
                using var channel = conn.CreateModel();
                //定义交换机
                channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);
                // 声明队列，标记为持久性
                channel.QueueDeclare(queueName, true, false, false, null);
                //绑定交换机-队列
                channel.QueueBind(queueName, exchangeName, routingKey, null);
                //序列化对象
                var msg = JsonConvert.SerializeObject(model);
                //在MQ上定义一个持久化队列，如果名称相同不会重复创建
                var messageBodyBytes = Encoding.UTF8.GetBytes(msg);
                // 将消息标记为持久性。
                var props = channel.CreateBasicProperties();
                // MIME类型
                props.ContentType = "text/plain";
                //非持久化1,持久化2
                props.DeliveryMode = 2;
                //发送消息到队列
                channel.BasicPublish(exchangeName, routingKey, props, messageBodyBytes);
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
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="exchangeType">交换机类型</param>
        /// <param name="routingKey">路由键</param>
        /// <param name="func">处理消息</param>
        public void Subscribe<T>(string queueName, string exchangeName, string exchangeType, string routingKey, Func<T, bool> func) where T : class
        {
            var isClose = false;//队列服务端是否关闭
            do
            {
                try
                {
                    // 实例化连接工厂。
                    var factory = new ConnectionFactory
                    {
                        VirtualHost = "/",
                        HostName = "localhost",
                        Port = 5672,
                        UserName = _mQConfiguration.UserName ?? "",
                        Password = _mQConfiguration.Password ?? ""
                    };
                    // 创建连接、信道。
                    using var conn = factory.CreateConnection();
                    using var channel = conn.CreateModel();
                    //定义交换机
                    channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);
                    // 获取发送消息
                    channel.QueueDeclare(queueName, true, false, false, null);
                    //绑定交换机-队列
                    channel.QueueBind(queueName, exchangeName, routingKey, null);
                    // 告知 RabbitMQ，在未收到当前 Worker 的消息确认信号时，不再分发给消息，确保公平调度。
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                    //设置消息持久化
                    var props = channel.CreateBasicProperties();
                    // MIME类型
                    props.ContentType = "text/plain";
                    //非持久化1,持久化2
                    props.DeliveryMode = 2;
                    //在队列上定义一个消费者
                    var consumer = new EventingBasicConsumer(channel);

                    var closeErrorCount = 0;//队列服务关闭重试次数

                    // 绑定消息接收事件。
                    consumer.Received += (model, ea) =>
                    {
                        var msg = "";
                        try
                        {
                            if (channel.IsClosed)
                            {
                                isClose = true;
                                return;
                            }

                            var bytes = ea.Body;
                            msg = Encoding.UTF8.GetString(bytes);
                            var instance = JsonConvert.DeserializeObject<T>(msg);
                            var success = func(instance);
                            //应答确认
                            if (success)
                            {
                                // 手动发送消息确认信号。
                                channel.BasicAck(ea.DeliveryTag, false);
                                if (closeErrorCount > 0)
                                    closeErrorCount = 0;
                            }
                            else
                            {
                                //消息未确认
                            }
                        }
                        catch (EndOfStreamException)//捕获队列服务端关闭的异常
                        {
                            isClose = true;
                            return;
                        }
                        catch (Exception ex)
                        {
                            isClose = true;
                            return;
                        }
                    };
                    // 消费队列，并设置应答模式为程序主动应答
                    // autoAck:false - 关闭自动消息确认，调用`BasicAck`方法进行手动消息确认。
                    // autoAck:true  - 开启自动消息确认，当消费者接收到消息后就自动发送 ack 信号，无论消息是否正确处理完毕。
                    channel.BasicConsume(queueName, false, consumer);
                }
                catch (Exception)
                {
                    isClose = true;
                }
                System.Threading.Thread.Sleep(2 * 60 * 1000);//休息 5 分钟重新连接队列服务端；
            } while (isClose);
        }
    }
}

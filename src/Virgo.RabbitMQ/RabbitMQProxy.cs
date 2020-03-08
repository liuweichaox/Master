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
        /// <typeparam name="T"></typeparam>
        /// <param name="queueName"></param>
        /// <param name="model"></param>
        /// <param name="exchangeName"></param>
        /// <param name="exchangeType"></param>
        /// <param name="routingKey"></param>
        /// <returns></returns>
        public bool Publish<T>(string queueName, T model, string exchangeName, string exchangeType, string routingKey) where T : class
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
                var factory = new ConnectionFactory
                {
                    VirtualHost = "/",
                    HostName = _mQConfiguration.HostName,
                    Port = _mQConfiguration.Port,
                    UserName = _mQConfiguration.UserName ?? "",
                    Password = _mQConfiguration.Password ?? ""
                };
                using var conn = factory.CreateConnection();
                using var channel = conn.CreateModel();
                //定义交换机
                channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);
                //定义队列
                channel.QueueDeclare(queueName, true, false, false, null);
                //绑定交换机-队列
                channel.QueueBind(queueName, exchangeName, routingKey, null);
                //序列化对象
                var msg = JsonConvert.SerializeObject(model);
                //在MQ上定义一个持久化队列，如果名称相同不会重复创建
                var messageBodyBytes = Encoding.UTF8.GetBytes(msg);
                //设置消息持久化
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
        /// <typeparam name="T"></typeparam>
        /// <param name="queueName"></param>
        /// <param name="exchangeName"></param>
        /// <param name="exchangeType"></param>
        /// <param name="routingKey"></param>
        /// <param name="func"></param>
        public void Subscribe<T>(string queueName, string exchangeName, string exchangeType, string routingKey, Func<T, bool> func) where T : class
        {
            var isClose = false;//队列服务端是否关闭
            do
            {
                try
                {
                    var factory = new ConnectionFactory
                    {
                        VirtualHost = "/",
                        HostName = "localhost",
                        Port = 5672,
                        UserName = _mQConfiguration.UserName ?? "",
                        Password = _mQConfiguration.Password ?? ""
                    };
                    using var conn = factory.CreateConnection();
                    using var channel = conn.CreateModel();
                    //定义交换机
                    channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);
                    //定义队列
                    channel.QueueDeclare(queueName, true, false, false, null);
                    //绑定交换机-队列
                    channel.QueueBind(queueName, exchangeName, routingKey, null);
                    //设置消息持久化
                    var props = channel.CreateBasicProperties();
                    // MIME类型
                    props.ContentType = "text/plain";
                    //非持久化1,持久化2
                    props.DeliveryMode = 2;
                    //在队列上定义一个消费者
                    var consumer = new EventingBasicConsumer(channel);

                    var closeErrorCount = 0;//队列服务关闭重试次数

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
                        catch (Exception)
                        {
                            //Logger.Error("队列异常，联系管理员吧：" + queueName + ":" + msg + "ex:" + ex);
                            isClose = true;
                            return;
                        }
                    }; 
                    //消费队列，并设置应答模式为程序主动应答
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

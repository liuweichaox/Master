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
    public class RabbitMQProxy : IRabbitMQProxy
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
        /// <param name="queueName">队列名称</param>
        /// <param name="model">消息内容</param>
        /// <returns>是否发布成功</returns>
        public bool Publish<T>(string queueName, T model) where T : QueueMessage
        {
            if (string.IsNullOrEmpty(_mQConfiguration.Url))
            {
                return false;
            }
            if (model == null)
            {
                return false;
            }
            //序列化对象
            var msg = JsonConvert.SerializeObject(model);
            try
            {
                var factory = new ConnectionFactory
                {
                    Uri = new Uri(_mQConfiguration.Url),
                    UserName = _mQConfiguration.UserName ?? "",
                    Password = _mQConfiguration.Password ?? ""
                };
                using (var conn = factory.CreateConnection())
                {
                    using (var channel = conn.CreateModel())
                    {
                        //在MQ上定义一个持久化队列，如果名称相同不会重复创建
                        channel.QueueDeclare(queueName, true, false, false, null);

                        byte[] bytes = Encoding.UTF8.GetBytes(msg);

                        //设置消息持久化
                        IBasicProperties properties = channel.CreateBasicProperties();

                        //非持久化1,持久化2
                        properties.DeliveryMode = 2;

                        //发送消息到队列
                        channel.BasicPublish("", queueName, properties, bytes);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 发布消息（批量发布）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queueName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool BatchPublish<T>(string queueName, IList<T> list) where T : QueueMessage
        {
            if (string.IsNullOrEmpty(_mQConfiguration.Url))
            {
                return false;
            }
            if (list == null || list.Count() == 0)
            {
                return false;
            }
            try
            {
                var factory = new ConnectionFactory
                {
                    Uri = new Uri(_mQConfiguration.Url),
                    UserName = _mQConfiguration.UserName ?? "",
                    Password = _mQConfiguration.Password ?? ""
                };
                using (var conn = factory.CreateConnection())
                {
                    using (var channel = conn.CreateModel())
                    {
                        channel.QueueDeclare(queueName, true, false, false, null);
                        //设置消息持久化
                        IBasicProperties properties = channel.CreateBasicProperties();

                        //非持久化1,持久化2
                        properties.DeliveryMode = 2;
                        foreach (var item in list)
                        {
                            var msg = JsonConvert.SerializeObject(item);
                            //在MQ上定义一个持久化队列，如果名称相同不会重复创建
                            byte[] bytes = Encoding.UTF8.GetBytes(msg);
                            //发送消息到队列
                            channel.BasicPublish("", queueName, properties, bytes);
                        }
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 消息订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queueName">队列名称</param>
        /// <param name="func">接收到消息后执行的操作</param>
        public void Subscribe<T>(string queueName, Func<T, bool> func) where T : QueueMessage
        {
            var isClose = false;//队列服务端是否关闭
            do
            {
                try
                {
                    var factory = new ConnectionFactory
                    {
                        Uri = new Uri(_mQConfiguration.Url),
                        UserName = _mQConfiguration.UserName ?? "",
                        Password = _mQConfiguration.Password ?? ""
                    };
                    using (var conn = factory.CreateConnection())
                    {
                        using (var channel = conn.CreateModel())
                        {
                            //在MQ上定义一个持久化队列，如果名称相同不会重复创建
                            channel.QueueDeclare(queueName, true, false, false, null);

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

                                    byte[] bytes = ea.Body;
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
                    }
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

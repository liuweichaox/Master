using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.RabbitMQ
{
    public class RabbitMQConfiguration : IRabbitMQConfiguration
    {
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}

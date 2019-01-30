using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.RabbitMQ
{
    public interface IRabbitMQConfiguration
    {
        string Url { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
    }
}

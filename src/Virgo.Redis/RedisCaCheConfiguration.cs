using Autofac.Extras.IocManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Redis
{
    public class RedisCaCheConfiguration : IRedisCaCheConfiguration, ISingletonDependency
    {
        public int DatabaseId { get; set; } = -1;

        public string HostAndPort { get; set; } = "localhost:6379";

        public string ConnectionString { get; set; } = "localhost:6379,Password=123456,ConnectTimeout=1000,ConnectRetry=1,SyncTimeout=10000";
    }
}

using Autofac.Extras.IocManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Redis
{
    public class RedisCaCheConfiguration : IRedisCaCheConfiguration, ISingletonDependency
    {
        public int DatabaseId { get; set; }

        public string HostAndPort { get; set; }

        public string ConnectionString { get; set; }
    }
}

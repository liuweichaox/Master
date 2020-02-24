using Virgo.DependencyInjection;

namespace Virgo.Redis
{
    public class RedisCaCheConfiguration
    {
        public int DatabaseId { get; set; }

        public string HostAndPort { get; set; }

        public string ConnectionString { get; set; }
    }
}

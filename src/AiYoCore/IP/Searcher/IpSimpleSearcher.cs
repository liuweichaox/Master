using System;
using System.IO;
using Virgo.IP.Models;

namespace Virgo.IP.Searcher
{
    public class IpSimpleSearcher : IIpSearcher
    {
        private readonly DbSearcher _search;

        public IpSimpleSearcher()
        {
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ip", "data", "ip2region.db");
            _search = new DbSearcher(dbPath);
        }

        public IpInfo Search(string ip)
        {
            if (string.IsNullOrEmpty(ip))
            {
                throw new ArgumentException(nameof(ip));
            }
            var region = _search.MemorySearch(ip).Region;
            var ipinfo = RegionStrToIpInfo(region);
            ipinfo.IpAddress = ip;
            return ipinfo;
        }

        public IpInfo SearchWithI18N(string ip, string langCode)
        {
            throw new NotImplementedException();
        }

        private IpInfo RegionStrToIpInfo(string region)
        {
            try
            {
                var array = region.Split('|');
                var info = new IpInfo()
                {
                    Country = array[0],
                    Province = array[2],
                    City = array[3],
                    NetworkOperator = array[4]
                };
                return info;
            }
            catch (Exception e)
            {
                throw new Exception("Error converting ip address information to ipinfo object", e);
            }
        }

        public void Dispose()
        {
            _search?.Dispose();
        }
    }
}

using Virgo.IP.Models;

namespace Virgo.IP.Searcher
{
    /// <summary>
    /// IP搜索接口
    /// </summary>
    public interface IIpSearcher
    {
        IpInfo Search(string ip);

        IpInfo SearchWithI18N(string ip, string langCode = "");
    }
}

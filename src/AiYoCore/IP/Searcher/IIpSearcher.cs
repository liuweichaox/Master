using Virgo.IP.Models;

namespace Virgo.IP.Searcher
{
    /// <summary>
    /// IP搜索接口
    /// </summary>
    public interface IIpSearcher
    {
        /// <summary>
        /// 搜索IP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        IpInfo Search(string ip);

        /// <summary>
        /// 搜索带国际化
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        IpInfo SearchWithI18N(string ip, string langCode = "");
    }
}

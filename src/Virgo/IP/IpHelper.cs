using System;
using Virgo.IP.Models;
using Virgo.IP.Searcher;

namespace Virgo.IP
{
    /// <summary>
    /// IP帮助类
    /// </summary>
    public static class IpHelper
    {
        private static readonly IIpSearcher Searcher;

        static IpHelper()
        {
            try
            {
                if (IpSettings.DefalutSearcherType == IpSearcherType.China)
                {
                    Searcher = new IpSimpleSearcher();
                }
                if (IpSettings.DefalutSearcherType == IpSearcherType.International)
                {
                    Searcher = new IpComplexSearcher();
                }
            }
            catch (System.Exception e)
            {
                throw new Exception("IPTools initialize failed.", e);
            }
        }


        /// <summary>
        /// 使用默认搜索器获取IP地址信息。
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns></returns>
        public static IpInfo Search(string ip)
        {
            return Searcher.Search(ip);
        }

        /// <summary>
        /// 使用默认搜索器获取i8n的IP地址信息。
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="langCode">语言代码.eg。 zh-CN，en。</param>
        /// <returns></returns>
        public static IpInfo SearchWithI18N(string ip, string langCode = "")
        {
            return Searcher.SearchWithI18N(ip, langCode);
        }
    }
}

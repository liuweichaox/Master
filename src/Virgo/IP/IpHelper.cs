using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Virgo.IP.Models;
using Virgo.IP.Searcher;

namespace Virgo.IP
{
    public static class IpHelper
    {
        /// <summary>
        /// If you add IPTools.International and IPTools.China to your project.The IPTools.China is Default IpSearcher.
        /// </summary>
        private static readonly IIpSearcher  searcher;


        static IpHelper()

        {
            try
            {              
                if (IpToolSettings.DefalutSearcherType == IpSearcherType.China)
                {
                    searcher = new IpSimpleSearcher();
                }
                if (IpToolSettings.DefalutSearcherType == IpSearcherType.International)
                {
                    searcher = new IpComplexSearcher();
                }
            }
            catch (System.Exception e)
            {
                throw new Exception("IPTools initialize failed.", e);
            }
        }


        /// <summary>
        /// Use DefaultSearcher get ip addredd information.
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static IpInfo Search(string ip)
        {
            return searcher.Search(ip);
        }

        /// <summary>
        /// Use DefaultSearcher get ip addredd information with i8n.
        /// <para/>
        /// Now support IPTools.China.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="langCode">language code.eg. zh-CN, en.</param>
        /// <returns></returns>
        public static IpInfo SearchWithI18N(string ip, string langCode = "")
        {
            return searcher.SearchWithI18N(ip, langCode);
        }
    }
}

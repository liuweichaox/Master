using System;
using System.Collections.Generic;
using System.Text;
using Virgo.IP.Models;

namespace Virgo.IP.Searcher
{
    public interface IIpSearcher
    {
        IpInfo Search(string ip);

        IpInfo SearchWithI18N(string ip, string langCode = "");
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Virgo.IP.Models;

namespace Virgo.IP
{
    public static class IpExtension
    {
        public static IpInfo GetRemoteIpInfo(this HttpContext context)
        {
            return IpHelper.Search(context.Connection.RemoteIpAddress.ToString());
        }

        /// <summary>
        /// Get ip info from request header.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="headerKey">request header key.</param>
        /// <returns></returns>
        public static IpInfo GetRemoteIpInfo(this HttpContext context, string headerKey)
        {
            return IpHelper.Search(context.Request.Headers[headerKey]);
        }
    }
}

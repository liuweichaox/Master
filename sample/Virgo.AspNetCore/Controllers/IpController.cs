using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Virgo.IP;
using Virgo.Extensions;
using System.Net;

namespace Virgo.AspNetCore.Controllers
{
    public class IpController : Controller
    {
        [HttpGet]
        public IActionResult Search(string ip)
        {          
           
            if (ip.IsNullOrEmpty())
            {
                ip = HttpContext.Connection.RemoteIpAddress.ToString();
            }
            if (ip == "::1")
            {
                ip = "127.0.0.1";
            }
            if (!IPAddress.TryParse(ip, out var iPAddress))
            {
                ip = "127.0.0.1";
            }
            var result = IpHelper.Search(ip);
            return Json(result);
        }
    }
}

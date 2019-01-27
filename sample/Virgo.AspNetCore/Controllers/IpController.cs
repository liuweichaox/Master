using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Virgo.IP;
using Virgo.Extensions;
using System.Net;
using System.Text.RegularExpressions;

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
            Regex regex = new Regex(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");
            if (!regex.IsMatch(ip))
            {
                ip = "127.0.0.1";
            }
            var result = IpHelper.Search(ip);
            return Json(result);
        }
    }
}

using Castle.Core.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Virgo.IP;

namespace Virgo.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ServiceController : Controller
    {
        public IActionResult SearchIP(string ip)
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

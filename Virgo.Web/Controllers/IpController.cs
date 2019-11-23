using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Virgo.Extensions;
using Virgo.IP;

namespace Virgo.Web.Controllers
{
    public class IpController : Controller
    {
        [HttpGet]
        [Route("IpSearch")]
        public IActionResult Search(string key)
        {

            if (key.IsNullOrEmpty())
            {
                key = HttpContext.Connection.RemoteIpAddress.ToString();
            }
            if (key == "::1")
            {
                key = "127.0.0.1";
            }
            Regex regex = new Regex(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");
            if (!regex.IsMatch(key))
            {
                key = "127.0.0.1";
            }
            var result = IpHelper.Search(key);
            return Json(result);
        }
    }
}

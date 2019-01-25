using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Virgo.IP;

namespace Virgo.AspNetCore.Controllers
{
    public class IpController : Controller
    {
        [HttpGet]
        public IActionResult Search(string ip="39.108.80.222")
        {
            var result = IpHelper.Search(ip);
            return Json(result);
        }
    }
}

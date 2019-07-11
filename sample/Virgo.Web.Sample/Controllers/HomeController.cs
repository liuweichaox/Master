using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Virgo.Web.Sample.Models;
using Virgo.Strings;
using Virgo.Net.Mime;
using Virgo.Systems;
using System.Threading;

namespace Virgo.Web.Sample.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route(nameof(ValidateCodePir))]
        public IActionResult ValidateCodePir()
        {
            var code = ValidateCode.CreateValidateCode(5);
            var ms = HttpContext.CreateValidateGraphic(code);
            return File(ms.ToArray(), ContentType.Jpg); ;
        }
    }
}

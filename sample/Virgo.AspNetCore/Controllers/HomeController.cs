using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Virgo.AspNetCore.Models;
using Virgo.Infrastructure;

namespace Virgo.AspNetCore.Controllers
{
    public class HomeController : Controller
    {
        public IOrderService OrderService { get; set; }
        public IInfrastruxtureTest InfrastruxtureTest { get; set; }
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
    }
}

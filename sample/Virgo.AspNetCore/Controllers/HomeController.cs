using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.DynamicProxy;
using Autofac.Extras.IocManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Virgo.AspNetCore.Models;
using Virgo.Infrastructure;
using System.Globalization;
namespace Virgo.AspNetCore.Controllers
{
    public class HomeController : Controller
    {
        public readonly IOrderService _orderService;
        private readonly IInfrastruxtureTest _infrastruxtureTest;
        public HomeController(IOrderService orderService, IInfrastruxtureTest test)
        {
            _orderService = orderService;
            _infrastruxtureTest = test;
        }
        public IActionResult Index()
        {
            var say = _orderService.Say();
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

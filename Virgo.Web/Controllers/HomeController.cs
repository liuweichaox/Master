using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Virgo.Application.IServices;
using Virgo.Web.Models;

namespace Virgo.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICustomService _customService;

        public HomeController(ILogger<HomeController> logger, ICustomService customService)
        {
            _logger = logger;
            _customService = customService;
        }

        public IActionResult Index()
        {
            var result = _customService.Call();
            _logger.LogInformation(Environment.NewLine + result+ Environment.NewLine);
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

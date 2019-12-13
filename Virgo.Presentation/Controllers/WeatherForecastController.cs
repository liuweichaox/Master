using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Virgo.Application.Interfaces;
using Virgo.DependencyInjection;
using Virgo.Presentation.Models;

namespace Virgo.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ICustomService _customService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, ICustomService customService)
        {
            _logger = logger;
            _customService = customService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet]
        public bool Call()
        {
            return _customService.Call();
        }

        [HttpGet]
        public bool Ioc()
        {
            return IocManager.Instance.GetInstance<ICustomService>().Call();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.IocManager;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using Virgo.Presentation.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Virgo.Presentation.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ValuesController : Controller
    {
        public ITest test { get; set; }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var data = test.Get();
            return new string[] { "value1", "value2" };
        }
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Virgo.Application.Interfaces;
using Virgo.Application.Models.Requests;
using Virgo.Application.Models.Responses;
using Virgo.AspNetCore;

namespace Virgo.UserInterface.Controllers.v2
{
    /// <summary>
    /// ValueController
    /// </summary>
    [Produces("application/json")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class ValueController : ApplicationController
    {
        /// <summary>
        /// <see cref="ICustomService"/>实例
        /// </summary>
        private readonly ICustomService _customService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="customService"></param>
        public ValueController(ICustomService customService)
        {
            _customService = customService;
        }

        /// <summary>
        /// Get: api/Value/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "OK";
        }

        /// <summary>
        /// Post: api/Value/5
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<CostomResponse>> Post([FromBody] CustomRequest request)
        {
            var result = await Task.Run(() => _customService.Call(request));
            return Success(result);
        }

        /// <summary>
        /// Put:api/Value/5
        /// </summary>
        [HttpPut("{id}")]
        public void Put(int id)
        {

        }

        /// <summary>
        /// DELETE: api/ApiWithActions/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

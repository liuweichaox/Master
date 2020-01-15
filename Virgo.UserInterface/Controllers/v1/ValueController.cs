using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Virgo.Application.Interfaces;
using Virgo.Application.Models.Requests;
using Virgo.Application.Models.Responses;
using Virgo.AspNetCore;

namespace Virgo.UserInterface.Controllers
{
    /// <summary>
    /// ValueController
    /// </summary>
    [Produces("application/json")]
    [ApiVersion("1.0")]
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
            var result = await Task.Run(() =>
            {
                return _customService.Call(request);
            });
            return Success(result);
        }

        /// <summary>
        /// Put:
        /// </summary>
        [HttpPut("{id}")]
        public void Put(OrderModel  orderModel)
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
        [HttpGet]
        public int Add([FromQuery]int a, [FromQuery] int b)
        {
            return a + b;
        }
    }
    public class OrderModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 订单类型
        /// <Remark>
        /// 0 商家入驻
        /// 1 线下交易
        /// </Remark>
        /// </summary>
        public OrderTypeInfo OrderType { get; set; }

    }

    /// <summary>
    /// OrderTypeInfo
    /// </summary>
    public enum OrderTypeInfo
    {
        /// <summary>
        /// 商家入驻
        /// </summary>
        [Description("商家入驻")]
        StoreEntry = 0,
        /// <summary>
        /// 线下交易
        /// </summary>
        [Description("线下交易")]
        StoreTrade = 1,

    }
}

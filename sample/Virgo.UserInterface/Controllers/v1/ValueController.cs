using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Virgo.Application.Interfaces;
using Virgo.Application.Models.Requests;
using Virgo.Application.Models.Responses;
using Virgo.AspNetCore;
using Virgo.DependencyInjection;
using Virgo.Elasticsearch;

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
        /// 仓储
        /// </summary>
        private readonly IBeautySpotRepository _beautySpotRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="customService"></param>
        /// <param name="beautySpotRepository"></param>
        public ValueController(ICustomService customService, IBeautySpotRepository beautySpotRepository)
        {
            _customService = customService;
            _beautySpotRepository = beautySpotRepository;
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
        /// Put:api/Value/5
        /// </summary>
        [HttpPut("{id}")]
        public void Put(OrderModel orderModel)
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

        /// <summary>
        /// 求和
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [HttpGet]
        public int Add([FromQuery]int a, [FromQuery] int b)
        {
            return a + b;
        }

        /// <summary>
        /// 初始化地理位置数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string InitLocationData()
        {
            var deleteIndexResponse = _beautySpotRepository.ElasticClient.Indices.Delete("virgo");
            var mappingResponse = _beautySpotRepository.ElasticClient.Indices.Create("virgo", c => c.Map<BeautySpot>(m => m.AutoMap()));
            var data = new List<BeautySpot>()
            {
                CreateBeautySpot("东方明珠广播电视塔",31.245105,121.506377),
                CreateBeautySpot("上海国际会议中心",31.245151,121.503475),
                CreateBeautySpot("上海环球金融中心",31.240165,121.514263),
                CreateBeautySpot("上海欢乐谷",31.10273,121.222777),
                CreateBeautySpot("上海迪士尼乐园 ",31.148267,121.671964)
            };
            var bulkResponse = _beautySpotRepository.BulkAsync(data).Result;
            return bulkResponse.ToString();
        }

        /// <summary>
        /// 地理位置搜索
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object QueryLocation()
        {
            var location = new GeoLocation(31.245105, 121.506377);
            var search = _beautySpotRepository.ElasticClient.Search<BeautySpot>(s => s
            .Query(q => q.GeoDistance(g => g
            .Boost(1.1)
            .Name("named_query")
            .Field(p => p.Location)
            .DistanceType(GeoDistanceType.Arc)
            .Location(location)
            .Distance(25, DistanceUnit.Kilometers)
            .ValidationMethod(GeoValidationMethod.IgnoreMalformed)))
            .Sort(t => t
            .GeoDistance(g => g
            .Field(f => f.Location)
            .Points(location)
            .Unit(DistanceUnit.Kilometers)
            .Ascending())));
            return search;
        }

        /// <summary>
        /// 创建景点实体
        /// </summary>
        /// <param name="name">景点名称</param>
        /// <param name="lat">维度</param>
        /// <param name="lon">经度</param>
        /// <returns></returns>    
        [NonAction]
        public BeautySpot CreateBeautySpot(string name, double lat, double lon)
        {
            return new BeautySpot
            {
                Id = Guid.NewGuid().ToString(),
                Country = "中国",
                Ciry = "上海",
                CreateTime = DateTime.Now,
                Location = new GeoLocation(lat, lon),
                Name = name
            };
        }
    }

    /// <summary>
    /// <see cref="BeautySpot"/>仓储接口
    /// </summary>
    public interface IBeautySpotRepository : IElasticsearchRepository<BeautySpot>
    {
    }

    /// <summary>
    /// <see cref="BeautySpot"/>仓储实现
    /// </summary>
    public class BeautySpotRepository : ElasticsearchRepositoryBase<BeautySpot>, IBeautySpotRepository, ITransientDependency
    {
        public BeautySpotRepository(IElasticClientFactory factory) : base(factory)
        {

        }
    }

    /// <summary>
    /// <see cref="ElasticClient"/>工厂
    /// </summary>
    public class ElasticClientFactory : IElasticClientFactory, ITransientDependency
    {
        public ElasticClient Create()
        {
            //var uri = new Uri("http://elastic:123456@localhost:9200");url内的身份验证
            var uri = new Uri("http://localhost:9200");
            var nodes = new Node[]
            {
                new Node(uri)
            };
            var pool = new StaticConnectionPool(nodes);
            var settings = new ConnectionSettings(pool).BasicAuthentication("elastic", "123456").DefaultIndex("virgo");
            var client = new ElasticClient(settings);
            return client;
        }
    }
    /// <summary>
    /// 景点
    /// </summary>    
    public class BeautySpot
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string Ciry { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 位置（lat：纬度,lon：经度）
        /// </summary>
        [GeoPoint(Name = "location")]
        public GeoLocation Location { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 订单模型
    /// </summary>
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

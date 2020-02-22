using Autofac;
using Nest;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Virgo.TestBase;
using Xunit;

namespace Virgo.Elasticsearch.Tests
{
    /// <summary>
    /// <see cref="ElasticClient"/>仓储测试
    /// </summary>
    public class ElasticsearchRepository_Tests : TestBaseWithIocBuilder
    {
        /// <summary>
        /// <see cref="BeautySpot"/>仓储实例
        /// </summary>
        private readonly IBeautySpotRepository _beautySpotRepository;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ElasticsearchRepository_Tests()
        {
            Building(builder =>
            {
                builder.RegisterType<ElasticClientFactory>().As<IElasticClientFactory>().SingleInstance();
                builder.RegisterType<BeautySpotRepository>().As<IBeautySpotRepository>().SingleInstance();
            });
            _beautySpotRepository = The<IBeautySpotRepository>();
        }

        /// <summary>
        /// 初始化地理位置数据
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void InitLocationData()
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
            bulkResponse.ShouldBe(true);
        }

        /// <summary>
        /// 地理位置搜索
        /// </summary>
        [Fact]
        public void QueryLocation()
        {
            var location = new GeoLocation(31.245105, 121.506377);
            var search = _beautySpotRepository.ElasticClient.Search<BeautySpot>(s => s
            .Query(q => q.GeoDistance(g => g
            .Boost(1.1)
            .Name("named_query")
            .Field(p => p.Location)
            .DistanceType(GeoDistanceType.Arc)
            .Location(location)
            .Distance(3, DistanceUnit.Kilometers)
            .ValidationMethod(GeoValidationMethod.IgnoreMalformed)))
            .Sort(t => t
            .GeoDistance(g => g
            .Field(f => f.Location)
            .Points(location)
            .Unit(DistanceUnit.Kilometers)
            .Ascending())));
            search.Total.ShouldBe(3);
        }

        /// <summary>
        /// 创建景点实体
        /// </summary>
        /// <param name="name">景点名称</param>
        /// <param name="lat">维度</param>
        /// <param name="lon">经度</param>
        /// <returns></returns>    
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
}

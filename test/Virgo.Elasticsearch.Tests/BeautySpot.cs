using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Elasticsearch.Tests
{
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
}

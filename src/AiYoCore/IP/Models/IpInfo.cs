namespace Virgo.IP.Models
{
    /// <summary>
    /// IP信息
    /// </summary>
    public class IpInfo
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 国家编码
        /// </summary>

        public string CountryCode { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 省编码
        /// </summary>
        public string ProvinceCode { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string PostCode { get; set; }
        /// <summary>
        /// 网络运营商
        /// </summary>
        public string NetworkOperator { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double? Latitude { get; set; } = 0d;
        /// <summary>
        /// 经度
        /// </summary>
        public double? Longitude { get; set; } = 0d;
        /// <summary>
        /// 精度半径
        /// </summary>
        public int? AccuracyRadius { get; set; }
    }
}

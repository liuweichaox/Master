using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Virgo.Net.Http.HttpContentHelper;
namespace Virgo.Net.Http
{
    /// <summary>
    /// <see cref="WebRequest"/>辅助类
    /// </summary>
    public static class WebRequestHelper
    {
        /// <summary>
        /// 通过WebRequest发起Get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns>JSON字符串</returns>
        public static async Task<string> GetAsync(string url, object data)
        {
            string jsonString = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create($"{url}?{BuildParam(ToKeyValuePair(data))}");
            request.Method = HttpMethod.Get.Method;
            request.ContentType = "application/json";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            var response = (HttpWebResponse)(await request.GetResponseAsync());
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (var stream = new StreamReader(response.GetResponseStream()))
                {
                    jsonString = await stream.ReadToEndAsync();
                }
            }
            return jsonString;
        }
        /// <summary>
        /// 通过WebRequest发起Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求参数</param>
        /// <returns>JSON字符串</returns>
        public static async Task<string> PostAsync(string url, object data)
        {
            string jsonString = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = HttpMethod.Post.Method;
            request.Timeout = Int32.MaxValue;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            var jsonToSend = JsonConvert.SerializeObject(data, Formatting.None, new IsoDateTimeConverter());
            byte[] btBodys = Encoding.UTF8.GetBytes(jsonToSend);
            request.ContentLength = btBodys.Length;
            request.GetRequestStream().Write(btBodys, 0, btBodys.Length);
            var response = (HttpWebResponse)(await request.GetResponseAsync());
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (var stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    jsonString = await stream.ReadToEndAsync();
                }
            }
            return jsonString;
        }
    }
}

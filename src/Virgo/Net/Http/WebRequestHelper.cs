using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
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
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;
            if (url.Contains("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            }
            httpRequest = (HttpWebRequest)WebRequest.Create($"{url}?{BuildParam(ToKeyValuePair(data))}");
            httpRequest.Method = HttpMethod.Get.Method;
            httpRequest.ContentType = "application/json; charset=UTF-8";
            httpRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            try
            {
                httpResponse = (HttpWebResponse)await httpRequest.GetResponseAsync();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                using var stream = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8);
                jsonString = await stream.ReadToEndAsync();
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
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;
            if (url.Contains("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            httpRequest.ContentType = "application/json; charset=UTF-8";
            httpRequest.Method = HttpMethod.Post.Method;
            httpRequest.Timeout = Int32.MaxValue;
            httpRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            var bodys = JsonConvert.SerializeObject(data, Formatting.None, new IsoDateTimeConverter());
            byte[] btBodys = Encoding.UTF8.GetBytes(bodys);
            if (0 < btBodys.Length)
            {
                httpRequest.ContentLength = btBodys.Length;
                using Stream stream = httpRequest.GetRequestStream();
                stream.Write(btBodys, 0, btBodys.Length);
            }
            try
            {
                httpResponse = (HttpWebResponse)await httpRequest.GetResponseAsync();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                using var stream = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8);
                jsonString = await stream.ReadToEndAsync();
            }
            return jsonString;
        }
    }
}

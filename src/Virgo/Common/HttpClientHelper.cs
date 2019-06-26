using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static Virgo.Common.HttpContentHelper;
namespace Virgo.Common
{
    /// <summary>
    /// <see cref="HttpContent"/>辅助类
    /// </summary>
    public static class HttpClientHelper
    {
        /// <summary>
        /// 通过HttpClient发起Get请求
        /// <para>键值对参数拼接在url上，后台使用[FromQuery]</para>
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求参数</param>
        /// <param name="action">Http请求头设置</param>
        /// <returns>JSON字符串</returns>
        public static async Task<string> GetAsync(string url, object data, Action<HttpRequestHeaders> action = null)
        { 
            if (url.ToLower().StartsWith("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            }
            string jsonString = string.Empty;
            using (var handler = new HttpClientHandler())
            {
                handler.AllowAutoRedirect = true;
                handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                handler.ClientCertificateOptions = ClientCertificateOption.Automatic;
                handler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true;
                using var client = new HttpClient(handler);
                action?.Invoke(client.DefaultRequestHeaders);
                var response = await client.GetAsync($"{url}?{BuildParam(ToKeyValuePair(data))}");
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    using var reader = new StreamReader(stream);
                    jsonString = await reader.ReadToEndAsync();
                }
            }
            return jsonString;
        }
        /// <summary>
        /// 通过HttpClient发起Post请求
        /// <para><see cref="HttpContext"/>区别：</para>
        /// <para><see cref="MultipartFormDataContent"/>、<see cref="FormUrlEncodedContent"/>、<see cref="StreamContent"/>、<see cref="ByteArrayContent"/>后台使用[FromForm]接受参数</para>
        /// <para><see cref="StringContent"/>后台使用[FromBody]接受参数</para>
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="content">请求参数</param>
        /// <param name="action">Http请求头设置</param>
        /// <returns>JSON字符串</returns>
        public static async Task<string> PostAsync(string url, HttpContent content, Action<HttpRequestHeaders> action = null)
        {
            string jsonString = string.Empty;
            using (var client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip }))
            {
                action?.Invoke(client.DefaultRequestHeaders);
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    using var reader = new StreamReader(stream);
                    jsonString = await reader.ReadToEndAsync();
                }
            }
            return jsonString;
        }

        /// <summary>
        /// 通过HttpClient发起Put请求
        /// <para><see cref="HttpContext"/>区别：</para>
        /// <para><see cref="MultipartFormDataContent"/>、<see cref="FormUrlEncodedContent"/>、<see cref="StreamContent"/>、<see cref="ByteArrayContent"/>后台使用[FromForm]接受参数</para>
        /// <para><see cref="StringContent"/>后台使用[FromBody]接受参数</para>
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="content">请求参数</param>
        /// <param name="action">Http请求头设置</param>
        /// <returns>JSON字符串</returns>
        public static async Task<string> PutAsync(string url, HttpContent content, Action<HttpRequestHeaders> action = null)
        {
            string jsonString = string.Empty;
            using (var client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip }))
            {
                action?.Invoke(client.DefaultRequestHeaders);
                var response = await client.PutAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    var stream = response.Content.ReadAsStreamAsync().Result;
                    using var reader = new StreamReader(stream);
                    jsonString = await reader.ReadToEndAsync();
                }
            }
            return jsonString;
        }
        /// <summary>
        /// 通过HttpClient发起Delete请求
        /// <para>键值对参数拼接在url上，后台使用[FromQuery]</para>
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求参数</param>
        /// <param name="action">Http请求头设置</param>
        /// <returns>JSON字符串</returns>
        public static async Task<string> DeleteAsync(string url, object data, Action<HttpRequestHeaders> action = null)
        {
            string jsonString = string.Empty;
            using (var client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip }))
            {
                action?.Invoke(client.DefaultRequestHeaders);
                var response = await client.DeleteAsync($"{url}?{BuildParam(ToKeyValuePair(data))}");
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    using var reader = new StreamReader(stream);
                    jsonString = await reader.ReadToEndAsync();
                }
            }
            return jsonString;
        }

        /// <summary>
        /// 将HTTP请求作为异步操作发送
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="method">请求方法</param>
        /// <param name="content">请求内容</param>
        /// <param name="action">Http请求头设置</param>
        /// <returns></returns>
        public static async Task<string> SendAsync(string url, HttpMethod method, HttpContent content, Action<HttpRequestHeaders> action = null)
        {
            string jsonString = string.Empty;
            using (var client = new HttpClient())
            {
                action?.Invoke(client.DefaultRequestHeaders);
                using var httpRequestMessage = new HttpRequestMessage(method, url);
                httpRequestMessage.Content = content;
                HttpResponseMessage httpResponse = await client.SendAsync(httpRequestMessage);
                jsonString = await httpResponse.Content.ReadAsStringAsync();
            }
            return jsonString;
        }

    }
}

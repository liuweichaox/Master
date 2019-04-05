using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Virgo.Data;
using static Virgo.Common.HttpContentHelper;
namespace Virgo.Net
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
        /// <returns>JSON字符串</returns>
        public static async Task<string> GetAsync(string url, object data)
        {
            string jsonString = string.Empty;
            using (var client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip }))
            {
                var response = await client.GetAsync($"{url}?{BuildParam(ToKeyValuePair(data))}");
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    using (var reader = new StreamReader(stream))
                    {
                        jsonString = await reader.ReadToEndAsync();
                    }
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
        /// <param name="data">请求参数</param>
        /// <returns>JSON字符串</returns>
        public static async Task<string> PostAsync(string url, HttpContent content)
        {
            string jsonString = string.Empty;
            using (var client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip }))
            {
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    using (var reader = new StreamReader(stream))
                    {
                        jsonString = await reader.ReadToEndAsync();
                    }
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
        /// <param name="data">请求参数</param>
        /// <returns>JSON字符串</returns>
        public static async Task<string> PutAsync(string url, HttpContent content)
        {
            string jsonString = string.Empty;
            using (var client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip }))
            {
                var response = await client.PutAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    var stream = response.Content.ReadAsStreamAsync().Result;
                    using (var reader = new StreamReader(stream))
                    {
                        jsonString = await reader.ReadToEndAsync();
                    }
                }
            }
            return jsonString;
        }
        /// <summary>
        /// 通过HttpClient发起Delete请求
        /// <para>键值对参数拼接在url上，后台使用[FromQuery]</para>
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns>JSON字符串</returns>
        public static async Task<string> DeleteAsync(string url, object data)
        {
            string jsonString = string.Empty;
            using (var client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip }))
            {
                var response = await client.DeleteAsync($"{url}?{BuildParam(ToKeyValuePair(data))}");
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    using (var reader = new StreamReader(stream))
                    {
                        jsonString = await reader.ReadToEndAsync();
                    }
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
        /// <returns></returns>
        public static async Task<string> SendAsync(string url, HttpMethod method, HttpContent content)
        {
            string jsonString = string.Empty;
            using (var client = new HttpClient())
            {
                using (var httpRequestMessage = new HttpRequestMessage(method, url))
                {
                    httpRequestMessage.Content = content;
                    HttpResponseMessage httpResponse = await client.SendAsync(httpRequestMessage);
                    jsonString = await httpResponse.Content.ReadAsStringAsync();
                }
            }
            return jsonString;
        }

    }
}

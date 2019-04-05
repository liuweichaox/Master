using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Virgo.Common
{
    public static class HttpContentHelper
    {
        /// <summary>
        /// Object转换为StreamContent
        /// </summary>
        /// <param name="data">请求参数</param>
        /// <returns>StreamContent</returns>
        public static HttpContent ToStreamContent(object data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.None, new IsoDateTimeConverter());
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            MemoryStream ms = new MemoryStream();
            ms.Write(bytes, 0, bytes.Length);
            ms.Position = 0;
            HttpContent streamContent = new StreamContent(ms);
            return streamContent;
        }

        /// <summary>
        /// Object转换为StringContent
        /// </summary>
        /// <param name="data">请求参数</param>
        /// <returns>StringContent</returns>
        public static HttpContent ToStringContent(object data)
        {
            var jsonToSend = JsonConvert.SerializeObject(data, Formatting.None, new IsoDateTimeConverter());
            HttpContent stringContent = new StringContent(jsonToSend, Encoding.UTF8, "application/json");
            return stringContent;
        }

        /// <summary>
        /// 将接受的文件和参数转换为MultipartFormDataContent
        /// </summary>
        /// <param name="data"></param>
        /// <returns>MultipartFormDataContent</returns>
        public static HttpContent ToMultipartFormDataContent(IFormFileCollection files = null, object data = null)
        {
            if (files == null && data == null)
            {
                return null;
            }
            var multipartFormDataContent = new MultipartFormDataContent();
            foreach (var item in files)
            {
                multipartFormDataContent.Add(new StreamContent(item.OpenReadStream()), item.Name, item.FileName);
            }

            foreach (var item in JObject.FromObject(data))
            {
                multipartFormDataContent.Add(new StringContent(item.Value.ToString()), item.Key);
            }
            return multipartFormDataContent;
        }

        /// <summary>
        /// Object转换为FormUrlEncodedContent
        /// </summary>
        /// <param name="data">请求参数</param>
        /// <returns>FormUrlEncodedContent</returns>
        public static HttpContent ToFormUrlEncodedContent(object data)
        {
            HttpContent formUrlEncodedContent = new FormUrlEncodedContent(ToKeyValuePair(data));
            return formUrlEncodedContent;
        }

        /// <summary>
        /// Object转换为ByteArrayContent
        /// </summary>
        /// <param name="data">请求参数</param>
        /// <returns>ByteArrayContent</returns>
        public static HttpContent ToByteArrayContent(object data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.None, new IsoDateTimeConverter());
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            HttpContent byteArrayContent = new ByteArrayContent(bytes);
            return byteArrayContent;
        }

        /// <summary>
        /// Object转换为Bytes
        /// </summary>
        /// <param name="data">请求参数</param>
        /// <returns>byte[]</returns>
        public static byte[] ToBytes(object data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.None, new IsoDateTimeConverter());
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            return bytes;
        }

        /// <summary>
        /// 将键值对参数集合拼接为Url字符串
        /// </summary>
        /// <param name="paramArray">键值对集合</param>
        /// <param name="encode">转码类型</param>
        /// <returns></returns>
        public static string BuildParam(List<KeyValuePair<string, string>> paramArray, Encoding encode = null)
        {
            string url = "";

            if (encode == null) encode = Encoding.UTF8;

            if (paramArray != null && paramArray.Count > 0)
            {
                var parms = "";
                foreach (var item in paramArray)
                {
                    parms += string.Format("{0}={1}&", Encode(item.Key, encode), Encode(item.Value, encode));
                }
                if (parms != "")
                {
                    parms = parms.TrimEnd('&');
                }
                url += parms;

            }
            return url;
        }

        /// <summary>
        /// Object转换为KeyValuePair
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> ToKeyValuePair(object data)
        {
            var param = new List<KeyValuePair<string, string>>();
            var values = JObject.FromObject(data);
            foreach (var item in values)
            {
                param.Add(new KeyValuePair<string, string>(item.Key, item.Value.ToString()));
            }
            return param;
        }        /// <summary>
                 /// Url编码
                 /// </summary>
                 /// <param name="content">内容</param>
                 /// <param name="encode">编码类型</param>
                 /// <returns></returns>
        public static string Encode(string content, Encoding encode = null)
        {
            if (encode == null) return content;

            return System.Web.HttpUtility.UrlEncode(content, Encoding.UTF8);

        }

        /// <summary>
        /// Url解码
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="encode">编码类型</param>
        /// <returns></returns>
        public static string Decode(string content, Encoding encode = null)
        {
            if (encode == null) return content;

            return System.Web.HttpUtility.UrlDecode(content, Encoding.UTF8);

        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Virgo.Extensions
{
    public static class JsonExtensions
    {
        /// <summary>
        /// 将类型序列化为字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize<T>(this T t)
        {
            return JsonConvert.SerializeObject(t);
        }

        /// <summary>
        /// 将字符串反序列化为类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static TResult Deserialize<TResult>(this string s)
        {
            return JsonConvert.DeserializeObject<TResult>(s);
        }

        /// <summary>
        /// 尝试将字符串反序列化为类型
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="s"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryDeserialize<TResult>(this string s, out TResult result)
        {
            try
            {
                result = Deserialize<TResult>(s);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Virgo.Extensions
{
    /// <summary>
    /// <see cref="object"/>拓展方法
    /// </summary>
    public static class ObjectExtensions
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

        /// <summary>
        /// 将字符串序列化为匿名类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="anonymousTypeObject"></param>
        /// <returns></returns>
        public static T DeserializeAnonymousType<T>(this string s, T anonymousTypeObject)
        {
            return JsonConvert.DeserializeAnonymousType(s, anonymousTypeObject);
        }

        /// <summary>
        /// 执行对象的深层复制
        /// </summary>
        /// <typeparam name="T">要复制的对象的类型</typeparam>
        /// <param name="obj">要复制的对象实例</param>
        /// <returns>The copied object.</returns>
        public static T DeepClone<T>(this T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(T));
                ser.WriteObject(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                retval = ser.ReadObject(ms);
                ms.Close();
            }
            return (T)retval;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Virgo.Extensions
{
    /// <summary>
    /// XML拓展
    /// </summary>
   public static class XMLExtensions
    {
        /// <summary>
        /// XMLHelper
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string XMLSerialize<T>(this T model) where T : class
        {
            string xml;
            using (var ms = new MemoryStream())
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(T));
                xmlSer.Serialize(ms, model);
                ms.Position = 0;
                StreamReader sr = new StreamReader(ms);
                xml = sr.ReadToEnd();
            }
            return xml;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        public static T XMLDeserialize<T>(this string xml) where T : class
        {
            try
            {
                using StringReader sr = new StringReader(xml);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return serializer.Deserialize(sr) as T;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

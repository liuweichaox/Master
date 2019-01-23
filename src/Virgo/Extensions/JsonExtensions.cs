using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static T ToObject<T>(this string s)
        {
            return JsonConvert.DeserializeObject<T>(s);
        }
    }
}

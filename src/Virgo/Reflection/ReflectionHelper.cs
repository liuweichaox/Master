using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Virgo.Reflection
{
    public class ReflectionHelper
    {
        /// <summary>
        /// 获取成员DescriptionAttribute值
        /// </summary>
        /// <param name="member">成员</param>
        public static string GetDescription<T>(string member)
        {
            return GetDescription(typeof(T), member);
        }

        public static string GetDescription(Type type, string member)
        {
            var memberInfo = type.GetMember(member).FirstOrDefault();
            if (string.IsNullOrEmpty(member))
                return null;
            return memberInfo.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute attribute ? attribute.Description : string.Empty;
        }
    }
}

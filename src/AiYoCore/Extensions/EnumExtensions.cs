using System;
using System.Collections.Generic;
using System.Text;
using Virgo.Reflection;

namespace Virgo.Extensions
{
    /// <summary>
    /// <see cref="Enum"/>拓展
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum @enum)
        {
            return ReflectionHelper.GetDescription(@enum.GetType(), @enum.ToString());
        }
    }
}

using Autofac.Extras.IocManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo
{
    /// <summary>
    /// Virgo拓展类
    /// </summary>
   public static  class VirgoExtensions
    {
        /// <summary>
        /// 注入Virgo
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IIocBuilder UseVirgo(this IIocBuilder builder)
        {
            var assembly = typeof(VirgoExtensions).Assembly;
            builder.RegisterServices(r => r.RegisterAssemblyByConvention(assembly));
            return builder;
        }
    }
}

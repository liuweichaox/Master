using Autofac.Extras.IocManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo
{
   public static  class VirgoExtensions
    {
        public static IIocBuilder UseVirgo(this IIocBuilder builder)
        {
            var assembly = typeof(VirgoExtensions).Assembly;
            builder.RegisterServices(r => r.RegisterAssemblyByConvention(assembly));
            return builder;
        }
    }
}

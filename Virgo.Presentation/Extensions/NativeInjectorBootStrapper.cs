using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Virgo.Presentation.Extensions
{
    /// <summary>
    /// 启动器
    /// </summary>
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddHttpClient();

            services.AddControllers();

            services.AddOptions();
        }
    }
}

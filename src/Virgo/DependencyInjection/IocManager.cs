using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.DependencyInjection
{
    public class IocManager : IIocManager,ISingletonDependency
    {
        static IocManager()
        {
            Instance = new IocManager();
        }
        public static IocManager Instance { get; private set; }
        public IServiceProvider ServiceProvider { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Virgo.Application.IService;
using Virgo.DependencyInjection;

namespace Virgo.Application.Service
{
    /// <summary>
    /// 自定一服务接口实现，作用域生命周期
    /// </summary>
    public class CustomService : ICustomService, ILifetimeScopeDependency
    {
        public string Call()
        {
            return "Service Call";
        }
    }
}

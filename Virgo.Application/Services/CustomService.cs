using System;
using System.Collections.Generic;
using System.Text;
using Virgo.Application.Interfaces;
using Virgo.DependencyInjection;

namespace Virgo.Application.Services
{
    /// <summary>
    /// 自定一服务接口实现，作用域生命周期
    /// </summary>
    public class CustomService : ICustomService, ILifetimeScopeDependency
    {
        public bool Call()
        {
            System.Diagnostics.Debug.WriteLine("Service Calling");
            return true;
        }
    }
}

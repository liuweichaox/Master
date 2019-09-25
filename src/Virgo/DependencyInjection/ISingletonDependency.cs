using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.DependencyInjection
{   
    /// <summary>
     /// 确定接口或类的生存期
     /// 单例模式，服务在第一次请求时被创建，其后的每次请求都沿用这个已创建的服务。
     /// </summary>
    public interface ISingletonDependency : ILifetime { }
}

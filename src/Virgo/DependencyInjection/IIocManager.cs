using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.DependencyInjection
{
    /// <summary>
    /// 管理器接口
    /// </summary>
    public interface IIocManager
    {
        IServiceProvider ServiceProvider { get; set; }
        TService GetInstance<TService>();
    }
}

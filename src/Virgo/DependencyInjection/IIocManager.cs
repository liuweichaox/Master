using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.DependencyInjection
{
    public interface IIocManager
    {
        IServiceProvider ServiceProvider { get; set; }
    }
}

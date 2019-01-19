using Autofac.Extras.IocManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Virgo.AspNetCore.Models
{
    public class OrderService : IOrderService, ITransientDependency
    {
    }
}

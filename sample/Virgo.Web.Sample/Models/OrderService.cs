using Autofac.Extras.IocManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Virgo.Domain.Uow;

namespace Virgo.AspNetCore.Models
{
    public class OrderService : IOrderService, ITransientDependency
    {
        public string Fuck()
        {
            return "刘大大";
        }

        [UnitOfWork]
        public string Say()
        {
           return "Virgo";
        }
    }
}

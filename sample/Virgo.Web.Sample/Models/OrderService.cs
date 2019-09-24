using Virgo.DependencyInjection;
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

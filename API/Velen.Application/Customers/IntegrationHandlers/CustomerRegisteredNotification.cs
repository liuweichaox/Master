using Velen.Domain.Customers;
using Velen.Domain.Events;

namespace Velen.Application.Customers.IntegrationHandlers
{
    public class CustomerRegisteredNotification : DomainNotificationBase<CustomerRegisteredEvent>
    {
        public CustomerRegisteredNotification(CustomerRegisteredEvent domainEvent) : base(domainEvent)
        {
        }
    }
}
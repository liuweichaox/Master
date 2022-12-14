using System.Text.Json.Serialization;
using Velen.Domain.Customers;
using Velen.Domain.Events;

namespace Velen.Application.Customers.IntegrationHandlers
{
    public class CustomerRegisteredNotification : DomainNotificationBase<CustomerRegisteredEvent>
    {
        public Guid CustomerId { get; }

        public CustomerRegisteredNotification(CustomerRegisteredEvent domainEvent) : base(domainEvent)
        {
            this.CustomerId = domainEvent.CustomerId;
        }

        [JsonConstructor]
        public CustomerRegisteredNotification(Guid customerId) : base(null)
        {
            this.CustomerId = customerId;
        }
    }
}
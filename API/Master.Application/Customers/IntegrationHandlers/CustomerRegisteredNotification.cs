using System.Text.Json.Serialization;
using Master.Domain.Customers;
using Master.Domain.Events;

namespace Master.Application.Customers.IntegrationHandlers
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
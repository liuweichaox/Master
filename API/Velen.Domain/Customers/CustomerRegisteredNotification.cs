using System.Text.Json.Serialization;
using Velen.Domain.Events;

namespace Velen.Domain.Customers
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
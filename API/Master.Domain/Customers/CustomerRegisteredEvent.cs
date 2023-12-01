using Master.Domain.SeedWork;

namespace Master.Domain.Customers
{
    public class CustomerRegisteredEvent : DomainEventBase
    {
        public Guid CustomerId { get; }

        public CustomerRegisteredEvent(Guid customerId)
        {
            this.CustomerId = customerId;
        }
    }
}
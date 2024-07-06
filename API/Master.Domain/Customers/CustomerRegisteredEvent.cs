using Master.Domain.SeedWork;

namespace Master.Domain.Customers;

public class CustomerRegisteredEvent : DomainEventBase
{
    public CustomerRegisteredEvent(Guid customerId)
    {
        CustomerId = customerId;
    }

    public Guid CustomerId { get; }
}
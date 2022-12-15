using MediatR;
using System.Text.Json;
using Velen.Domain.Customers;

namespace Velen.Application.Customers.IntegrationHandlers
{
    public class CustomerRegisteredEventHandler : INotificationHandler<CustomerRegisteredEvent>
    {
        public Task Handle(CustomerRegisteredEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine("CustomerRegisteredEventHandler - Handle command json " + JsonSerializer.Serialize(notification));
            return Task.CompletedTask;
        }
    }
}

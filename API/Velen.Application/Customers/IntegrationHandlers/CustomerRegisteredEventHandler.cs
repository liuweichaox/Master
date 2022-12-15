using MediatR;
using System.Text.Json;
using Velen.Domain.Customers;

namespace Velen.Application.Customers.IntegrationHandlers
{
    public class CustomerRegisteredEventHandler : INotificationHandler<CustomerRegisteredEvent>
    {
        public async Task Handle(CustomerRegisteredEvent notification, CancellationToken cancellationToken)
        {

             await Task.Run(() =>
            {
                Console.WriteLine("CustomerRegisteredEventHandler - Handle command json " + JsonSerializer.Serialize(notification));
            });
        }
    }
}

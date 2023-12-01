using System.Text.Json;
using MediatR;
using Master.Infrastructure.Processing;

namespace Master.Application.Customers.IntegrationHandlers
{
    public class CustomerRegisteredNotificationHandler : INotificationHandler<CustomerRegisteredNotification>
    {
        private readonly ICommandsScheduler _commandsScheduler;

        public CustomerRegisteredNotificationHandler(
            ICommandsScheduler commandsScheduler)
        {
            _commandsScheduler = commandsScheduler;
        }

        public async Task Handle(CustomerRegisteredNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine("CustomerRegisteredNotificationHandler - Handle command json "+JsonSerializer.Serialize(notification));
            await this._commandsScheduler.EnqueueAsync(new MarkCustomerAsWelcomedCommand(
                notification.Id,
                notification.CustomerId));
        }
    }
}
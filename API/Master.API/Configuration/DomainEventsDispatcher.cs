using Master.Domain.Events;
using Master.Infrastructure.Processing.Outbox;

namespace Master.API.Configuration
{
    /// <summary>
    /// DomainEventsDispatcher
    /// </summary>
    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _appDbContext;

        public DomainEventsDispatcher(IMediator mediator, AppDbContext appDbContext)
        {
            this._mediator = mediator;
            this._appDbContext = appDbContext;
        }

        public async Task DispatchEventsAsync()
        {
            var domainEntities = this._appDbContext.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            var domainEventNotifications = new List<IDomainEventNotification<IDomainEvent>>();
            foreach (var domainEvent in domainEvents)
            {
                Type domainEvenNotificationType = typeof(IDomainEventNotification<>);
                var domainNotificationWithGenericType =
                    domainEvenNotificationType.MakeGenericType(domainEvent.GetType());
                var domainNotificationType = ApplicationModule.Assembly.GetTypes().SingleOrDefault(x =>
                    x.GetInterfaces().Any(y => y == domainNotificationWithGenericType));
                if (domainNotificationType == null)
                {
                    continue;
                }
                var domainNotification = Activator.CreateInstance(domainNotificationType, domainEvent);
                if (domainNotification != null)
                {
                    Console.WriteLine("DomainEventsDispatcher: DispatchEventsAsync: domainNotification: " + JsonSerializer.Serialize(domainNotification) + domainNotificationType.Name);
                    domainEventNotifications.Add(domainNotification as IDomainEventNotification<IDomainEvent>);
                }
            }

            domainEntities
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    Console.WriteLine("DomainEventsDispatcher: DispatchEventsAsync: domainEvent: " + JsonSerializer.Serialize(domainEvent));
                    await _mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);

            foreach (var domainEventNotification in domainEventNotifications)
            {
                string type = domainEventNotification.GetType().FullName;
                var data = JsonSerializer.Serialize(domainEventNotification, domainEventNotification.GetType());
                OutboxMessage outboxMessage = new OutboxMessage(
                    domainEventNotification.DomainEvent.OccurredOn,
                    type,
                    data);
                this._appDbContext.OutboxMessages.Add(outboxMessage);
            }
        }
    }
}
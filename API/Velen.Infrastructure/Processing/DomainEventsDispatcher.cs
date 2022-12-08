using System.Text.Json;
using MediatR;
using Velen.Domain.DomainEvents;
using Velen.Domain.SeedWork;
using Velen.Infrastructure.Domain;
using Velen.Infrastructure.Processing.Outbox;
using Microsoft.Extensions.DependencyInjection;

namespace Velen.Infrastructure.Processing
{
    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;
        private readonly AppDbContext _appDbContext;

        public DomainEventsDispatcher(IMediator mediator, IServiceProvider serviceProvider, AppDbContext appDbContext)
        {
            this._mediator = mediator;
            this._serviceProvider = serviceProvider;
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
                var domainNotificationWithGenericType = domainEvenNotificationType.MakeGenericType(domainEvent.GetType());
                var domainNotification = _serviceProvider.GetService(domainNotificationWithGenericType) ?? Activator.CreateInstance(domainNotificationWithGenericType, domainEvent);
                if (domainNotification != null)
                {
                    domainEventNotifications.Add(domainNotification as IDomainEventNotification<IDomainEvent>);
                }
            }

            domainEntities
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await _mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);

            foreach (var domainEventNotification in domainEventNotifications)
            {
                string type = domainEventNotification.GetType().FullName;
                var data = JsonSerializer.Serialize(domainEventNotification);
                OutboxMessage outboxMessage = new OutboxMessage(
                    domainEventNotification.DomainEvent.OccurredOn,
                    type,
                    data);
                this._appDbContext.OutboxMessages.Add(outboxMessage);
            }
        }
    }
}